<%@ Page Title="VitalSigns Plus - Mobile Users" Language="C#" MasterPageFile="~/DashboardSite.Master"
	AutoEventWireup="true" CodeBehind="MobileUsers.aspx.cs" Inherits="VSWebUI.Dashboard.MobileUsers" %>

<%@ MasterType VirtualPath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
	<script src="../js/bootstrap.min.js" type="text/javascript"></script>
	<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet'
		type='text/css' />
	<script type="text/javascript">
		$(document).ready(function () {
			$('.imagen[src=""]').hide();
			$('.imagen:not([src=""])').show();
			$('.logoimg[src=""]').hide();
			$('.logoimg:not([src=""])').show();
		});

		$("#grid").click(function () {
			$('.imagen[src=""]').hide();
			$('.imagen:not([src=""])').show();
		});

		function toggleOSTypeGraph(me) {
			if (me.innerHTML == "Show Bar Chart (top 20 OS names)") {
				$get("OSBar").style.display = "block";
				$get("OSBar").style.visibility = "visible";
				$get("OSPie").style.display = "none";
				$get("OSPie").style.visibility = "hidden";
				me.innerHTML = "Show Pie Chart (top 20 OS names)"
			}
			else {
				$get("OSPie").style.display = "block";
				$get("OSPie").style.visibility = "visible";
				$get("OSBar").style.display = "none";
				$get("OSBar").style.visibility = "hidden";
				me.innerHTML = "Show Bar Chart (top 20 OS names)"
			}
		}
		function Resized() {
			var callbackState = document.getElementById('ContentPlaceHolder1_callbackState').value;

			if (callbackState == 0)
				DoCallback();
		}

		function DoCallback() {
			//10/8/2013 NS modified
			document.getElementById('ContentPlaceHolder1_chartWidth').value = document.body.offsetWidth - 325;
			var chartwidth = document.getElementById('ContentPlaceHolder1_chartWidth').value;
			//3/28/2014 NS commented out for 
			//FileOpensCumulativeWebChart.PerformCallback();
			//FileOpensWebChart.PerformCallback();
			//chttpSessionsWebChart.PerformCallback();
			DeviceTypeChart.PerformCallback();
			OSTypeChart.PerformCallback();
			OSBarChart1.PerformCallback();
			SyncTypeChart.PerformCallback();
			chartwidth = document.body.offsetWidth - 105;
			//3/28/2014 NS commented out for 
			//var mailpanel = document.getElementById('ContentPlaceHolder1_ASPxPageControl1_mailFileOpensDeltaRoundPanel');
			//mailpanel.style.width = chartwidth + "px";
			//var mailpanel2 = document.getElementById('ContentPlaceHolder1_ASPxPageControl1_mailFileOpensRoundPanel');
			//mailpanel2.style.width = chartwidth + "px";
			//var httppanel = document.getElementById('ContentPlaceHolder1_ASPxPageControl1_httpSessionsASPxRoundPanel');
			//httppanel.style.width = chartwidth + "px";
		}

		function ResizeChart(s, e) {
			document.getElementById('ContentPlaceHolder1_callbackState').value = 0;
			s.GetMainElement().style.width = document.getElementById('ContentPlaceHolder1__chartWidth').value + "px";
			//cSelectServerRoundPanel.style.width = document.getElementById('ContentPlaceHolder1__chartWidth').value + "px";
		}

		function ResetCallbackState() {
			window.document.form1.elements['ContentPlaceHolder1_callbackState'].value = 1;
		}
		function InitPopupMenuHandler(s, e) {
			//var menu1 = document.getElementById('ContentPlaceHolder1_ASPxPageControl1_UserDetailsMenu');
			//alert(menu1.style.visibility);
			//if (menu1.style.visibility == "visible") {
			var gridCell = document.getElementById('Td1');
			ASPxClientUtils.AttachEventToElement(gridCell, 'contextmenu', OnGridContextMenu);
			//        var imgButton = document.getElementById('popupButton');
			//        ASPxClientUtils.AttachEventToElement(imgButton, 'contextmenu', OnPreventContextMenu);
			//}
		}
		//    function OnGridContextMenu(evt) {
		//        var SortPopupMenu = popupmenu;
		//        SortPopupMenu.ShowAtPos(evt.clientX + ASPxClientUtils.GetDocumentScrollLeft(), evt.clientY + ASPxClientUtils.GetDocumentScrollTop());
		//        return OnPreventContextMenu(evt);
		//    }
		function OnPreventContextMenu(evt) {
			return ASPxClientUtils.PreventEventAndBubble(evt);
		}
		function UsersGrid_ContextMenu(s, e) {
			if (e.objectType == "row") {
				s.SetFocusedRowIndex(e.index);
				StatusListPopup.ShowAtPos(ASPxClientUtils.GetEventX(e.htmlEvent), ASPxClientUtils.GetEventY(e.htmlEvent));
			}
		}
		function UsersGrid_FocusedRowChanged(s, e) {
			if (e.objectType != "row") return;
			s.SetFocusedRowIndex(e.index);
		}
		function findPos0(obj, event) {

			var ispan = obj.id;
			ispan = ispan.replace("detailsLabel", "detailsspan");
			var ispanCtl = document.getElementById(ispan);
			ispanCtl.style.left = (event.clientX + 25) + "px"; //obj.offsetParent.offsetLeft + "px";
			ispanCtl.style.top = (event.clientY - 40) + "px";
		}
		function findPos(obj, event, replacestring, replacewith) {
			//alert(obj.offsetParent.offsetLeft);
			var ispan = obj.id;
			ispan = ispan.replace(replacestring, replacewith);
			var ispanCtl = document.getElementById(ispan);

			var xOffset = Math.max(document.documentElement.scrollLeft, document.body.scrollLeft);
			var yOffset = Math.max(document.documentElement.scrollTop, document.body.scrollTop);

			ispanCtl.style.left = (event.clientX + xOffset + 25) + "px"; //obj.offsetParent.offsetLeft + "px";
			ispanCtl.style.top = (event.clientY + yOffset + -40) + "px";
		}


		function OnGridContextMenu(evt) {
			UsersGrid.SetFocusedRowIndex(e.index);
			var SortPopupMenu = StatusListPopup;
			SortPopupMenu.ShowAtPos(evt.clientX + ASPxClientUtils.GetDocumentScrollLeft(), evt.clientY + ASPxClientUtils.GetDocumentScrollTop());
			return OnPreventContextMenu(evt);
		}
		function OnItemClick(s, e) {
			if (e.item.parent == s.GetRootItem())
				e.processOnServer = false;
		}
		document.onmousemove = follow;
   
	</script>
	<style id="styles" type="text/css">
		/* 4/21/2014 NS added */
		.textalignmiddle
		{
			width: 100%;
			height: 100%;
		}
		.parent
		{
			position: relative;
			height: 30px;
			width: 145px;
		}
		.msg
		{
			/* 4/21/2014 NS commented out */ /*
            position: absolute;
            left: 0px;
            top: 12px;
            */
			text-align: right;
			vertical-align: middle;
			height: 30px;
		}
		.b
		{
			position: absolute;
			top: -10px;
		}
		a.tooltip2
		{
			text-decoration: none;
			outline: none; /* 4/21/2014 NS commented out */ /* display: inline-block; */
			width: 100%;
			height: 100%; /* 4/21/2014 NS commented out */ /*color: Black;*/
			top: -5px;
		}
		a.tooltip2 strong
		{
			line-height: 30px;
		}
		a.tooltip2:hover
		{
			text-decoration: none; /* 4/21/2014 NS commented out */ /*color: Black;*/
		}
		a.tooltip2 .span2
		{
			text-decoration: none;
			z-index: 10;
			display: none;
			padding: 14px 20px;
			margin-top: -30px;
			float: left;
			width: 240px;
			line-height: 16px;
		}
		a.tooltip2:hover .span2
		{
			text-decoration: none;
			text-align: left;
			float: left;
			margin: 0px;
			left: -240px;
			display: inline-block;
			position: absolute;
			color: #111;
			white-space: normal;
			border: 1px solid #DCA;
			background: #fffAF0;
		}
		
		.callout2
		{
			z-index: 20;
			position: absolute;
			top: 30px;
			border: 0;
			left: -12px;
		}
		
		
		a.tooltip
		{
			text-decoration: none;
			outline: none;
			display: inline-block;
			width: 100%;
			height: 100%;
			color: Black;
			top: -5px;
		}
		a.tooltip strong
		{
			line-height: 30px;
		}
		a.tooltip:hover
		{
			text-decoration: none;
			color: Black;
		}
		a.tooltip span
		{
			z-index: 10;
			display: none;
			padding: 14px 20px;
			margin-top: -30px;
			margin-left: 28px;
			width: 240px;
			line-height: 16px;
		}
		a.tooltip:hover span
		{
			text-align: left;
			display: inline-block;
			position: absolute;
			color: #111;
			white-space: normal;
			border: 1px solid #DCA;
			background: #fffAF0;
		}
		
		.callout
		{
			z-index: 20;
			position: absolute;
			top: 30px;
			border: 0;
			left: -12px;
		}
		a.noclass
		{
			line-height: 0px;
			visibility: hidden;
		}
		a.noclass span.tip
		{
			z-index: 10;
			display: none;
			padding: 14px 20px;
			margin-top: -50px;
			margin-left: -290px;
			width: 240px;
			line-height: 0px;
		}
		a.tooltip1
		{
			outline: none; /* 4/21/2014 NS commented out */ /* display: inline-block; */
			width: 100%;
			height: 100%;
			color: Black;
			top: -5px;
		}
		a.tooltip1 strong
		{
			line-height: 30px;
		}
		a.tooltip1:hover
		{
			text-decoration: none;
			color: Black;
		}
		a.tooltip1 span.tip
		{
			z-index: 10;
			display: none;
			padding: 14px 20px;
			margin-top: -50px;
			margin-left: -290px;
			width: 240px;
			line-height: 16px;
		}
		a.tooltip1:hover span.tip
		{
			text-align: left;
			display: inline-block;
			position: absolute;
			color: #111;
			white-space: normal;
			border: 1px solid #DCA;
			background: #fffAF0;
		}
		
		.callout1
		{
			z-index: 20;
			position: absolute;
			top: 30px;
			border: 0;
			left: 279px;
		}
		
		/*CSS3 extras*/
		a.tooltip2 .span2
		{
			text-decoration: none;
			border-radius: 4px;
			-moz-border-radius: 4px;
			-webkit-border-radius: 4px;
			-moz-box-shadow: 5px 5px 8px #CCC;
			-webkit-box-shadow: 5px 5px 8px #CCC;
			box-shadow: 5px 5px 8px #CCC;
		}
		a.tooltip span
		{
			border-radius: 4px;
			-moz-border-radius: 4px;
			-webkit-border-radius: 4px;
			-moz-box-shadow: 5px 5px 8px #CCC;
			-webkit-box-shadow: 5px 5px 8px #CCC;
			box-shadow: 5px 5px 8px #CCC;
		}
		a.tooltip1 span.tip
		{
			border-radius: 4px;
			-moz-border-radius: 4px;
			-webkit-border-radius: 4px;
			-moz-box-shadow: 5px 5px 8px #CCC;
			-webkit-box-shadow: 5px 5px 8px #CCC;
			box-shadow: 5px 5px 8px #CCC;
		}
	</style>
	<style type="text/css">
		.circle1
		{
			border-radius: 50%/50%;
			width: 8px;
			height: 8px;
		}
		.circle2
		{
			border-radius: 50%/50%;
			width: 8px;
			height: 8px;
		}
		
		.circle3
		{
			border-radius: 50%/50%;
			width: 8px;
			height: 8px;
		}
		
		.normalrow
		{
			background-color: white;
		}
		
		.highlightrow
		{
			background-color: #cccccc;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<asp:HiddenField ID="hdnResetTable" runat="server" Value="" />
	<dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="Callback">
		<ClientSideEvents CallbackComplete="function(s, e) { LoadingPanel.Hide(); }" />
	</dx:ASPxCallback>
	<table width="100%">
		<tr>
			<td>
				<table>
					<tr>
						<td>
							<img alt="" src="../images/icons/group.png" />
						</td>
						<td>
							<div class="header" id="servernamelbldisp" runat="server">
								Mobile Users</div>
						</td>
					</tr>
				</table>
			</td>
			<td>
				&nbsp;
			</td>
			<td align="right">
				<table>
					<tr>
						<td>
							<div class="infored" id="lastSyncDiv" runat="server" style="display: none">
								<asp:Label ID="LastSyncLabel" runat="server" Text=""></asp:Label>
							</div>
						</td>
						<td>
							<dx:ASPxMenu ID="ASPxMenu1" runat="server" ShowAsToolbar="True" Theme="Moderno" OnItemClick="ASPxMenu1_ItemClick">
								<ClientSideEvents ItemClick="OnItemClick" />
								<Items>
									<dx:MenuItem Name="MainItem">
										<Items>
											<dx:MenuItem Name="ExportXLSItem" Text="Export to XLS">
											</dx:MenuItem>
											<dx:MenuItem Name="ExportXLSXItem" Text="Export to XLSX">
											</dx:MenuItem>
											<dx:MenuItem Name="ExportPDFItem" Text="Export to PDF">
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
	<input id="chartWidth" type="hidden" runat="server" value="400" />
	<input id="callbackState" type="hidden" runat="server" value="0" />
	<table width="100%">
		<tr>
			<td valign="top">
				<dx:ASPxNavBar ID="ASPxNavBar1" runat="server" AutoPostBack="True" Theme="MetropolisBlue"
					AllowSelectItem="True" BackColor="Transparent" EnableAnimation="True" EnableTheming="False"
					EnableViewState="False" OnItemClick="ASPxNavBar1_ItemClick">
					<Groups>
						<dx:NavBarGroup Name="Users" Text="Users">
							<Items>
								<dx:NavBarItem Name="item1" Text="Users with more than one device">
								</dx:NavBarItem>
								<dx:NavBarItem Name="item2" Text="All key users">
								</dx:NavBarItem>
							</Items>
						</dx:NavBarGroup>
						<dx:NavBarGroup Name="Devices" Text="Devices">
							<Items>
								<dx:NavBarItem Name="item4" Text="Unique devices" Selected="True">
								</dx:NavBarItem>
								<dx:NavBarItem Name="item2" Text="Devices synchronized in the last X minutes">
								</dx:NavBarItem>
								<dx:NavBarItem Name="item3" Text="Devices NOT synchronized in the last X minutes">
								</dx:NavBarItem>
								<dx:NavBarItem Name="item5" Text="Devices NOT synchronized within the last X day(s)">
								</dx:NavBarItem>
								<dx:NavBarItem Name="item6" Text="Devices by server">
								</dx:NavBarItem>
								<dx:NavBarItem Name="item7" Text="Devices by OS">
								</dx:NavBarItem>
								<dx:NavBarItem Name="item8" Text="Device sync times">
								</dx:NavBarItem>
								<dx:NavBarItem Name="item9" Text="Device count per user">
								</dx:NavBarItem>
								<dx:NavBarItem Name="item1" Text="All devices">
								</dx:NavBarItem>
							</Items>
						</dx:NavBarGroup>
					</Groups>
					<CollapseImage Height="13px" Url="~/images/icons/nbCollapse.gif" Width="13px">
					</CollapseImage>
					<ExpandImage Height="13px" Url="~/images/icons/nbExpand.gif" Width="13px">
					</ExpandImage>
					<GroupHeaderStyle BackColor="#587EC4" ForeColor="White" HorizontalAlign="Left">
						<Paddings PaddingBottom="5px" PaddingLeft="12px" PaddingRight="9px" PaddingTop="4px" />
						<Border BorderColor="#C9C9C9" />
						<BorderBottom BorderStyle="None" />
					</GroupHeaderStyle>
					<GroupHeaderStyleCollapsed>
						<BorderBottom BorderStyle="Solid" />
					</GroupHeaderStyleCollapsed>
					<ItemStyle Wrap="True" BackColor="Transparent" CssClass="lblsmallFont" HorizontalAlign="Left"
						VerticalAlign="Middle">
						<SelectedStyle BackColor="#AEAFB0" ForeColor="White">
							<Border BorderWidth="0px" />
						</SelectedStyle>
						<HoverStyle BackColor="#E0EEFE" ForeColor="#1473D2">
							<Border BorderWidth="0px" />
						</HoverStyle>
						<Paddings PaddingBottom="2px" PaddingLeft="2px" PaddingRight="5px" PaddingTop="2px" />
					</ItemStyle>
				</dx:ASPxNavBar>
			</td>
			<td>
				&nbsp;
			</td>
			<td valign="top">
				<div id="gridDiv" style="display: block" runat="server">
					<div id="infoDivPersistent" class="info" runat="server">
						Right click on the user that you want to monitor in the All Devices list to alert
						if the sync does not happen in x number of minutes.
					</div>
					<div id="infoDivInactive" class="info" style="display: none" runat="server">
						Devices that synchronize with more than one server will display in the grid multiple
						times.
					</div>
					<asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
						<ContentTemplate>
							<table>
								<tr>
									<td>
										<dx:ASPxLabel ID="trackLabel" runat="server" Text="Enter the number of minutes:"
											CssClass="lblsmallFont" Visible="False">
										</dx:ASPxLabel>
									</td>
									<td>
										<dx:ASPxSpinEdit ID="ASPxSpinEdit1" runat="server" Number="15" Increment="5" MaxValue="720"
											Visible="False" OnNumberChanged="ASPxSpinEdit1_NumberChanged" AutoPostBack="True">
											<%-- 2/1/2016 Durga Modified for VSPLUS 2505 --%>
											<ClientSideEvents NumberChanged="function(s, e) {																			
                                            Callback.PerformCallback();
                                            LoadingPanel.Show();
                                        }" KeyPress="function(s, e) {
										
											evt = e || window.event;
										
											if (evt.htmlEvent.keyCode == 13)
											{
    											evt.htmlEvent.preventDefault();
												
											}
											
                                        }" />
										</dx:ASPxSpinEdit>
										<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel"
											Modal="True" ContainerElementID="UsersGrid" Theme="Moderno">
										</dx:ASPxLoadingPanel>
									</td>
								</tr>
							</table>
							<dx:ASPxTrackBar ID="TravelerTrackBar" runat="server" AutoPostBack="True" LargeTickEndValue="120"
								LargeTickInterval="5" LargeTickStartValue="15" MaxValue="120" MinValue="1" OnPositionChanged="TravelerTrackBar_PositionChanged"
								Position="15" PositionEnd="15" PositionStart="5" Step="1" Theme="Office2010Blue"
								Width="400px" Visible="False">
								<ScaleStyle>
									<Border BorderStyle="None" />
								</ScaleStyle>
							</dx:ASPxTrackBar>
							<table class="navbarTbl">
								<tr>
									<td>
										<dx:ASPxPopupMenu ID="TravelerusersPopupMenu" runat="server" ClientInstanceName="popupmenu"
											OnItemClick="TravelerusersPopupMenu_ItemClick" PopupAction="LeftMouseClick" PopupHorizontalAlign="RightSides"
											PopupVerticalAlign="TopSides">
											<Items>
												<dx:MenuItem Name="DenyAccess" Text="Deny Access">
												</dx:MenuItem>
												<dx:MenuItem Name="WipeDevice" Text="Wipe Device">
												</dx:MenuItem>
												<dx:MenuItem Name="ClearWipeRequest" Text="Clear Wipe Request">
												</dx:MenuItem>
												<dx:MenuItem BeginGroup="True" Name="ChangeApproval-Deny" Text="Change Approval - Deny">
												</dx:MenuItem>
												<dx:MenuItem Name="ChangeApproval-Approve" Text="Change Approval - Approve">
												</dx:MenuItem>
												<dx:MenuItem BeginGroup="True" Name="LogLevel-EnableFinest" Text="Log Level - Enable Finest">
												</dx:MenuItem>
												<dx:MenuItem Name="LogLevel-DisableFinest" Text="Log Level - Disable Finest">
												</dx:MenuItem>
												<dx:MenuItem Name="LogLevel-CreateDumpFile" Text="Log Level - Create Dump File">
												</dx:MenuItem>
											</Items>
										</dx:ASPxPopupMenu>
										<dx:ASPxMenu ID="StatusListMenu" runat="server" OnItemClick="StatusListMenu_ItemClick"
											Theme="DevEx" Visible="False">
											<Items>
												<dx:MenuItem Name="Suspend" Text="Monitor Device">
												</dx:MenuItem>
											</Items>
										</dx:ASPxMenu>
										<dx:ASPxPopupMenu ID="StatusListPopupMenu" runat="server" PopupAction="LeftMouseClick"
											PopupHorizontalAlign="RightSides" PopupVerticalAlign="TopSides" ClientInstanceName="StatusListPopup"
											OnItemClick="StatusListPopupMenu_ItemClick">
											<ClientSideEvents Init="InitPopupMenuHandler"></ClientSideEvents>
											<Items>
												<dx:MenuItem Text="Monitor Device" Name="Suspend">
												</dx:MenuItem>
											</Items>
										</dx:ASPxPopupMenu>
										<dx:ASPxPopupControl ID="SuspendPopupControl" runat="server" AllowDragging="True"
											AllowResize="True" ClientInstanceName="FeedPopupControl" CloseAction="CloseButton"
											EnableHierarchyRecreation="True" EnableViewState="False" FooterText="To resize the control use the resize grip or the control's edges"
											HeaderText="Sync Duration" Height="70px" PopupHorizontalAlign="WindowCenter"
											PopupVerticalAlign="WindowCenter" ShowFooter="True" Width="400px" Theme="MetropolisBlue">
											<ContentCollection>
												<dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
													<table id="FeedBackTable" class="EditorsTable" style="width: 100%; height: 100%;">
														<tr>
															<td class="Label">
																<dx:ASPxLabel ID="lblDuration" runat="server" Text="Duration (mins):" Wrap="False">
																</dx:ASPxLabel>
															</td>
															<td>
																<dx:ASPxTextBox ID="TbDuration" runat="server" EnableViewState="False" Width="100%">
																	<MaskSettings Mask="&lt;20..240&gt;" />
																	<ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																		<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																			ValidationExpression="^\d+$" />
																		<RequiredField ErrorText="Enter Sync Duration." IsRequired="True" />
																	</ValidationSettings>
																</dx:ASPxTextBox>
															</td>
														</tr>
														<tr>
															<td class="Label" colspan="2">
																<dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="If a device has not synced for more than the amount of duration specified above, an alert will be triggered by the system. Minimum duration value can be 20 minutes and maximum - 240 minutes.">
																</dx:ASPxLabel>
															</td>
														</tr>
														<tr>
															<td colspan="2">
																&nbsp;
															</td>
														</tr>
														<tr>
															<td colspan="2">
																<dx:ASPxButton ID="test123" runat="server" AutoPostBack="False" CausesValidation="False"
																	EnableTheming="True" OnClick="BtnApply_Click" Text="Apply" CssClass="sysButton">
																</dx:ASPxButton>
															</td>
														</tr>
													</table>
												</dx:PopupControlContentControl>
											</ContentCollection>
										</dx:ASPxPopupControl>
										<table class="navbarTbl">
											<tr>
												<td id="Td1">
													<dx:ASPxGridView ID="UsersGrid" runat="server" AutoGenerateColumns="False" Cursor="pointer"
														EnableTheming="True" KeyFieldName="DeviceID" OnHtmlDataCellPrepared="UsersGrid_HtmlDataCellPrepared"
														OnPageSizeChanged="UsersGrid_PageSizeChanged" Theme="Office2003Blue" OnCustomGroupDisplayText="UsersGrid_CustomGroupDisplayText"
														OnBeforeColumnSortingGrouping="UsersGrid_BeforeColumnSortingGrouping" Width="100%"
														EnableCallBacks="False">
														<ClientSideEvents ContextMenu="UsersGrid_ContextMenu"></ClientSideEvents>
														<SettingsBehavior AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True"
															ColumnResizeMode="NextColumn"></SettingsBehavior>
														<SettingsBehavior AllowFocusedRow="true" />
														<Columns>
															<dx:GridViewCommandColumn ShowInCustomizationForm="True" Visible="False" VisibleIndex="1">
															</dx:GridViewCommandColumn>
															<dx:GridViewDataTextColumn Caption="Monitor?" FieldName="Monitor" VisibleIndex="0"
																Width="80px">
																<DataItemTemplate>
																	<div class="msg">
																		<table class="textalignmiddle" style='<%# GetImage(Eval("Monitoring")) %>'>
																			<tr>
																				<td align="center">
																					<a class='tooltip2'><span id="detailsLabel" runat="server" onmousemove="findPos(this,event,'detailsLabel', 'detailsspan');">
																						<%--<img id="img" runat="server" src='<%# GetImage(Eval("Monitoring")) %>' />--%>
																						<asp:ImageButton ID="ibtnMonitoring" runat="server" OnClientClick="return confirm('Do you want to Suspend Monitoring for this Device?');"
																							OnClick="ibtnMonitoring_click" CommandName="stopMonitoring" CommandArgument='<%#Eval("DeviceID") %>'
																							ImageUrl="~/images/icons/Monitoring.png" />
																					</span>
																						<asp:Label ID="detailsspan" class="span2" runat="server">
																							<img class='callout2' src='../images/callout.gif' />
																							<font style="color: blue; font-size: 16px;">&nbsp<b>Monitoring Information</b></font><br>
																							<asp:Label ID="Label3" Text="&nbsp<b>DeviceId:</b>" runat="server" />
																							<asp:Label ID="Label1" Text='<%# Eval("Monitoring") %>' runat="server" /><br>
																							<asp:Label ID="Label2" Text="&nbsp<b>Sync Duration:</b>" runat="server" />
																							<asp:Label ID="detailsinfoLabel" Text='<%# Eval("SyncTimeThreshold") %>' runat="server" />
																						</asp:Label></a>
																				</td>
																			</tr>
																		</table>
																	</div>
																</DataItemTemplate>
																<HeaderStyle CssClass="GridCssHeader" />
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn FixedStyle="Left" ShowInCustomizationForm="True" VisibleIndex="3"
																Width="30px">
																<PropertiesTextEdit>
																	<FocusedStyle HorizontalAlign="Center">
																	</FocusedStyle>
																</PropertiesTextEdit>
																<DataItemTemplate>
																	<asp:Label ID="lblIcon" runat="server" Text='<%# string.Concat(Eval("OS_Type"),Eval("DeviceName")) %>'
																		Visible="false"></asp:Label><asp:Image ID="IconImage" runat="server" />
																	<dx:ASPxHyperLink ID="ASPxHyperLink2" runat="server" NavigateUrl="<%# SetIcon(Container) %>"
																		Visible="false">
																	</dx:ASPxHyperLink>
																</DataItemTemplate>
																<HeaderStyle CssClass="GridCssHeader" />
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="User Name" FieldName="UserName" FixedStyle="Left"
																ShowInCustomizationForm="True" VisibleIndex="4" Width="170px">
																<PropertiesTextEdit>
																	<FocusedStyle HorizontalAlign="Center">
																	</FocusedStyle>
																</PropertiesTextEdit>
																<Settings AllowAutoFilter="True" AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																<EditCellStyle CssClass="GridCss">
																</EditCellStyle>
																<EditFormCaptionStyle CssClass="GridCss">
																</EditFormCaptionStyle>
																<HeaderStyle CssClass="GridCssHeader" />
																<CellStyle CssClass="GridCss" HorizontalAlign="Center">
																</CellStyle>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Device Name" FieldName="DeviceName" FixedStyle="Left"
																ShowInCustomizationForm="True" VisibleIndex="5">
																<PropertiesTextEdit>
																	<FocusedStyle HorizontalAlign="Center">
																	</FocusedStyle>
																</PropertiesTextEdit>
																<Settings AllowAutoFilter="True" AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																<EditCellStyle CssClass="GridCss">
																</EditCellStyle>
																<EditFormCaptionStyle CssClass="GridCss">
																</EditFormCaptionStyle>
																<HeaderStyle CssClass="GridCssHeader" />
																<CellStyle CssClass="GridCss" HorizontalAlign="Center">
																</CellStyle>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Connection State" FieldName="ConnectionState"
																ShowInCustomizationForm="True" Visible="False" VisibleIndex="6" Width="105px">
																<PropertiesTextEdit>
																	<FocusedStyle HorizontalAlign="Center">
																	</FocusedStyle>
																</PropertiesTextEdit>
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																<EditCellStyle CssClass="GridCss">
																</EditCellStyle>
																<EditFormCaptionStyle CssClass="GridCss">
																</EditFormCaptionStyle>
																<HeaderStyle CssClass="GridCssHeader" />
																<CellStyle CssClass="GridCss" HorizontalAlign="Center">
																</CellStyle>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataDateColumn Caption="Last SyncTime" FieldName="LastSyncTime" ShowInCustomizationForm="True"
																VisibleIndex="9" Width="150px">
																<PropertiesDateEdit DisplayFormatString="">
																</PropertiesDateEdit>
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																<EditCellStyle CssClass="GridCss">
																</EditCellStyle>
																<EditFormCaptionStyle CssClass="GridCss">
																</EditFormCaptionStyle>
																<HeaderStyle CssClass="GridCssHeader" />
																<CellStyle CssClass="GridCss" Font-Bold="False" Font-Overline="False" HorizontalAlign="Center">
																</CellStyle>
															</dx:GridViewDataDateColumn>
															<dx:GridViewDataTextColumn Caption="OS Type" FieldName="OS_Type_Min" ShowInCustomizationForm="True"
																VisibleIndex="8" Width="150px">
																<PropertiesTextEdit>
																	<FocusedStyle HorizontalAlign="Center">
																	</FocusedStyle>
																</PropertiesTextEdit>
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																<EditCellStyle CssClass="GridCss">
																</EditCellStyle>
																<EditFormCaptionStyle CssClass="GridCss">
																</EditFormCaptionStyle>
																<HeaderStyle CssClass="GridCssHeader" />
																<CellStyle CssClass="GridCss" HorizontalAlign="Center">
																</CellStyle>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="HA Pool" FieldName="HAPoolName" ShowInCustomizationForm="True"
																VisibleIndex="13">
																<PropertiesTextEdit>
																	<FocusedStyle HorizontalAlign="Center">
																	</FocusedStyle>
																</PropertiesTextEdit>
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																<EditCellStyle CssClass="GridCss">
																</EditCellStyle>
																<EditFormCaptionStyle CssClass="GridCss">
																</EditFormCaptionStyle>
																<HeaderStyle CssClass="GridCssHeader" />
																<CellStyle CssClass="GridCss1" HorizontalAlign="Center">
																</CellStyle>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Client Build" FieldName="Client_Build" ShowInCustomizationForm="True"
																VisibleIndex="12">
																<PropertiesTextEdit>
																	<FocusedStyle HorizontalAlign="Center">
																	</FocusedStyle>
																</PropertiesTextEdit>
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																<EditCellStyle CssClass="GridCss">
																</EditCellStyle>
																<EditFormCaptionStyle CssClass="GridCss">
																</EditFormCaptionStyle>
																<HeaderStyle CssClass="GridCssHeader" />
																<CellStyle CssClass="GridCss1" HorizontalAlign="Center">
																</CellStyle>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Notification Type" FieldName="NotificationType"
																ShowInCustomizationForm="True" Visible="False" VisibleIndex="17">
																<PropertiesTextEdit>
																	<FocusedStyle HorizontalAlign="Center">
																	</FocusedStyle>
																</PropertiesTextEdit>
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																<EditCellStyle CssClass="GridCss">
																</EditCellStyle>
																<EditFormCaptionStyle CssClass="GridCss">
																</EditFormCaptionStyle>
																<HeaderStyle CssClass="GridCssHeader" />
																<CellStyle CssClass="GridCss" HorizontalAlign="Center">
																</CellStyle>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="ID" FieldName="ID" ShowInCustomizationForm="True"
																Visible="False" VisibleIndex="18">
																<PropertiesTextEdit>
																	<FocusedStyle HorizontalAlign="Center">
																	</FocusedStyle>
																</PropertiesTextEdit>
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																<EditCellStyle CssClass="GridCss">
																</EditCellStyle>
																<EditFormCaptionStyle CssClass="GridCss">
																</EditFormCaptionStyle>
																<HeaderStyle CssClass="GridCssHeader" />
																<CellStyle CssClass="GridCss" HorizontalAlign="Center">
																</CellStyle>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Doc ID" FieldName="DocID" ShowInCustomizationForm="True"
																Visible="False" VisibleIndex="19" Width="250px">
																<PropertiesTextEdit>
																	<FocusedStyle HorizontalAlign="Center">
																	</FocusedStyle>
																</PropertiesTextEdit>
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																<EditCellStyle CssClass="GridCss">
																</EditCellStyle>
																<EditFormCaptionStyle CssClass="GridCss">
																</EditFormCaptionStyle>
																<HeaderStyle CssClass="GridCssHeader" />
																<CellStyle CssClass="GridCss" HorizontalAlign="Center">
																</CellStyle>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Device Type" FieldName="device_type" ShowInCustomizationForm="True"
																Visible="False" VisibleIndex="20">
																<PropertiesTextEdit>
																	<FocusedStyle HorizontalAlign="Center">
																	</FocusedStyle>
																</PropertiesTextEdit>
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																<EditCellStyle CssClass="GridCss">
																</EditCellStyle>
																<EditFormCaptionStyle CssClass="GridCss">
																</EditFormCaptionStyle>
																<HeaderStyle CssClass="GridCssHeader" />
																<CellStyle CssClass="GridCss" HorizontalAlign="Center">
																</CellStyle>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Access" FieldName="Access" ShowInCustomizationForm="True"
																VisibleIndex="11">
																<PropertiesTextEdit>
																	<FocusedStyle HorizontalAlign="Center">
																	</FocusedStyle>
																</PropertiesTextEdit>
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																<EditCellStyle CssClass="GridCss">
																</EditCellStyle>
																<EditFormCaptionStyle CssClass="GridCss">
																</EditFormCaptionStyle>
																<HeaderStyle CssClass="GridCssHeader" />
																<CellStyle CssClass="GridCss" HorizontalAlign="Center">
																</CellStyle>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Security Policy" FieldName="Security_Policy"
																ShowInCustomizationForm="True" VisibleIndex="7">
																<PropertiesTextEdit>
																	<FocusedStyle HorizontalAlign="Center">
																	</FocusedStyle>
																</PropertiesTextEdit>
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																<EditCellStyle CssClass="GridCss">
																</EditCellStyle>
																<EditFormCaptionStyle CssClass="GridCss">
																</EditFormCaptionStyle>
																<HeaderStyle CssClass="GridCssHeader" />
																<CellStyle CssClass="GridCss" HorizontalAlign="Center">
																</CellStyle>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Wipe Requested" FieldName="wipeRequested" ShowInCustomizationForm="True"
																Visible="False" VisibleIndex="21">
																<PropertiesTextEdit>
																	<FocusedStyle HorizontalAlign="Center">
																	</FocusedStyle>
																</PropertiesTextEdit>
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																<EditCellStyle CssClass="GridCss">
																</EditCellStyle>
																<EditFormCaptionStyle CssClass="GridCss">
																</EditFormCaptionStyle>
																<HeaderStyle CssClass="GridCssHeader" />
																<CellStyle CssClass="GridCss" HorizontalAlign="Center">
																</CellStyle>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Wipe Options" FieldName="wipeOptions" ShowInCustomizationForm="True"
																Visible="False" VisibleIndex="22">
																<PropertiesTextEdit>
																	<FocusedStyle HorizontalAlign="Center">
																	</FocusedStyle>
																</PropertiesTextEdit>
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																<EditCellStyle CssClass="GridCss">
																</EditCellStyle>
																<EditFormCaptionStyle CssClass="GridCss">
																</EditFormCaptionStyle>
																<HeaderStyle CssClass="GridCssHeader" />
																<CellStyle CssClass="GridCss" HorizontalAlign="Center">
																</CellStyle>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Wipe Status" FieldName="wipeStatus" ShowInCustomizationForm="True"
																Visible="False" VisibleIndex="23">
																<PropertiesTextEdit>
																	<FocusedStyle HorizontalAlign="Center">
																	</FocusedStyle>
																</PropertiesTextEdit>
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																<EditCellStyle CssClass="GridCss">
																</EditCellStyle>
																<EditFormCaptionStyle CssClass="GridCss">
																</EditFormCaptionStyle>
																<HeaderStyle CssClass="GridCssHeader" />
																<CellStyle CssClass="GridCss" HorizontalAlign="Center">
																</CellStyle>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Sync Type" FieldName="SyncType" ShowInCustomizationForm="True"
																VisibleIndex="16">
																<PropertiesTextEdit>
																	<FocusedStyle HorizontalAlign="Center">
																	</FocusedStyle>
																</PropertiesTextEdit>
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																<EditCellStyle CssClass="GridCss">
																</EditCellStyle>
																<EditFormCaptionStyle CssClass="GridCss">
																</EditFormCaptionStyle>
																<HeaderStyle CssClass="GridCssHeader" />
																<CellStyle CssClass="GridCss1" HorizontalAlign="Center">
																</CellStyle>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Wipe Supported" FieldName="wipeSupported" ShowInCustomizationForm="True"
																Visible="False" VisibleIndex="15">
																<PropertiesTextEdit>
																	<FocusedStyle HorizontalAlign="Center">
																	</FocusedStyle>
																</PropertiesTextEdit>
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																<EditCellStyle CssClass="GridCss">
																</EditCellStyle>
																<EditFormCaptionStyle CssClass="GridCss">
																</EditFormCaptionStyle>
																<HeaderStyle CssClass="GridCssHeader" />
																<CellStyle CssClass="GridCss" HorizontalAlign="Center">
																</CellStyle>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Server Name" FieldName="ServerName" ShowInCustomizationForm="True"
																VisibleIndex="14" Width="170px">
																<PropertiesTextEdit>
																	<FocusedStyle HorizontalAlign="Center">
																	</FocusedStyle>
																</PropertiesTextEdit>
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																<EditCellStyle CssClass="GridCss">
																</EditCellStyle>
																<EditFormCaptionStyle CssClass="GridCss">
																</EditFormCaptionStyle>
																<HeaderStyle CssClass="GridCssHeader" />
																<CellStyle CssClass="GridCss" HorizontalAlign="Center" Wrap="False">
																</CellStyle>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Device ID" FieldName="DeviceID" ShowInCustomizationForm="True"
																VisibleIndex="24" Width="200px">
																<PropertiesTextEdit>
																	<FocusedStyle HorizontalAlign="Center">
																	</FocusedStyle>
																</PropertiesTextEdit>
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																<EditCellStyle CssClass="GridCss">
																</EditCellStyle>
																<EditFormCaptionStyle CssClass="GridCss">
																</EditFormCaptionStyle>
																<HeaderStyle CssClass="GridCssHeader" />
																<CellStyle CssClass="GridCss" HorizontalAlign="Center" Wrap="False">
																</CellStyle>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataDateColumn Caption="Last Updated" FieldName="LastUpdated" ShowInCustomizationForm="True"
																VisibleIndex="10" Width="150px">
																<PropertiesDateEdit DisplayFormatString="">
																</PropertiesDateEdit>
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																<EditCellStyle CssClass="GridCss">
																</EditCellStyle>
																<EditFormCaptionStyle CssClass="GridCss">
																</EditFormCaptionStyle>
																<HeaderStyle CssClass="GridCssHeader" />
																<CellStyle CssClass="GridCss" Font-Bold="False" Font-Overline="False" HorizontalAlign="Center">
																</CellStyle>
															</dx:GridViewDataDateColumn>
														</Columns>
														<SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" ColumnResizeMode="NextColumn" />
														<SettingsPager AlwaysShowPager="True" NumericButtonCount="30" PageSize="50">
															<PageSizeItemSettings Items="10,20,50, 100, 200" Visible="True">
															</PageSizeItemSettings>
														</SettingsPager>
														<Settings ShowFilterRow="True" ShowHorizontalScrollBar="True" ShowGroupPanel="True" />
														<Styles>
															<Header VerticalAlign="Middle">
															</Header>
															<AlternatingRow CssClass="GridAltRow" Enabled="True">
															</AlternatingRow>
														</Styles>
														<GroupSummary>
															<dx:ASPxSummaryItem FieldName="UserName" SummaryType="Count" />
														</GroupSummary>
													</dx:ASPxGridView>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td>
										<dx:ASPxPopupControl ID="msgPopupControl" runat="server" HeaderText="VitalSigns"
											Height="120px" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
											Theme="Glass" Width="300px">
											<ContentCollection>
												<dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">
													<table class="tableWidth100Percent">
														<tr>
															<td colspan="3">
																<dx:ASPxLabel ID="msgLabel" runat="server" Text="msglbl">
																</dx:ASPxLabel>
															</td>
														</tr>
														<tr>
															<td align="right">
																&nbsp;
															</td>
															<td align="left">
																&nbsp;
															</td>
															<td align="left">
																&nbsp;
															</td>
														</tr>
														<tr>
															<td align="right">
																<dx:ASPxButton ID="YesButton" runat="server" OnClick="YesButton_Click" Text="Yes"
																	Theme="Office2010Blue" Width="70px">
																</dx:ASPxButton>
															</td>
															<td align="left">
																<dx:ASPxButton ID="NOButton" runat="server" OnClick="NOButton_Click" Text="No" Theme="Office2010Blue"
																	Width="70px">
																</dx:ASPxButton>
															</td>
															<td align="left">
																<dx:ASPxButton ID="CancelButton" runat="server" OnClick="CancelButton_Click" Text="Cancel"
																	Theme="Office2010Blue" Visible="False" Width="70px">
																</dx:ASPxButton>
															</td>
														</tr>
													</table>
												</dx:PopupControlContentControl>
											</ContentCollection>
										</dx:ASPxPopupControl>
										<dx:ASPxPopupControl ID="WipePopupControl" runat="server" HeaderText="Wipe Device"
											Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
											Theme="Glass" Width="400px">
											<ContentCollection>
												<dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server" SupportsDisabledAttribute="True">
													<table class="tableWidth100Percent">
														<tr>
															<td align="left">
																<dx:ASPxLabel ID="ASPxLabel6" runat="server" CssClass="lblsmallFont" Height="16px"
																	Text="Device ID:" Wrap="False">
																</dx:ASPxLabel>
															</td>
															<td align="let">
																<dx:ASPxLabel ID="DeviceIDLabel" runat="server">
																</dx:ASPxLabel>
															</td>
														</tr>
														<tr>
															<td align="left">
																<dx:ASPxLabel ID="ASPxLabel7" runat="server" CssClass="lblsmallFont" Text="User Name:"
																	Wrap="False">
																</dx:ASPxLabel>
															</td>
															<td align="left">
																<dx:ASPxLabel ID="UserNameLabel" runat="server">
																</dx:ASPxLabel>
															</td>
														</tr>
														<tr>
															<td align="left">
																<dx:ASPxLabel ID="ASPxLabel8" runat="server" CssClass="lblsmallFont" Text="Device Name:"
																	Wrap="False">
																</dx:ASPxLabel>
															</td>
															<td align="left" bgcolor="#FFFFCC">
																<dx:ASPxLabel ID="DeviceNameLabel" runat="server">
																</dx:ASPxLabel>
															</td>
														</tr>
														<tr>
															<td align="center" colspan="2" valign="middle">
																&nbsp;
															</td>
														</tr>
														<tr>
															<td align="center" colspan="2">
																<dx:ASPxRoundPanel ID="wipeoptionsRoundPanel" runat="server" HeaderText="Wipe Options"
																	Theme="Glass" Width="300px">
																	<PanelCollection>
																		<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
																			<table class="tableWidth100Percent">
																				<tr>
																					<td align="left">
																						<dx:ASPxCheckBox ID="HardCheckBox" runat="server" CheckState="Unchecked" Text="Hard Device Reset">
																						</dx:ASPxCheckBox>
																					</td>
																				</tr>
																				<tr>
																					<td align="left">
																						<dx:ASPxCheckBox ID="TravelerAppCheckBox" runat="server" CheckState="Unchecked" Text="Traveler Application and Data">
																						</dx:ASPxCheckBox>
																					</td>
																				</tr>
																				<tr>
																					<td align="left">
																						<dx:ASPxCheckBox ID="StorageCadrCheckBox" runat="server" CheckState="Unchecked" Text="Storage Card">
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
															<td colspan="2">
																<table class="tableWidth100Percent">
																	<tr>
																		<td align="right">
																			<dx:ASPxButton ID="WipeButton" runat="server" OnClick="WipeButton_Click" Text="Wipe"
																				Theme="Office2010Blue" Width="70px">
																			</dx:ASPxButton>
																		</td>
																		<td>
																			<dx:ASPxButton ID="WipeCancelButton" runat="server" OnClick="WipeCancelButton_Click"
																				Text="Cancel" Theme="Office2010Blue" Width="70px">
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
									<td>
										&nbsp;
									</td>
								</tr>
							</table>
						</ContentTemplate>
					</asp:UpdatePanel>
				</div>
				<div id="devicesSrvDiv" style="display: none" runat="server">
					<asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
						<Triggers>
							<asp:AsyncPostBackTrigger ControlID="ServerComboBox" />
						</Triggers>
						<ContentTemplate>
							<table class="navbarTbl">
								<tr>
									<td>
										<table>
											<tr>
												<td>
													<dx:ASPxLabel ID="ASPxLabel4" runat="server" CssClass="lblsmallFont" Text="Server:">
													</dx:ASPxLabel>
												</td>
												<td>
													<dx:ASPxComboBox ID="ServerComboBox" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ServerComboBox_SelectedIndexChanged">
													</dx:ASPxComboBox>
												</td>
												<td>
													&nbsp;
												</td>
												<td>
													<dx:ASPxLabel ID="SortLabel" runat="server" Text="Sort by:" Width="40px" CssClass="lblsmallFont"
														Wrap="False">
													</dx:ASPxLabel>
												</td>
												<td>
													<dx:ASPxRadioButtonList ID="SortRadioButtonList1" runat="server" RepeatDirection="Horizontal"
														SelectedIndex="0" AutoPostBack="True" OnSelectedIndexChanged="SortRadioButtonList1_SelectedIndexChanged"
														Width="250px">
														<Items>
															<dx:ListEditItem Text="Device Name" Value="DeviceName" />
															<dx:ListEditItem Selected="True" Text="Device Count" Value="No_of_Users" />
														</Items>
													</dx:ASPxRadioButtonList>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td>
										<dxchartsui:WebChartControl ID="deviceTypeWebChart" runat="server" ClientInstanceName="DeviceTypeChart"
											Height="500px" OnCustomCallback="deviceTypeWebChart_CustomCallback" Width="1000px"
											CrosshairEnabled="True">
											<diagramserializable>
                                                    <cc1:XYDiagram>
                                                        <axisx visibleinpanesserializable="-1">
                                                            <range sidemarginsenabled="True" />
                                                        <range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /></axisx><axisy visibleinpanesserializable="-1">
                                                            <range sidemarginsenabled="True" />
                                                            <numericoptions format="Number" precision="0" />
                                                        <range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /></axisy></cc1:XYDiagram></diagramserializable>
											<fillstyle>
                                                    <optionsserializable>
                                                        <cc1:SolidFillOptions />
                                                    </optionsserializable></fillstyle>
											<seriesserializable>
                                                    <cc1:Series Name="Series 1">
                                                        <viewserializable>
                                                            <cc1:SideBySideBarSeriesView>
                                                            </cc1:SideBySideBarSeriesView>
                                                        </viewserializable><labelserializable>
                                                            <cc1:SideBySideBarSeriesLabel LineVisible="True">
                                                                <fillstyle>
                                                                    <optionsserializable>
                                                                        <cc1:SolidFillOptions />
                                                                    </optionsserializable>
                                                                </fillstyle>
                                                                <pointoptionsserializable>
                                                                    <cc1:PointOptions>
                                                                    </cc1:PointOptions>
                                                                </pointoptionsserializable>
                                                            </cc1:SideBySideBarSeriesLabel>
                                                        </labelserializable><legendpointoptionsserializable>
                                                            <cc1:PointOptions>
                                                            </cc1:PointOptions>
                                                        </legendpointoptionsserializable></cc1:Series><cc1:Series Name="Series 2">
                                                        <viewserializable>
                                                            <cc1:SideBySideBarSeriesView>
                                                            </cc1:SideBySideBarSeriesView>
                                                        </viewserializable><labelserializable>
                                                            <cc1:SideBySideBarSeriesLabel LineVisible="True">
                                                                <fillstyle>
                                                                    <optionsserializable>
                                                                        <cc1:SolidFillOptions />
                                                                    </optionsserializable>
                                                                </fillstyle>
                                                                <pointoptionsserializable>
                                                                    <cc1:PointOptions>
                                                                    </cc1:PointOptions>
                                                                </pointoptionsserializable>
                                                            </cc1:SideBySideBarSeriesLabel>
                                                        </labelserializable><legendpointoptionsserializable>
                                                            <cc1:PointOptions>
                                                            </cc1:PointOptions>
                                                        </legendpointoptionsserializable></cc1:Series></seriesserializable>
											<seriestemplate>
                                                    <viewserializable>
                                                        <cc1:SideBySideBarSeriesView>
                                                        </cc1:SideBySideBarSeriesView>
                                                    </viewserializable><labelserializable>
                                                        <cc1:SideBySideBarSeriesLabel LineVisible="True">
                                                            <fillstyle>
                                                                <optionsserializable>
                                                                    <cc1:SolidFillOptions />
                                                                </optionsserializable>
                                                            </fillstyle>
                                                            <pointoptionsserializable>
                                                                <cc1:PointOptions>
                                                                </cc1:PointOptions>
                                                            </pointoptionsserializable>
                                                        </cc1:SideBySideBarSeriesLabel>
                                                    </labelserializable><legendpointoptionsserializable>
                                                        <cc1:PointOptions>
                                                        </cc1:PointOptions>
                                                    </legendpointoptionsserializable></seriestemplate>
											<crosshairoptions>
                                                    <commonlabelpositionserializable>
                                                        <cc1:CrosshairMousePosition />
                                                    </commonlabelpositionserializable></crosshairoptions>
											<tooltipoptions>
                                                    <tooltippositionserializable>
                                                        <cc1:ToolTipMousePosition />
                                                    </tooltippositionserializable></tooltipoptions>
										</dxchartsui:WebChartControl>
									</td>
								</tr>
							</table>
						</ContentTemplate>
					</asp:UpdatePanel>
				</div>
				<div id="devicesOSDiv" style="display: none" runat="server">
					<asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
						<Triggers>
							<asp:AsyncPostBackTrigger ControlID="OSComboBox" />
						</Triggers>
						<ContentTemplate>
							<table class="navbarTbl">
								<tr>
									<td>
										<table>
											<tr>
												<td>
													<dx:ASPxLabel ID="ASPxLabel5" runat="server" CssClass="lblsmallFont" Text="Server:">
													</dx:ASPxLabel>
												</td>
												<td>
													<dx:ASPxComboBox ID="OSComboBox" runat="server" AutoPostBack="True" OnSelectedIndexChanged="OSComboBox_SelectedIndexChanged">
													</dx:ASPxComboBox>
												</td>
												<td>
													&nbsp;
												</td>
												<td>
													<dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Sort by:" Width="40px" CssClass="lblsmallFont"
														Wrap="False">
													</dx:ASPxLabel>
												</td>
												<td>
													<dx:ASPxRadioButtonList ID="SortRadioButtonList2" runat="server" RepeatDirection="Horizontal"
														SelectedIndex="0" AutoPostBack="True" OnSelectedIndexChanged="SortRadioButtonList2_SelectedIndexChanged"
														Width="250px">
														<Items>
															<dx:ListEditItem Text="OS Name" Value="OS_Type_Min" />
															<dx:ListEditItem Selected="True" Text="Device Count" Value="No_of_Users" />
														</Items>
													</dx:ASPxRadioButtonList>
												</td>
												<td>
													&nbsp;
												</td>
												<td>
													<a id="OSGraphType" onclick="toggleOSTypeGraph(this);" href="#" style="color: Blue;">
														Show Pie Chart (top 20 OS names)</a>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td>
										<div id="OSPie" style="visibility: hidden; display: none;">
											<dxchartsui:WebChartControl ID="OSTypeWebChartControl" runat="server" ClientInstanceName="OSTypeChart"
												Height="400px" OnCustomCallback="OSTypeWebChartControl_CustomCallback" Width="1000px"
												CrosshairEnabled="True">
												<diagramserializable>
                                                    <cc1:SimpleDiagram>
                                                    </cc1:SimpleDiagram>
                                                </diagramserializable>
												<fillstyle>
                                                    <optionsserializable>
                                                        <cc1:SolidFillOptions />
                                                    </optionsserializable></fillstyle>
												<seriesserializable>
                                                    <cc1:Series Name="Series 1">
                                                        <viewserializable>
                                                            <cc1:PieSeriesView RuntimeExploding="False">
                                                            </cc1:PieSeriesView>
                                                        </viewserializable><labelserializable>
                                                            <cc1:PieSeriesLabel LineVisible="True">
                                                                <fillstyle>
                                                                    <optionsserializable>
                                                                        <cc1:SolidFillOptions />
                                                                    </optionsserializable>
                                                                </fillstyle>
                                                                <pointoptionsserializable>
                                                                    <cc1:PiePointOptions>
                                                                    </cc1:PiePointOptions>
                                                                </pointoptionsserializable>
                                                            </cc1:PieSeriesLabel>
                                                        </labelserializable><legendpointoptionsserializable>
                                                            <cc1:PiePointOptions>
                                                            </cc1:PiePointOptions>
                                                        </legendpointoptionsserializable></cc1:Series></seriesserializable>
												<seriestemplate>
                                                    <viewserializable>
                                                        <cc1:PieSeriesView RuntimeExploding="False">
                                                        </cc1:PieSeriesView>
                                                    </viewserializable><labelserializable>
                                                        <cc1:PieSeriesLabel LineVisible="True">
                                                            <fillstyle>
                                                                <optionsserializable>
                                                                    <cc1:SolidFillOptions />
                                                                </optionsserializable>
                                                            </fillstyle>
                                                            <pointoptionsserializable>
                                                                <cc1:PiePointOptions PercentOptions-ValueAsPercent="False">
                                                                </cc1:PiePointOptions>
                                                            </pointoptionsserializable>
                                                        </cc1:PieSeriesLabel>
                                                    </labelserializable><legendpointoptionsserializable>
                                                        <cc1:PiePointOptions PercentOptions-ValueAsPercent="False">
                                                        </cc1:PiePointOptions>
                                                    </legendpointoptionsserializable></seriestemplate>
												<crosshairoptions>
                                                    <commonlabelpositionserializable>
                                                        <cc1:CrosshairMousePosition />
                                                    </commonlabelpositionserializable></crosshairoptions>
												<tooltipoptions>
                                                    <tooltippositionserializable>
                                                        <cc1:ToolTipMousePosition />
                                                    </tooltippositionserializable></tooltipoptions>
											</dxchartsui:WebChartControl>
										</div>
										<div id="OSBar">
											<dxchartsui:WebChartControl ID="OSBarChart1" runat="server" ClientInstanceName="OSBarChart1"
												Height="600px" OnCustomCallback="OSBarChart1_CustomCallback" Width="1000px" CrosshairEnabled="True">
												<diagramserializable>
                                                    <cc1:XYDiagram>
                                                        <axisx visibleinpanesserializable="-1">
                                                            <range sidemarginsenabled="True" />
                                                        <range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /></axisx><axisy visibleinpanesserializable="-1">
                                                            <range sidemarginsenabled="True" />
                                                            <numericoptions format="Number" precision="0" />
                                                        <range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /></axisy></cc1:XYDiagram></diagramserializable>
												<fillstyle>
                                                    <optionsserializable>
                                                        <cc1:SolidFillOptions />
                                                    </optionsserializable></fillstyle>
												<seriesserializable>
                                                    <cc1:Series Name="Series 1">
                                                        <viewserializable>
                                                            <cc1:SideBySideBarSeriesView>
                                                            </cc1:SideBySideBarSeriesView>
                                                        </viewserializable><labelserializable>
                                                            <cc1:SideBySideBarSeriesLabel LineVisible="True">
                                                                <fillstyle>
                                                                    <optionsserializable>
                                                                        <cc1:SolidFillOptions />
                                                                    </optionsserializable>
                                                                </fillstyle>
                                                                <pointoptionsserializable>
                                                                    <cc1:PointOptions>
                                                                    </cc1:PointOptions>
                                                                </pointoptionsserializable>
                                                            </cc1:SideBySideBarSeriesLabel>
                                                        </labelserializable><legendpointoptionsserializable>
                                                            <cc1:PointOptions>
                                                            </cc1:PointOptions>
                                                        </legendpointoptionsserializable></cc1:Series><cc1:Series Name="Series 2">
                                                        <viewserializable>
                                                            <cc1:SideBySideBarSeriesView>
                                                            </cc1:SideBySideBarSeriesView>
                                                        </viewserializable><labelserializable>
                                                            <cc1:SideBySideBarSeriesLabel LineVisible="True">
                                                                <fillstyle>
                                                                    <optionsserializable>
                                                                        <cc1:SolidFillOptions />
                                                                    </optionsserializable>
                                                                </fillstyle>
                                                                <pointoptionsserializable>
                                                                    <cc1:PointOptions>
                                                                    </cc1:PointOptions>
                                                                </pointoptionsserializable>
                                                            </cc1:SideBySideBarSeriesLabel>
                                                        </labelserializable><legendpointoptionsserializable>
                                                            <cc1:PointOptions>
                                                            </cc1:PointOptions>
                                                        </legendpointoptionsserializable></cc1:Series></seriesserializable>
												<seriestemplate>
                                                    <viewserializable>
                                                        <cc1:SideBySideBarSeriesView>
                                                        </cc1:SideBySideBarSeriesView>
                                                    </viewserializable><labelserializable>
                                                        <cc1:SideBySideBarSeriesLabel LineVisible="True">
                                                            <fillstyle>
                                                                <optionsserializable>
                                                                    <cc1:SolidFillOptions />
                                                                </optionsserializable>
                                                            </fillstyle>
                                                            <pointoptionsserializable>
                                                                <cc1:PointOptions>
                                                                </cc1:PointOptions>
                                                            </pointoptionsserializable>
                                                        </cc1:SideBySideBarSeriesLabel>
                                                    </labelserializable><legendpointoptionsserializable>
                                                        <cc1:PointOptions>
                                                        </cc1:PointOptions>
                                                    </legendpointoptionsserializable></seriestemplate>
												<crosshairoptions>
                                                    <commonlabelpositionserializable>
                                                        <cc1:CrosshairMousePosition />
                                                    </commonlabelpositionserializable></crosshairoptions>
												<tooltipoptions>
                                                    <tooltippositionserializable>
                                                        <cc1:ToolTipMousePosition />
                                                    </tooltippositionserializable></tooltipoptions>
											</dxchartsui:WebChartControl>
										</div>
									</td>
								</tr>
							</table>
						</ContentTemplate>
					</asp:UpdatePanel>
				</div>
				<div id="devicesSyncTimesDiv" style="display: none" runat="server">
					<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
						<Triggers>
							<asp:AsyncPostBackTrigger ControlID="OSComboBox" />
						</Triggers>
						<ContentTemplate>
							<table class="navbarTbl">
								<tr>
									<td>
										<table>
											<tr>
												<td>
													<dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" Text="Type:">
													</dx:ASPxLabel>
												</td>
												<td>
													<dx:ASPxComboBox ID="SyncTypeComboBox" runat="server" AutoPostBack="True" OnSelectedIndexChanged="SyncTypeComboBox_SelectedIndexChanged">
													</dx:ASPxComboBox>
												</td>
												<td>
													&nbsp;
												</td>
												<td>
													<dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" Text="Sub-type:">
													</dx:ASPxLabel>
												</td>
												<td>
													<dx:ASPxComboBox ID="SyncTypeSubComboBox" runat="server" AutoPostBack="True" OnSelectedIndexChanged="SyncTypeSubComboBox_SelectedIndexChanged">
													</dx:ASPxComboBox>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td>
										<dxchartsui:WebChartControl ID="SyncTypeWebChartControl" runat="server" ClientInstanceName="SyncTypeChart"
											Height="400px" OnCustomCallback="SyncTypeWebChartControl_CustomCallback" Width="1000px"
											CrosshairEnabled="True">
											<diagramserializable>
                                                    <cc1:SimpleDiagram>
                                                    </cc1:SimpleDiagram>
                                                </diagramserializable>
											<fillstyle>
                                                    <optionsserializable>
                                                        <cc1:SolidFillOptions />
                                                    </optionsserializable></fillstyle>
											<seriesserializable>
                                                    <cc1:Series Name="Series 1">
                                                        <viewserializable>
                                                            <cc1:PieSeriesView RuntimeExploding="False">
                                                            </cc1:PieSeriesView>
                                                        </viewserializable><labelserializable>
                                                            <cc1:PieSeriesLabel LineVisible="True">
                                                                <fillstyle>
                                                                    <optionsserializable>
                                                                        <cc1:SolidFillOptions />
                                                                    </optionsserializable>
                                                                </fillstyle>
                                                                <pointoptionsserializable>
                                                                    <cc1:PiePointOptions>
                                                                    </cc1:PiePointOptions>
                                                                </pointoptionsserializable>
                                                            </cc1:PieSeriesLabel>
                                                        </labelserializable><legendpointoptionsserializable>
                                                            <cc1:PiePointOptions>
                                                            </cc1:PiePointOptions>
                                                        </legendpointoptionsserializable></cc1:Series></seriesserializable>
											<seriestemplate>
                                                    <viewserializable>
                                                        <cc1:PieSeriesView RuntimeExploding="False">
                                                        </cc1:PieSeriesView>
                                                    </viewserializable><labelserializable>
                                                        <cc1:PieSeriesLabel LineVisible="True">
                                                            <fillstyle>
                                                                <optionsserializable>
                                                                    <cc1:SolidFillOptions />
                                                                </optionsserializable>
                                                            </fillstyle>
                                                            <pointoptionsserializable>
                                                                <cc1:PiePointOptions PercentOptions-ValueAsPercent="False">
                                                                </cc1:PiePointOptions>
                                                            </pointoptionsserializable>
                                                        </cc1:PieSeriesLabel>
                                                    </labelserializable><legendpointoptionsserializable>
                                                        <cc1:PiePointOptions PercentOptions-ValueAsPercent="False">
                                                        </cc1:PiePointOptions>
                                                    </legendpointoptionsserializable></seriestemplate>
											<crosshairoptions>
                                                    <commonlabelpositionserializable>
                                                        <cc1:CrosshairMousePosition />
                                                    </commonlabelpositionserializable></crosshairoptions>
											<tooltipoptions>
                                                    <tooltippositionserializable>
                                                        <cc1:ToolTipMousePosition />
                                                    </tooltippositionserializable></tooltipoptions>
										</dxchartsui:WebChartControl>
									</td>
								</tr>
							</table>
						</ContentTemplate>
					</asp:UpdatePanel>
				</div>
				<div id="devicesCountDiv" style="display: none" runat="server">
					<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
						<ContentTemplate>
							<table class="navbarTbl">
								<tr>
									<td>
										<dxchartsui:WebChartControl ID="DeviceCountChart" runat="server" ClientInstanceName="DeviceCountChart"
											Height="400px" Width="1000px" CrosshairEnabled="True">
											<diagramserializable>
                                                    <cc1:SimpleDiagram>
                                                    </cc1:SimpleDiagram>
                                                </diagramserializable>
											<fillstyle>
                                                    <optionsserializable>
                                                        <cc1:SolidFillOptions />
                                                    </optionsserializable></fillstyle>
											<seriesserializable>
                                                    <cc1:Series Name="Series 1">
                                                        <viewserializable>
                                                            <cc1:PieSeriesView RuntimeExploding="False">
                                                            </cc1:PieSeriesView>
                                                        </viewserializable><labelserializable>
                                                            <cc1:PieSeriesLabel LineVisible="True">
                                                                <fillstyle>
                                                                    <optionsserializable>
                                                                        <cc1:SolidFillOptions />
                                                                    </optionsserializable>
                                                                </fillstyle>
                                                                <pointoptionsserializable>
                                                                    <cc1:PiePointOptions>
                                                                    </cc1:PiePointOptions>
                                                                </pointoptionsserializable>
                                                            </cc1:PieSeriesLabel>
                                                        </labelserializable><legendpointoptionsserializable>
                                                            <cc1:PiePointOptions>
                                                            </cc1:PiePointOptions>
                                                        </legendpointoptionsserializable></cc1:Series></seriesserializable>
											<seriestemplate>
                                                    <viewserializable>
                                                        <cc1:PieSeriesView RuntimeExploding="False">
                                                        </cc1:PieSeriesView>
                                                    </viewserializable><labelserializable>
                                                        <cc1:PieSeriesLabel LineVisible="True">
                                                            <fillstyle>
                                                                <optionsserializable>
                                                                    <cc1:SolidFillOptions />
                                                                </optionsserializable>
                                                            </fillstyle>
                                                            <pointoptionsserializable>
                                                                <cc1:PiePointOptions PercentOptions-ValueAsPercent="False">
                                                                </cc1:PiePointOptions>
                                                            </pointoptionsserializable>
                                                        </cc1:PieSeriesLabel>
                                                    </labelserializable><legendpointoptionsserializable>
                                                        <cc1:PiePointOptions PercentOptions-ValueAsPercent="False">
                                                        </cc1:PiePointOptions>
                                                    </legendpointoptionsserializable></seriestemplate>
											<crosshairoptions>
                                                    <commonlabelpositionserializable>
                                                        <cc1:CrosshairMousePosition />
                                                    </commonlabelpositionserializable></crosshairoptions>
											<tooltipoptions>
                                                    <tooltippositionserializable>
                                                        <cc1:ToolTipMousePosition />
                                                    </tooltippositionserializable></tooltipoptions>
										</dxchartsui:WebChartControl>
									</td>
								</tr>
							</table>
						</ContentTemplate>
					</asp:UpdatePanel>
				</div>
			</td>
		</tr>
	</table>
	<dx:ASPxGridViewExporter ID="UsersGridViewExporter" runat="server" GridViewID="UsersGrid">
	</dx:ASPxGridViewExporter>
</asp:Content>
