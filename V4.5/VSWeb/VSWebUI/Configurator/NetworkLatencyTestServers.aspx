<%@ Page Title="VitalSigns Plus- Network Latency" Language="C#" MasterPageFile="~/Site1.Master"
	AutoEventWireup="true" CodeBehind="NetworkLatencyTestServers.aspx.cs" Inherits="VSWebUI.Configurator.NetworkLatencyTestServers" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx1" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
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
// 	});

 	function OnItemClick(s, e) {
 		if (e.item.parent == s.GetRootItem())
 			e.processOnServer = false;
 	}

// 	$(document).ready(function () {
 		$('.alert-danger').delay(10000).fadeOut("slow", function () {
 		});
// 	});

// 	$(document).ready(function () {
 		$('#ContentPlaceHolder1_hdbtnEvent').val('false');
 	});
 </script>

<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript">
	$(document).ready(function () {
		$("th").click(function () {
			var columnIndex = $(this).index();
			var tdArray = $(this).closest("table").find("tr td:nth-child(" + (columnIndex + 1) + ")");
			tdArray.sort(function (p, n) {
				var pData = $(p).text();
				var nData = $(n).text();
				return pData < nData ? -1 : 1;
			});
			tdArray.each(function () {
				var row = $(this).parent();
				$("#NetworkLatencyTestgrd").append(row);
			});
		});
	})
    </script>
<script type="text/javascript">
	var gridViewId = '#<%= NetworkLatencyTestgrd.ClientID %>';
	function checkAll(selectAllCheckbox) {
		//get all checkbox and select it
	
		$('td :checkbox', gridViewId).prop("checked", selectAllCheckbox.checked);
		
	}
	function unCheckSelectAll(selectCheckbox) {
		//if any item is unchecked, uncheck header checkbox as also
		if (!selectCheckbox.checked)
			$('th :checkbox', gridViewId).prop("checked", false);
		else {
			$('th :checkbox', gridViewId).prop("checked", true);
		}
	}
	$("[id*=chkHeader]").live("click", function () {
		var chkHeader = $(this);
		var grid = $(this).closest("table");
		$("input[type=checkbox]", grid).each(function () {
			if (chkHeader.is(":checked")) {
				$(this).attr("checked", "checked");
				$("td", $(this).closest("tr")).addClass("selected");
			} else {
				$(this).removeAttr("checked");
				$("td", $(this).closest("tr")).removeClass("selected");
			}
		});
	});
	$("[id*=chkRow]").live("click", function () {
		var grid = $(this).closest("table");
		var chkHeader = $("[id*=chkHeader]", grid);
		if (!$(this).is(":checked")) {
			$("td", $(this).closest("tr")).removeClass("selected");
			chkHeader.removeAttr("checked");
		} else {
			$("td", $(this).closest("tr")).addClass("selected");
			if ($("[id*=chkRow]", grid).length == $("[id*=chkRow]:checked", grid).length) {
			
				chkHeader.prop("checked", "checked");
			}
		}
	});
	function OnItemClick(s, e) {
		if (e.item.parent == s.GetRootItem())
			e.processOnServer = false;
	}
		
</script>



          

	<style type="text/css">
		.style5
		{
			width: 53px;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table width="100%">
		<tr>
			<td valign="top">
				<div class="header" id="servernamelbldisp" runat="server">
				</div>
			</td>
			<td>
			  <dx:ASPxMenu ID="ASPxMenu1" runat="server" EnableTheming="True"  Visible="false"
                    HorizontalAlign="Right" onitemclick="ASPxMenu1_ItemClick" ShowAsToolbar="True"  
                    Theme="Moderno">
                    <ClientSideEvents ItemClick="OnItemClick" />
                    <Items>
                        <dx:MenuItem Name="MainItem">
                            <Items>
                                <dx:MenuItem Name="NetworkLatencyTest" Text="Network Latency Test">
                                </dx:MenuItem>
                                
                            </Items>
                            <Image Url="~/images/icons/Gear.png">
                            </Image>
                        </dx:MenuItem>
                    </Items>
                </dx:ASPxMenu>
			</td>
		</tr>
		<tr>
			<td align="left" valign="top">
				<table width="100%">
					<tr>
						<td>
							<div class="info">
								A Network Latency Test record is designed to test a group of servers for network response time.
							</div>
						</td>
					</tr>
					<tr>
						<td>
							<dx:ASPxRoundPanel ID="DiskSettingsRoundPanel" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
								CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Network Latency Test"
								SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
								<ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
								<ContentPaddings Padding="2px" PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px">
								</ContentPaddings>
								<HeaderStyle Height="23px"></HeaderStyle>
								<PanelCollection>
									<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
										<table>
											<tr>
												<td>
													<dx:ASPxLabel ID="ASPxLabel14" runat="server" CssClass="lblsmallFont" Text=" Test Name:">
													</dx:ASPxLabel>
												</td>
												<td>
													<dx:ASPxTextBox ID="testtxtname" runat="server">
														<ValidationSettings CausesValidation="true" ValidationGroup="val" RequiredField-ErrorText="Enter Test Name"
															ErrorDisplayMode="ImageWithTooltip">
															<RequiredField IsRequired="True"></RequiredField>
														</ValidationSettings>
													</dx:ASPxTextBox>
												</td>
												<td class="style5">
													&nbsp;
												</td>
												<td>
													&nbsp;
												</td>
												<td align="right">
													<dx1:ASPxCheckBox ID="SrvAtrScanCheckBox" runat="server" CheckState="checked" CssClass="lblsmallFont"
														Text="Enabled for scanning" >
													</dx1:ASPxCheckBox>
												</td>
											</tr>
											<tr>
												<td>
													<dx:ASPxLabel ID="ASPxLabel18" runat="server" CssClass="lblsmallFont" Text="Test Duration:">
													</dx:ASPxLabel>
												</td>
												<td>
													<dx:ASPxTextBox ID="txtduration" runat="server" AutoPostBack="false">
														<ValidationSettings CausesValidation="true" ValidationGroup="val" RequiredField-ErrorText="Enter Test Duration"
															ErrorDisplayMode="ImageWithTooltip">
															<RequiredField IsRequired="True"></RequiredField>
														</ValidationSettings>
													</dx:ASPxTextBox>
												</td>
												<td class="style5">
													<dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="seconds" CssClass="lblsmallFont">
													</dx:ASPxLabel>
												</td>
												<td>
													&nbsp;
												</td>
											</tr>
											<tr>
												<td>
													<dx1:ASPxLabel ID="ASPxLabel16" runat="server" CssClass="lblsmallFont" Text="Scan Interval:">
													</dx1:ASPxLabel>
												</td>
												<td>
													<dx1:ASPxTextBox ID="txtscaninterval" runat="server">
														<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="val">
															<RequiredField ErrorText="Enter Scan Interval" IsRequired="True" />
														</ValidationSettings>
													</dx1:ASPxTextBox>
												</td>
												<td class="style5">
													<dx1:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" Text="minutes">
													</dx1:ASPxLabel>
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
					<td>
					<dx:ASPxCheckBox ID="ASPxCheckBox1" runat="server"  CssClass="lblsmallFont" Visible="false"
														Text="Select Thresholds to multiple Servers at a time." 
						autopostback="true">
													</dx:ASPxCheckBox>
													</td>
													</tr>
					<tr>
						<td>
							<dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
								CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Server Selection/Threshold Update"
								SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
								<ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
								<ContentPaddings Padding="2px" PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px">
								</ContentPaddings>
								<HeaderStyle Height="23px"></HeaderStyle>
								<PanelCollection>
									<dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
										<table>
											
										    <tr>
                                                <td>
                                                    <div ID="infothershold" runat="server" class="info">
                                                        Selected servers will be used in Network Latency testing.
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div>
							<dx:ASPxGridView runat="server" CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css"
								CssPostfix="Office2010Silver" KeyFieldName="ID" AutoGenerateColumns="False" Width="100%"
								ID="NetworkLatencyTestgrd"  EnableTheming="True" Theme="Office2003Blue"
								OnPreRender="NetworkLatencyTestgrd_PreRender" 
								OnPageSizeChanged="NetworkLatencyTestgrd_OnPageSizeChanged" >
														<Columns>
									<dx:GridViewCommandColumn Caption="Select" ShowSelectCheckbox="True" SelectAllCheckboxMode="Page"
                                         VisibleIndex="1" >
										<ClearFilterButton Visible="True">
										</ClearFilterButton>
										<HeaderStyle CssClass="GridCssHeader1" />
									</dx:GridViewCommandColumn>
									<dx:GridViewDataTextColumn Caption="Name" FieldName="ServerName" VisibleIndex="2">
										<Settings  />
										<Settings AutoFilterCondition="Contains"></Settings>
										<EditCellStyle CssClass="GridCss">
										</EditCellStyle>
										<EditFormCaptionStyle CssClass="GridCss" HorizontalAlign="Left">
										</EditFormCaptionStyle>
										<HeaderStyle CssClass="GridCssHeader" />
										<CellStyle CssClass="GridCss">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Yellow Threshold (ms)" FieldName="LatencyYellowThreshold"
										VisibleIndex="7" Width="100px">
										<PropertiesTextEdit>
											<Style CssClass="GridCss2">
												
											</Style>
										</PropertiesTextEdit>
										<DataItemTemplate>
											<dx:ASPxTextBox ID="txtyellowthreshValue" runat="server" KeyFieldName="ID" Width="100px"
												Value='<%# Eval("LatencyYellowThreshold") %>' >
											</dx:ASPxTextBox>
										</DataItemTemplate>
										<HeaderStyle CssClass="GridCssHeader2" />
										<CellStyle CssClass="GridCss2">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Red Threshold (ms)" FieldName="LatencyRedThreshold"
										VisibleIndex="8" Width="100px">
										<PropertiesTextEdit>
											<Style CssClass="GridCss2">
												
											</Style>
										</PropertiesTextEdit>
										<DataItemTemplate>
											<dx:ASPxTextBox ID="txtredthreshValue" runat="server" KeyFieldName="ID" Width="100px"
												Value='<%# Eval("LatencyRedThreshold") %>'>
											</dx:ASPxTextBox>
										</DataItemTemplate>
										<HeaderStyle CssClass="GridCssHeader2" />
										<CellStyle CssClass="GridCss2">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn FieldName="Location" Caption="Location" VisibleIndex="3">
										<Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
										<Settings AllowAutoFilter="True" AutoFilterCondition="Contains"></Settings>
										<HeaderStyle CssClass="GridCssHeader"></HeaderStyle>
										<CellStyle CssClass="GridCss">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn FieldName="ServerType" Caption="Server Type" VisibleIndex="4">
										
										<Settings AllowAutoFilter="True" AutoFilterCondition="Contains"></Settings>
										<HeaderStyle CssClass="GridCssHeader"></HeaderStyle>
										<CellStyle CssClass="GridCss">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="ID" FieldName="ID" VisibleIndex="9" Visible="false">
									</dx:GridViewDataTextColumn>
								</Columns>
								
								
								<SettingsPager AlwaysShowPager="True">
									<PageSizeItemSettings Visible="True">
									</PageSizeItemSettings>
								</SettingsPager>
								<Styles CssPostfix="Office2010Silver" CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css">
									<AlternatingRow CssClass="GridCssAltRow">
									</AlternatingRow>
								</Styles>
								</dx:ASPxGridView>
								</div>
                                                </td>
                                            </tr>
											<tr>
                                                <td>
                                                    <div id ="successDiv" runat="server" class="alert alert-success" 
                                                        style="display: none" >
								                        Selected servers were successfully updated.
                                                        <button class="close" data-dismiss="alert" type="button">
                                                            <span aria-hidden="true">×</span><span class="sr-only">Close</span>
                                                        </button>
							</div>
                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dx1:ASPxButton ID="btnupdate" runat="server" CssClass="sysButton" 
                                                        OnClick="btnupdate_Click" Text="Bulk Update Thresholds" ValidationGroup="val1">
                                                        <ClientSideEvents Click="function(){ $('#ContentPlaceHolder1_hdbtnEvent').val('true'); }" />
                                                    </dx1:ASPxButton>
                                                    <asp:HiddenField ID="hdbtnEvent" runat="server" />
                                                </td>
                                            </tr>
										</table>
									</dx:PanelContent>
								</PanelCollection>
							</dx:ASPxRoundPanel>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		
		<tr valign="top">
			<td valign="top">
				<table width="100%">
					<tr>
						<td valign="top">
							<div id="errorDiv" class="alert alert-danger" runat="server" style="display: none">
								Please select at least one server.
								<button type="button" class="close" data-dismiss="alert">
									<span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
							</div>
                            <dx:ASPxPopupControl ID="ThresholdPopupControl" runat="server" 
                                Theme="MetropolisBlue" HeaderText="Threshold Settings" Width="760px" 
                                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
                                Modal="True">
                                <ContentCollection>
<dx:PopupControlContentControl runat="server">
    <table>
        <tr>
            <td>
                <div ID="infoDiv2" runat="server" class="info">
                                                        Select servers from the grid below to apply the threshold values.
                                                    </div>
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
													<dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="lblsmallFont" 
                                                        Text=" Yellow Threshold:" Wrap="False">
													</dx:ASPxLabel>
												</td>
												<td>
													<dx:ASPxTextBox ID="txtyellothld" runat="server" Text="10">
														<ValidationSettings CausesValidation="true" ValidationGroup="val1" RequiredField-ErrorText="Enter Yellow Threshold"
															ErrorDisplayMode="ImageWithTooltip">
															<RequiredField IsRequired="True"></RequiredField>
														</ValidationSettings>
													</dx:ASPxTextBox>
												</td>
												<td class="style5">
													<dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="milliseconds" CssClass="lblsmallFont">
													</dx:ASPxLabel>
												</td>
												<td>
													&nbsp;
												</td>
												<td>
													<dx:ASPxLabel ID="ASPxLabel7" runat="server" CssClass="lblsmallFont" 
                                                        Text="Red Threshold:" Wrap="False">
													</dx:ASPxLabel>
												</td>
												<td>
													<dx:ASPxTextBox ID="txtredthreshold" runat="server" AutoPostBack="false" 
                                                        Text="10">
														<ValidationSettings CausesValidation="true" ValidationGroup="val1" RequiredField-ErrorText="Enter Red Threshold"
															ErrorDisplayMode="ImageWithTooltip">
															<RequiredField IsRequired="True"></RequiredField>
														</ValidationSettings>
													</dx:ASPxTextBox>
												</td>
												<td class="style5">
													<dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="milliseconds" CssClass="lblsmallFont">
													</dx:ASPxLabel>
												</td>
												<td>
													&nbsp;
												</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <dx1:ASPxGridView ID="ThresholdGridView" runat="server" 
        AutoGenerateColumns="False" EnableTheming="True" Theme="Office2003Blue" 
        Width="100%" KeyFieldName="ID" OnPageSizeChanged = "ThresholdGridView_OnPageSizeChanged">
        <Columns>
            <dx1:GridViewCommandColumn Caption="Select" SelectAllCheckboxMode="Page" 
                ShowInCustomizationForm="True" ShowSelectCheckbox="True" VisibleIndex="0">
                <HeaderStyle CssClass="GridCssHeader1" />
                <CellStyle CssClass="GridCss1">
                </CellStyle>
            </dx1:GridViewCommandColumn>
            <dx1:GridViewDataTextColumn Caption="Name" FieldName="ServerName" 
                ShowInCustomizationForm="True" VisibleIndex="1">
            </dx1:GridViewDataTextColumn>
            <dx1:GridViewDataTextColumn Caption="Location" FieldName="Location" 
                ShowInCustomizationForm="True" VisibleIndex="2">
            </dx1:GridViewDataTextColumn>
            <dx1:GridViewDataTextColumn Caption="Server Type" FieldName="ServerType" 
                ShowInCustomizationForm="True" VisibleIndex="3">
            </dx1:GridViewDataTextColumn>
            <dx1:GridViewDataTextColumn Caption="Yellow Threshold (ms)" 
                FieldName="LatencyYellowThreshold" ShowInCustomizationForm="True" 
                VisibleIndex="4">
                <HeaderStyle CssClass="GridCssHeader2" />
                <CellStyle CssClass="GridCss2">
                </CellStyle>
            </dx1:GridViewDataTextColumn>
            <dx1:GridViewDataTextColumn Caption="Red Threshold (ms)" 
                FieldName="LatencyRedThreshold" ShowInCustomizationForm="True" VisibleIndex="5">
                <HeaderStyle CssClass="GridCssHeader2" />
                <CellStyle CssClass="GridCss2">
                </CellStyle>
            </dx1:GridViewDataTextColumn>
            <dx1:GridViewDataTextColumn Caption="ID" FieldName="ID" Name="ID" 
                ShowInCustomizationForm="True" Visible="False" VisibleIndex="6">
            </dx1:GridViewDataTextColumn>
        </Columns>
        <SettingsPager AlwaysShowPager="True">
            <PageSizeItemSettings Visible="True">
            </PageSizeItemSettings>
        </SettingsPager>
        <Styles>
            <AlternatingRow CssClass="GridAltRow">
            </AlternatingRow>
            <Header CssClass="GridCssHeader">
            </Header>
            <Cell CssClass="GridCss">
            </Cell>
        </Styles>
    </dx1:ASPxGridView>
            </td>
        </tr>
        <tr>
            <td>
                <div ID="errorDiv2" runat="server" class="alert alert-danger" 
                                                        style="display: none">
                                                        Please select at least one server.
                                                        <button class="close" data-dismiss="alert" type="button">
                                                            <span aria-hidden="true">×</span><span class="sr-only">Close</span>
                                                        </button>
                                                    </div>
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <dx:ASPxButton ID="TOKButton" runat="server" Text="OK" CssClass="sysButton" 
                                OnClick="TOKButton_Click">
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="TCancelButton" runat="server" Text="Cancel" 
                                CssClass="sysButton" OnClick="TCancelButton_Click">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
                                    </dx:PopupControlContentControl>
</ContentCollection>
                            </dx:ASPxPopupControl>
						</td>
					</tr>
					
					<tr>
						<td align="left">
                            <%--	 <asp:HiddenField ID="hfcheckid" runat="server" Value='<%# Eval("ID") %>' />--%>
							<table>
								<tr>
									<td>
										<dx:ASPxButton ID="FormOkButton" runat="server" Text="OK" CssClass="sysButton" OnClick="FormOkButton_Click"
											AutoPostBack="True" ValidationGroup="val">
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
	</table>
</asp:Content>
