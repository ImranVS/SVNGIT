<%@ Page Title="VitalSigns Plus - WebSphere Server Health" Language="C#" MasterPageFile="~/DashboardSite.Master"
	AutoEventWireup="true" CodeBehind="WebsphereServerHealth.aspx.cs" Inherits="VSWebUI.Dashboard.WebsphereServerHealth" %>
	<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>




<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraCharts" TagPrefix="cc2" %>




<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet'
		type='text/css' />
	<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
	<style type="text/css">
		.headerCell
		{
			color: Black;
			background-color: #045FB4;
		}
		.rowCell
		{
			color: Black;
			background-color: White;
			text-align: right;
		}
	</style>
	<script type="text/javascript" language="javascript">
		function EXGHealthGridView_ContextMenu(s, e) {
			if (e.objectType == "row") {
				s.SetFocusedRowIndex(e.index);
				StatusListPopup.ShowAtPos(ASPxClientUtils.GetEventX(e.htmlEvent), ASPxClientUtils.GetEventY(e.htmlEvent));
			}
		}
		function EXGHealthGridView_FocusedRowChanged(s, e) {
			if (e.objectType != "row") return;
			alert(e.index);
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

		function InitPopupMenuHandler(s, e) {
			//var menu1 = document.getElementById('ContentPlaceHolder1_ASPxPageControl1_UserDetailsMenu');
			//alert(menu1.style.visibility);
			//if (menu1.style.visibility == "visible") {
			var gridCell = document.getElementById('gridCell');
			ASPxClientUtils.AttachEventToElement(gridCell, 'contextmenu', OnGridContextMenu);
			//        var imgButton = document.getElementById('popupButton');
			//        ASPxClientUtils.AttachEventToElement(imgButton, 'contextmenu', OnPreventContextMenu);
			//}
		}
		function OnGridContextMenu(evt) {
			DeviceGridView.SetFocusedRowIndex(e.index);
			var SortPopupMenu = StatusListPopup;
			SortPopupMenu.ShowAtPos(evt.clientX + ASPxClientUtils.GetDocumentScrollLeft(), evt.clientY + ASPxClientUtils.GetDocumentScrollTop());
			return OnPreventContextMenu(evt);
		}
		function OnPreventContextMenu(evt) {
			return ASPxClientUtils.PreventEventAndBubble(evt);
		}
		function DeviceGridView_ContextMenu(s, e) {
			if (e.objectType == "row") {
				s.SetFocusedRowIndex(e.index);
				StatusListPopup.ShowAtPos(ASPxClientUtils.GetEventX(e.htmlEvent), ASPxClientUtils.GetEventY(e.htmlEvent));
			}
		}
		function DeviceGridView_FocusedRowChanged(s, e) {
			if (e.objectType != "row") return;
			s.SetFocusedRowIndex(e.index);
		}


		function tblmouseout() {
			idiv = document.getElementById("div1");
			idiv.style.visibility = "hidden";
			lPendingMail = document.getElementById("TDp");
			ldeadmail = document.getElementById("TDd");
			lHeldmail = document.getElementById("TDh");
			lPendingMail.style.visibility = "hidden";
			ldeadmail.style.visibility = "hidden";
			lHeldmail.style.visibility = "hidden";

		}
		function tblmouseover(pav, pth, dav, dth, hav, hth) {

			idiv = document.getElementById("div1");

			idiv.style.visibility = "visible";

			//        lPendingMail=document.getElementById("lblPendingMail");
			//        if (pav == 0) {
			//        lPendingMail.style.visibility = "hidden";
			//    } 
			//        else
			//        {
			//         lPendingMail.style.visibility = "visible";
			//         }
			var str = '';
			lPendingMail = document.getElementById("TDp");
			lPendingMail.style.visibility = "visible";
			//        ldeadmail = document.getElementById("TDd");
			//        lHeldmail = document.getElementById("TDh");
			//        if (pav == 0) {
			//            //lPendingMail.style.visibility = "hidden";
			//        }
			//        else if (pav > 0) {
			// lPendingMail.style.visibility = "visible";
			//lPendingMail.innerHTML = "Pending Mails : " + pav + "";
			str += "Pending Messages : " + pav + "/" + pth + "<br>";
			//        }
			//        if (dav == 0) {
			//            //ldeadmail.style.visibility = "hidden";
			//        }
			//        else if (dav > 0) {
			//            ldeadmail.style.visibility = "visible";
			//           ldeadmail.innerHTML="Dead Mails :" + dav + "";
			str += "Dead Messages : " + dav + "/" + dth + "<br>";

			//        }
			//        if (hav == 0) {
			//            //lHeldmail.style.visibility = "hidden";
			//        }
			//        else if (hav > 0) {
			//            lHeldmail.style.visibility = "visible";
			//            lHeldmail.innerHTML = "Held Mails :" + hav + "";
			str += "Held Messages : " + hav + "/" + hth;

			//}
			lPendingMail.innerHTML = str;

			//alert(lPendingMail.innerHTML);
		}
		// Simple follow the mouse script
		//document.onmousemove = follow;
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table width="100%">
		<tr>
			<td>
			<%--	<img alt="" src="../images/icons/small-DAG-icon.png" />--%>
			
				<div class="header" id="Div3" runat="server">
					WebSphere Server Health</div>
			</td>
		</tr>
		<tr>
			<td valign="top">
				<dxchartsui:WebChartControl ID="StatusWebChart" runat="server" 
                    CrosshairEnabled="True" Height="200px" PaletteName="Module" Width="400px" 
                    oncustomdrawseriespoint="StatusWebChart_CustomDrawSeriesPoint">
                    <diagramserializable>
                        <cc2:SimpleDiagram3D RotationMatrixSerializable="1;0;0;0;0;0.5;-0.866025403784439;0;0;0.866025403784439;0.5;0;0;0;0;1">
                        </cc2:SimpleDiagram3D>
                    </diagramserializable>
                    <seriesserializable>
                        <cc2:Series ArgumentScaleType="Qualitative" Name="Series 1" 
                            SynchronizePointOptions="False">
                            <viewserializable>
                                <cc2:Pie3DSeriesView SizeAsPercentage="100">
                                </cc2:Pie3DSeriesView>
                            </viewserializable>
                            <labelserializable>
                                <cc2:Pie3DSeriesLabel Position="Inside">
                                    <pointoptionsserializable>
                                        <cc2:PiePointOptions PercentOptions-ValueAsPercent="False">
                                            <valuenumericoptions format="Number" precision="0" />
                                        </cc2:PiePointOptions>
                                    </pointoptionsserializable>
                                </cc2:Pie3DSeriesLabel>
                            </labelserializable>
                            <legendpointoptionsserializable>
                                <cc2:PiePointOptions Pattern="{A} - {V}" PercentOptions-ValueAsPercent="False" 
                                    PointView="ArgumentAndValues">
                                    <valuenumericoptions format="Number" precision="0" />
<ValueNumericOptions Format="Number" Precision="0"></ValueNumericOptions>
                                </cc2:PiePointOptions>
                            </legendpointoptionsserializable>
                        </cc2:Series>
                    </seriesserializable>
                    <seriestemplate>
                        <viewserializable>
                            <cc2:Pie3DSeriesView SizeAsPercentage="100">
                            </cc2:Pie3DSeriesView>
                        </viewserializable>
                        <labelserializable>
                            <cc2:Pie3DSeriesLabel>
                                <pointoptionsserializable>
                                    <cc2:PiePointOptions>
                                        <valuenumericoptions format="General" />
                                    </cc2:PiePointOptions>
                                </pointoptionsserializable>
                            </cc2:Pie3DSeriesLabel>
                        </labelserializable>
                        <legendpointoptionsserializable>
                            <cc2:PiePointOptions>
                                <valuenumericoptions format="General" />
<ValueNumericOptions Format="General"></ValueNumericOptions>
                            </cc2:PiePointOptions>
                        </legendpointoptionsserializable>
                    </seriestemplate>
                    <titles>
                        <cc2:ChartTitle Text="By Status" />
                    </titles>
                </dxchartsui:WebChartControl>
			</td>
		</tr>
		<tr>
			<td>
				<dx:ASPxGridView ID="WebsphereCellGridview" runat="server" AutoGenerateColumns="False"
					EnableTheming="True" KeyFieldName="CellID" Theme="Office2003Blue" EnableCallBacks="False"
					OnHtmlRowCreated="WebsphereCellGridview_HtmlRowCreated" 
                    OnHtmlDataCellPrepared="WebsphereCellGridview_HtmlDataCellPrepared" OnPageSizeChanged="WebsphereCellGridview_OnPageSizeChanged"
                    Width="100%">
					<Columns>
						<dx:GridViewDataTextColumn Caption="Cell Name" FieldName="CellName" 
                            VisibleIndex="0">
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Status" FieldName="Status" VisibleIndex="1">
							<HeaderStyle CssClass="GridCssHeader1" />
							<CellStyle CssClass="GridCss1">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="LastScan" Visible="false" FieldName="LastScan"
							VisibleIndex="2">
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="CellID" FieldName="CellID" VisibleIndex="3" Visible="false">
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Total JVM's" FieldName="TotalJVM" VisibleIndex="4">
						    <HeaderStyle CssClass="GridCssHeader2" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Monitored JVM's" FieldName="MonitoredJVMs" VisibleIndex="5">
						    <HeaderStyle CssClass="GridCssHeader2" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
						</dx:GridViewDataTextColumn>
					</Columns>
					<SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True" />
					<SettingsPager AlwaysShowPager="True" SEOFriendly="Enabled">
						<PageSizeItemSettings Visible="True">
						</PageSizeItemSettings>
					</SettingsPager>
					<Styles>
						<Header CssClass="GridCssHeader">
						</Header>
						<AlternatingRow CssClass="GridAltRow">
						</AlternatingRow>
						<Cell CssClass="GridCss">
						</Cell>
					</Styles>
				</dx:ASPxGridView>
			</td>
		</tr>
		<tr>
			<td>
				<dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
                    Theme="Glass" Width="100%" EnableHierarchyRecreation="False">
                    <TabPages>
                        <dx:TabPage Text="Nodes">
                            <ContentCollection>
                                <dx:ContentControl runat="server">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <dx:ASPxGridView ID="WebsphereNodesgridview" runat="server" 
                                        AutoGenerateColumns="False" EnableCallBacks="False" EnableTheming="True" 
                                        KeyFieldName="NodeID" 
                                        OnHtmlDataCellPrepared="WebsphereNodesgridview_HtmlDataCellPrepared" 
                                        OnHtmlRowCreated="WebsphereNodesgridview_HtmlRowCreated" OnPageSizeChanged="WebsphereNodesgridview_OnPageSizeChanged"
                                        Theme="Office2003Blue" Width="100%">
                                        <Columns>
                                            <dx:GridViewDataTextColumn Caption="Node Name" FieldName="NodeName" 
                                                ShowInCustomizationForm="True" VisibleIndex="0">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="NodeID" FieldName="NodeID" 
                                                ShowInCustomizationForm="True" Visible="False" VisibleIndex="1">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="JVM's" FieldName="JVMs" 
                                                ShowInCustomizationForm="True" VisibleIndex="3">
                                                <HeaderStyle CssClass="GridCssHeader2" />
                                                <CellStyle CssClass="GridCss2">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Status" FieldName="Status" 
                                                ShowInCustomizationForm="True" VisibleIndex="2">
                                                <HeaderStyle CssClass="GridCssHeader1" />
                                                <CellStyle CssClass="GridCss1">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                        <SettingsPager AlwaysShowPager="True" SEOFriendly="Enabled">
                                            <PageSizeItemSettings Visible="True">
                                            </PageSizeItemSettings>
                                        </SettingsPager>
                                        <Styles>
                                            <Header CssClass="GridCssHeader">
                                            </Header>
                                            <AlternatingRow CssClass="GridAltRow">
                                            </AlternatingRow>
                                            <Cell CssClass="GridCss">
                                            </Cell>
                                        </Styles>
                                    </dx:ASPxGridView>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="WebsphereCellGridview" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Servers">
                            <ContentCollection>
                                <dx:ContentControl runat="server">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <dx:ASPxGridView ID="Websphereservergridview" runat="server" 
                                        AutoGenerateColumns="False" EnableCallBacks="False" EnableTheming="True" 
                                        KeyFieldName="ID" 
                                        OnHtmlDataCellPrepared="Websphereservergridview_HtmlDataCellPrepared" 
                                        OnSelectionChanged="Websphereservergridview_SelectionChanged" OnPageSizeChanged="Websphereservergridview_OnPageSizeChanged"
                                        Theme="Office2003Blue" Width="100%">
                                        <Columns>
                                            <dx:GridViewDataTextColumn Caption="JVM Name" FieldName="ServerName" 
                                                VisibleIndex="2">
                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="ServerID" FieldName="ServerID" 
                                                Visible="False" VisibleIndex="3">
                                            </dx:GridViewDataTextColumn>
                                           <dx:GridViewDataTextColumn Caption="Avg Thread Pool" 
                                                FieldName="AveragePoolSize" VisibleIndex="5">
                                                <Settings AllowAutoFilter="False" />
                                                <HeaderStyle CssClass="GridCss2" />
                                                <CellStyle CssClass="GridCss2">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Active Thread Count" 
                                                FieldName="ActiveThreadCount" 
                                                VisibleIndex="6">
                                                <Settings AllowAutoFilter="False" />
                                                <HeaderStyle CssClass="GridCssHeader2" />
                                                <CellStyle CssClass="GridCss2">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Current Heap" FieldName="CurrentHeapSize" 
                                                VisibleIndex="7">
                                                <Settings AllowAutoFilter="False" />
                                                <HeaderStyle CssClass="GridCssHeader2" />
                                                <CellStyle CssClass="GridCss2">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <%--<dx:GridViewDataTextColumn Caption="Max Heap" FieldName="MaxHeap" 
                                                VisibleIndex="8">
                                                <Settings AllowAutoFilter="False" />
                                                <HeaderStyle CssClass="GridCssHeader2" />
                                                <CellStyle CssClass="GridCss2">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>--%>
                                            <dx:GridViewDataTextColumn Caption="Free Memory (%)" FieldName="Memory" 
                                                VisibleIndex="9">
                                                <Settings AllowAutoFilter="False" />
                                                <HeaderStyle CssClass="GridCssHeader2" />
                                                <CellStyle CssClass="GridCss2">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Process CPU Utilization" FieldName="ProcessCPUUsage" 
                                                VisibleIndex="10">
                                                <Settings AllowAutoFilter="False" />
                                                <HeaderStyle CssClass="GridCssHeader2" />
                                                <CellStyle CssClass="GridCss2">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Up time" FieldName="UpTime" 
                                                VisibleIndex="11">
                                                <Settings AllowAutoFilter="False" />
                                                <HeaderStyle CssClass="GridCssHeader2" />
                                                <CellStyle CssClass="GridCss2">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Hung Thread Count" 
                                                FieldName="CurrentHungThreadCount" 
                                                VisibleIndex="12">
                                                <Settings AllowAutoFilter="False" />
                                                <HeaderStyle CssClass="GridCssHeader2" />
                                                <CellStyle CssClass="GridCss2">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
<%--                                            <dx:GridViewDataTextColumn Caption="Dump Generated" FieldName="DumpGenerated" 
                                                VisibleIndex="13">
                                                <Settings AllowAutoFilter="False" />
                                                <HeaderStyle CssClass="GridCss2" />
                                                <CellStyle CssClass="GridCss2">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>--%>
                                            <dx:GridViewDataTextColumn Caption="Status" FieldName="Status" VisibleIndex="4" 
                                                Width="150px">
                                               <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                <HeaderStyle CssClass="GridCssHeader1" />
                                                <CellStyle CssClass="GridCss1">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" Visible="False" 
                                                VisibleIndex="14">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Redirect To" FieldName="redirectto" 
                                                Visible="False" VisibleIndex="15">
                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" />
                                                <CellStyle CssClass="GridCss1">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Node Name" FieldName="NodeName" 
                                                VisibleIndex="1">
                                               <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" 
                                            ProcessSelectionChangedOnServer="True" />
                                        <SettingsPager AlwaysShowPager="True" SEOFriendly="Enabled">
                                            <PageSizeItemSettings Visible="True">
                                            </PageSizeItemSettings>
                                        </SettingsPager>
                                                <Settings ShowFilterRow="True" />
                                        <Styles>
                                            <Header CssClass="GridCssHeader">
                                            </Header>
                                            <AlternatingRow CssClass="GridAltRow">
                                            </AlternatingRow>
                                            <Cell CssClass="GridCss">
                                            </Cell>
                                        </Styles>
                                    </dx:ASPxGridView>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="WebsphereCellGridview" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                    </TabPages>
                </dx:ASPxPageControl>
			</td>
		</tr>
		</table>
</asp:Content>
