<%@ Page Title="VitalSigns Plus- Domino Properties" Language="C#" MasterPageFile="~/Site1.Master"
	AutoEventWireup="true" CodeBehind="DominoProperties.aspx.cs" Inherits="VSWebUI.Configurator.DominoProperties" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx1" %>
<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxScheduler" TagPrefix="dxwschs" %>
<%@ Register Assembly="DevExpress.XtraScheduler.v14.2.Core, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraScheduler" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
	<script src="../js/bootstrap.min.js" type="text/javascript"></script>
    
   
	<script type="text/javascript">
		$(document).ready(function () {
			$('.alert-success').delay(10000).fadeOut("slow", function () {
			});
		});
		function OnItemClick(s, e) {
			if (e.item.parent == s.GetRootItem())
				e.processOnServer = false;
		}
	</script>
	<script type="text/javascript">
	    //3/8/2016 Durga Added for VSPLUS-2680

	    function Copytoclipboard(s, e) {
	        // Create a "hidden" input
	        var aux = document.createElement("input");
	       
	        // Assign it the value of the specified element
	        aux.setAttribute("value", document.getElementById("ContentPlaceHolder1_RConsolePopupControl_RConsoleMemo_I").innerHTML);
	       
	        // Append it to the body
	        document.body.appendChild(aux);

	        // Highlight its content
	        aux.select();

	        // Copy the highlighted text
	        document.execCommand("copy");

	        // Remove it from the body
	        document.body.removeChild(aux);
	        return false;
	    }
		function TabClick(e) {

			if (e.tab.name == "Server Tasks") {

				var id = document.getElementById("ContentPlaceHolder1_ASPxPageControl1_btnDisable");

				id.UseSubmitBehavior = true;
				var currentlyrunningtasksid = document.getElementsByName("ctl00$ContentPlaceHolder1$ASPxPageControl1$SrvTskCurRunTskButton0");
				currentlyrunningtasksid.UseSubmitBehavior = false;

}

		}
	</script>
    <script type="text/javascript">
	 	function showendtime() {
//		2/8/2016 Durga Added for VSPLUS 2581
	 		lStartTime = document.getElementById("ContentPlaceHolder1_ASPxPageControl1_ASPxRoundPanel2_StartTimeEdit_I");
	 		
	 		

	 		lDuration = document.getElementById("ContentPlaceHolder1_ASPxPageControl1_ASPxRoundPanel2_MaintDurationTextBox_I");

	 		lEndTime = document.getElementById("ContentPlaceHolder1_ASPxPageControl1_ASPxRoundPanel2_lblEndTime");
	 		lblDuration = document.getElementById("ContentPlaceHolder1_ASPxPageControl1_ASPxRoundPanel2_lblDuration");

	 		var d = new Date();//  new Date(lstartDate.value);
	 		
	 		var curr_date = d.getDate();
	 		var curr_month = d.getMonth() + 1; //Months are zero based
	 		var curr_year = d.getFullYear();
	 		
	 		var d1 = new Date(curr_month + "/" + curr_date + "/" + curr_year + ' ' + lStartTime.value);

	 		var d2 = new Date(d1.getTime() + (lDuration.value * 60000));
	 		var hours = d2.getHours();
	 		//alert(hours);
	 		var minutes = d2.getMinutes();
	 		//alert(minutes);
	 		var ampm = hours >= 12 ? 'PM' : 'AM';
	 		hours = hours % 12;
	 		hours = hours ? hours : 12; // the hour '0' should be '12'
	 		minutes = minutes < 10 ? '0' + minutes : minutes;
	 		var strTime = hours + ':' + minutes + ' ' + ampm;


	 		lEndTime.innerHTML = strTime;
	         lblDuration.innerHTML = Math.floor(lDuration.value / 60) + ' hour(s) ' + (lDuration.value % 60) + ' minutes';
	 		
	 	}
	 	
	 	function OnTextChanged(s, e) {
	 		trackBar.SetPosition(s.GetText());
	 		//alert(s.GetText());
	 		showendtime();
	 	}

        //8/6/2015 NS added for VSPLUS-1802
	 	function OnCheckedChangedEXJScan(s, e) {
	 	    var ischecked = EXJournalScanCheckBox.GetChecked();
	 	    EXJournalLookBackCheckBox.SetVisible(ischecked);
	 	    if (ischecked == false) {
	 	        EXJournalLookBackCheckBox.SetChecked(ischecked);
                lblStartTime.SetVisible(ischecked);
	 	        StartTime.SetVisible(ischecked);
	 	        lblEndTime1.SetVisible(ischecked);
	 	        EndTime.SetVisible(ischecked);
	 	        lblDuration1.SetVisible(ischecked);
	 	        MaintDuration.SetVisible(ischecked);
	 	        Duration.SetVisible(ischecked);
	 	        trackBar.SetVisible(ischecked);
	 	        lblLookBack.SetVisible(ischecked);
	 	        LookBackPeriodTextBox.SetVisible(ischecked);
	 	        lblMin.SetVisible(ischecked);
            }
	 	}

	 	//8/6/2015 NS added for VSPLUS-1802
	 	function OnCheckedChangedLookBackScan(s, e) {
	 	    var ischecked = EXJournalLookBackCheckBox.GetChecked();
	 	    lblStartTime.SetVisible(ischecked);
	 	    StartTime.SetVisible(ischecked);
	 	    lblEndTime1.SetVisible(ischecked);
	 	    EndTime.SetVisible(ischecked);
	 	    lblDuration1.SetVisible(ischecked);
	 	    MaintDuration.SetVisible(ischecked);
	 	    Duration.SetVisible(ischecked);
	 	    trackBar.SetVisible(ischecked);
	 	    lblLookBack.SetVisible(ischecked);
	 	    LookBackPeriodTextBox.SetVisible(ischecked);
	 	    lblMin.SetVisible(ischecked);
	 	}
    </script>
	<style type="text/css">
		.style1
		{
			width: 904px;
		}
		.memo Diskinfo
		{
			overflow: auto !important;
			overflow-x: hidden !important;
			overflow-y: auto !important;
		}
	.dxeMemoSys td {
    padding-right: 0px;
}

	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table width="100%">
		<tr>
			<td align="left" valign="top">
				<asp:Label ID="ErrorLabel" runat="server" Visible="false" Font-Size="X-Large" ForeColor="Red"
					Text="Error: No records on the server could be found." />
			</td>
			<td valign="top">
				<div style="float: left; margin-left: 2%; display: none" id="summaryContainer">
					<dx:ASPxValidationSummary ID="vsValidationSummary1" runat="server" RenderMode="BulletedList"
						Width="100%" ClientInstanceName="validationSummary">
						<Border BorderColor="Yellow" BorderStyle="Double" BorderWidth="2px" />
						<Border BorderColor="Yellow" BorderStyle="Double" BorderWidth="2px"></Border>
					</dx:ASPxValidationSummary>
				</div>
				<table width="100%">
					<tr>
						<td>
							<table width="100%">
								<tr>
									<td valign="top">
										<div class="header" id="servernamelbldisp" runat="server">
											IBM Domino Servers</div>
									</td>
									<td valign="top" align="right">
										<table>
											<tr>
												<td>
													<dx:ASPxMenu ID="ASPxMenu1" runat="server" EnableTheming="True" HorizontalAlign="Right"
														OnItemClick="ASPxMenu1_ItemClick" ShowAsToolbar="True" Theme="Moderno">
														<ClientSideEvents ItemClick="OnItemClick" />
														<Items>
															<dx:MenuItem Name="MainItem">
																<Items>
																	<dx:MenuItem Name="ServerDetailsPage" Text="View Details in Dashboard">
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
						</td>
					</tr>
					<tr>
						<td align="left" valign="top">
							<dx:ASPxPageControl Font-Bold="True" ID="ASPxPageControl1" runat="server" ActiveTabIndex="0"
								CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
								TabSpacing="0px" Width="100%" EnableHierarchyRecreation="False">
								<ClientSideEvents TabClick="function(s, e) {TabClick(e)}" />
								<TabPages>
									<dx:TabPage Text="Server Attributes" >
										<TabImage Url="~/images/information.png">
										</TabImage>
										<ContentCollection>
											<dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">
												<table width="100%">
													<tr>
														<td>
															<dx:ASPxRoundPanel ID="DiskSettingsRoundPanel" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
																CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Server Attributes"
																Height="50px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
																<ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
																<ContentPaddings Padding="2px" PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px">
																</ContentPaddings>
																<HeaderStyle Height="23px"></HeaderStyle>
																<PanelCollection>
																	<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
																		<table width="100%" cellspacing="3px">
																			<tr>
																				<td>
																					<dx:ASPxLabel ID="ASPxLabel14" runat="server" CssClass="lblsmallFont" Text="Name:">
																					</dx:ASPxLabel>
																				</td>
																				<td>
																					<dx:ASPxComboBox ID="SrvAtrSrvNameComboBox" runat="server" Enabled="False">
																					</dx:ASPxComboBox>
																				</td>
																				<td>
																					<dx:ASPxLabel ID="ASPxLabel16" runat="server" CssClass="lblsmallFont" Text="Description:">
																					</dx:ASPxLabel>
																				</td>
																				<td>
																					<dx:ASPxTextBox ID="SrvAtrDescriptionTextBox" runat="server" Enabled="False">
																					</dx:ASPxTextBox>
																				</td>
																				<td>
																					&nbsp;
																				</td>
																				<td>
																					<dx:ASPxCheckBox ID="SrvAtrScanCheckBox" runat="server" CheckState="Unchecked" CssClass="lblsmallFont"
																						Text="Enabled for scanning" Wrap="False">
																					</dx:ASPxCheckBox>
																				</td>
																			</tr>
																			<tr>
																				<td>
																					<dx:ASPxLabel ID="ASPxLabel18" runat="server" CssClass="lblsmallFont" Text="Location:">
																					</dx:ASPxLabel>
																				</td>
																				<td>
																					<dx:ASPxTextBox ID="SrvAtrLocationTextBox" runat="server" Enabled="False">
																						<ValidationSettings>
																							<RequiredField ErrorText="Please enter the location of the device, such as '8th floor server room'"
																								IsRequired="True" />
																							<RequiredField IsRequired="True" ErrorText="Please enter the location of the device, such as &#39;8th floor server room&#39;">
																							</RequiredField>
																						</ValidationSettings>
																					</dx:ASPxTextBox>
																				</td>
																				<td>
																					<dx:ASPxLabel ID="ASPxLabel20" runat="server" CssClass="lblsmallFont" Text="Category:">
																					</dx:ASPxLabel>
																				</td>
																				<td>
																					<dx:ASPxComboBox ID="SrvAtrCategoryComboBox" runat="server" SelectedIndex="10">
																						<Items>
																							<dx:ListEditItem Text="Administration Server" Value="Administration Server" />
																							<dx:ListEditItem Text="Application" Value="Application" />
																							<dx:ListEditItem Text="Backup" Value="Backup" />
																							<dx:ListEditItem Text="Development" Value="Development" />
																							<dx:ListEditItem Text="Directory" Value="Directory" />
																							<dx:ListEditItem Text="Disaster Recovery" Value="Disaster Recovery" />
																							<dx:ListEditItem Text="Gateway" Value="Gateway" />
																							<dx:ListEditItem Text="Hub" Value="Hub" />
																							<dx:ListEditItem Text="iNotes" Value="iNotes" />
																							<dx:ListEditItem Text="Internet Cluster Manager" Value="Internet Cluster Manager" />
																							<dx:ListEditItem Selected="True" Text="Mail" Value="Mail" />
																							<dx:ListEditItem Text="Multifunction" Value="Multifunction" />
																							<dx:ListEditItem Text="Production" Value="Production" />
																							<dx:ListEditItem Text="Pass-thru" Value="Pass-thru" />
																							<dx:ListEditItem Text="QuickPlace" Value="QuickPlace" />
																							<dx:ListEditItem Text="Replication" Value="Replication" />
																							<dx:ListEditItem Text="Web" Value="Web" />
																							<dx:ListEditItem Text="Test" Value="Test" />
																							<dx:ListEditItem Text="Other" Value="Other" />
																						</Items>
																					</dx:ASPxComboBox>
																				</td>
																				<td>
																					&nbsp;
																				</td>
																				<td>
																					&nbsp;
																				</td>
																			</tr>
																		</table>
																	</dx:PanelContent>
																</PanelCollection>
															</dx:ASPxRoundPanel>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
																CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Scan Settings"
																Height="50px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
																<ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
																<ContentPaddings Padding="2px" PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px">
																</ContentPaddings>
																<HeaderStyle Height="23px"></HeaderStyle>
																<PanelCollection>
																	<dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
																		<table cellspacing="3px">
																			<tr>
																				<td>
																					<dx:ASPxLabel ID="ASPxLabel4" runat="server" CssClass="lblsmallFont" Text="Scan Interval:">
																					</dx:ASPxLabel>
																				</td>
																				<td>
																					<dx:ASPxTextBox ID="SrvAtrScanIntvlTextBox" runat="server" CssClass="txtsmall" Text="8">
																						<MaskSettings Mask="&lt;0..9999&gt;" />
																						<MaskSettings Mask="&lt;0..9999&gt;"></MaskSettings>
																						<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																							<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																								ValidationExpression="^\d+$" />
																							<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																								ValidationExpression="^\d+$"></RegularExpression>
																						</ValidationSettings>
																					</dx:ASPxTextBox>
																				</td>
																				<td>
																					<dx:ASPxLabel ID="ASPxLabel5" runat="server" CssClass="lblsmallFont" Text="minutes">
																					</dx:ASPxLabel>
																				</td>
																				<td>
																					&nbsp;
																				</td>
																				<td>
																					<dx:ASPxLabel ID="ASPxLabel6" runat="server" CssClass="lblsmallFont" Text="Response Threshold:"
																						Wrap="False">
																					</dx:ASPxLabel>
																				</td>
																				<td>
																					<dx:ASPxTextBox ID="SrvAtrResponseThTextBox" runat="server" CssClass="txtmed" Text="50000">
																						<MaskSettings Mask="&lt;0..99999&gt;" />
																						<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>
																						<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																							<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																								ValidationExpression="^\d+$" />
																							<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																								ValidationExpression="^\d+$"></RegularExpression>
																						</ValidationSettings>
																					</dx:ASPxTextBox>
																				</td>
																				<td>
																					<dx:ASPxLabel ID="ASPxLabel7" runat="server" CssClass="lblsmallFont" Text="milliseconds">
																					</dx:ASPxLabel>
																				</td>
																				<td>
																					&nbsp;
																				</td>
																				<td>
																					<dx:ASPxButton ID="SrvAtrSuggestButton" runat="server" CssClass="lblsmallFont" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
																						CssPostfix="Office2010Blue" Font-Bold="False" OnClick="SrvAtrSuggestButton_Click"
																						SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="Suggest"
																						Visible="False">
																					</dx:ASPxButton>
																				</td>
																			</tr>
																			<tr>
																				<td>
																					<dx:ASPxLabel ID="ASPxLabel11" runat="server" CssClass="lblsmallFont" Text="Off-Hours Scan Interval:"
																						Wrap="False">
																					</dx:ASPxLabel>
																				</td>
																				<td>
																					<dx:ASPxTextBox ID="SrvAtrOffScanIntvlTextBox" runat="server" CssClass="txtsmall"
																						Text="15">
																						<MaskSettings Mask="&lt;0..9999&gt;" />
																						<MaskSettings Mask="&lt;0..9999&gt;"></MaskSettings>
																						<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																							<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																								ValidationExpression="^\d+$" />
																							<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																								ValidationExpression="^\d+$"></RegularExpression>
																						</ValidationSettings>
																					</dx:ASPxTextBox>
																				</td>
																				<td>
																					<dx:ASPxLabel ID="ASPxLabel10" runat="server" CssClass="lblsmallFont" Text="minutes">
																					</dx:ASPxLabel>
																				</td>
																				<td>
																					&nbsp;
																				</td>
																				<td>
																					<dx:ASPxLabel ID="ASPxLabel9" runat="server" CssClass="lblsmallFont" Text="Failures before Alert:"
																						Wrap="False">
																					</dx:ASPxLabel>
																				</td>
																				<td>
																					<dx:ASPxTextBox ID="SrvAtrFailBefAlertTextBox" runat="server" CssClass="txtsmall"
																						Text="2">
																						<MaskSettings ErrorText="Enter number between 1 to 100" Mask="&lt;0..9999&gt;" ShowHints="True" />
																						<MaskSettings Mask="&lt;0..9999&gt;" ErrorText="Enter number between 1 to 100" ShowHints="True">
																						</MaskSettings>
																						<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																							<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																								ValidationExpression="^\d+$" />
																							<RequiredField ErrorText="Please enter the failure threshold.  How many times can the server be down before an alert is sent?"
																								IsRequired="True" />
																							<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																								ValidationExpression="^\d+$"></RegularExpression>
																							<RequiredField IsRequired="True" ErrorText="Please enter the failure threshold.  How many times can the server be down before an alert is sent?">
																							</RequiredField>
																						</ValidationSettings>
																					</dx:ASPxTextBox>
																				</td>
																				<td>
																					<dx:ASPxLabel ID="ASPxLabel8" runat="server" CssClass="lblsmallFont" Text="consecutive failures">
																					</dx:ASPxLabel>
																				</td>
																				<td>
																					&nbsp;
																				</td>
																				<td>
																					&nbsp;
																					<dx:ASPxLabel ID="lblServerID" runat="server" Visible="False">
																					</dx:ASPxLabel>
																				</td>
																			</tr>
																			<tr>
																				<td>
																					<dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" Text="Retry Interval:">
																					</dx:ASPxLabel>
																				</td>
																				<td>
																					<dx:ASPxTextBox ID="SrvAtrRetryIntvlTextBox" runat="server" CssClass="txtsmall" Text="2">
																						<MaskSettings Mask="&lt;1..9999&gt;" />
																						<MaskSettings Mask="&lt;1..9999&gt;"></MaskSettings>
																						<ValidationSettings CausesValidation="True">
																							<RegularExpression ErrorText="Please enter a numeric value using the numbers only (1-9)."
																								ValidationExpression="^\d+$" />
																							<RegularExpression ErrorText="Please enter a numeric value using the numbers only (1-9)."
																								ValidationExpression="^\d+$"></RegularExpression>
																						</ValidationSettings>
																					</dx:ASPxTextBox>
																				</td>
																				<td>
																					<dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="lblsmallFont" Text="minutes">
																					</dx:ASPxLabel>
																				</td>
																				<td>
																					&nbsp;
																				</td>
																				<td>
									        									    <dx:ASPxLabel ID="ASPxLabel51" runat="server" CssClass="lblsmallFont" 
                                                                                        Text="Availability Index Threshold:">
                                                                                    </dx:ASPxLabel>
																				</td>
																				<td>
																					<dx:ASPxTextBox ID="SrvAIThresholdTextBox" runat="server" CssClass="txtsmall" Text="20">
                                                                                        <MaskSettings Mask="&lt;1..999&gt;" />
																						<MaskSettings Mask="&lt;1..999&gt;"></MaskSettings>
																						<ValidationSettings CausesValidation="True">
																							<RegularExpression ErrorText="Please enter a numeric value using the numbers only (1-9)."
																								ValidationExpression="^\d+$" />
																							<RegularExpression ErrorText="Please enter a numeric value using the numbers only (1-9)."
																								ValidationExpression="^\d+$"></RegularExpression>
																						</ValidationSettings>
                                                                                    </dx:ASPxTextBox>
																				</td>
																				<td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel22" runat="server" CssClass="lblsmallFont" Text="%">
                                                                                    </dx:ASPxLabel>
																				</td>
																				<td>
																					&nbsp;
																				</td>
																			</tr>
																		</table>
																	</dx:PanelContent>
																</PanelCollection>
															</dx:ASPxRoundPanel>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<dx:ASPxRoundPanel runat="server" GroupBoxCaptionOffsetY="-24px" HeaderText="Mail Settings"
																Width="100%" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" CssPostfix="Glass"
																CssFilePath="~/App_Themes/Glass/{0}/styles.css" ID="ASPxRoundPanel3">
																<ContentPaddings PaddingTop="10px" PaddingBottom="10px" PaddingLeft="4px"></ContentPaddings>
																<HeaderStyle Height="23px"></HeaderStyle>
																<PanelCollection>
																	<dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
																		<table>
																			<tr>
																				<td valign="top">
																					<table>
																						<tr>
																							<td valign="top">
																								<dx:ASPxLabel runat="server" Text="Pending Mail Threshold:" ID="ASPxLabel23" CssClass="lblsmallFont"
																									Wrap="False">
																								</dx:ASPxLabel>
																							</td>
																							<td valign="top">
																								<dx:ASPxTextBox runat="server" CssClass="txtsmall" ID="SrvAtrPendingMailThTextBox"
																									Text="200">
																									<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>
																									<MaskSettings Mask="&lt;0..99999&gt;" />
																									<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																										<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																											ValidationExpression="^\d+$" />
																										<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																											ValidationExpression="^\d+$"></RegularExpression>
																									</ValidationSettings>
																								</dx:ASPxTextBox>
																							</td>
																						</tr>
																						<tr>
																							<td valign="top">
																								<dx:ASPxLabel ID="ASPxLabel25" runat="server" CssClass="lblsmallFont" Text="Dead Mail Threshold:"
																									Wrap="False">
																								</dx:ASPxLabel>
																							</td>
																							<td valign="top">
																								<dx:ASPxTextBox ID="SrvAtrDeadMailThTextBox" runat="server" CssClass="txtsmall" Text="500">
																									<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>
																									<MaskSettings Mask="&lt;0..99999&gt;" />
																									<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip">
																										<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																											ValidationExpression="^\d+$" />
																										<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																											ValidationExpression="^\d+$"></RegularExpression>
																									</ValidationSettings>
																								</dx:ASPxTextBox>
																							</td>
																						</tr>
																						<tr>
																							<td valign="top">
																								<dx:ASPxLabel ID="ASPxLabel26" runat="server" CssClass="lblsmallFont" Text="Held Mail Threshold:"
																									Wrap="False">
																								</dx:ASPxLabel>
																							</td>
																							<td valign="top">
																								<dx:ASPxTextBox ID="SrvAtrHeldMailThTextBox" runat="server" CssClass="txtsmall" Text="200">
																									<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>
																									<MaskSettings Mask="&lt;0..99999&gt;" />
																									<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																										<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																											ValidationExpression="^\d+$" />
																										<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																											ValidationExpression="^\d+$"></RegularExpression>
																									</ValidationSettings>
																								</dx:ASPxTextBox>
																							</td>
																						</tr>
																					</table>
																				</td>
																				<td valign="top">
																					<table>
																						<tr>
																							<td colspan="3" valign="top">
																								<dx:ASPxCheckBox ID="SendRouterRestartCheckBox" runat="server" CheckState="Unchecked"
																									Text="Send 'Tell Router Restart' command if pending mail exceeds 1.5 times the threshold"
																									CssClass="lblsmallFont" Value='<%# Eval("SendRouterRestart") %>'>
																								</dx:ASPxCheckBox>
																							</td>
																						</tr>
																						<tr>
																							<td valign="top">
																								<dx:ASPxCheckBox ID="SrvAtrDelDeadMailCheckBox" runat="server" CheckState="Unchecked"
																									CssClass="lblsmallFont" Text="Delete dead mail if count exceeds" Wrap="False">
																								</dx:ASPxCheckBox>
																							</td>
																							<td valign="top">
																								<dx:ASPxTextBox ID="SrvAtrDeadMailCountTextBox" runat="server" CssClass="txtsmall">
																									<MaskSettings Mask="&lt;0..99999&gt;" />
																									<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																										<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																											ValidationExpression="^\d+$" />
																									</ValidationSettings>
																								</dx:ASPxTextBox>
																							</td>
																							<td>
																								<dx:ASPxLabel ID="ASPxLabel17" runat="server" Text="documents" CssClass="lblsmallFont">
																								</dx:ASPxLabel>
																							</td>
																						</tr>
																						<tr>
																							<td colspan="3" valign="top">
																								<dx:ASPxCheckBox ID="SrvAtrstopcountingCheckBox" runat="server" CheckState="Unchecked"
																									CssClass="lblsmallFont" Text="Stop counting if mail exceeds the threshold" Wrap="False">
																								</dx:ASPxCheckBox>
																							</td>
																						</tr>
																					</table>
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
															<dx:ASPxRoundPanel ID="ASPxRoundPanel13" runat="server" HeaderText="Traveler Settings"
																Theme="Glass" Width="100%">
																<PanelCollection>
																	<dx:PanelContent ID="PanelContent4" runat="server">
																		<table>
																			<tr>
																				<td>
																					<dx:ASPxLabel ID="ASPxLabel15" runat="server" CssClass="lblsmallFont" Text="External alias name:"
																						Wrap="False">
																					</dx:ASPxLabel>
																				</td>
																				<td>
																					<dx:ASPxTextBox ID="ExternalAliastextBox" runat="server">
																					</dx:ASPxTextBox>
																				</td>
																				<td>
																					&nbsp;
																				</td>
																				<td>
																					<dx1:ASPxCheckBox ID="RequireSLLCheckBox" runat="server" CheckState="Unchecked" CssClass="lblsmallFont"
																						Text="This server requires SSL" Value='<%# Eval("RequireSSL") %>'>
																					</dx1:ASPxCheckBox>
																				</td>
																				<td>
																					&nbsp;
																				</td>
																				<td>
																					<dx1:ASPxCheckBox ID="EnableServletScan" runat="server" CheckState="Unchecked" CssClass="lblsmallFont" Visible="false"
																						Text="Scan servlet" Value='<%# Eval("EnableServletScan") %>'>
																					</dx1:ASPxCheckBox>
																				</td>
																			</tr>
																			<tr>
																				<td>
																					<dx:ASPxLabel ID="ASPxLabel50" runat="server" CssClass="lblsmallFont" Text="Credentials:">
																					</dx:ASPxLabel>
																				</td>
																				<td>
																					<dx:ASPxComboBox ID="CredentialsComboBox" runat="server" AutoPostBack="true" OnSelectedIndexChanged="CredentialsComboBox_SelectedIndexChanged">
																					</dx:ASPxComboBox>
																				</td>
																				<td>
																					&nbsp;
																				</td>
																				<td>
																					<dx1:ASPxCheckBox ID="EnableTravelerCheckBox" runat="server" CheckState="Unchecked" Visible="false"
																						CssClass="lblsmallFont" Text="Enable Traveler HA - back end data store" Value='<%# Eval("EnableTravelerBackend") %>'
																						OnCheckedChanged="EnableTravelerCheckBox_CheckedChanged">
																					</dx1:ASPxCheckBox>
																				</td>
																				<td>
																					&nbsp;
																				</td>
																				<td>
																					<dx1:ASPxCheckBox ID="ScanTravelerServer" runat="server" CheckState="Unchecked" CssClass="lblsmallFont" Visible="false"
																						Text="Scan Traveler Server" Value='<%# Eval("ScanTravelerServer") %>'>
																					</dx1:ASPxCheckBox>
																				</td>
																			</tr>
																			<tr>
																				<td>
																					<dx:ASPxButton ID="ASPxButton1" runat="server" Text="Add Credentials" CssClass="sysButton"
																						OnClick="btn_clickcopyprofile" CausesValidation="false" Visible="true" UseSubmitBehavior="false">
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
                                                            <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" Width="100%" 
                                                                HeaderText="EXJournal Settings" Theme="Glass" Visible="false" 
                                                                EnableHierarchyRecreation="False">
                                                                <PanelCollection>
<dx:PanelContent ID="PanelContent6" runat="server">
    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                   <Triggers>
            <asp:AsyncPostBackTrigger ControlID="DurationTrackBar" />
        </Triggers>
        <ContentTemplate>
        <table width="100%">
        <tr>
            <td colspan="5">
                <dx1:ASPxCheckBox ID="EXJournalScanCheckBox" 
                    ClientInstanceName="EXJournalScanCheckBox" runat="server" 
                    CheckState="Unchecked" Text="Enabled for EXJournal scanning" 
                    oncheckedchanged="EXJournalScanCheckBox_CheckedChanged">
                    <ClientSideEvents CheckedChanged="function(s,e){OnCheckedChangedEXJScan(s,e)}" />
                </dx1:ASPxCheckBox>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="5">
                <dx:ASPxCheckBox ID="EXJournalLookBackCheckBox" 
                    ClientInstanceName="EXJournalLookBackCheckBox" runat="server"
                    CheckState="Unchecked" Text="Enabled for look back" 
                    oncheckedchanged="EXJournalLookBackCheckBox_CheckedChanged">
                    <ClientSideEvents CheckedChanged="function(s,e){OnCheckedChangedLookBackScan(s,e)}" />
                </dx:ASPxCheckBox>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
                            <tr>
                                <td>
                                    <dx1:ASPxLabel ID="lblStartTime" ClientInstanceName="lblStartTime" runat="server" CssClass="lblsmallFont" 
                                        Text="Start Time:">
                                    </dx1:ASPxLabel>
                                </td>
                                <td>
								<%--2/8/2016 Durga Added forVSPLUS-2582--%>

                                    <dx1:ASPxTimeEdit ID="StartTimeEdit" runat="server" AutoPostBack="True" 
                                        ClientInstanceName="StartTime" OnValueChanged="StartTimeEdit_ValueChanged" 
                                        Width="100px" EditFormat="Time" DateTime="1/2/2015">
                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField ErrorText="Please enter Start Time." />
                                        </ValidationSettings>
                                    </dx1:ASPxTimeEdit>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <dx1:ASPxLabel ID="lblEndTime1" ClientInstanceName="lblEndTime1" runat="server" CssClass="lblsmallFont" 
                                        Text="End Time:">
                                    </dx1:ASPxLabel>
                                </td>
                                <td>
                                    <dx1:ASPxLabel ID="lblEndTime" runat="server" ClientInstanceName="EndTime" 
                                        CssClass="lblsmallFont">
                                    </dx1:ASPxLabel>
                                </td>
                                <td>
                                    &nbsp;</td>
                                <td style="width:50px">
                                    <dx1:ASPxLabel ID="lblDuration1" runat="server"  
                                        ClientInstanceName="lblDuration1" CssClass="lblsmallFont" Text="Duration(minutes):">
                                    </dx1:ASPxLabel>
                                </td>
                                <td>
                                    <dx1:ASPxTextBox ID="MaintDurationTextBox" runat="server" 
                                        ClientInstanceName="MaintDuration" Width="100px">
                                        <ClientSideEvents LostFocus="OnTextChanged" />
                                        <MaskSettings Mask="&lt;0..1440&gt;" />
                                        <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                            SetFocusOnError="True">
                                            <RequiredField ErrorText="Please enter Duration." />
                                        </ValidationSettings>
                                    </dx1:ASPxTextBox>
                                </td>
                                <td>
                                    <dx1:ASPxLabel ID="lblDuration" runat="server" ClientInstanceName="Duration" 
                                        CssClass="lblsmallFont">
                                    </dx1:ASPxLabel>
                                </td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <dx1:ASPxLabel ID="lblLookBack" runat="server" ClientInstanceName="lblLookBack" 
                                        CssClass="lblsmallFont" Text="Look Back Period:">
                                    </dx1:ASPxLabel>
                                </td>
                                <td>
                                    <dx1:ASPxTextBox ID="LookBackPeriodTextBox" runat="server" 
                                        ClientInstanceName="LookBackPeriodTextBox" CssClass="txtsmall" Width="100px">
                                        <MaskSettings Mask="&lt;0..9999&gt;" />
                                        <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                            SetFocusOnError="True">
                                            <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                ValidationExpression="^\d+$" />
                                        </ValidationSettings>
                                    </dx1:ASPxTextBox>
                                </td>
                                <td>
                                    <dx1:ASPxLabel ID="lblMin" runat="server" ClientInstanceName="lblMin" 
                                        CssClass="lblsmallFont" Text="minutes">
                                    </dx1:ASPxLabel>
                                </td>
                                <td>
                                    &nbsp;</td>
                            </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;
            </td>
            <td colspan="2">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td colspan="3">
                <dx1:ASPxTrackBar ID="DurationTrackBar" runat="server" AutoPostBack="True" 
                    ClientInstanceName="trackBar" EnableViewState="False" LargeTickEndValue="1440" 
                    LargeTickInterval="240" MaxValue="1440" 
                    OnPositionChanged="DurationTrackBar_PositionChanged" Position="0" 
                    PositionStart="0" ScalePosition="LeftOrTop" Step="5" Theme="Office2010Blue" 
                    Width="100%">

                    <ValueToolTipStyle >

           

                    </ValueToolTipStyle>
                </dx1:ASPxTrackBar>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
            </ContentTemplate>
    </asp:UpdatePanel>            
    
    
                                                                    </dx:PanelContent>
</PanelCollection>
                                                            </dx:ASPxRoundPanel>
                                                        </td>
                                                    </tr>
													<tr>
														<td>
															<dx:ASPxRoundPanel ID="ASPxRoundPanel5" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
																CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Optional Scan Settings"
																SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%" Height="50%">
																<ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
																<HeaderStyle Height="23px"></HeaderStyle>
																<PanelCollection>
																	<dx:PanelContent ID="PanelContent5" runat="server">
																		<table>
																			<tr>
																				<td>
																					<div id="dbhealthDiv" class="info">
																						The database health service queries the Domino server on a daily basis to produce
																						an inventory of the Notes databases on that server.
																						<br />
																						This information is used to populate a variety of screens, most notably the 'Biggest
																						Mail Files' report and 'Mail Files by Template'.</div>
																				</td>
																			</tr>
																			<tr>
																				<td>
																					<dx:ASPxCheckBox ID="SrvAtrDBHealthCheckBox" runat="server" CheckState="Unchecked"
																						CssClass="lblsmallFont" Text="Scan this server for database information">
																					</dx:ASPxCheckBox>
																					<dx:ASPxCheckBox ID="logCheckBox" Visible="false" runat="server" CheckState="Unchecked" CssClass="lblsmallFont"
																						Text="Scan this server's log.nsf entries">
																					</dx:ASPxCheckBox>
																					<dx:ASPxCheckBox ID="agentlogCheckBox" Visible="false" runat="server" CheckState="Unchecked" CssClass="lblsmallFont"
																						Text="Scan this server's agentlog.nsf entries">
																					</dx:ASPxCheckBox>
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
															<dx:ASPxRoundPanel ID="ASPxRoundPanel4" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
																CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Alert Notification Group"
																Height="50px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Visible="False"
																Width="837px">
																<ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
																<HeaderStyle Height="23px"></HeaderStyle>
																<PanelCollection>
																	<dx:PanelContent ID="PanelContent7" runat="server">
																		<table>
																			<tr>
																				<td colspan="3" style="text-align: justify">
																					<asp:Label ID="Label1" runat="server" CssClass="lblsmallFont" Text="&lt;b&gt;Additional Notification Group: &lt;/b&gt;Use the field below to complement the general alert settings for this specific server.  If you enter a value here, alerts for this server will &lt;b&gt;also &lt;/b&gt;go to this person or group.  &lt;br/&gt;&lt;br/&gt;Enter any valid email address or group name."></asp:Label>
																				</td>
																			</tr>
																			<tr>
																				<td>
																					<table width="620px">
																						<tr>
																							<td width="15%">
																								<dx:ASPxLabel ID="ASPxLabel48" runat="server" CssClass="lblsmallFont" Text="Send To:">
																								</dx:ASPxLabel>
																							</td>
																							<td width="25%">
																								<dx:ASPxTextBox ID="SendTextBox" runat="server" Width="170px">
																								</dx:ASPxTextBox>
																							</td>
																							<td>
																								<dx:ASPxButton ID="sendmailBrowsButton" runat="server" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
																									CssPostfix="Office2010Blue" OnClick="sendmailBrowsButton_Click" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
																									Text="..." Visible="False">
																								</dx:ASPxButton>
																							</td>
																						</tr>
																					</table>
																				</td>
																			</tr>
																		</table>
																	</dx:PanelContent>
																</PanelCollection>
															</dx:ASPxRoundPanel>
														</td>
													</tr>
												</table>
											</dx:ContentControl>
										</ContentCollection>
									</dx:TabPage>
									<dx:TabPage Text="Server Tasks" Name="Server Tasks" >
										<TabImage Url="~/images/cog.png">
										</TabImage>
										<ContentCollection>
											<dx:ContentControl ID="ContentControl2" runat="server" SupportsDisabledAttribute="True">
												
												<div class="info">
													Server Tasks are programs that run in the environment of the IBM Domino server.
													Common tasks include HTTP, Router, etc. You do not need to monitor all the server
													tasks, only those that you consider critical. If you are not sure what tasks are
													running currently, click the Currently Running Tasks button.<br />If a server 
                                                    task you need does not appear on the list, you may add it using the Actions 
                                                    column button.
												</div>
												<table width="100%">
													<tr>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxButton ID="NewButton" runat="server" CssClass="sysButton" Text="New" AutoPostBack="False">
                                                                        <ClientSideEvents Click="function() { SrvTskDSTaskSettingsGridView.AddNewRow(); }" />
                                                                            <Image Url="~/images/icons/add.png">
                                                                            </Image>
                                                                        </dx:ASPxButton>
                                                                    </td>
                                                                    <td>
															<dx:ASPxButton ID="SrvTskCurRunTskButton0" runat="server" CssClass="sysButton" OnClick="SrvTskCurRunTskButton_Click"
																Text="Currently Running Tasks" UseSubmitBehavior="false">
															</dx:ASPxButton>
														</td>
                                                                </tr>
                                                            </table>
                                                        </td>
														
													</tr>
													<tr>
														<td>
															<dx:ASPxGridView ID="SrvTskDSTaskSettingsGridView" runat="server" AutoGenerateColumns="False"
																ClientInstanceName="SrvTskDSTaskSettingsGridView" CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css"
																CssPostfix="Office2010Silver" KeyFieldName="MyID" OnHtmlRowCreated="SrvTskDSTaskSettingsGridView_HtmlRowCreated"
																OnRowDeleting="SrvTskDSTaskSettingsGridView_RowDeleting" OnRowInserting="SrvTskDSTaskSettingsGridView_RowInserting"
																OnRowUpdating="SrvTskDSTaskSettingsGridView_RowUpdating" Width="100%" OnHtmlDataCellPrepared="SrvTskDSTaskSettingsGridView_HtmlDataCellPrepared"
																ViewStateMode="Enabled" OnAutoFilterCellEditorInitialize="SrvTskDSTaskSettingsGridView_AutoFilterCellEditorInitialize"
																OnCellEditorInitialize="SrvTskDSTaskSettingsGridView_CellEditorInitialize" EnableTheming="True"
																Theme="Office2003Blue" OnPageSizeChanged="SrvTskDSTaskSettingsGridView_PageSizeChanged">
																<SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn"></SettingsBehavior>
																<SettingsEditing EditFormColumnCount="1">
																</SettingsEditing>
																<SettingsText ConfirmDelete="Are you sure you want to delete this record?"></SettingsText>
																<Columns>
																	<dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" ShowInCustomizationForm="True"
																		Width="70px" VisibleIndex="0">
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
																		<CellStyle>
																			<Paddings Padding="3px" />
																			<Paddings Padding="3px"></Paddings>
																		</CellStyle>
																		<ClearFilterButton>
																			<Image Url="~/images/clear.png">
																			</Image>
																		</ClearFilterButton>
																		<HeaderStyle CssClass="GridCssHeader1" />
																	</dx:GridViewCommandColumn>
																	<dx:GridViewDataTextColumn Caption="MyID" FieldName="MyID" ShowInCustomizationForm="True"
																		Visible="False" VisibleIndex="1">
																	</dx:GridViewDataTextColumn>
																	<dx:GridViewDataCheckColumn Caption="Enabled" FieldName="Enabled" ShowInCustomizationForm="True"
																		VisibleIndex="2">
																		<Settings AllowAutoFilter="False" />
																		<Settings AllowAutoFilter="False"></Settings>
																		<EditCellStyle CssClass="GridCss">
																		</EditCellStyle>
																		<EditFormCaptionStyle CssClass="GridCss">
																		</EditFormCaptionStyle>
																		<HeaderStyle CssClass="GridCssHeader1">
																			<Paddings Padding="5px" />
																			<Paddings Padding="5px"></Paddings>
																		</HeaderStyle>
																		<CellStyle CssClass="GridCss1">
																		</CellStyle>
																	</dx:GridViewDataCheckColumn>
																	<dx:GridViewDataComboBoxColumn Caption="Task Name" FieldName="TaskName" ShowInCustomizationForm="True"
																		VisibleIndex="3">
                                                                        <Settings AllowAutoFilter="false" AllowAutoFilterTextInputTimer="False"></Settings>
																		<PropertiesComboBox ><ClearButton Visibility="True" ></ClearButton>
																		<ValidationSettings>
																		<RequiredField ErrorText="Please Select a Task Name" IsRequired="True" />
																		</ValidationSettings>
																		</PropertiesComboBox>
																		
																		<EditCellStyle CssClass="GridCss">
																		</EditCellStyle>
																		<EditFormCaptionStyle CssClass="GridCss">
																		</EditFormCaptionStyle>
																		<HeaderStyle CssClass="GridCssHeader" />
																		<CellStyle CssClass="GridCss">
																		</CellStyle>
																	</dx:GridViewDataComboBoxColumn>
																	<dx:GridViewDataCheckColumn Caption="Load" FieldName="SendLoadCommand" ShowInCustomizationForm="True"
																		VisibleIndex="4">
																		<Settings AllowAutoFilter="False"></Settings>
																		<EditFormSettings Caption="If Task not loaded, send &#39;Load&#39; command to server (i.e. try to start it)">
																		</EditFormSettings>
																		<Settings AllowAutoFilter="False" />
																		<EditFormSettings Caption="If Task not loaded, send 'Load' command to server (i.e. try to start it)" />
																		<EditCellStyle CssClass="GridCss">
																		</EditCellStyle>
																		<EditFormCaptionStyle CssClass="GridCss">
																		</EditFormCaptionStyle>
																		<HeaderStyle CssClass="GridCssHeader1" />
																		<CellStyle CssClass="GridCss1">
																		</CellStyle>
																	</dx:GridViewDataCheckColumn>
																	<dx:GridViewDataCheckColumn Caption="Restart ASAP" FieldName="SendRestartCommand"
																		ShowInCustomizationForm="True" VisibleIndex="5">
																		<Settings AllowAutoFilter="False"></Settings>
																		<EditFormSettings Caption="If Task is HUNG, or LOAD TASK fails, send &#39;Tell Server Restart&#39; command, AS SOON AS POSSIBLE">
																		</EditFormSettings>
																		<Settings AllowAutoFilter="False" />
																		<EditFormSettings Caption="If Task is HUNG, or LOAD TASK fails, send 'Tell Server Restart' command, AS SOON AS POSSIBLE" />
																		<EditCellStyle CssClass="GridCss">
																		</EditCellStyle>
																		<EditFormCaptionStyle CssClass="GridCss">
																		</EditFormCaptionStyle>
																		<HeaderStyle CssClass="GridCssHeader1" />
																		<CellStyle CssClass="GridCss1">
																		</CellStyle>
																	</dx:GridViewDataCheckColumn>
																	<dx:GridViewDataCheckColumn Caption="Restart Later" FieldName="RestartOffHours" ShowInCustomizationForm="True"
																		VisibleIndex="6">
																		<Settings AllowAutoFilter="False"></Settings>
																		<EditFormSettings Caption="If Task is HUNG, or LOAD TASK fails, send &#39;Tell Server Restart&#39; command, OFF PEAK HOURS ONLY">
																		</EditFormSettings>
																		<Settings AllowAutoFilter="False" />
																		<EditFormSettings Caption="If Task is HUNG, or LOAD TASK fails, send 'Tell Server Restart' command, OFF PEAK HOURS ONLY" />
																		<EditCellStyle CssClass="GridCss">
																		</EditCellStyle>
																		<EditFormCaptionStyle CssClass="GridCss">
																		</EditFormCaptionStyle>
																		<HeaderStyle CssClass="GridCssHeader1" />
																		<CellStyle CssClass="GridCss1">
																		</CellStyle>
																	</dx:GridViewDataCheckColumn>
																	<dx:GridViewDataCheckColumn Caption="Disallow" FieldName="SendExitCommand" ShowInCustomizationForm="True"
																		VisibleIndex="7">
																		<Settings AllowAutoFilter="False"></Settings>
																		<EditFormSettings Visible="True" Caption="If Task is running, send &#39;Task exit&#39; command (i.e prohibit this command)">
																		</EditFormSettings>
																		<PropertiesCheckEdit>
																			<CheckBoxStyle HorizontalAlign="Left" Wrap="True" />
																			<Style HorizontalAlign="Left" Wrap="True">
																				
																			</Style>
																		</PropertiesCheckEdit>
																		<Settings AllowAutoFilter="False" />
																		<EditFormSettings Caption="If Task is running, send 'Task exit' command (i.e prohibit this command)"
																			Visible="True" />
																		<EditCellStyle CssClass="GridCss">
																		</EditCellStyle>
																		<EditFormCaptionStyle CssClass="GridCss">
																		</EditFormCaptionStyle>
																		<HeaderStyle CssClass="GridCssHeader1" />
																		<CellStyle CssClass="GridCss1">
																		</CellStyle>
																	</dx:GridViewDataCheckColumn>
																</Columns>
																<SettingsBehavior ConfirmDelete="True" />
																<SettingsBehavior ColumnResizeMode="NextColumn" />
																<SettingsBehavior ColumnResizeMode="NextColumn"></SettingsBehavior>
																<SettingsPager PageSize="50">
																	<PageSizeItemSettings Visible="True">
																	</PageSizeItemSettings>
																</SettingsPager>
																<Settings ShowFilterRow="True" ShowGroupPanel="True" />
																<Settings ShowFilterRow="True" ShowGroupPanel="True"></Settings>
																<SettingsEditing EditFormColumnCount="1" />
																<SettingsText ConfirmDelete="Are you sure you want to delete this record?" />
																<Styles>
																	<Header ImageSpacing="5px" SortingImageSpacing="5px">
																	</Header>
																	<GroupRow Font-Bold="True">
																	</GroupRow>
																	<AlternatingRow CssClass="GridAltRow">
																	</AlternatingRow>
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
																		<table cellspacing="10" style="width: 100%;">
																			<tr>
																				<td align="right" valign="top" width="10px">
																					<asp:HiddenField ID="SrvTskGrdMyIDHiddenField" runat="server" Value='<%# Eval("MyID")%>' />
																					<dx:ASPxCheckBox ID="SrvTskGrdEnabledCheckBox" runat="server" CheckState="Unchecked"
																						Value='<%# Eval("Enabled") %>'>
																					</dx:ASPxCheckBox>
																				</td>
																				<td valign="top">
																					<dx:ASPxLabel ID="ASPxLabel41" runat="server" CssClass="lblsmallFont" Text="Enabled for Task Scanning">
																					</dx:ASPxLabel>
																				</td>
																				<td valign="top">
																					<dx:ASPxLabel ID="ASPxLabel42" runat="server" CssClass="lblsmallFont" Text="Task Name">
																					</dx:ASPxLabel>
																				</td>
																				<td valign="top">
																					<dx:ASPxComboBox ID="SrvTskGrdTaskComboBox" runat="server" CssClass="txtsmallFont" >
																					<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" ErrorText="Task Name is required.">
																				<RequiredField ErrorText="Task Name is required." IsRequired="True" />
																			</ValidationSettings>
																					</dx:ASPxComboBox>
																				</td>
																				
																			</tr>
																			<tr>
																				<td align="right" valign="top">
																					<dx:ASPxCheckBox ID="SrvTskGrdLoadCheckBox" runat="server" CheckState="Unchecked"
																						Value='<%# Eval("SendLoadCommand") %>'>
																					</dx:ASPxCheckBox>
																				</td>
																				<td valign="top">
																					<dx:ASPxLabel ID="ASPxLabel43" runat="server" CssClass="lblsmallFont" Text="If Task not loaded, send 'Load' command to server (i.e. try to start it)">
																					</dx:ASPxLabel>
																				</td>
																				<td align="right" valign="top">
																					<dx:ASPxCheckBox ID="SrvTskGrdRestartOffCheckBox" runat="server" CheckState="Unchecked"
																						Value='<%# Eval("RestartOffHours") %>'>
																					</dx:ASPxCheckBox>
																				</td>
																				<td valign="top">
																					<dx:ASPxLabel ID="ASPxLabel45" runat="server" CssClass="lblsmallFont" Text="If Task is HUNG, or LOAD TASK fails, send 'Tell Server Restart' command, OFF PEAK HOURS ONLY">
																					</dx:ASPxLabel>
																				</td>
																			</tr>
																			<tr>
																				<td align="right" valign="top">
																					<dx:ASPxCheckBox ID="SrvTskGrdRestartASAPCheckBox" runat="server" CheckState="Unchecked"
																						Value='<%# Eval("SendRestartCommand") %>'>
																					</dx:ASPxCheckBox>
																				</td>
																				<td valign="top">
																					<dx:ASPxLabel ID="ASPxLabel44" runat="server" CssClass="lblsmallFont" Text="If Task is HUNG, or LOAD TASK fails, send 'Tell Server Restart' command, AS SOON AS POSSIBLE">
																					</dx:ASPxLabel>
																				</td>
																				<td align="right" valign="top">
																					<dx:ASPxCheckBox ID="SrvTskGrdDisallowCheckBox" runat="server" CheckState="Unchecked"
																						Value='<%# Eval("SendExitCommand") %>' OnCheckedChanged="SrvTskGrdDisallowCheckBox_CheckedChanged"
																						AutoPostBack="True">
																					</dx:ASPxCheckBox>
																				</td>
																				<td valign="top">
																					<dx:ASPxLabel ID="ASPxLabel46" runat="server" CssClass="lblsmallFont" Text="If Task is running, send 'Task exit' command (i.e prohibit this command)">
																					</dx:ASPxLabel>
																				</td>
																			</tr>
																			<tr>
																				<td align="right" valign="top">
																					&nbsp;
																				</td>
																				<td valign="top">
																					<asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/update.gif" 
																						Visible="False" />
																					&nbsp;<asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/images/cancel.gif"
																						Visible="False" />
																				</td>
																				<td align="right" valign="top">
																					&nbsp;
																				</td>
																				<td align="right" valign="bottom">
																					<table cellspacing="3" style="width: 100%;">
																						<tr>
																							<td align="right">
																								<dx:ASPxGridViewTemplateReplacement ID="UpdateButton" runat="server" ReplacementType="EditFormUpdateButton" />
																							</td>
																							<td>
																								<dx:ASPxGridViewTemplateReplacement ID="CancelButton" runat="server" ReplacementType="EditFormCancelButton" />
																							</td>
																						</tr>
																					</table>
																				</td>
																			</tr>
																		</table>
																	</EditForm>
																</Templates>
															</dx:ASPxGridView>
														</td>
													</tr>
													<tr>
														<td>
															<dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Search SH TA...." CssClass="lblsmallFont"
																Visible="False">
															</dx:ASPxLabel>
														</td>
													</tr>
													<tr>
														<td>
															<asp:Button ID="btnDisable" runat="server" Text="click" OnClientClick="return false;"
																Style="display: none;" />
														</td>
													</tr>
												</table>
												
											</dx:ContentControl>
										</ContentCollection>
									</dx:TabPage>
									<dx:TabPage Text="Disk Settings">
										<TabImage Url="~/images/drive.png">
										</TabImage>
										<ContentCollection>
											<dx:ContentControl ID="ContentControl3" runat="server" SupportsDisabledAttribute="True">
												<table width="100%">
													<tr>
														<td>
															<dx:ASPxRoundPanel ID="DiskSettingsRoundPanel2" runat="server" Width="100%" HeaderText="Disk Settings"
																Theme="Glass">
																<PanelCollection>
																	<dx:PanelContent ID="PanelContent11" runat="server" SupportsDisabledAttribute="True">
																		<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
																			<Triggers>
																				<asp:AsyncPostBackTrigger ControlID="SelCriteriaRadioButtonList" />
																			</Triggers>
																			<ContentTemplate>
																				<table width="100%">
																					<tr>
																						<td valign="top">
																							<dx:ASPxRadioButtonList ID="SelCriteriaRadioButtonList" ClientInstanceName="diskSettingsRadioBtn"
																								runat="server" SelectedIndex="0" OnSelectedIndexChanged="SelCriteriaRadioButtonList_SelectedIndexChanged"
																								AutoPostBack="True" TextWrap="False">
																								<Items>
																									<dx:ListEditItem Selected="True" Text="All Disks - By Percentage" Value="0" />
																									<dx:ListEditItem Text="All Disks - By GB" Value="3" />
																									<dx:ListEditItem Text="Selected Disks" Value="1" />
																									<dx:ListEditItem Text="No Disk Alerts" Value="2" />
																								</Items>
																							</dx:ASPxRadioButtonList>
																						</td>
																						<td>
																							&nbsp;
																						</td>
																						<td valign="top">
																							<table>
																								<tr>
																									<td>
																										<div id="infoDiskDiv" class="info" runat="server" style="display: none">
																											The column 'Free Space Threshold' should be set to a value in GB or Percent (1 to
																											100) of the remaining free space when an alert should be generated.
																											<br />
																											The column 'Threshold Type' should be set accordingly in either percent or GB.
																										</div>
																										<dx:ASPxLabel ID="DiskGridInfo" runat="server" Text="Please select disks from the grid below:"
																											Visible="False" CssClass="lblsmallFont">
																										</dx:ASPxLabel>
																										<dx:ASPxLabel ID="DiskLabel" runat="server" Visible="True" CssClass="lblsmallFont">
																										</dx:ASPxLabel>
																									</td>
																								</tr>
																								<tr>
																									<td>
																										<dx:ASPxGridView ID="DiskGridView" ClientInstanceName="diskSettingsGrid" runat="server"
																											AutoGenerateColumns="False" Cursor="pointer" KeyFieldName="DiskName" Theme="Office2003Blue"
																											OnPageSizeChanged="DiskGridView_PageSizeChanged" OnPreRender="DiskGridView_PreRender"
																											OnRowUpdating="DiskGridView_RowUpdating" Visible="False" Width="100%">
																											<Columns>
																												<dx:GridViewDataTextColumn Caption="isSelected" FieldName="isSelected" VisibleIndex="12"
																													Visible="false">
																													<Settings AutoFilterCondition="Contains" />
																													<Settings AutoFilterCondition="Contains"></Settings>
																													<EditCellStyle CssClass="GridCss">
																													</EditCellStyle>
																													<EditFormCaptionStyle CssClass="GridCss" HorizontalAlign="Left">
																													</EditFormCaptionStyle>
																													<HeaderStyle CssClass="GridCssHeader" />
																													<CellStyle CssClass="GridCss">
																													</CellStyle>
																												</dx:GridViewDataTextColumn>
																												<dx:GridViewDataTextColumn Caption="Disk Name" FieldName="DiskName" ReadOnly="true"
																													VisibleIndex="4" Width="100px">
																													<Settings AutoFilterCondition="Contains" />
																													<Settings AutoFilterCondition="Contains"></Settings>
																													<EditCellStyle CssClass="GridCss">
																													</EditCellStyle>
																													<EditFormCaptionStyle CssClass="GridCss" HorizontalAlign="Left">
																													</EditFormCaptionStyle>
																													<HeaderStyle CssClass="GridCssHeader" />
																													<CellStyle CssClass="GridCss">
																													</CellStyle>
																												</dx:GridViewDataTextColumn>
																												<dx:GridViewDataTextColumn Caption="Free Space Threshold" FieldName="Threshold" VisibleIndex="9">
																													<PropertiesTextEdit DisplayFormatString="g" Width="50px">
																														<ValidationSettings>
																															<RegularExpression ValidationExpression="(^(100(?:\.0{1,2})?))|(?!^0*$)(?!^0*\.0*$)^\d{1,2}(\.\d{1,2})?$" />
																														</ValidationSettings>
																													</PropertiesTextEdit>
																													<DataItemTemplate>
																														<dx:ASPxTextBox ID="txtFreeSpaceThresholdValue" runat="server" Width="150px" Value='<%# Eval("Threshold") %>'>
																														</dx:ASPxTextBox>
																													</DataItemTemplate>
																													<Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
																													<HeaderStyle CssClass="GridCssHeader" />
																													<CellStyle CssClass="GridCss2">
																													</CellStyle>
																												</dx:GridViewDataTextColumn>
																												<dx:GridViewDataComboBoxColumn Caption="Threshold Type (% or GB)" FieldName="ThresholdType"
																													VisibleIndex="11">
																													<PropertiesComboBox Width="50px">
																													</PropertiesComboBox>
																													<DataItemTemplate>
																														<dx:ASPxComboBox ID="txtFreeSpaceThresholdType" runat="server" TextField="ThresholdType"
																															Value='<%# Eval("ThresholdType") %>' ValueField="ThresholdType" ValueType="System.String">
																															<Items>
																																<dx:ListEditItem Text="Percent" Value="Percent" />
																																<dx:ListEditItem Text="GB" Value="GB" />
																															</Items>
																														</dx:ASPxComboBox>
																													</DataItemTemplate>
																													<HeaderStyle CssClass="GridCssHeader" />
																													<CellStyle CssClass="GridCss">
																													</CellStyle>
																												</dx:GridViewDataComboBoxColumn>
																												<dx:GridViewCommandColumn Caption="Select" Visible="true" VisibleIndex="3" ShowSelectCheckbox="True">
																													<HeaderStyle CssClass="GridCssHeader" />
																													<CellStyle CssClass="GridCss1">
																													</CellStyle>
																												</dx:GridViewCommandColumn>
																												<dx:GridViewDataTextColumn Caption="Disk Size" FieldName="DiskSize" VisibleIndex="5">
																													<HeaderStyle CssClass="GridCssHeader" />
																													<CellStyle CssClass="GridCss2">
																													</CellStyle>
																												</dx:GridViewDataTextColumn>
																												<dx:GridViewDataTextColumn Caption="Disk Free Space" FieldName="DiskFree" VisibleIndex="6">
																													<HeaderStyle CssClass="GridCssHeader" />
																													<CellStyle CssClass="GridCss2">
																													</CellStyle>
																												</dx:GridViewDataTextColumn>
																												<dx:GridViewDataTextColumn Caption="Percent Free" FieldName="PercentFree" VisibleIndex="7">
																													<HeaderStyle CssClass="GridCssHeader" />
																													<CellStyle CssClass="GridCss2">
																													</CellStyle>
																												</dx:GridViewDataTextColumn>
																												<dx:GridViewDataTextColumn Caption="Disk Info" FieldName="DiskInfo" 
																													VisibleIndex="13">
																													<PropertiesTextEdit>
																														<ValidationSettings>
																															<RequiredField ErrorText="Please enter the comments For Disk utilization'" IsRequired="True" />
																														</ValidationSettings>
																													</PropertiesTextEdit>
																													<DataItemTemplate>
																														<dx:ASPxMemo ID="Diskinfo" SkinID="None" runat="server" Native="True" Height="40px" Value='<%# Eval("DiskInfo") %>' 	Width="150px" style="font-family:Arial;font-size:12px">
																														
																															<ClientSideEvents TextChanged="function(s, e) { DXEventMonitor.Trace(s, e, 'TextChanged') }">
																															</ClientSideEvents>
																														</dx:ASPxMemo>
																													</DataItemTemplate>
																													<Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
																													<HeaderStyle CssClass="GridCssHeader"  />
																													<CellStyle CssClass="GridCss" HorizontalAlign="Left">
																													</CellStyle>
																												</dx:GridViewDataTextColumn>
																											</Columns>
																											<SettingsPager PageSize="50" SEOFriendly="Enabled" Mode="ShowAllRecords">
																											</SettingsPager>
																											<SettingsEditing Mode="Inline">
																											</SettingsEditing>
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
																										</dx:ASPxGridView>
																										<dx:ASPxTrackBar ID="AdvDiskSpaceThTrackBar" runat="server" AutoPostBack="True" CssFilePath="~/App_Themes/Office2010Black/{0}/styles.css"
																											CssPostfix="Office2010Black" EnableViewState="false" OnValueChanged="AdvDiskSpaceThTrackBar_ValueChanged"
																											Position="10" PositionStart="10" ScalePosition="LeftOrTop" SmallTickFrequency="5"
																											SpriteCssFilePath="~/App_Themes/Office2010Black/{0}/sprite.css" Step="1" Width="100%"
																											Theme="Office2010Blue">
																										</dx:ASPxTrackBar>
																										<table>
																											<tr>
																												<td>
																													<dx:ASPxLabel ID="GBTitle" runat="server" Text="Current threshold:" CssClass="lblsmallFont"
																														Visible="false">
																													</dx:ASPxLabel>
																												</td>
																												<td>
																													<dx:ASPxTextBox ID="GBTextBox" runat="server" Width="170px" Visible="False">
																													</dx:ASPxTextBox>
																												</td>
																												<td>
																													<dx:ASPxLabel ID="GBLabel" runat="server" Text="GB free space" CssClass="lblsmallFont"
																														Visible="False">
																													</dx:ASPxLabel>
																												</td>
																											</tr>
																											<tr>
																												<td colspan="3">
																													&nbsp;
																												</td>
																											</tr>
																										</table>
																										<asp:Label ID="Label4" runat="server" ForeColor="#0033CC" CssClass="lblsmallFont"
																											Text="&nbsp;&lt;b&gt;Disk Space &lt;/b&gt;alerts will trigger if any of the drives on the Domino server fall below the threshold set."
																											Visible="True"></asp:Label>
																									</td>
																								</tr>
																							</table>
																						</td>
																					</tr>
																				</table>
																			</ContentTemplate>
																		</asp:UpdatePanel>
																	</dx:PanelContent>
																</PanelCollection>
															</dx:ASPxRoundPanel>
														</td>
													</tr>
												</table>
											</dx:ContentControl>
										</ContentCollection>
									</dx:TabPage>
									<dx:TabPage Text="Advanced">
										<TabImage Url="~/images/package_green.png">
										</TabImage>
										<ContentCollection>
											<dx:ContentControl ID="ContentControl4" runat="server" SupportsDisabledAttribute="True">
												<table  >
													<tr>
														<td valign="top">
															<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" >
																<ContentTemplate >
																	<dx:ASPxRoundPanel ID="MemRoundPanel9" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
																		CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Memory Usage Alert"
																		 SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"   Width="500px" Height="60px">
																		<ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px"   />
																		<HeaderStyle Height="23px"></HeaderStyle>
																		<PanelCollection>
																			<dx:PanelContent ID="PanelContent8" runat="server" SupportsDisabledAttribute="True">
																				<table cellspacing="3px" width="100%">
																					<tr>
																						<td>
																							<dx:ASPxLabel ID="MemLabel" runat="server" Visible="False">
																							</dx:ASPxLabel>
																							<dx:ASPxTrackBar ID="AdvMemoryThTrackBar" runat="server" AutoPostBack="True" CssFilePath="~/App_Themes/Office2010Black/{0}/styles.css"
																								CssPostfix="Office2010Black" EnableViewState="False" OnPositionChanged="AdvMemoryThTrackBar_PositionChanged"
																								Position="95" PositionStart="95" ScalePosition="LeftOrTop" SpriteCssFilePath="~/App_Themes/Office2010Black/{0}/sprite.css"
																								Step="1" Width="95%" Theme="Office2010Blue">
																								<ValueToolTipStyle>
																								</ValueToolTipStyle>
																							</dx:ASPxTrackBar>
																						</td>
																					</tr>
																					<tr>
																						<td align="left">
																							<asp:Label ID="Label5" runat="server" Style="color: #0033CC" Text="&lt;b&gt;Memory Utilization&lt;/b&gt; alerts will trigger if the percentage of memory in use on the server exceeds this threshold.  &lt;br/&gt;&lt;br/&gt; If you don't want to get memory alerts, set the threshold to zero."></asp:Label>
																						</td>
																					</tr>
																				</table>
																			</dx:PanelContent>
																		</PanelCollection>
																	</dx:ASPxRoundPanel>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
														<td valign="top" >
															<asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<dx:ASPxRoundPanel ID="CPURoundPanel" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
																		CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="CPU Utilization Alert"
																		SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="500px" Height="60px">
																		<ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
																		<HeaderStyle Height="23px"></HeaderStyle>
																		<PanelCollection>
																			<dx:PanelContent ID="PanelContent9" runat="server" SupportsDisabledAttribute="True">
																				<table cellspacing="3px" width="100%">
																					<tr>
																						<td>
																							<dx:ASPxLabel ID="CpuLabel" runat="server" Visible="False">
																							</dx:ASPxLabel>
																							<dx:ASPxTrackBar ID="AdvCPUThTrackBar" runat="server" AutoPostBack="True" CssFilePath="~/App_Themes/Office2010Black/{0}/styles.css"
																								CssPostfix="Office2010Black" EnableViewState="False" OnValueChanged="AdvCPUThTrackBar_ValueChanged"
																								Position="90" PositionStart="90" ScalePosition="LeftOrTop" SpriteCssFilePath="~/App_Themes/Office2010Black/{0}/sprite.css"
																								Step="1" Width="95%" Theme="Office2010Blue">
																							</dx:ASPxTrackBar>
																						</td>
																					</tr>
																					<tr>
																						<td align="left">
																							<asp:Label ID="Label6" runat="server" Style="color: #0033CC" Text="&lt;b&gt;CPU Utilization&lt;/b&gt; alerts will trigger if the CPU utilization rate exceeds this threshold. &lt;br/&gt;&lt;br/&gt;&lt;br/&gt;  If you don't want to get CPU alerts, set the threshold to zero. "></asp:Label>
																						</td>
																					</tr>
																				</table>
																			</dx:PanelContent>
																		</PanelCollection>
																	</dx:ASPxRoundPanel>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
													</tr>
													<tr>
														<td valign="top" colspan="2">
															<dx:ASPxRoundPanel ID="ASPxRoundPanel12" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
																CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="BlackBerry Enterprise Server"
																Height="50px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%"
																Visible="False">
																<ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
																<ContentPaddings Padding="2px" PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px">
																</ContentPaddings>
																<HeaderStyle Height="23px"></HeaderStyle>
																<PanelCollection>
																	<dx:PanelContent ID="PanelContent10" runat="server" SupportsDisabledAttribute="True">
																		<asp:Label ID="Label9" runat="server" Style="color: #0033CC" Text="The &lt;b&gt;BlackBerry Message Queue &lt;/b&gt;is a server task that runs on Domino servers which are running BlackBerry Enterprise Server. &lt;br/&gt;  If this task is running it will report the number of messages waiting to be forwarded to the RIM network."></asp:Label>
																		<table>
																			<tr>
																				<td colspan="3">
																					<dx:ASPxCheckBox ID="AdvMonitorBESNtwrkQCheckBox" runat="server" CheckState="Unchecked"
																						CssClass="lblsmallFont" Text="Monitor BES Network Queue">
																					</dx:ASPxCheckBox>
																				</td>
																			</tr>
																			<tr>
																				<td>
																					<dx:ASPxLabel ID="ASPxLabel38" runat="server" CssClass="lblsmallFont" Text="Alert if BES message queue exceeds:">
																					</dx:ASPxLabel>
																				</td>
																				<td>
																					<dx:ASPxTextBox ID="AdvBESMsgQTextBox" runat="server" CssClass="txtsmall">
																						<MaskSettings Mask="&lt;0..99999&gt;" />
																						<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>
																						<ValidationSettings ErrorDisplayMode="ImageWithTooltip" CausesValidation="True" SetFocusOnError="True">
																							<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																								ValidationExpression="^\d+$" />
																							<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																								ValidationExpression="^\d+$"></RegularExpression>
																						</ValidationSettings>
																					</dx:ASPxTextBox>
																				</td>
																				<td>
																					<dx:ASPxLabel ID="ASPxLabel39" runat="server" CssClass="lblsmallFont" Text="messages">
																					</dx:ASPxLabel>
																				</td>
																			</tr>
																		</table>
																	</dx:PanelContent>
																</PanelCollection>
															</dx:ASPxRoundPanel>
														</td>
													</tr>
													<tr>
														<td colspan="2">
															<dx:ASPxRoundPanel ID="ASPxRoundPanel6" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
																CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Server Running Days Alert"
																 SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"  Width="1005px" Height="60px">
																<ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
																<ContentPaddings Padding="2px" PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px">
																</ContentPaddings>
																<HeaderStyle Height="23px"></HeaderStyle>
																<PanelCollection>
																	<dx:PanelContent ID="PanelContent111" runat="server" SupportsDisabledAttribute="True">
																		<asp:Label ID="Label3" runat="server" Style="color: #0033CC" Text="Some companies have a practice to reboot Domino servers after a set number of days. Enter a value here if you would like to be notified if a server is running beyond this limit."></asp:Label>
																		<table width="480px">
																			<tr>
																				<td align="right">
																					<dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Alert if total elapsed running time exceeds:"
																						CssClass="lblsmallFont">
																					</dx:ASPxLabel>
																				</td>
																				<td align="left" width="50px">
																					<dx:ASPxTextBox ID="ServerDaysAlert" runat="server" Width="50px">
																					</dx:ASPxTextBox>
																				</td>
																				<td width="150px">
																					<dx:ASPxLabel ID="ASPxLabel13" runat="server" Text=" consecutive days" CssClass="lblsmallFont">
																					</dx:ASPxLabel>
																				</td>
																			</tr>
																		</table>
																	</dx:PanelContent>
																</PanelCollection>
															</dx:ASPxRoundPanel>
														</td>
													</tr>
													<tr>
														<td colspan="2">
															<dx:ASPxRoundPanel ID="ASPxRoundPanel10" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
																CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Cluster Replication Delays Alert"  Width="1005px" Height="60px"
																SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" >
																<ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
																<ContentPaddings Padding="2px" PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px">
																</ContentPaddings>
																<HeaderStyle Height="23px"></HeaderStyle>
																<PanelCollection>
																	<dx:PanelContent ID="PanelContent12" runat="server" SupportsDisabledAttribute="True">
																		<asp:Label ID="Label8" runat="server" Style="color: #0033CC" Text="&lt;b&gt;Cluster Replication Delay&lt;/b&gt; alerts will trigger if the amount of time  &lt;i&gt;(in seconds)&lt;/i&gt;  that it takes for a document created on this server to be pushed to other servers in the cluster rate exceeds this threshold.  If you don't want to get Cluster Replication Delay alerts, set the threshold to zero."></asp:Label>
																		<table>
																			<tr>
																				<td>
																					<dx:ASPxLabel ID="ASPxLabel36" runat="server" CssClass="lblsmallFont" Text="Alert if Cluster Replication Queue time exceeds:"
																						Wrap="False">
																					</dx:ASPxLabel>
																				</td>
																				<td>
																					<dx:ASPxTextBox ID="AdvClusterRepTextBox" runat="server" CssClass="txtsmall" Text="120">
																						<MaskSettings Mask="&lt;0..999999&gt;" />
																						<MaskSettings Mask="&lt;0..999999&gt;"></MaskSettings>
																						<ValidationSettings ErrorDisplayMode="ImageWithTooltip" CausesValidation="True" SetFocusOnError="True">
																							<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																								ValidationExpression="^\d+$" />
																							<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																								ValidationExpression="^\d+$"></RegularExpression>
																						</ValidationSettings>
																					</dx:ASPxTextBox>
																				</td>
																				<td>
																					<dx:ASPxLabel ID="ASPxLabel40" runat="server" CssClass="lblsmallFont" Text="seconds">
																					</dx:ASPxLabel>
																				</td>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel19" runat="server" CssClass="lblsmallFont" Text="Load another Cluster Replicator instance if cluster replication queue exceeds"
																						Wrap="False">
																					</dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                <dx:ASPxTextBox ID="LoadClusterRepTextBox" runat="server" CssClass="txtsmall" Text="120">
																						<MaskSettings Mask="&lt;0..999999&gt;" />
																						<MaskSettings Mask="&lt;0..999999&gt;"></MaskSettings>
																						<ValidationSettings ErrorDisplayMode="ImageWithTooltip" CausesValidation="True" SetFocusOnError="True">
																							<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																								ValidationExpression="^\d+$" />
																							<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																								ValidationExpression="^\d+$"></RegularExpression>
																						</ValidationSettings>
																					</dx:ASPxTextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel21" runat="server" CssClass="lblsmallFont" Text="seconds">
																					</dx:ASPxLabel>
                                                                                </td>
																			</tr>
																		</table>
																	</dx:PanelContent>
																</PanelCollection>
															</dx:ASPxRoundPanel>
														</td>
													</tr>
													<tr>
														<td colspan="2">
															<dx:ASPxRoundPanel ID="ASPxRoundPanel8" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
																CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Network Connectivity"
																Height="50px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Visible="False"
																Width="100%">
																<ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
																<ContentPaddings Padding="2px" PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px">
																</ContentPaddings>
																<HeaderStyle Height="23px"></HeaderStyle>
																<PanelCollection>
																	<dx:PanelContent ID="PanelContent13" runat="server" SupportsDisabledAttribute="True">
																		<table cellspacing="3px" width="100%">
																			<tr>
																				<td>
																					<asp:Label ID="Label7" runat="server" Style="color: #0033CC" Text="&lt;b&gt;Network Connectivity &lt;/b&gt;allows you to ping a hub or switch to see if the problem is the network or the server.   A PING request (ICMP ECHO) will be sent to the address."></asp:Label>
																				</td>
																			</tr>
																			<tr>
																				<td>
																					&nbsp;
																				</td>
																			</tr>
																			<tr>
																				<td>
																					<dx:ASPxCheckBox ID="AdvNtwrkConCheckBox" runat="server" CheckState="Unchecked" CssClass="lblsmallFont"
																						Text="Test for network connectivity">
																					</dx:ASPxCheckBox>
																				</td>
																			</tr>
																			<tr>
																				<td>
																					&nbsp;
																				</td>
																			</tr>
																			<tr>
																				<td>
																					<table>
																						<tr>
																							<td>
																								<dx:ASPxLabel ID="ASPxLabel34" runat="server" CssClass="lblsmallFont" Text="IP Address">
																								</dx:ASPxLabel>
																							</td>
																							<td>
																								<dx:ASPxTextBox ID="AdvIPAddressTextBox" runat="server" CssClass="txtmed">
																								</dx:ASPxTextBox>
																							</td>
																							<td width="70">
																							</td>
																							<td>
																								<dx:ASPxButton ID="AdvPingTestButton" runat="server" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
																									CssPostfix="Office2010Blue" Font-Bold="False" OnClick="AdvPingTestButton_Click"
																									SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="Ping Test">
																								</dx:ASPxButton>
																							</td>
																						</tr>
																					</table>
																				</td>
																			</tr>
																		</table>
																	</dx:PanelContent>
																</PanelCollection>
															</dx:ASPxRoundPanel>
														</td>
													</tr>
												</table>
											</dx:ContentControl>
										</ContentCollection>
									</dx:TabPage>
									<dx:TabPage Text="Maintenance Windows">
										<TabImage Url="~/images/application_view_tile.png">
										</TabImage>
										<ContentCollection>
											<dx:ContentControl ID="ContentControl5" runat="server" SupportsDisabledAttribute="True">
												<table width="100%">
													<tr>
														<td>
															<dx:ASPxButton ID="ToggleVeiwButton" runat="server" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
																CssPostfix="Office2010Blue" OnClick="ToggleVeiwButton_Click" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
																Text="Switch to Calendar View" Width="178px" Visible="False">
															</dx:ASPxButton>
														</td>
													</tr>
													<tr>
														<td>
															<div id="infoDivMW" class="info">
																Maintenance Windows are times when you do not want the server monitored. You can
																define maintenance windows using the Hours &amp; Maintenance\Maintenance menu option.
																Use the Actions column to modify maintenance windows information.
															</div>
														</td>
													</tr>
													<tr>
														<td>
															<dx:ASPxGridView ID="MaintWinListGridView" runat="server" AutoGenerateColumns="False"
																CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" CssPostfix="Office2010Blue"
																Cursor="pointer" KeyFieldName="ID" OnHtmlRowCreated="MaintWinListGridView_HtmlRowCreated"
																OnSelectionChanged="MaintWinListGridView_SelectionChanged" Theme="Office2003Blue"
																Width="100%" OnPageSizeChanged="MaintWinListGridView_PageSizeChanged">
																<Columns>
																	<dx:GridViewCommandColumn ShowInCustomizationForm="True" Visible="False" VisibleIndex="0">
																		<ClearFilterButton Visible="True">
																		</ClearFilterButton>
																	</dx:GridViewCommandColumn>
																	<dx:GridViewDataTextColumn FieldName="ID" ReadOnly="True" ShowInCustomizationForm="True"
																		Visible="False" VisibleIndex="1">
																		<EditFormSettings Visible="False" />
																		<EditFormSettings Visible="False"></EditFormSettings>
																	</dx:GridViewDataTextColumn>
																	<dx:GridViewDataTextColumn FieldName="ServerID" ReadOnly="True" ShowInCustomizationForm="True"
																		Visible="False" VisibleIndex="2">
																		<EditFormSettings Visible="False" />
																		<EditFormSettings Visible="False"></EditFormSettings>
																	</dx:GridViewDataTextColumn>
																	<dx:GridViewDataTextColumn Caption="Name" FieldName="Name" ShowInCustomizationForm="True"
																		VisibleIndex="3">
																		<Settings AutoFilterCondition="Contains" />
																		<Settings AutoFilterCondition="Contains"></Settings>
																		<EditCellStyle CssClass="GridCss">
																		</EditCellStyle>
																		<EditFormCaptionStyle CssClass="GridCss" HorizontalAlign="Left">
																		</EditFormCaptionStyle>
																		<HeaderStyle CssClass="GridCssHeader">
																			<Paddings Padding="5px" />
																			<Paddings Padding="5px"></Paddings>
																		</HeaderStyle>
																		<CellStyle CssClass="GridCss">
																		</CellStyle>
																	</dx:GridViewDataTextColumn>
																	<dx:GridViewDataDateColumn FieldName="StartDate" ShowInCustomizationForm="True" VisibleIndex="4">
																		<EditCellStyle CssClass="GridCss">
																		</EditCellStyle>
																		<EditFormCaptionStyle CssClass="GridCss">
																		</EditFormCaptionStyle>
																		<HeaderStyle CssClass="GridCssHeader" />
																		<CellStyle CssClass="GridCss">
																		</CellStyle>
																	</dx:GridViewDataDateColumn>
																	<dx:GridViewDataDateColumn FieldName="StartTime" ShowInCustomizationForm="True" VisibleIndex="5">
																		<Settings AllowAutoFilter="False" />
																		<PropertiesDateEdit DisplayFormatString="t">
																		</PropertiesDateEdit>
																		<EditCellStyle CssClass="GridCss">
																		</EditCellStyle>
																		<EditFormCaptionStyle CssClass="GridCss">
																		</EditFormCaptionStyle>
																		<HeaderStyle CssClass="GridCssHeader" />
																		<CellStyle CssClass="GridCss">
																		</CellStyle>
																	</dx:GridViewDataDateColumn>
																	<dx:GridViewDataTextColumn FieldName="Duration" ShowInCustomizationForm="True" VisibleIndex="6">
																		<Settings AllowAutoFilter="False" />
																		<EditCellStyle CssClass="GridCss">
																		</EditCellStyle>
																		<EditFormCaptionStyle CssClass="GridCss">
																		</EditFormCaptionStyle>
																		<HeaderStyle CssClass="GridCssHeader" />
																		<CellStyle CssClass="GridCss">
																		</CellStyle>
																	</dx:GridViewDataTextColumn>
																	<dx:GridViewDataDateColumn FieldName="EndDate" ShowInCustomizationForm="True" VisibleIndex="7">
																		<EditCellStyle CssClass="GridCss">
																		</EditCellStyle>
																		<EditFormCaptionStyle CssClass="GridCss">
																		</EditFormCaptionStyle>
																		<HeaderStyle CssClass="GridCssHeader" />
																		<CellStyle CssClass="GridCss">
																		</CellStyle>
																	</dx:GridViewDataDateColumn>
																	<dx:GridViewDataTextColumn FieldName="MaintType" ReadOnly="True" ShowInCustomizationForm="True"
																		VisibleIndex="8">
																		<EditCellStyle CssClass="GridCss">
																		</EditCellStyle>
																		<EditFormCaptionStyle CssClass="GridCss">
																		</EditFormCaptionStyle>
																		<HeaderStyle CssClass="GridCssHeader" />
																		<CellStyle CssClass="GridCss">
																		</CellStyle>
																	</dx:GridViewDataTextColumn>
																</Columns>
																<SettingsBehavior AllowSelectByRowClick="True" ColumnResizeMode="NextColumn" ProcessSelectionChangedOnServer="True" />
																<SettingsBehavior AllowSelectByRowClick="True" ColumnResizeMode="NextColumn" ProcessSelectionChangedOnServer="True">
																</SettingsBehavior>
																<SettingsPager PageSize="50">
																	<PageSizeItemSettings Visible="True">
																	</PageSizeItemSettings>
																</SettingsPager>
																<Settings ShowFilterRow="True" ShowGroupPanel="True" />
																<Settings ShowFilterRow="True" ShowGroupPanel="True"></Settings>
																<Styles CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" CssPostfix="Office2010Blue">
																	<Header ImageSpacing="5px" SortingImageSpacing="5px">
																	</Header>
																	<GroupRow Font-Bold="True">
																	</GroupRow>
																	<AlternatingRow CssClass="GridAltRow">
																	</AlternatingRow>
																	<LoadingPanel ImageSpacing="5px">
																	</LoadingPanel>
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
															</dx:ASPxGridView>
															<dxwschs:ASPxScheduler ID="ASPxScheduler1" runat="server" ActiveViewType="Timeline"
																ClientIDMode="AutoID" GroupType="Resource" Start="2014-10-26" Visible="False">
																<ClientSideEvents MouseUp="" />
																<Storage>
																	<Appointments ResourceSharing="True">
																		<Mappings AllDay="AllDay" AppointmentId="ID" End="EndDate" Label="Label" RecurrenceInfo="RecurrenceInfo"
																			ReminderInfo="ReminderInfo" Start="StartDate" Subject="Name" Type="EventType" />
																		<Mappings AppointmentId="ID" AllDay="AllDay" Label="Label" End="EndDate" RecurrenceInfo="RecurrenceInfo"
																			ReminderInfo="ReminderInfo" Start="StartDate" Subject="Name" Type="EventType">
																		</Mappings>
																	</Appointments>
																	<Resources>
																		<Mappings Caption="Name" Image="Photo" />
																		<Mappings Caption="Name" Image="Photo"></Mappings>
																	</Resources>
																</Storage>
																<ResourceColorSchemas>
																	<cc1:SchedulerColorSchema Cell="255, 244, 188" CellBorder="243, 228, 177" CellBorderDark="234, 208, 152"
																		CellLight="255, 255, 213" CellLightBorder="255, 239, 199" CellLightBorderDark="246, 219, 162">
																	</cc1:SchedulerColorSchema>
																	<cc1:SchedulerColorSchema Cell="Control" CellBorder="ControlDark" CellBorderDark="ControlDark"
																		CellLight="Window" CellLightBorder="ControlDark" CellLightBorderDark="ControlDark">
																	</cc1:SchedulerColorSchema>
																	<cc1:SchedulerColorSchema Cell="179, 212, 151" CellBorder="168, 203, 138" CellBorderDark="140, 180, 104"
																		CellLight="213, 236, 188" CellLightBorder="205, 228, 180" CellLightBorderDark="186, 209, 162">
																	</cc1:SchedulerColorSchema>
																	<cc1:SchedulerColorSchema Cell="139, 158, 191" CellBorder="128, 147, 181" CellBorderDark="97, 116, 152"
																		CellLight="207, 216, 230" CellLightBorder="193, 201, 219" CellLightBorderDark="161, 175, 204">
																	</cc1:SchedulerColorSchema>
																	<cc1:SchedulerColorSchema Cell="190, 134, 161" CellBorder="180, 124, 149" CellBorderDark="156, 101, 122"
																		CellLight="227, 203, 214" CellLightBorder="218, 189, 199" CellLightBorderDark="197, 163, 171">
																	</cc1:SchedulerColorSchema>
																	<cc1:SchedulerColorSchema Cell="137, 177, 167" CellBorder="123, 168, 156" CellBorderDark="84, 142, 128"
																		CellLight="193, 214, 209" CellLightBorder="174, 202, 195" CellLightBorderDark="145, 182, 173">
																	</cc1:SchedulerColorSchema>
																	<cc1:SchedulerColorSchema Cell="247, 180, 127" CellBorder="235, 167, 113" CellBorderDark="202, 131, 71"
																		CellLight="250, 208, 174" CellLightBorder="238, 196, 163" CellLightBorderDark="225, 166, 118">
																	</cc1:SchedulerColorSchema>
																	<cc1:SchedulerColorSchema Cell="221, 140, 142" CellBorder="210, 129, 131" CellBorderDark="179, 100, 101"
																		CellLight="239, 200, 201" CellLightBorder="233, 187, 189" CellLightBorderDark="222, 164, 166">
																	</cc1:SchedulerColorSchema>
																	<cc1:SchedulerColorSchema Cell="137, 150, 132" CellBorder="129, 138, 122" CellBorderDark="102, 100, 89"
																		CellLight="208, 216, 203" CellLightBorder="196, 207, 191" CellLightBorderDark="172, 181, 169">
																	</cc1:SchedulerColorSchema>
																	<cc1:SchedulerColorSchema Cell="0, 199, 200" CellBorder="0, 186, 187" CellBorderDark="0, 151, 153"
																		CellLight="168, 236, 236" CellLightBorder="144, 226, 227" CellLightBorderDark="84, 203, 204">
																	</cc1:SchedulerColorSchema>
																	<cc1:SchedulerColorSchema Cell="168, 148, 207" CellBorder="155, 136, 194" CellBorderDark="118, 99, 155"
																		CellLight="221, 213, 236" CellLightBorder="210, 199, 230" CellLightBorderDark="185, 169, 216">
																	</cc1:SchedulerColorSchema>
																	<cc1:SchedulerColorSchema Cell="204, 204, 204" CellBorder="189, 189, 189" CellBorderDark="121, 121, 121"
																		CellLight="230, 230, 230" CellLightBorder="204, 204, 204" CellLightBorderDark="177, 177, 177">
																	</cc1:SchedulerColorSchema>
																</ResourceColorSchemas>
																<Views>
																	<WorkWeekView>
																		<TimeSlots>
																			<cc1:TimeSlot Value="01:00:00" DisplayName="60 Minutes" MenuCaption="6&amp;0 Minutes">
																			</cc1:TimeSlot>
																			<cc1:TimeSlot Value="00:30:00" DisplayName="30 Minutes" MenuCaption="&amp;30 Minutes">
																			</cc1:TimeSlot>
																			<cc1:TimeSlot Value="00:15:00" DisplayName="15 Minutes" MenuCaption="&amp;15 Minutes">
																			</cc1:TimeSlot>
																			<cc1:TimeSlot Value="00:10:00" DisplayName="10 Minutes" MenuCaption="10 &amp;Minutes">
																			</cc1:TimeSlot>
																			<cc1:TimeSlot Value="00:06:00" DisplayName="6 Minutes" MenuCaption="&amp;6 Minutes">
																			</cc1:TimeSlot>
																			<cc1:TimeSlot DisplayName="5 Minutes" MenuCaption="&amp;5 Minutes"></cc1:TimeSlot>
																		</TimeSlots>
																		<TimeRulers>
																			<cc1:TimeRuler />
																		</TimeRulers>
																	</WorkWeekView>
																	<DayView ResourcesPerPage="3">
																		<TimeSlots>
																			<cc1:TimeSlot Value="01:00:00" DisplayName="60 Minutes" MenuCaption="6&amp;0 Minutes">
																			</cc1:TimeSlot>
																			<cc1:TimeSlot Value="00:30:00" DisplayName="30 Minutes" MenuCaption="&amp;30 Minutes">
																			</cc1:TimeSlot>
																			<cc1:TimeSlot Value="00:15:00" DisplayName="15 Minutes" MenuCaption="&amp;15 Minutes">
																			</cc1:TimeSlot>
																			<cc1:TimeSlot Value="00:10:00" DisplayName="10 Minutes" MenuCaption="10 &amp;Minutes">
																			</cc1:TimeSlot>
																			<cc1:TimeSlot Value="00:06:00" DisplayName="6 Minutes" MenuCaption="&amp;6 Minutes">
																			</cc1:TimeSlot>
																			<cc1:TimeSlot DisplayName="5 Minutes" MenuCaption="&amp;5 Minutes"></cc1:TimeSlot>
																		</TimeSlots>
																		<TimeRulers>
																			<cc1:TimeRuler />
																		</TimeRulers>
																		<DayViewStyles ScrollAreaHeight="400px">
																		</DayViewStyles>
																	</DayView>
																	<MonthView NavigationButtonVisibility="Always" ResourcesPerPage="3">
																	</MonthView>
																	<TimelineView ResourcesPerPage="3">
																		<Scales>
																			<cc1:TimeScaleYear Enabled="False" />
																			<cc1:TimeScaleQuarter Enabled="False" />
																			<cc1:TimeScaleMonth />
																			<cc1:TimeScaleWeek />
																			<cc1:TimeScaleDay />
																			<cc1:TimeScaleHour Enabled="False" />
																			<cc1:TimeScaleFixedInterval Enabled="False" />
																		</Scales>
																		<AppointmentDisplayOptions AppointmentAutoHeight="True" />
																		<AppointmentDisplayOptions AppointmentAutoHeight="True"></AppointmentDisplayOptions>
																	</TimelineView>
																	<FullWeekView>
																		<TimeSlots>
																			<cc1:TimeSlot Value="01:00:00" DisplayName="60 Minutes" MenuCaption="6&amp;0 Minutes">
																			</cc1:TimeSlot>
																			<cc1:TimeSlot Value="00:30:00" DisplayName="30 Minutes" MenuCaption="&amp;30 Minutes">
																			</cc1:TimeSlot>
																			<cc1:TimeSlot Value="00:15:00" DisplayName="15 Minutes" MenuCaption="&amp;15 Minutes">
																			</cc1:TimeSlot>
																			<cc1:TimeSlot Value="00:10:00" DisplayName="10 Minutes" MenuCaption="10 &amp;Minutes">
																			</cc1:TimeSlot>
																			<cc1:TimeSlot Value="00:06:00" DisplayName="6 Minutes" MenuCaption="&amp;6 Minutes">
																			</cc1:TimeSlot>
																			<cc1:TimeSlot DisplayName="5 Minutes" MenuCaption="&amp;5 Minutes"></cc1:TimeSlot>
																		</TimeSlots>
																		<TimeRulers>
																			<cc1:TimeRuler></cc1:TimeRuler>
																		</TimeRulers>
																	</FullWeekView>
																</Views>
																<ClientSideEvents MouseUp=""></ClientSideEvents>
															</dxwschs:ASPxScheduler>
														</td>
													</tr>
													<tr>
														<td>
															&nbsp;
														</td>
													</tr>
												</table>
											</dx:ContentControl>
										</ContentCollection>
									</dx:TabPage>
									<dx:TabPage Text="Alert">
										<TabImage Url="../images/icons/error.png">
										</TabImage>
										<ContentCollection>
											<dx:ContentControl ID="ContentControl6" runat="server" SupportsDisabledAttribute="True">
												<table>
													<tr>
														<td>
															<table>
																<tr>
																	<td>
																		<div id="infoDivA" class="info">
																			The list of configured alerts that apply to the server is listed below. In order
																			to add new alerts or configure existing alerts, please use the Alerts\Alert Definitions
																			menu.
																		</div>
																	</td>
																</tr>
																<tr>
																	<td>
																		<dx:ASPxGridView ID="AlertGridView" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
																			CssPostfix="Office2010Blue" Cursor="pointer" KeyFieldName="ID" Theme="Office2003Blue"
																			Width="100%" OnPageSizeChanged="AlertGridView_PageSizeChanged" OnCustomColumnDisplayText="AlertGridView_CustomColumnDisplayText"
																			OnCustomUnboundColumnData="AlertGridView_CustomUnboundColumnData">
																			<Columns>
																				<dx:GridViewDataTextColumn Caption="Name" FieldName="Name" ReadOnly="True" ShowInCustomizationForm="True"
																					VisibleIndex="0">
																					<Settings AutoFilterCondition="Contains" />
																					<Settings AutoFilterCondition="Contains"></Settings>
																					<HeaderStyle CssClass="GridCssHeader" />
																					<CellStyle CssClass="GridCss">
																					</CellStyle>
																				</dx:GridViewDataTextColumn>
																				<dx:GridViewDataTextColumn Caption="Start Time" FieldName="StartTime" ReadOnly="True"
																					ShowInCustomizationForm="True" VisibleIndex="1" Width="70px">
																					<HeaderStyle CssClass="GridCssHeader" />
																					<CellStyle CssClass="GridCss">
																					</CellStyle>
																				</dx:GridViewDataTextColumn>
																				<dx:GridViewDataTextColumn Caption="Duration (min)" FieldName="Duration" ShowInCustomizationForm="True"
																					VisibleIndex="2" Width="60px">
																					<Settings AutoFilterCondition="Contains" />
																					<Settings AutoFilterCondition="Contains"></Settings>
																					<EditCellStyle CssClass="GridCss">
																					</EditCellStyle>
																					<EditFormCaptionStyle CssClass="GridCss" HorizontalAlign="Left">
																					</EditFormCaptionStyle>
																					<HeaderStyle CssClass="GridCssHeader2" Wrap="True">
																						<Paddings Padding="5px"></Paddings>
																					</HeaderStyle>
																					<CellStyle CssClass="GridCss2">
																					</CellStyle>
																				</dx:GridViewDataTextColumn>
																				<dx:GridViewDataTextColumn Caption="To" FieldName="SendTo" ShowInCustomizationForm="True"
																					VisibleIndex="4" Visible="False">
																					<HeaderStyle CssClass="GridCssHeader" />
																					<CellStyle CssClass="GridCss">
																					</CellStyle>
																				</dx:GridViewDataTextColumn>
																				<dx:GridViewDataTextColumn Caption="Cc" FieldName="CopyTo" ShowInCustomizationForm="True"
																					VisibleIndex="5">
																					<PropertiesTextEdit DisplayFormatString="t">
																					</PropertiesTextEdit>
																					<EditCellStyle CssClass="GridCss">
																					</EditCellStyle>
																					<EditFormCaptionStyle CssClass="GridCss">
																					</EditFormCaptionStyle>
																					<HeaderStyle CssClass="GridCssHeader" />
																					<CellStyle CssClass="GridCss">
																					</CellStyle>
																				</dx:GridViewDataTextColumn>
																				<dx:GridViewDataTextColumn Caption="Bcc" FieldName="BlindCopyTo" ShowInCustomizationForm="True"
																					VisibleIndex="6">
																					<PropertiesTextEdit DisplayFormatString="d">
																					</PropertiesTextEdit>
																					<EditCellStyle CssClass="GridCss">
																					</EditCellStyle>
																					<EditFormCaptionStyle CssClass="GridCss">
																					</EditFormCaptionStyle>
																					<HeaderStyle CssClass="GridCssHeader" />
																					<CellStyle CssClass="GridCss">
																					</CellStyle>
																				</dx:GridViewDataTextColumn>
																				<dx:GridViewDataTextColumn Caption="Event Type" FieldName="EventName" ShowInCustomizationForm="True"
																					VisibleIndex="7" Width="200px">
																					<HeaderStyle CssClass="GridCssHeader" />
																					<CellStyle CssClass="GridCss">
																					</CellStyle>
																				</dx:GridViewDataTextColumn>
																				<dx:GridViewDataTextColumn Caption="Day(s)" FieldName="Day" ShowInCustomizationForm="True"
																					VisibleIndex="8">
																					<HeaderStyle CssClass="GridCssHeader" />
																					<CellStyle CssClass="GridCss">
																					</CellStyle>
																				</dx:GridViewDataTextColumn>
																				<dx1:GridViewDataTextColumn Caption="To" FieldName="SendToAll" ShowInCustomizationForm="True"
																					UnboundType="String" VisibleIndex="3">
																					<HeaderStyle CssClass="GridCssHeader" />
																					<CellStyle CssClass="GridCss">
																					</CellStyle>
																				</dx1:GridViewDataTextColumn>
																			</Columns>
																			<SettingsBehavior ColumnResizeMode="NextColumn" />
																			<SettingsBehavior ColumnResizeMode="NextColumn"></SettingsBehavior>
																			<SettingsPager PageSize="50">
																				<PageSizeItemSettings Visible="True">
																				</PageSizeItemSettings>
																			</SettingsPager>
																			<Settings ShowFilterRow="True" ShowGroupPanel="True" />
																			<Settings ShowFilterRow="True" ShowGroupPanel="True"></Settings>
																			<Styles CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" CssPostfix="Office2010Blue">
																				<Header ImageSpacing="5px" SortingImageSpacing="5px">
																				</Header>
																				<GroupRow Font-Bold="True">
																				</GroupRow>
																				<AlternatingRow CssClass="GridAltRow">
																				</AlternatingRow>
																				<LoadingPanel ImageSpacing="5px">
																				</LoadingPanel>
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
																		</dx:ASPxGridView>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
												</table>
											</dx:ContentControl>
										</ContentCollection>
									</dx:TabPage>
								</TabPages>
								<Paddings PaddingLeft="0px" />
								<ActiveTabStyle Font-Bold="True">
								</ActiveTabStyle>
								<ContentStyle>
									<Border BorderColor="#4986A2" />
								</ContentStyle>
							</dx:ASPxPageControl>
						</td>
					</tr>
					<tr>
						<td align="left">
							<div id="errorDiv" class="alert alert-danger" runat="server" style="display: none">
								Error.</div>
						</td>
					</tr>
                    <tr>
                        <td>
                            <table>
						<tr>
							<td>
								<dx:ASPxButton ID="FormOkButton" runat="server" Text="OK" CssClass="sysButton" OnClick="FormOkButton_Click"
									AutoPostBack="True" UseSubmitBehavior="true">
								</dx:ASPxButton>
							</td>
							<td>
								<dx:ASPxButton ID="FormCancelButton" runat="server" Text="Cancel" CssClass="sysButton"
									OnClick="FormCancelButton_Click" CausesValidation="False">
								</dx:ASPxButton>
							</td>
						</tr>
					</table>        
                        </td>
                    </tr>
					
				</table>
			</td>
		</tr>
		<tr>
			<td>
				<dx:ASPxPopupControl ID="ErrorMessagePopupControl" runat="server" CloseAction="CloseButton"
					Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
					HeaderText="Validation Failure" AllowDragging="True" EnableAnimation="False"
					Height="150px" Width="300px" ClientInstanceName="pcErrorMessage" EnableViewState="False"
					CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
					Theme="MetropolisBlue">
					<HeaderStyle></HeaderStyle>
					<ContentCollection>
						<dx:PopupControlContentControl ID="PopupControlContentControl11" runat="server">
							<dx:ASPxPanel ID="Panel1" runat="server" DefaultButton="btOK">
								<PanelCollection>
									<dx:PanelContent ID="PanelContent14" runat="server">
										<div style="min-height: 70px;">
											<dx:ASPxLabel ID="ErrorMessageLabel" runat="server" Text="Username:">
											</dx:ASPxLabel>
										</div>
										<div>
											<table width="100%" cellpadding="0" cellspacing="0">
												<tr>
													<td align="center">
														<dx:ASPxButton ID="ValidationOkButton" runat="server" Text="OK" CssClass="sysButton"
															AutoPostBack="False">
															<ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }" />
															<ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }"></ClientSideEvents>
														</dx:ASPxButton>
														<dx:ASPxButton ID="ValidationUpdatedButton" runat="server" AutoPostBack="False" OnClick="ValidationUpdatedButton_Click"
															Text="OK" CssClass="sysButton" Visible="False">
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
			</td>
		</tr>
		<tr>
			<td>
				<dx:ASPxPopupControl ID="RConsolePopupControl" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
					CssPostfix="Glass" HeaderText="Remote Console" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
					AllowDragging="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
					Modal="True" AllowResize="True" Height="180px" Width="500px" Theme="MetropolisBlue">
					<HeaderStyle></HeaderStyle>
					<ContentCollection>
						<dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
							<dx:ASPxMemo ID="RConsoleMemo" runat="server" Height="150px" Width="100%" AutoResizeWithContainer="True" ClientInstanceName="RConsoleMemo">
                         
							</dx:ASPxMemo>
							<table>
								<tr>
									<td colspan="3">
									</td>
								</tr>
								<tr>
									<td colspan="3">
										<dx:ASPxTextBox ID="ASPxTextBox1" runat="server" Width="170px" Visible="False">
										</dx:ASPxTextBox>
										&nbsp;
									</td>
								</tr>
								<tr>
									<td>
										<dx:ASPxButton ID="RefreshButton" runat="server" CssClass="sysButton" OnClick="RefreshButton_Click"
											Text="Refresh">
										</dx:ASPxButton>
									</td>
									<td>
										<dx:ASPxButton ID="CopyToClipboardButton" runat="server" CssClass="sysButton" Text="Copy To Clipboard"
											Wrap="False"  ClientInstanceName="CopyToClipboardButton" AutoPostBack="false">
                                            <%-- 3/8/2016 Durga Added for VSPLUS-2680--%>
                                                 <ClientSideEvents Click= "function(s, e){return Copytoclipboard(s,e);}" />
										</dx:ASPxButton>
									</td>
									<td>
										<dx:ASPxButton ID="DoneButton" runat="server" CssClass="sysButton" OnClick="DoneButton_Click"
											Text="Done">
										</dx:ASPxButton>
									</td>
								</tr>
							</table>
						</dx:PopupControlContentControl>
					</ContentCollection>
				</dx:ASPxPopupControl>
			</td>
		</tr>
		<tr>
			<td>
				<dx:ASPxPopupControl ID="CopyProfilePopupControl" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
					CssPostfix="Glass" HeaderText="Please Enter Credentials" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
					Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
					Theme="MetropolisBlue" EnableHierarchyRecreation="False" Width="300px">
					<HeaderStyle>
						<Paddings PaddingLeft="10px" PaddingRight="6px" PaddingTop="1px" />
					</HeaderStyle>
					<ContentCollection>
						<dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">
							<table>
								<tr>
									<td>
										<dx:ASPxLabel ID="AliasNameLabel" runat="server" CssClass="lblsmallFont" 
                                            Text="Alias Name:" Wrap="False">
										</dx:ASPxLabel>
									</td>
									<td>
										<dx:ASPxTextBox ID="AliasName" runat="server" ClientInstanceName="AliasName" Width="170px">
										</dx:ASPxTextBox>
									</td>
								</tr>
								<tr>
									<td>
										<dx:ASPxLabel ID="UserIDLabel" runat="server" Text="User ID:" 
                                            CssClass="lblsmallFont" Wrap="False">
										</dx:ASPxLabel>
									</td>
									<td>
										<dx:ASPxTextBox ID="UserID" runat="server" Width="170px" ClientInstanceName="UserID">
											
										</dx:ASPxTextBox>
									</td>
								</tr>
								<tr>
									<td>
										<dx:ASPxLabel ID="PasswordLabel" runat="server" Text="Password:" 
                                            CssClass="lblsmallFont" Wrap="False">
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
