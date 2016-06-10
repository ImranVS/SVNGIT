    <%@ Page Title="VitalSigns Plus - Office 365 Skype for Business Health" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="Office365LyncHealth.aspx.cs" Inherits="VSWebUI.Dashboard.Office365LyncHealth" %>
	<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>









<%@ Register assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>
<%@ Register assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts.Web" tagprefix="dxchartsui" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
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
		if (me.innerHTML == "Show Bar Graph") {
			$get("OSBar").style.display = "block";
			$get("OSBar").style.visibility = "visible";
			$get("OSPie").style.display = "none";
			$get("OSPie").style.visibility = "hidden";
			me.innerHTML = "Show Pie Graph"
		}
		else {
			$get("OSPie").style.display = "block";
			$get("OSPie").style.visibility = "visible";
			$get("OSBar").style.display = "none";
			$get("OSBar").style.visibility = "hidden";
			me.innerHTML = "Show Bar Graph"
		}
	}
	function Resized() {
		var callbackState = document.getElementById('ContentPlaceHolder1_callbackState').value;

		if (callbackState == 0)
			DoCallback();
	}

	function DoCallback() {
		//10/8/2013 NS modified
		document.getElementById('ContentPlaceHolder1_chartWidth').value = document.body.offsetWidth - 125;
		var chartwidth = document.getElementById('ContentPlaceHolder1_chartWidth').value;
		//3/28/2014 NS commented out for 
		//FileOpensCumulativeWebChart.PerformCallback();
		//FileOpensWebChart.PerformCallback();
		//chttpSessionsWebChart.PerformCallback();
		DeviceTypeChart.PerformCallback();
		OSTypeChart.PerformCallback();
		P2PSessionsChart1.PerformCallback();
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
        .textalignmiddle {
            width: 100%;
            height: 100%;
        }
         .parent
        {
            position: relative;height:30px;width:145px;
        }
        .msg
        {
            /* 4/21/2014 NS commented out */
            /*
            position: absolute;
            left: 0px;
            top: 12px;
            */
            text-align: right;
            vertical-align: middle;
            height: 30px;
        }
        .b
        { position: absolute;
             top: -10px;
            }
        a.tooltip2
        {text-decoration: none;
            outline: none;
            /* 4/21/2014 NS commented out */
            /* display: inline-block; */
            width: 100%;
            height: 100%;
            /* 4/21/2014 NS commented out */
            /*color: Black;*/
            top: -5px;
            
        }
        a.tooltip2 strong
        {
            line-height: 30px;
        }
        a.tooltip2:hover
        {
            text-decoration: none;
            /* 4/21/2014 NS commented out */
            /*color: Black;*/
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
        {text-decoration: none;
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
        a.noclass{
           line-height: 0px; visibility :hidden;
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
            outline: none;
            /* 4/21/2014 NS commented out */
            /* display: inline-block; */
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
        {text-decoration: none;
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
    <table width="100%">
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <img alt="" src="../images/icons/group.png" />
                        </td>
                        <td>
                            <div class="header" id="servernamelbldisp" runat="server">Office 365 Skype for Business Stats</div>
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
                             <dx:ASPxMenu ID="ASPxMenu1" runat="server" ShowAsToolbar="True" 
                                             OnInit="ASPxMenu1_Init" Theme="Moderno" HorizontalAlign="Right">
                                            <ClientSideEvents ItemClick="OnItemClick" />
                                            <Items>
                                                <dx:MenuItem BeginGroup="True" Name="EditConfigItem2" Text="">
                                                    <Items>
<%--                                                        <dx:MenuItem Name="BackItem" Text="Back" NavigateUrl='/WesWasHere.aspx'>
                                                        </dx:MenuItem>
                                                        <dx:MenuItem Name="ScanItem" Text="Scan Now">
                                                        </dx:MenuItem>
                                                        <dx:MenuItem Name="EditConfigItem" Text="Edit in Configurator">
                                                        </dx:MenuItem>
                                                        <dx:MenuItem Name="SuspendItem" Text="Suspend Temporarily">
                                                        </dx:MenuItem>--%>
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
    <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
        EnableTheming="True" Theme="Glass" Width="100%" EnableHierarchyRecreation="False">
        <TabPages>
			<dx:TabPage Text="Mobile Devices">
                <TabImage Url="~/images/icons/phone.png">
                </TabImage>
                <ContentCollection>
                    <dx:ContentControl ID="ContentControl2" runat="server" SupportsDisabledAttribute="True">
                        <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table class="navbarTbl">
                                    <tr>
                                        <td>
										</td></tr><tr>
                                        <td>
                                            <dxchartsui:WebChartControl ID="deviceTypeWebChart" runat="server" 
                                                ClientInstanceName="DeviceTypeChart" Height="500px" 
                                                 Width="1200px">
                                                <diagramserializable>
                                                    <cc1:XYDiagram>
                                                        <axisx visibleinpanesserializable="-1">
                                                            <range sidemarginsenabled="True" />
                                                        <range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /></axisx><axisy visibleinpanesserializable="-1">
                                                            <range sidemarginsenabled="True" />
                                                            <numericoptions format="Number" precision="0" />
                                                        <range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /></axisy></cc1:XYDiagram></diagramserializable><fillstyle>
                                                    <optionsserializable>
                                                        <cc1:SolidFillOptions />
                                                    </optionsserializable></fillstyle><seriesserializable>
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
                                                        </legendpointoptionsserializable></cc1:Series></seriesserializable><seriestemplate>
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
                                                    </legendpointoptionsserializable></seriestemplate><crosshairoptions>
                                                    <commonlabelpositionserializable>
                                                        <cc1:CrosshairMousePosition />
                                                    </commonlabelpositionserializable></crosshairoptions><tooltipoptions>
                                                    <tooltippositionserializable>
                                                        <cc1:ToolTipMousePosition />
                                                    </tooltippositionserializable></tooltipoptions></dxchartsui:WebChartControl></td></tr></table></ContentTemplate></asp:UpdatePanel></dx:ContentControl></ContentCollection></dx:TabPage> 
			<dx:TabPage Text="P2P Sessions">
                <TabImage Url="~/images/icons/chart_pie.png">
                </TabImage>
                <ContentCollection>
                    <dx:ContentControl ID="ContentControl3" runat="server" SupportsDisabledAttribute="True">
                        <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                            
                            <ContentTemplate>
                                <table class="navbarTbl">
                                    <tr>
                                        <td>
                                           </td></tr><tr>
 <td>
										<div id="OSPie" style="visibility:hidden;display:none;">
										<dxchartsui:WebChartControl ID="OSTypeWebChartControl" runat="server" 
                                                ClientInstanceName="OSTypeChart" Height="400px" 
                                                 Width="1200px">
                                                <diagramserializable>
                                                    <cc1:SimpleDiagram>
                                                    </cc1:SimpleDiagram>
                                                </diagramserializable>
                                                <fillstyle>
                                                    <optionsserializable>
                                                        <cc1:SolidFillOptions />
                                                    </optionsserializable></fillstyle><seriesserializable>
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
                                                        </legendpointoptionsserializable></cc1:Series></seriesserializable><seriestemplate>
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
                                                    </legendpointoptionsserializable></seriestemplate><crosshairoptions>
                                                    <commonlabelpositionserializable>
                                                        <cc1:CrosshairMousePosition />
                                                    </commonlabelpositionserializable></crosshairoptions><tooltipoptions>
                                                    <tooltippositionserializable>
                                                        <cc1:ToolTipMousePosition />
                                                    </tooltippositionserializable></tooltipoptions></dxchartsui:WebChartControl></div><div id="OSBar" >
										<dxchartsui:WebChartControl ID="P2PSessionsChart1" runat="server" 
                                                ClientInstanceName="P2PSessionsChart1" Height="600px" 
                                                 Width="1200px">
                                                <diagramserializable>
                                                    <cc1:XYDiagram>
                                                        <axisx visibleinpanesserializable="-1">
                                                            <range sidemarginsenabled="True" />
                                                        <range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /></axisx><axisy visibleinpanesserializable="-1">
                                                            <range sidemarginsenabled="True" />
                                                            <numericoptions format="Number" precision="0" />
                                                        <range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /></axisy></cc1:XYDiagram></diagramserializable><fillstyle>
                                                    <optionsserializable>
                                                        <cc1:SolidFillOptions />
                                                    </optionsserializable></fillstyle><seriesserializable>
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
                                                        </legendpointoptionsserializable></cc1:Series></seriesserializable><seriestemplate>
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
                                                    </legendpointoptionsserializable></seriestemplate><crosshairoptions>
                                                    <commonlabelpositionserializable>
                                                        <cc1:CrosshairMousePosition />
                                                    </commonlabelpositionserializable></crosshairoptions><tooltipoptions>
                                                    <tooltippositionserializable>
                                                        <cc1:ToolTipMousePosition />
                                                    </tooltippositionserializable></tooltipoptions></dxchartsui:WebChartControl></div></td></tr></table></ContentTemplate></asp:UpdatePanel></dx:ContentControl></ContentCollection></dx:TabPage> 
			<dx:TabPage Text="AV Time Report">
                <TabImage Url="~/images/icons/chart_pie.png">
                </TabImage>
                <ContentCollection>
                    <dx:ContentControl ID="ContentControl4" runat="server" SupportsDisabledAttribute="True">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                           
                            <ContentTemplate>
                                <table class="navbarTbl">
                                   
                                    <tr>
                                        <td>
                                            <dxchartsui:WebChartControl ID="AVSessionsChart" runat="server" 
                                                ClientInstanceName="SyncTypeChart" Height="400px" 
                                                 Width="1200px">
                                                <diagramserializable>
                                                    <cc1:SimpleDiagram>
                                                    </cc1:SimpleDiagram>
                                                </diagramserializable>
                                                <fillstyle>
                                                    <optionsserializable>
                                                        <cc1:SolidFillOptions />
                                                    </optionsserializable></fillstyle><seriesserializable>
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
                                                        </legendpointoptionsserializable></cc1:Series></seriesserializable><seriestemplate>
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
                                                    </legendpointoptionsserializable></seriestemplate><crosshairoptions>
                                                    <commonlabelpositionserializable>
                                                        <cc1:CrosshairMousePosition />
                                                    </commonlabelpositionserializable></crosshairoptions><tooltipoptions>
                                                    <tooltippositionserializable>
                                                        <cc1:ToolTipMousePosition />
                                                    </tooltippositionserializable></tooltipoptions></dxchartsui:WebChartControl></td></tr></table></ContentTemplate></asp:UpdatePanel></dx:ContentControl></ContentCollection></dx:TabPage> 
			<dx:TabPage Text="Conferences Report">
                <TabImage Url="~/images/icons/chart_pie.png">
                </TabImage>
                <ContentCollection>
                    <dx:ContentControl ID="ContentControl5" runat="server" SupportsDisabledAttribute="True">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table class="navbarTbl">
                                   <%-- <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="lblsmallFont" 
                                                            Text="Select Type:">
                                                        </dx:ASPxLabel>
                                                    </td>
                                                  <td>
                                                        <dx:ASPxComboBox ID="DeviceCountComboBox" runat="server" AutoPostBack="True" 
                                                            OnSelectedIndexChanged="SyncTypeComboBox_SelectedIndexChanged">
                                                        </dx:ASPxComboBox>
                                                    </td>
													<td>
                                                        <dx:ASPxLabel ID="ASPxLabel9" runat="server" CssClass="lblsmallFont" 
                                                            Text="Select Sub Type:">
                                                        </dx:ASPxLabel>
                                                    </td>
													 <td>
                                                        <dx:ASPxComboBox ID="ASPxComboBox2" runat="server" AutoPostBack="True" 
                                                            OnSelectedIndexChanged="SyncTypeSubComboBox_SelectedIndexChanged">
                                                        </dx:ASPxComboBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td>
                                            <dxchartsui:WebChartControl ID="ConfReportChart" runat="server" 
                                                ClientInstanceName="ConfReportChart" Height="400px" 
                                                 Width="1200px">
                                                <diagramserializable>
                                                    <cc1:SimpleDiagram>
                                                    </cc1:SimpleDiagram>
                                                </diagramserializable>
                                                <fillstyle>
                                                    <optionsserializable>
                                                        <cc1:SolidFillOptions />
                                                    </optionsserializable></fillstyle><seriesserializable>
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
                                                        </legendpointoptionsserializable></cc1:Series></seriesserializable><seriestemplate>
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
                                                    </legendpointoptionsserializable></seriestemplate><crosshairoptions>
                                                    <commonlabelpositionserializable>
                                                        <cc1:CrosshairMousePosition />
                                                    </commonlabelpositionserializable></crosshairoptions><tooltipoptions>
                                                    <tooltippositionserializable>
                                                        <cc1:ToolTipMousePosition />
                                                    </tooltippositionserializable></tooltipoptions></dxchartsui:WebChartControl></td></tr></table></ContentTemplate></asp:UpdatePanel></dx:ContentControl></ContentCollection></dx:TabPage> 
		</TabPages>
		</dx:ASPxPageControl><dx:ASPxGridViewExporter ID="UsersGridViewExporter" runat="server" 
            GridViewID="UsersGrid">
        </dx:ASPxGridViewExporter>
</asp:Content>
