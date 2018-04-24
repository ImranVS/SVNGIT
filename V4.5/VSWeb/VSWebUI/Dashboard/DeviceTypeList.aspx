<%@ Page Title="VitalSigns Plus - Status List" Language="C#" MasterPageFile="~/DashboardSite.Master"
	AutoEventWireup="true" CodeBehind="DeviceTypeList.aspx.cs" Inherits="VSWebUI.WebForm1" %>

<%@ MasterType VirtualPath="~/DashboardSite.master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="../Controls/MailStatus.ascx" TagName="MailStatus" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxGauges" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxGauges.Gauges" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxGauges.Gauges.Linear" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxGauges.Gauges.Circular" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxGauges.Gauges.State" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxGauges.Gauges.Digital" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
	<script src="../js/bootstrap.min.js" type="text/javascript"></script>
	<script type="text/javascript">
		$(document).ready(function () {
			$('.alert-success').delay(10000).fadeOut("slow", function () {
			});
			$('.alert-danger').delay(10000).fadeOut("slow", function () {
			});
		});
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

		function InitPopupMenuHandler(s, e) {
			//var menu1 = document.getElementById('ContentPlaceHolder1_ASPxPageControl1_UserDetailsMenu');
			//alert(menu1.style.visibility);
			//if (menu1.style.visibility == "visible") {
			var gridCell = document.getElementById('gridCell');

			ASPxClientUtils.AttachEventToElement(gridCell, 'contextmenu', OnGridContextMenu);
			//        var imgButton = document.getElementById('popupButton');
			//ASPxClientUtils.AttachEventToElement(gridCell, 'contextmenu', OnPreventContextMenu);
			//}

		}

		function OnGridContextMenu(evt) {

			DeviceGridView.SetFocusedRowIndex(e.index);


			DeviceGridView.SetFocusedRowIndex(e.index);


			var SortPopupMenu = StatusListPopup;
			SortPopupMenu.ShowAtPos(evt.clientX + ASPxClientUtils.GetDocumentScrollLeft(), evt.clientY + ASPxClientUtils.GetDocumentScrollTop());
			return OnPreventContextMenu(evt);

		}

		function OnPreventContextMenu(evt) {
			return ASPxClientUtils.PreventEventAndBubble(evt);
		}
		//10/14/2014 NS modified for VSPLUS-1022
		function DeviceGridView_ContextMenu(s, e) {

			if (e.objectType == "row") {
				s.GetRowValues(e.index, 'Type', OnGetRowValues);


				//            	alert(s.GetRowValues)
				s.SetFocusedRowIndex(e.index);
				//            	s.PerformCallback(e.visibleIndex.toString());
				//				alert(e.index)
				StatusListPopup.ShowAtPos(ASPxClientUtils.GetEventX(e.htmlEvent), ASPxClientUtils.GetEventY(e.htmlEvent));



				if (e.objectType == "row") {
					s.GetRowValues(e.index, 'Type', OnGetRowValues);
					s.SetFocusedRowIndex(e.index);
					StatusListPopup.ShowAtPos(ASPxClientUtils.GetEventX(e.htmlEvent), ASPxClientUtils.GetEventY(e.htmlEvent));



				}
			}
			function OnGetRowValues(Value, e) {
				if (Value != 'Domino') {
					StatusListPopup.GetItemByName("SendConsoleCommand").SetEnabled(false);
				}
				else {
					StatusListPopup.GetItemByName("SendConsoleCommand").SetEnabled(true);

				}


			}
			function DeviceGridView_FocusedRowChanged(s, e) {
				//alert("focus");
				if (e.objectType != "row") return;

				s.SetFocusedRowIndex(e.index);

			}

			s.SetFocusedRowIndex(e.index);


		}
		//Sowjanya VSPLUS-2255
		if (sessionStorage.getItem("Force refresh")) {
			sessionStorage.removeItem("Force refresh");
			window.location.reload(true); // force refresh page
		}

      
		

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
			word-wrap: break-word;
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
			padding: 24px 20px;
			margin-top: -50px;
			margin-left: -300px;
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
		.circle4
		{
		width: 50px;
	height: 50px;
	font-weight:normal;
	text-align:center;
	text-justify="auto";
	-moz-border-radius: 150px;
	-webkit-border-radius: 50px;
	
	
		}
		.normalrow
		{
			background-color: white;
		}
		
		.highlightrow
		{
			background-color: #cccccc;
		}
		.GridCss2 td {
font-weight: normal;
font-family: Arial, Helvetica, sans-serif;
font-size: 12px;
text-align:center;
vertical-align: middle;
}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<script language="javascript">
		function tblmouseout() {
			idiv = document.getElementById("div1");
			idiv.style.visibility = "hidden";
			lPendingMail = document.getElementById("TDp");
			diskstatus = document.getElementById("TDp");
			ldeadmail = document.getElementById("TDd");
			lHeldmail = document.getElementById("TDh");
			lPendingMail.style.visibility = "hidden";
			ldeadmail.style.visibility = "hidden";
			lHeldmail.style.visibility = "hidden";

		}
		function tblmouseover2(str9) {//Somaraj-VSPLUS:2284
			//alert(str9);
			idiv = document.getElementById("div1");

			idiv.style.visibility = "visible";

			var str = '';
			diskstatus = document.getElementById("TDp");
			diskstatus.style.visibility = "visible";

			diskstatus.innerHTML = str9;
		}
		function tblmouseover(stype, pav, pth, dav, dth, hav, hth, sec) {
			//alert(stype);
			idiv = document.getElementById("div1");

			idiv.style.visibility = "visible";

			var str = '';
			lPendingMail = document.getElementById("TDp");
			lPendingMail.style.visibility = "visible";

			if (stype == "Exchange") {
				str += "Submission Queues : " + pav + "/" + pth + "<br>";
			}
			else {
				str += "Pending Messages : " + pav + "/" + pth + "<br>";
			}
			//        }
			//        if (dav == 0) {
			//            //ldeadmail.style.visibility = "hidden";
			//        }
			//        else if (dav > 0) {
			//            ldeadmail.style.visibility = "visible";
			//           ldeadmail.innerHTML="Dead Mails :" + dav + "";
			if (stype == "Exchange") {
				str += "Unreachable Queues : " + dav + "/" + dth + "<br>";
			}
			else {
				str += "Dead Messages : " + dav + "/" + dth + "<br>";
			}
			//        }
			//        if (hav == 0) {
			//            //lHeldmail.style.visibility = "hidden";
			//        }
			//        else if (hav > 0) {
			//            lHeldmail.style.visibility = "visible";
			//            lHeldmail.innerHTML = "Held Mails :" + hav + "";
			if (stype == "Exchange") {
				str += "Shadow Queues : " + hav + "/" + hth;
			}
			else {
				str += "Held Messages : " + hav + "/" + hth;
			}

			//}
			lPendingMail.innerHTML = str;

			//alert(lPendingMail.innerHTML);
		}
		// Simple follow the mouse script
		document.onmousemove = follow;
		var divName = 'div1'; // div that is to follow the mouse
		// (must be position:absolute)
		var offX = -120;          // X offset from mouse position
		var offY = 23;          // Y offset from mouse position

		function mouseX(evt) { if (!evt) evt = window.event; if (evt.pageX) return evt.pageX; else if (evt.clientX) return evt.clientX + (document.documentElement.scrollLeft ? document.documentElement.scrollLeft : document.body.scrollLeft); else return 0; }
		function mouseY(evt) { if (!evt) evt = window.event; if (evt.pageY) return evt.pageY; else if (evt.clientY) return evt.clientY + (document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop); else return 0; }

		function follow(evt) {

			if (document.getElementById) {
				var obj = document.getElementById(divName).style; // obj.visibility = 'visible';
				obj.left = (parseInt(mouseX(evt)) + offX) + 'px';
				obj.top = (parseInt(mouseY(evt)) + offY) + 'px';
				//alert(obj.left + ' ' + obj.top);

			}
		}
	</script>
	<div id="div1" style="position: absolute; visibility: hidden; width: auto; height: auto;
		z-index: 999; padding: 5px; background-color: #fffAF0; font-size: 11px; font-family: Arial;">
		<div style="border-color: #ccc; border: 2px; width: 100%; visibility: hidden; color: Black"
			id="TDp">
		</div>
	</div>
	<table width="100%">
		<tr>
			<td>
				<table width="100%">
					<tr>
						<td>
							<table>
								<tr>
									<td align="left">
										<dx:ASPxButton ID="CollapseButton" runat="server" OnClick="CollapseButton_Click"
											Text="Collapse All Rows" CssClass="sysButton">
											<Image Url="~/images/icons/forbidden.png">
											</Image>
										</dx:ASPxButton>
									</td>
									<td align="center">
										<dx:ASPxButton ID="ASPxButton2" runat="server" CausesValidation="False" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
											CssPostfix="Office2010Blue" Font-Bold="False" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
											Text="Scan Preferences" Width="75px" OnClick="Rrport_Click" Visible="false">
										</dx:ASPxButton>
									</td>
									<td align="left">
										<dx:ASPxButton ID="ExpandButton" runat="server" OnClick="ExpandButton_Click" Style="padding-right: 5px"
											Text="Expand All Rows" Theme="Office2010Blue" Visible="False">
											<Image Url="~/images/icons/forbidden.png">
											</Image>
										</dx:ASPxButton>
									</td>
								</tr>
							</table>
						</td>
						<td>
							&nbsp;
						</td>
						<td>
							&nbsp;
						</td>
						<td align="right">
							&nbsp;
						</td>
					</tr>
					<tr>
						<td valign="top">
							<%-- <ClientSideEvents GotFocus="function(s, e) { s.SelectAll() }" />--%>
							<dx:ASPxMenu ID="StatusListMenu" runat="server" Theme="DevEx" Visible="False" OnItemClick="StatusListMenu_ItemClick">
								<Items>
									<dx:MenuItem Text="Scan Now" Name="ScanNow">
									</dx:MenuItem>
									<dx:MenuItem Text="Edit in Configurator" Name="EditConfigurator">
									</dx:MenuItem>
									<dx:MenuItem Text="Suspend Temporarily" Name="Suspend">
									</dx:MenuItem>
								</Items>
							</dx:ASPxMenu>
							<dx:ASPxPopupMenu ID="StatusListPopupMenu" runat="server" AutoPostBack="true" PopupAction="LeftMouseClick"
								PopupHorizontalAlign="OutsideRight" PopupVerticalAlign="TopSides" ClientInstanceName="StatusListPopup"
								OnItemClick="StatusListPopupMenu_ItemClick" Theme="Moderno">
								<ClientSideEvents Init="InitPopupMenuHandler"></ClientSideEvents>
								<Items>
									<%-- <dx:MenuItem Text="Scan Now" Name="ScanNow">
                        </dx:MenuItem>
                        <dx:MenuItem Text="Edit in Configurator" Name="EditConfigurator">
                        </dx:MenuItem>
                        <dx:MenuItem Text="Suspend Temporarily" Name="Suspend">
                        </dx:MenuItem>   --%>
								</Items>
							</dx:ASPxPopupMenu>
						</td>
						<td>
							<dx:ASPxPopupControl ID="SuspendPopupControl" runat="server" AllowDragging="True"
								AllowResize="True" CloseAction="CloseButton" EnableViewState="False" PopupHorizontalAlign="WindowCenter"
								PopupVerticalAlign="WindowCenter" ShowFooter="True" ShowOnPageLoad="False" Width="400px"
								Height="70px" FooterText="To resize the control use the resize grip or the control's edges"
								HeaderText="Suspend Monitoring" ClientInstanceName="FeedPopupControl" EnableHierarchyRecreation="True"
								Theme="MetropolisBlue">
								<ContentCollection>
									<dx:PopupControlContentControl ID="PopupControlContentControlPopup" runat="server">
										<table id="FeedBackTable" class="EditorsTable" style="width: 100%; height: 100%;">
											<tr>
												<td class="Label">
													<dx:ASPxLabel ID="lblDuration" runat="server" Text="Duration (mins):" Wrap="False">
													</dx:ASPxLabel>
												</td>
												<td>
													<dx:ASPxTextBox ID="TbDuration" runat="server" Width="100%" EnableViewState="False">
														<MaskSettings Mask="&lt;1..120&gt;" />
														<ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
															<RequiredField ErrorText="Enter Time to Suspend Monitoring." IsRequired="True" />
															<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																ValidationExpression="^\d+$" />
														</ValidationSettings>
														<%-- <ClientSideEvents GotFocus="function(s, e) { s.SelectAll() }" />--%>
													</dx:ASPxTextBox>
												</td>
											</tr>
											<tr>
												<td class="Label" colspan="2">
													<dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="You may temporarily suspend monitoring for a maximum duration of two hours. If you need to suspend monitoring for more than two hours, please use the Maintenance Windows functionality in the VitalSigns Configurator.">
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
													<dx:ASPxButton ID="test123" runat="server" OnClick="BtnApply_Click" AutoPostBack="False"
														CausesValidation="False" EnableTheming="True" Text="Apply" CssClass="sysButton">
													</dx:ASPxButton>
												</td>
											</tr>
										</table>
									</dx:PopupControlContentControl>
								</ContentCollection>
							</dx:ASPxPopupControl>
							<dx:ASPxPopupControl ID="ConsoleCmdPopupControl" runat="server" HeaderText="Send Console Command"
								Height="70px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
								Theme="MetropolisBlue" Width="400px" AllowDragging="True" AllowResize="True"
								CloseAction="CloseButton" FooterText="To resize the control use the resize grip or the control's edges"
								ShowFooter="True">
								<ContentCollection>
									<dx:PopupControlContentControl runat="server">
										<table class="navbarTbl">
											<tr>
												<td>
													<dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Enter console command:" Wrap="False">
													</dx:ASPxLabel>
												</td>
												<td>
													<dx:ASPxTextBox ID="ConsoleCmdTextBox" runat="server" Width="170px">
													</dx:ASPxTextBox>
												</td>
											</tr>
											<tr>
												<td colspan="2">
													<dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="Note: the console command will be submitted to the server only if you have proper authorization within VitalSigns. You must have Console Command Access flag enabled in your user profile to be able to perform this operation.">
													</dx:ASPxLabel>
												</td>
											</tr>
											<tr>
												<td colspan="2">
													&nbsp;
												</td>
											</tr>
											<tr>
												<td>
													<dx:ASPxButton ID="ConsoleCmdButton" runat="server" Text="Submit" OnClick="ConsoleCmdButton_Click"
														CssClass="sysButton">
													</dx:ASPxButton>
												</td>
												<td>
													&nbsp;
												</td>
											</tr>
										</table>
									</dx:PopupControlContentControl>
								</ContentCollection>
							</dx:ASPxPopupControl>
						</td>
						<td>
							<dx:ASPxPopupControl ID="msgPopupControl" runat="server" HeaderText="VitalSigns"
								Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
								Width="300px" Height="120px" Theme="Glass">
								<ContentCollection>
									<dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
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
													<dx:ASPxButton ID="YesButton" runat="server" Text="Yes" Theme="Office2010Blue" Width="70px">
													</dx:ASPxButton>
												</td>
												<td align="left">
													<dx:ASPxButton ID="NOButton" runat="server" Text="No" Theme="Office2010Blue" Width="70px">
													</dx:ASPxButton>
												</td>
												<td align="left">
													<dx:ASPxButton ID="CancelButton" runat="server" Text="Cancel" Theme="Office2010Blue"
														Visible="False" Width="70px">
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
						<td colspan="5">
							<div id="ErrorMsg" class="alert alert-danger" runat="server" style="display: none">
								The settings were not updated</div>
							<div id="SuccessMsg" class="alert alert-success" runat="server" style="display: none">
								Temporarily Suspended monitoring</div>
						</td>
					</tr>
					<tr>
						<td id="gridCell" colspan="5">
							<asp:UpdatePanel ID="updatepan1" runat="server" UpdateMode="Conditional">
								<ContentTemplate>
									<dx:ASPxGridView ID="DeviceGridView" runat="server" AutoGenerateColumns="False" Width="100%"
										KeyFieldName="TypeandName" OnPageSizeChanged="DeviceGridView_PageSizeChanged"
										OnSelectionChanged="DeviceGridView_SelectionChanged" Cursor="pointer" OnHtmlDataCellPrepared="DeviceGridView_HtmlDataCellPrepared"
										OnBeforeGetCallbackResult="BeforeGetCallbackResult" OnPreRender="PreRender" OnDataBound="DataBound"
										OnHtmlRowCreated="DeviceGridView_HtmlRowCreated" EnableTheming="True" Theme="Office2003Blue"
										OnHtmlRowPrepared="DeviceGridView_HtmlRowPrepared" CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css"
										CssPostfix="Office2010Silver" oncustomcolumndisplaytext="DeviceGridView_CustomColumnDisplayText">
										<ClientSideEvents ContextMenu="DeviceGridView_ContextMenu"></ClientSideEvents>
										<SettingsBehavior AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True"
											ColumnResizeMode="NextColumn"></SettingsBehavior>
										<SettingsBehavior AllowFocusedRow="true" />
										<Settings ShowGroupPanel="true" ShowFilterRow="true" />
										<SettingsPager PageSize="20">
											<PageSizeItemSettings Visible="True">
											</PageSizeItemSettings>
										</SettingsPager>
										<TotalSummary>
											<dx:ASPxSummaryItem FieldName="Name" SummaryType="Count" ShowInColumn="Name" DisplayFormat="{0} Item(s)" />
										</TotalSummary>
										<Columns>
											<dx:GridViewDataTextColumn Caption="Name" VisibleIndex="1" FieldName="Name" Width="300px">
												<%--  <DataItemTemplate><dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" NavigateUrl="<%# GetUrl(Container) %>"
                           Text='<%# Eval("Name") %>' Width="100%">
                           </dx:ASPxHyperLink>
						
					</DataItemTemplate>--%>
												<PropertiesTextEdit>
													<Style HorizontalAlign="Center">
														
													</Style>
												</PropertiesTextEdit>
												<Settings AllowDragDrop="True" AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
												<EditCellStyle CssClass="GridCss">
												</EditCellStyle>
												<EditFormCaptionStyle CssClass="GridCss">
												</EditFormCaptionStyle>
												<HeaderStyle CssClass="GridCssHeader">
												</HeaderStyle>
												<CellStyle CssClass="GridCss">
												</CellStyle>
												<FooterCellStyle ForeColor="#FF3300">
												</FooterCellStyle>
												<GroupFooterCellStyle Font-Names="Arial Black">
												</GroupFooterCellStyle>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn VisibleIndex="12" Width="78px" Caption="Disk Status" 
                                                Visible="true">
												<DataItemTemplate>
													<asp:Label ID="srvrname1" runat="server" Text='<%# Eval("Name")%>' align="center"
														Visible="false"></asp:Label>
													<asp:Label ID="Type" runat="server" Text='<%# Eval("Type")%>' align="center" Font-Bold="true"
														Visible="false"></asp:Label>
													<a class='tooltip1' runat="server" id="ahover">
														<table id="tbl2" runat="server" align="center">
															<tr>
																<td class="circle4" id="td2" align="center" runat="server">
																	<asp:Label ID="lblstatus2" runat="server" Text='<%#Eval("Diskstatus")%>' Visible="false"
																		CssClass="circle4"></asp:Label>
																</td>
															</tr>
														</table>
														<asp:Label ID="msgLabel2" runat="server" Text="" Visible="true" Font-Bold="true"></asp:Label>
														</div> <span class="tip">
															<img class='callout1' src='../images/callout_2.gif' />
															<img src='<%# Eval("imgsource") %>' /><font style="color: blue; font-size: 16px;"> &nbsp<b>Disk
																Status </b></font>
															<asp:Label ID="lblCPUDetails" runat="server" Visible="false" ForeColor="Blue"></asp:Label>
															<br />
															<dx:ASPxGridView ID="DiskHealthGrid" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False"
																CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" OnHtmlDataCellPrepared="DiskHealthGrid_HtmlDataCellPrepared"
																CssPostfix="Office2010Silver"  KeyFieldName="servername" Cursor="pointer"
																EnableTheming="True" Theme="Office2003Blue"  Width="108%">
																<SettingsBehavior AllowDragDrop="False" AllowFocusedRow="false" AllowSelectByRowClick="false"
																	ColumnResizeMode="NextColumn" ProcessSelectionChangedOnServer="True" AutoExpandAllGroups="True">
																</SettingsBehavior>
																<Columns>
																	<dx:GridViewDataTextColumn Caption="Name" VisibleIndex="1" Visible="true" FieldName="diskname">
																		<DataItemTemplate>
																			<a class='tooltip2'>
																				<%--<img id="Imgforinfo" runat="server" src="~/images/icons/information.png" />--%>
																				<asp:Label ID="hdlbldskname" Text='<%# Eval("diskname") %>' runat="server" onmousemove="findPos(this,event,'hfNameLabel21', 'detailsspan');" />
																				<asp:Label ID="hdlbldiskfree" Text='<%# Eval("diskfree") %>' runat="server" onmousemove="findPos(this,event,'hfNameLabel21', 'detailsspan');"
																					Visible="false" />
																			<%--	<asp:Label ID="hdlblredthrvalue" Text='<%# Eval("RedThresholdValue") %>' runat="server"
																					onmousemove="findPos(this,event,'hfNameLabel21', 'detailsspan');" Visible="false" />--%>
																				<%--<asp:Label ID="hdlblyellowthrvalue" Text='<%# Eval("YellowThresholdvalue") %>' runat="server"
																					onmousemove="findPos(this,event,'hfNameLabel21', 'detailsspan');" Visible="false" />--%>
																				<asp:Label ID="detailsspan" class="span2" runat="server">
																					<img class='callout1' src='../images/callout_2.gif' />
																					<font style="color: Blue; font-size: 16px;">&nbsp<b>Disk name:<asp:Label ID="NameLabel"
																						Text='<%# Eval("diskname") %>' runat="server"></asp:Label></b></font><br>
																					&nbsp </asp:Label>
																				<asp:Label ID="test21" runat="server" Text="<%# GetDiskInfo(Container) %>" Visible="false">
																				</asp:Label>
																			</a>
																			<%--<asp:Label ID="hfNameLabel22" Text='<%# Eval("servername") %>' runat="server"></asp:Label>--%>
																			<asp:Label ID="test" runat="server" Text="<%# GetDiskInfo(Container) %>" Visible="false">
																			</asp:Label>
																		</DataItemTemplate>
																	</dx:GridViewDataTextColumn>
																	<dx:GridViewDataTextColumn Caption="Server Name" VisibleIndex="2" FieldName="servername"
																		Visible="false">
																		<Settings AllowAutoFilter="False" AllowDragDrop="True" AutoFilterCondition="Contains"
																			AllowHeaderFilter="True" HeaderFilterMode="CheckedList" ShowFilterRowMenu="True"
																			ShowInFilterControl="True" />
																		<Settings AllowDragDrop="True" AllowAutoFilter="True" AutoFilterCondition="Contains"
																			AllowAutoFilterTextInputTimer="False"></Settings>
																		<EditCellStyle CssClass="GridCss">
																		</EditCellStyle>
																		<EditFormCaptionStyle CssClass="GridCss">
																		</EditFormCaptionStyle>
																		<HeaderStyle CssClass="GridCssHeader">
																			<Paddings Padding="5px" />
																			<Paddings Padding="5px"></Paddings>
																		</HeaderStyle>
																		<CellStyle CssClass="GridCss">
																		</CellStyle>
																	</dx:GridViewDataTextColumn>
																	<dx:GridViewDataTextColumn Caption="Disk Used/Disk Free" VisibleIndex="3">
																		<DataItemTemplate>
																			<dxchartsui:WebChartControl ID="DiskWebChart" runat="server" Visible="true" OnLoad="DiskWebChart_Load"
																				CrosshairEnabled="True" Height="25px" PaletteName="Palette 1" Width="110px" BackColor="Transparent">
																				<borderoptions visible="False" />
																				<borderoptions visible="False"></borderoptions>
																				<diagramserializable>
                                            <cc1:XYDiagram Rotated="True">
                                                <axisx reverse="True" visibleinpanesserializable="-1" visible="False">
                                                    <tickmarks minorvisible="False" visible="False" />
<Tickmarks Visible="False" MinorVisible="False"></Tickmarks>

                                                    <label visible="False">
                                                    </label>
                                                </axisx>
                                                <axisy title-text="Total Disk Size (GB)" title-visible="False" 
                                                    visibleinpanesserializable="-1" visible="False">
                                                    <tickmarks minorvisible="False" visible="False" />
<Tickmarks Visible="False" MinorVisible="False"></Tickmarks>

                                                    <label visible="False">
                                                    </label>
                                                    <gridlines visible="False">
                                                    </gridlines>
                                                </axisy>
                                                <margins bottom="0" left="0" right="0" top="0" />

<Margins Left="0" Top="0" Right="0" Bottom="0"></Margins>
                                                <defaultpane backcolor="Transparent" bordervisible="False">
                                                </defaultpane>
                                            </cc1:XYDiagram>
                                        </diagramserializable>
																				<legend visible="False"></legend>
																				<seriesserializable>
																				  <cc1:Series ArgumentScaleType="Qualitative" LabelsVisibility="True"
                                                Name="Disk Used">
                                                <viewserializable>
                                                    <cc1:StackedBarSeriesView>
                                                    </cc1:StackedBarSeriesView>
                                                </viewserializable>
                                           </cc1:Series> 
                                            <cc1:Series ArgumentScaleType="Qualitative" LabelsVisibility="True" Name="Disk Free">
                                                <viewserializable>
                                                    <cc1:StackedBarSeriesView>
                                                    </cc1:StackedBarSeriesView>
                                                </viewserializable>
                                            </cc1:Series>
                                          
                                        </seriesserializable>
																				<seriestemplate>
                                            <viewserializable>
                                                <cc1:StackedBarSeriesView>
                                                </cc1:StackedBarSeriesView>
                                            </viewserializable>
                                        </seriestemplate>
																				<palettewrappers>
                                            <dxchartsui:PaletteWrapper Name="Palette 1" ScaleMode="Repeat">
                                                <palette>
                                                    <cc1:PaletteEntry Color="Red" Color2="Red"></cc1:PaletteEntry>
                                                    <cc1:PaletteEntry Color="Green" Color2="Green"></cc1:PaletteEntry>
                                                </palette>
                                            </dxchartsui:PaletteWrapper>
                                        </palettewrappers>
																			</dxchartsui:WebChartControl>
																		</DataItemTemplate>
																	</dx:GridViewDataTextColumn>
																</Columns>
															</dx:ASPxGridView>
														</span></a>
													<dx:ASPxHyperLink ID="ASPxHyperLink2" runat="server" NavigateUrl="<%# SetDisk(Container) %>"
														Text='<%# Eval("Name") %>' Width="100%" Visible="false">
													</dx:ASPxHyperLink>
												</DataItemTemplate>
												<PropertiesTextEdit>
													<FocusedStyle HorizontalAlign="Center">
													</FocusedStyle>
												</PropertiesTextEdit>
												<Settings AllowDragDrop="True" AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
												<EditCellStyle CssClass="GridCss">
												</EditCellStyle>
												<EditFormCaptionStyle CssClass="GridCss">
												</EditFormCaptionStyle>
												<HeaderStyle CssClass="GridCssHeader" />
												<CellStyle CssClass="GridCss1">
												</CellStyle>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Status" VisibleIndex="2" FieldName="Status" Width="200px">
												<DataItemTemplate>
													<a class='tooltip2'>
														<asp:Label ID="hfNameLabel" Text='<%# Eval("Status") %>' runat="server" onmousemove="findPos(this,event,'hfNameLabel', 'detailsspan');" />
														<asp:Label ID="hfNameLabel2" Text='<%# Eval("StatusCode") %>' runat="server" Visible="false" />
														<asp:Label ID="detailsspan" class="span2" runat="server">
															<img class='callout2' src='../images/callout.gif' />
															<img src='<%# Eval("imgsource") %>' />
															<font style="color: blue; font-size: 16px;">&nbsp<b><asp:Label ID="NameLabel" Text='<%# Eval("Name") %>'
																runat="server"></asp:Label></b></font>
															<br />
															<b>
																<asp:Label ID="LabelType" Text='<%# Eval("Type") %>' runat="server"></asp:Label></b><br />
															<br />
															Previous Scan:
															<asp:Label ID="Label2" Text='<%# Eval("LastUpdate") %>' runat="server"></asp:Label>
															<br />
															Next Scan after:
															<asp:Label ID="Label3" Text='<%# Eval("NextScan") %>' runat="server"></asp:Label>
															<br />
															<br />
															<b>
																<asp:Label ID="lblResponseTime" Text='<%# Eval("ResponseTime") %>' runat="server"
																	Visible="false"></asp:Label>
																<asp:Label ID="lblUserCount" Text='<%# Eval("UserCount") %>' runat="server" Visible="false"></asp:Label>
																<asp:Label ID="lblDownMinutes" Text='<%# Eval("DownMinutes") %>' runat="server" Visible="false"></asp:Label></b>
															<asp:Label ID="lblPendingMail" Text='<%# Eval("PendingMail") %>' runat="server" Visible="false"></asp:Label>
															<asp:Label ID="lblPendingPercent" Text='<%# Eval("PendingPercent") %>' runat="server"
																Visible="false"></asp:Label><asp:Label ID="lblPendingThreshold" Text='<%# Eval("PendingThreshold") %>'
																	runat="server" Visible="false"></asp:Label>
															<asp:Label ID="lblDeadMail" Text='<%# Eval("DeadMail") %>' runat="server" Visible="false"></asp:Label><asp:Label
																ID="lblDeadPercent" Text='<%# Eval("DeadPercent") %>' runat="server" Visible="false"></asp:Label><asp:Label
																	ID="lblDeadThreshold" Text='<%# Eval("DeadThreshold") %>' runat="server" Visible="false"></asp:Label>
															<asp:Label ID="lblHeldMail" Text='<%# Eval("HeldMail") %>' runat="server" Visible="false"></asp:Label>
															
															<asp:Label ID="lblHeldPercent" Text='<%# Eval("HeldPercent") %>' runat="server" Visible="false"></asp:Label>
															<asp:Label ID="lblHeldMailThreshold" Text='<%# Eval("HeldMailThreshold") %>' runat="server"
																Visible="false"></asp:Label>
															Details:
															<asp:Label ID="Label6" Text='<%# Eval("Details") %>' runat="server"></asp:Label>
															<asp:Label ID="Label16" Text="<%# SetStatus(Container) %>" runat="server" Visible="false"></asp:Label>
															<br />
															<br />
														<b>
														Total Open Alerts:
																<asp:Label ID="lblIssueCount" Text='<%# Eval("IssueCount") %>' runat="server" ></asp:Label></b>&nbsp;(Click the server name to see details.)<br />
														
                                                        <br />
														
														<asp:Label ID="lblMonitoredURL" Text= '<%# Eval("MonitoredURL") %>' Visible ="false" runat="server" ></asp:Label><br />
														</asp:Label>
															
													</a>
												</DataItemTemplate>
												<PropertiesTextEdit>
													<FocusedStyle HorizontalAlign="Center">
													</FocusedStyle>
												</PropertiesTextEdit>
												<Settings AllowDragDrop="True" AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
												<EditCellStyle CssClass="GridCss">
												</EditCellStyle>
												<EditFormCaptionStyle CssClass="GridCss">
												</EditFormCaptionStyle>
												<HeaderStyle CssClass="GridCssHeader" />
												<CellStyle CssClass="GridCss1">
												</CellStyle>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Details"  VisibleIndex="3"  FieldName="Details">
                                           <DataItemTemplate>
                                               	<a class='tooltip2'>
														<asp:Label ID="detailsLabel" Text='<%# Eval("details") %>' runat="server" onmousemove="findPos(this,event,'detailsLabel', 'detailsspan');" />
														<asp:Label ID="detailsspan" class="span2" runat="server">
															<img class='callout2' src='../images/callout.gif' />
															<img src='<%# Eval("imgsource") %>' />
															<font style="color: blue; font-size: 16px;">&nbsp<b>Details</b></font><br>
															Details Info:
															<asp:Label ID="detailsinfoLabel" Text='<%# Eval("details") %>' runat="server" />
														</asp:Label></a></DataItemTemplate><Settings AllowAutoFilter="False" AllowDragDrop="True" AutoFilterCondition="Contains"
													AllowAutoFilterTextInputTimer="False" />
												<EditCellStyle CssClass="GridCss" >
												</EditCellStyle>
												<EditFormCaptionStyle CssClass="GridCss">
												</EditFormCaptionStyle>
												<HeaderStyle CssClass="GridCssHeader" />
												<CellStyle CssClass="GridCss" Wrap="True" >
												</CellStyle>
											</dx:GridViewDataTextColumn>
											<%--<dx:GridViewDataTextColumn Caption="Description" VisibleIndex="4" 
                            FieldName="Description">
                              <DataItemTemplate>                
                                                    <a class='tooltip2'>
                                               <asp:Label ID="desLabel" Text='<%# Eval("Description") %>' runat="server" onmousemove="findPos(this,event,'desLabel', 'desspan');" />
                                                <asp:Label ID="desspan" class="span2" runat="server">
                                                    <img class='callout2' src='../images/callout.gif' />
                                                    <img src=<%# Eval("imgsource") %> />
                                                    <font style="color: blue; font-size: 16px;">&nbsp<b>Description</b></font><br>
                                                    Details Info:
                                                    <asp:Label ID="desinfoLabel" Text='<%# Eval("Description") %>' runat="server" />
                                                </asp:Label></a></DataItemTemplate><Settings AllowAutoFilter="False" AllowDragDrop="True" />
                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle 
                                  CssClass="GridCssHeader" /><CellStyle CssClass="GridCss" Wrap="False"></CellStyle>
                        </dx:GridViewDataTextColumn>--%>
											<dx:GridViewDataTextColumn Caption="Type" FieldName="Type" Visible="True" VisibleIndex="4">
												<Settings AllowAutoFilter="True" AllowDragDrop="True" AutoFilterCondition="Contains"
													AllowAutoFilterTextInputTimer="False" />
												<EditCellStyle CssClass="GridCss">
												</EditCellStyle>
												<EditFormCaptionStyle CssClass="GridCss">
												</EditFormCaptionStyle>
												<HeaderStyle CssClass="GridCssHeader" />
												<CellStyle CssClass="GridCss">
												</CellStyle>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Location" FieldName="Location" Visible="True"
												VisibleIndex="5" Width="100px">
												<Settings AllowDragDrop="True" AutoFilterCondition="Contains" />
												<EditCellStyle CssClass="GridCss">
												</EditCellStyle>
												<EditFormCaptionStyle CssClass="GridCss">
												</EditFormCaptionStyle>
												<HeaderStyle CssClass="GridCssHeader" />
												<CellStyle CssClass="GridCss">
												</CellStyle>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Mail" VisibleIndex="6" Width="50px">
												<EditCellStyle CssClass="GridCss">
												</EditCellStyle>
												<EditFormCaptionStyle CssClass="GridCss">
												</EditFormCaptionStyle>
												<HeaderStyle CssClass="GridCssHeader" />
												<CellStyle CssClass="GridCss1">
												</CellStyle>
												<DataItemTemplate>
													<table id="tbl" runat="server">
														<tr>
															<td class="circle1">
															</td>
															<td class="circle2">
															</td>
															<td class="circle3">
															</td>
														</tr>
													</table>
													<asp:Label ID="lblPendingActual" runat="server" Text='<%#Eval("PendingMail") %>'
														Visible="false"></asp:Label><asp:Label ID="lblPendingthresholdval" runat="server"
															Text='<%#Eval("PendingThreshold")%>' Visible="false"></asp:Label><asp:Label ID="lblDeadactual"
																runat="server" Text='<%#Eval("DeadMail") %>' Visible="false"></asp:Label><asp:Label
																	ID="lblDeadthresholdval" runat="server" Text='<%#Eval("DeadThreshold")%>' Visible="false"></asp:Label><asp:Label
																		ID="lblHeldactual" runat="server" Text='<%#Eval("HeldMail") %>' Visible="false"></asp:Label><asp:Label
																			ID="lblHeldthresholdval" runat="server" Text='<%#Eval("HeldMailThreshold")%>'
																			Visible="false"></asp:Label><asp:Label ID="lblservertype" runat="server" Text='<%# Eval("Type")%>'
																				Visible="false"></asp:Label><asp:Label ID="lblSecondaryRole" runat="server" Text='<%# Eval("SecondaryRole")%>'
																					Visible="false"></asp:Label><dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server"
																						NavigateUrl="<%# SetCircleStatus(Container) %>" Text='<%# Eval("Name") %>' Width="100%"
																						Visible="false">
																					</dx:ASPxHyperLink>
													<%-- <asp:HyperLink ID="hyper" runat="server" Visible="false" NavigateUrl='<%# SetCircleStatus(Container) %>'></asp:HyperLink>
                                <uc1:MailStatus ID="MailStatus1" runat="server" ActualValue='<%# Eval("PendingMail") %>' ThresholdValue='<%# Eval("PendingThreshold") %>'/>--%>
												</DataItemTemplate>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="CPU" VisibleIndex="8" Width="60px">
												<DataItemTemplate>
													<asp:Label ID="lblCPUTH" runat="server" Text='<%#Eval("CPUThreshold")%>' Visible="false"
														align="center" Font-Bold="true"></asp:Label><asp:Label ID="lblCPU" runat="server"
															Text='<%#Eval("CPU")%>' Visible="false" align="center" Font-Bold="true"></asp:Label><%--<asp:Label ID="msgLabel" runat="server" Text="" Visible="true" align="center" Font-Bold="true"></asp:Label>--%><asp:Label
																ID="Type" runat="server" Text='<%# Eval("Type")%>' align="center" Font-Bold="true"
																Visible="false"></asp:Label><asp:Label ID="Sec" runat="server" Text='<%# Eval("Type")%>'
																	align="center" Font-Bold="true" Visible="false"></asp:Label><asp:Label ID="poplbl"
																		runat="server" Text="" align="center" Font-Bold="true" Visible="false"></asp:Label><a
																			class='tooltip1' runat="server" id="ahover"><asp:Label ID="msgLabel" runat="server"
																				Text="" Visible="true" Font-Bold="true"></asp:Label><dx:ASPxGaugeControl runat="server"
																					Width="145px" Height="30px" BackColor="Transparent" ID="gControl_Page3" ClientInstanceName="Gauge3"
																					SaveStateOnCallbacks="False" AutoLayout="False" Visible="false">
																					<LayoutPadding All="0" Left="0" Top="0" Right="0" Bottom="0"></LayoutPadding>
																				</dx:ASPxGaugeControl>
																			</div> <span class="tip">
																				<img class='callout1' src='../images/callout_2.gif' />
																				<img src='<%# Eval("imgsource") %>' /><font style="color: blue; font-size: 16px;"> &nbsp<b>CPU
																					Utilization</b></font><br>
																				The Percentage of utilization of the CPU(s). This value is based on the 'Platform.System.PctCombinedCpuUtil'
																				statistic reported by the server. <asp:Label ID="lblCPUDetails" runat="server" Visible="true"></asp:Label></span></a><dx:ASPxHyperLink
																					ID="ASPxHyperLink2" runat="server" NavigateUrl="<%# SetCPU(Container) %>" Text='<%# Eval("Name") %>'
																					Width="100%" Visible="false">
																				</dx:ASPxHyperLink>
												</DataItemTemplate>
												<EditCellStyle CssClass="GridCss">
												</EditCellStyle>
												<EditFormCaptionStyle CssClass="GridCss">
												</EditFormCaptionStyle>
												<HeaderStyle CssClass="GridCssHeader" />
												<CellStyle CssClass="GridCss2" HorizontalAlign="Right">
												</CellStyle>
											</dx:GridViewDataTextColumn>
											<%--<dx:GridViewDataTextColumn Caption="Disk Status" VisibleIndex="10" Width="78px" Visible="false">
												<DataItemTemplate>
													<table id="tbl2" runat="server" align="center">
														<tr>
															<td class="circle4" id="td2" align="center" runat="server">
																<asp:Label ID="lblCPUTH" runat="server" Text='<%#Eval("Diskstatus")%>' Visible="false"
																	CssClass="circle4"></asp:Label></td></tr></table><asp:Label ID="lblstatus" runat="server" Text='<%#Eval("Diskstatus")%>' Visible="false"
														align="center" Font-Bold="true" CssClass="circle4"></asp:Label><asp:Label ID="Type"
															runat="server" Text='<%# Eval("Type")%>' align="center" Visible="false"></asp:Label><asp:Label
																ID="srvrname" runat="server" Text='<%# Eval("Name")%>' align="center" Visible="false"></asp:Label><a
																	class='tooltip1' runat="server" id="ahover"><dx:ASPxGaugeControl runat="server" Width="145px"
																		Height="30px" BackColor="Transparent" ID="gControl_Page3" ClientInstanceName="Gauge3"
																		SaveStateOnCallbacks="False" AutoLayout="False" Visible="false">
																		<LayoutPadding All="0" Left="0" Top="0" Right="0" Bottom="0"></LayoutPadding>
																	</dx:ASPxGaugeControl>
																	</div> <span class="tip">
																		<img class='callout1' src='../images/callout_2.gif' />
																		<img src='<%# Eval("imgsource") %>' /><font style="font-size: 16px;"> &nbsp<b>Disk Status
																			Details</b></font><br>
																		<asp:Label ID="lblCPUDetails" runat="server" Visible="true"></asp:Label></span></a><dx:ASPxHyperLink
																			ID="ASPxHyperLink2" runat="server" NavigateUrl="<%# SetDisk2(Container) %>" Text='<%# Eval("Name") %>'
																			Width="100%" Visible="false">
																		</dx:ASPxHyperLink>
												</DataItemTemplate>
												<EditCellStyle CssClass="GridCss">
												</EditCellStyle>
												<EditFormCaptionStyle CssClass="GridCss">
												</EditFormCaptionStyle>
												<HeaderStyle CssClass="GridCssHeader" />
												<CellStyle CssClass="GridCss2" HorizontalAlign="Center">
												</CellStyle>
											</dx:GridViewDataTextColumn>--%>
											<dx:GridViewDataTextColumn VisibleIndex="0" Width="35px">
												<DataItemTemplate>
													<div class="msg">
														<table class="textalignmiddle">
															<tr>
																<td>
																	<a class='tooltip2'><span id="detailsLabel" runat="server" onmousemove="findPos(this,event,'detailsLabel', 'detailsspan');">
																		<img src='<%# Eval("imgsource") %>' /></span>
																		<asp:Label ID="detailsspan" class="span2" runat="server">
																			<img class='callout2' src='../images/callout.gif' />
																			<img src='<%# Eval("imgsource") %>' />
																			<font style="color: blue; font-size: 16px;">&nbsp<b>System Information</b></font><br>
																			<asp:Label ID="Label1" Text='<%# Eval("DominoVersion") %>' runat="server" /><br>
																			<asp:Label ID="detailsinfoLabel" Text='<%# Eval("OperatingSystem") %>' runat="server" />
																		</asp:Label></a></td></tr></table></div></DataItemTemplate><HeaderStyle CssClass="GridCss" />
												<CellStyle CssClass="GridCss1">
													<BorderTop BorderStyle="Groove" />
												</CellStyle>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Memory" Width="60px" VisibleIndex="7">
												<DataItemTemplate>
													<a class='tooltip1' runat="server" id="ahover">
														<asp:Label ID="Type" runat="server" Text='<%# Eval("Type")%>' align="center" Font-Bold="true"
															Visible="false"></asp:Label><asp:Label ID="Sec" runat="server" Text='<%# Eval("Type")%>'
																align="center" Font-Bold="true" Visible="false"></asp:Label><asp:Label ID="poplbl"
																	runat="server" Text="" align="center" Font-Bold="true" Visible="false"></asp:Label><asp:Label
																		ID="lblMemTH" runat="server" Text='<%#Eval("MemoryThreshold")%>' Visible="false"
																		align="center" Font-Bold="true"></asp:Label><asp:Label ID="lblMem" runat="server"
																			Text='<%#Eval("MemoryPercent")%>' Visible="false" align="center" Font-Bold="true"></asp:Label><asp:Label
																				ID="msgLabel" runat="server" Text="" Visible="true" Font-Bold="true"></asp:Label><span
																					class="tip"><img class='callout1' src='../images/callout_2.gif' /><img src='<%# Eval("imgsource") %>' /><font
																						style="color: blue; font-size: 16px;">&nbsp<b>Memory Utilization</b></font><br>
																					The Percentage of utilization of the Memory. This value is based on the 'Mem.PercentUsed'
																					statistic reported by the server. <asp:Label ID="lblMemDetails" runat="server" Visible="true"></asp:Label></span></a><dx:ASPxHyperLink
																						ID="ASPxHyperLink2" runat="server" NavigateUrl="<%# SetMemory(Container) %>"
																						Text='<%# Eval("Name") %>' Width="100%" Visible="false">
																					</dx:ASPxHyperLink>
												</DataItemTemplate>
												<EditCellStyle CssClass="GridCss">
												</EditCellStyle>
												<EditFormCaptionStyle CssClass="GridCss">
												</EditFormCaptionStyle>
												<HeaderStyle CssClass="GridCssHeader" />
												<CellStyle CssClass="GridCss2" HorizontalAlign="Right">
												</CellStyle>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="" VisibleIndex="9" FieldName="StatusCode" Width="0px"
												Visible="false">
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Secondary Role" 
                                                FieldName="SecondaryRole" Visible="False"
												VisibleIndex="11"></dx:GridViewDataTextColumn>
										</Columns>
										<SettingsBehavior AllowDragDrop="False" AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True">
										</SettingsBehavior>
										<SettingsPager PageSize="20" SEOFriendly="Enabled">
											<PageSizeItemSettings Visible="true" />
										</SettingsPager>
										<Settings ShowGroupPanel="True" ShowFilterRow="True" />
										<Styles CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver">
											<Header ImageSpacing="5px" SortingImageSpacing="5px">
											</Header>
											<GroupRow Font-Bold="True" Font-Italic="False">
											</GroupRow>
											<AlternatingRow Enabled="True">
											</AlternatingRow>
											<Cell CssClass="GridCss">
											</Cell>
											<GroupFooter Font-Bold="True" Font-Italic="False" ForeColor="Black">
											</GroupFooter>
											<GroupPanel Font-Bold="False">
											</GroupPanel>
											<LoadingPanel ImageSpacing="5px">
											</LoadingPanel>
										</Styles>
										<StylesEditors ButtonEditCellSpacing="0">
											<ProgressBar Height="21px">
											</ProgressBar>
										</StylesEditors>
										<Templates>
											<GroupRowContent>
												<%# Container.GroupText%>
											</GroupRowContent>
										</Templates>
									</dx:ASPxGridView>
								</ContentTemplate>
								<Triggers>
									<asp:AsyncPostBackTrigger ControlID="timer1" />
								</Triggers>
							</asp:UpdatePanel>
						</td>
					</tr>
				</table>
				<%--  <DataItemTemplate><dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" NavigateUrl="<%# GetUrl(Container) %>"
                           Text='<%# Eval("Name") %>' Width="100%">
                           </dx:ASPxHyperLink>
						
					</DataItemTemplate>--%>
			</td>
		</tr>
	</table>
</asp:Content>
