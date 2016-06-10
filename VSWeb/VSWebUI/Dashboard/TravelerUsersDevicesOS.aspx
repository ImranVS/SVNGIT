<%@ Page Title="VitalSigns Plus - Traveler Health" Language="C#" MasterPageFile="~/DashboardSite.Master"
    AutoEventWireup="true" CodeBehind="TravelerUsersDevicesOS.aspx.cs" Inherits="VSWebUI.Dashboard.TravelerUsersDevicesOS_NEW" %>
	<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>





<%@ Register assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts.Web" tagprefix="dxchartsui" %>
<%@ Register assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
        <link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
    <style type="text/css">
        .imagen img[src=""] {
           display: none;
        }
        
        /* 3/31/2015 NS commented out the code below - the fix is not correct, the proper fix is in the bootstrap1.min.css file */
        /*
        #ContentPlaceHolder1_DeviceSyncsRoundPanel_DeviceSyncsWebChart

		{
			height:400px !important;
			width:1200px !important;
		}
		#ContentPlaceHolder1_DeviceSyncsRoundPanel_DeviceSyncsWebChart_IMG
		{
			height:400px !important;
			width:1200px !important;
		}
		
		   #ContentPlaceHolder1_httpSessionsASPxRoundPanel_httpSessionsWebChart



		{
			height:400px !important;
			width:1200px !important;
		}
		#ContentPlaceHolder1_httpSessionsASPxRoundPanel_httpSessionsWebChart_IMG
		{
			height:400px !important;
			width:1200px !important;
		}
		
		
		
		
		  #ContentPlaceHolder1_mailFileOpensDeltaRoundPanel_mailFileOpensWebChart

		{
			height:400px !important;
			width:1200px !important;
		}
		#ContentPlaceHolder1_mailFileOpensDeltaRoundPanel_mailFileOpensWebChart_IMG
		{
			height:400px !important;
			width:1200px !important;
		}
		
		   #ContentPlaceHolder1_mailFileOpensRoundPanel_mailFileOpensCumulativeWebChart



		{
			height:400px !important;
			width:1200px !important;
		}
		#ContentPlaceHolder1_mailFileOpensRoundPanel_mailFileOpensCumulativeWebChart_IMG
		{
			height:400px !important;
			width:1200px !important;
		}
        */
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.imagen[src=""]').hide();
            $('.imagen:not([src=""])').show();
            $('.logoimg[src=""]').hide();
            $('.logoimg:not([src=""])').show();
            $('.alert-success').delay(10000).fadeOut("slow", function () {
            });
            $('.alert-danger').delay(10000).fadeOut("slow", function () {
            });
        });

        $("#grid").click(function () {
            $('.imagen[src=""]').hide();
            $('.imagen:not([src=""])').show();
        });

        function Resized() {
            var callbackState = document.getElementById('ContentPlaceHolder1_callbackState').value;

            if (callbackState == 0)
                DoCallback();
        }

        function DoCallback() {
            //10/8/2013 NS modified
            document.getElementById('ContentPlaceHolder1_chartWidth').value = document.body.offsetWidth - 55;
            var chartwidth = document.getElementById('ContentPlaceHolder1_chartWidth').value;
            //8/19/2014 NS added for VSPLUS-884
            //8/25/2015 NS modified
            //DeviceSyncWebChart.PerformCallback();
            //FileOpensCumulativeWebChart.PerformCallback();
            //FileOpensWebChart.PerformCallback();
            //8/25/2015 NS modified
            //chttpSessionsWebChart.PerformCallback();
            //3/28/2014 NS commented out for 
            //DeviceTypeChart.PerformCallback();
            //OSTypeChart.PerformCallback();
            chartwidth = (document.body.offsetWidth - 35);
            //7/31/2015 NS added
            charthalfwidth = (document.body.offsetWidth - 25) / 2;
            //8/19/2014 NS added for VSPLUS-884
            //8/25/2015 NS modified
            //var devicesyncpanel = document.getElementById('ContentPlaceHolder1_DeviceSyncsRoundPanel');
            //devicesyncpanel.style.width = charthalfwidth + "px";
            //3/28/2014 NS modified for (no more page control)
            //var mailpanel = document.getElementById('ContentPlaceHolder1_ASPxPageControl1_mailFileOpensDeltaRoundPanel');
            //var mailpanel = document.getElementById('ContentPlaceHolder1_mailFileOpensDeltaRoundPanel');
            //mailpanel.style.width = charthalfwidth + "px";
            //3/28/2014 NS modified for (no more page control)
            //var mailpanel2 = document.getElementById('ContentPlaceHolder1_ASPxPageControl1_mailFileOpensRoundPanel');
            //var mailpanel2 = document.getElementById('ContentPlaceHolder1_mailFileOpensRoundPanel');
            //mailpanel2.style.width = charthalfwidth + "px";
            //3/28/2014 NS modified for (no more page control)
            //var httppanel = document.getElementById('ContentPlaceHolder1_ASPxPageControl1_httpSessionsASPxRoundPanel');
            //8/25/2015 NS modified
            //var httppanel = document.getElementById('ContentPlaceHolder1_httpSessionsASPxRoundPanel');
            //httppanel.style.width = charthalfwidth + "px";
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
            var gridCell = document.getElementById('gridCell');
            ASPxClientUtils.AttachEventToElement(gridCell, 'contextmenu', OnGridContextMenu);
            //        var imgButton = document.getElementById('popupButton');
            //        ASPxClientUtils.AttachEventToElement(imgButton, 'contextmenu', OnPreventContextMenu);
            //}
        }
        function OnGridContextMenu(evt) {
            var SortPopupMenu = popupmenu;
            SortPopupMenu.ShowAtPos(evt.clientX + ASPxClientUtils.GetDocumentScrollLeft(), evt.clientY + ASPxClientUtils.GetDocumentScrollTop());
            return OnPreventContextMenu(evt);
        }
        function OnPreventContextMenu(evt) {
            return ASPxClientUtils.PreventEventAndBubble(evt);
        }
    </script>
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
	   	//10/14/2014 NS modified for VSPLUS-1022
	   	function DeviceGridView_ContextMenu(s, e) {
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
	   		if (e.objectType != "row") return;
	   		s.SetFocusedRowIndex(e.index);
	   	}

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
    <script type="text/javascript">
     function grid_ContextMenu(s, e) {
         if (e.objectType == "row") {
             s.SetFocusedRowIndex(e.index);
             popupmenu.ShowAtPos(ASPxClientUtils.GetEventX(e.htmlEvent), ASPxClientUtils.GetEventY(e.htmlEvent));
         }
     }
     
     function grid_SelectionChanged(s, e) {
            s.GetSelectedFieldValues("Name", GetSelectedFieldValuesCallback);
        }
        function GetSelectedFieldValuesCallback(values) {
            selList.BeginUpdate();
            try {
                selList.ClearItems();
                for (var i = 0; i < values.length; i++) {
                    selList.AddItem(values[i]);
                }
            } finally {
                selList.EndUpdate();
            }
            document.getElementById("selCount").innerHTML = grid.GetSelectedRowCount();
        }        
      
    </script>

	<script language="javascript">
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
		function tblmouseover(stype, pav, pth, dav, dth, hav, hth) {
			//alert(stype);
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
			//            if (stype == "Sametime") {
			//            	idiv = document.getElementById("div1");
			//            	idiv.style.visibility = "hidden";
			//            	lPendingMail = document.getElementById("TDp");
			//            	ldeadmail = document.getElementById("TDd");
			//            	lHeldmail = document.getElementById("TDh");
			//            	lPendingMail.style.visibility = "hidden";
			//            	ldeadmail.style.visibility = "hidden";
			//            	lHeldmail.style.visibility = "hidden";
			//			}
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
				str += "Poison Queues : " + hav + "/" + hth;
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
        padding: 5px;  background-color: #fffAF0; font-size: 11px; font-family: Arial;">
        <div style="border-color:#ccc;border: 2px;width: 100%; visibility: hidden; color: Black" id="TDp">
        </div>
    </div>
    <table>
        <tr>
            <td align="center">
                <img width="70%" height="70%" alt="" src="../images/icons/traveler3.png" />
            </td>
            <td>
                <div class="header" id="servernamelbldisp" runat="server">IBM Traveler Server Health</div>
            </td>
        </tr>
    </table>
    <input id="chartWidth" type="hidden" runat="server" value="400" />
    <input id="callbackState" type="hidden" runat="server" value="0" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%">
                            <tr>
                                <td>
                               <%--  //08/3/2016 Sowmya comment for VSPLUS 2692
                                    <dx:ASPxPopupMenu ID="ServerPopupMenu" runat="server" Theme="Moderno" OnItemClick="ServerPopupMenu_ItemClick"
                                        ClientInstanceName="popupmenu">
                                        <Items>
                                            <dx:MenuItem Name="ScanNow" Text="Scan Now">
                                            </dx:MenuItem>
                                            <dx:MenuItem Name="EditInConfig" Text="Edit in Configurator">
                                            </dx:MenuItem>
                                        </Items>
                                    </dx:ASPxPopupMenu>
                                    <div id="errorDiv" runat="server" class="alert alert-danger" style="display: none">
                                        Please select a server in the grid below in order to proceed.
                                        <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                                    </div>--%>
                                    <dx:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server" KeyFieldName="ID"
                                        Width="100%" AutoGenerateColumns="False" Theme="Office2003Blue" OnPageSizeChanged="grid_PageSizeChanged"
                                        OnHtmlDataCellPrepared="grid_HtmlDataCellPrepared" EnableCallBacks="False" 
                                        onselectionchanged="grid_SelectionChanged" >
                                         <ClientSideEvents SelectionChanged="grid_SelectionChanged" ContextMenu="grid_ContextMenu">
                        </ClientSideEvents>
                                        <SettingsPager PageSize="10">
                                            <PageSizeItemSettings Visible="True">
                                            </PageSizeItemSettings>
                                        </SettingsPager>
                                        <Columns>
										<%--<dx:GridViewDataTextColumn Caption="Reasons" FieldName="Reasons" VisibleIndex="15"  Visible="false"
                                                Width="300px">
                                                <Settings AllowDragDrop="True" AutoFilterCondition="Contains"></Settings>
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
                                            </dx:GridViewDataTextColumn>--%>
                                            <dx:GridViewDataTextColumn Caption="Name" FieldName="Name"  VisibleIndex="0" Width="150px">
                                                <HeaderStyle CssClass="GridCssHeader">
                                                    <Paddings Padding="5px" />
                                                    <Paddings Padding="5px"></Paddings>
                                                </HeaderStyle>
                                                <CellStyle CssClass="GridCss">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Status" FieldName="Status" VisibleIndex="1">
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
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Details" FieldName="Details" VisibleIndex="4"
                                                Width="300px">
                                                <Settings AllowDragDrop="True" AutoFilterCondition="Contains"></Settings>
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
                                            <dx:GridViewDataTextColumn Caption="HA" VisibleIndex="8">
                                                <HeaderStyle CssClass="GridCssHeader1" />
                                                <CellStyle CssClass="GridCss1">
                                                </CellStyle>
                                                <DataItemTemplate>
                                                    <img class="imagen" alt="" src='<%# Eval("HAImage") %>' />
                                                </DataItemTemplate>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Traveler Servlet" FieldName="TravelerServlet"
                                                VisibleIndex="6">
                                                <Settings AllowAutoFilter="False" />
                                                <HeaderStyle CssClass="GridCssHeader" />
                                                <CellStyle CssClass="GridCss1">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Heart Beat" FieldName="HeartBeat" VisibleIndex="5"
                                                Width="150px">
                                                <Settings AllowDragDrop="True" AutoFilterCondition="Contains"></Settings>
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader">
                                                    <Paddings Padding="5px" />
                                                </HeaderStyle>
                                                <CellStyle CssClass="GridCss">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Users" FieldName="Users" VisibleIndex="7" 
                                                Width="60px">
                                                <Settings AllowDragDrop="True" AutoFilterCondition="Contains"></Settings>
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader">
                                                    <Paddings Padding="5px" />
                                                </HeaderStyle>
                                                <CellStyle CssClass="GridCss2">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Incremental Syncs" FieldName="IncrementalSyncs"
                                                VisibleIndex="10" Width="80px" HeaderStyle-Wrap="True">
                                                <HeaderStyle CssClass="GridCssHeader1">
                                                    <Paddings Padding="5px" />
                                                </HeaderStyle>
                                                <CellStyle CssClass="GridCss2">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" VisibleIndex="11" 
                                                Visible="False">
                                                <HeaderStyle CssClass="GridCssHeader">
                                                    <Paddings Padding="5px" />
                                                </HeaderStyle>
                                                <CellStyle CssClass="GridCss2">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Devices" 
                                                FieldName="Devices" VisibleIndex="12"
                                                Width="70px" Visible="False"><HeaderStyle CssClass="GridCssHeader1">
                                                    <Paddings Padding="5px" />
                                                </HeaderStyle>
                                                <CellStyle CssClass="GridCss2">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="HTTP Status" FieldName="HTTP_Status" VisibleIndex="13"
                                                Width="80px">
                                                <Settings AllowDragDrop="True" AutoFilterCondition="Contains"></Settings>
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader1" Wrap="True">
                                                    <Paddings Padding="5px" />
                                                    <Paddings Padding="5px"></Paddings>
                                                </HeaderStyle>
                                                <CellStyle CssClass="GridCss1">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="HTTP Threads" FieldName="HTTP_Details" 
                                                VisibleIndex="14"><Settings AllowDragDrop="True" AutoFilterCondition="Contains"></Settings>
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader1"><Paddings Padding="5px" />
                                                    <Paddings Padding="5px"></Paddings>
                                                </HeaderStyle>
                                                <CellStyle CssClass="GridCss1"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn 
                                                Caption="HTTP Peak Connections" FieldName="HTTP_PeakConnections"
                                                VisibleIndex="15" Width="90px"><Settings AllowDragDrop="True" AutoFilterCondition="Contains"></Settings>
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader1">
                                                    <Paddings Padding="5px" />
                                                    <Paddings Padding="5px"></Paddings>
                                                </HeaderStyle>
                                                <CellStyle CssClass="GridCss2">
                                                </CellStyle>
                                                <Settings AllowDragDrop="True" AutoFilterCondition="Contains" />
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader1" Wrap="True">
                                                    <Paddings Padding="5px" />
                                                    <Paddings Padding="5px" />
                                                </HeaderStyle>
                                                <CellStyle CssClass="GridCss1">
                                                </CellStyle>
                                                <Settings AllowDragDrop="True" AutoFilterCondition="Contains" />
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" Wrap="True">
                                                    <Paddings Padding="5px" />
                                                    <Paddings Padding="5px" />
                                                </HeaderStyle>
                                                <CellStyle CssClass="GridCss">
                                                </CellStyle>
                                                <Settings AllowDragDrop="True" AutoFilterCondition="Contains" />
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" Wrap="True">
                                                    <Paddings Padding="5px" />
                                                </HeaderStyle>
                                                <CellStyle CssClass="GridCss2">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn 
                                                Caption="HTTP Max Configured Connections" FieldName="HTTP_MaxConfiguredConnections"
                                                VisibleIndex="16" Width="160px"><Settings AllowAutoFilter="False" />
                                                <Settings AllowDragDrop="True" AllowAutoFilter="False" AutoFilterCondition="Contains">
                                                </Settings>
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader1" Wrap="True">
                                                    <Paddings Padding="5px" />
                                                    <Paddings Padding="5px"></Paddings>
                                                </HeaderStyle>
                                                <CellStyle CssClass="GridCss2">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Version" FieldName="TravelerVersion" VisibleIndex="2"
                                                Width="70px">
                                                <Settings AllowAutoFilter="False" />
                                                <Settings AllowDragDrop="True" AllowAutoFilter="False" AutoFilterCondition="Contains">
                                                </Settings>
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" Wrap="False">
                                                    <Paddings Padding="5px" />
                                                    <Paddings Padding="5px"></Paddings>
                                                </HeaderStyle>
                                                <CellStyle CssClass="GridCss1">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Datastore Test Status" FieldName="HA_Datastore_Status"
                                                VisibleIndex="9">
                                                <HeaderStyle CssClass="GridCssHeader1" Wrap="True" />
                                                <CellStyle CssClass="GridCss1">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Devices API Status" FieldName="DevicesAPIStatus"
                                                VisibleIndex="17">
                                                <HeaderStyle CssClass="GridCssHeader1" Wrap="True" />
                                                <CellStyle CssClass="GridCss1">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Resource Constraint" 
                                                FieldName="ResourceConstraint" VisibleIndex="3">
                                                <HeaderStyle CssClass="GridCssHeader1" Wrap="True" />
                                                <CellStyle CssClass="GridCss1">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                        <SettingsBehavior AllowDragDrop="True" AutoExpandAllGroups="True"
                                            AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True" 
                                             ColumnResizeMode="NextColumn" AllowSelectSingleRowOnly="True" /><Settings ShowHorizontalScrollBar="True" />
                                        <Settings ShowFilterRow="True" ShowHorizontalScrollBar="True"></Settings>
                                        <Styles>
                                            <AlternatingRow CssClass="CssAltRow">
                                            </AlternatingRow>
                                        </Styles>
                                    </dx:ASPxGridView>
                                </td>
                            </tr>
                <tr>
                                <td>
                                    
                                </td>
                            </tr>
                <tr>
                    <td>
                    <table>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Select a mail server:" 
                                                CssClass="lblsmallFont" Visible="False"></dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Select an interval:" 
                                                CssClass="lblsmallFont" Visible="False">
                                            </dx:ASPxLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxComboBox ID="mailServerListComboBox" runat="server" ValueType="System.String"
                                                AutoPostBack="True" 
                                                OnSelectedIndexChanged="travelerIntervalComboBox_SelectedIndexChanged" 
                                                Visible="False"></dx:ASPxComboBox>
                                        </td>
                                        <td>
                                            <dx:ASPxComboBox ID="travelerIntervalComboBox" runat="server" ValueType="System.String"
                                                AutoPostBack="True" 
                                                OnSelectedIndexChanged="travelerIntervalComboBox_SelectedIndexChanged" 
                                                Visible="False">
                                            </dx:ASPxComboBox>
                                        </td>
                                    </tr>
                                </table>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td>
                                <table>
                                    <tr>
                                        <td>
                                            <dx:ASPxRoundPanel ID="mailFileOpensDeltaRoundPanel" runat="server" ClientInstanceName="cmailFileOpensDeltaRoundPanel"

                                            CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                            
                    SpriteCssFilePath="~/App_Themes/Glass/{0}/styles.css" Theme="Default" 
                                            Width="100%" GroupBoxCaptionOffsetY="-24px" 
                                            HeaderText="Mail File Open Times Delta" Visible="False"><ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                            <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                            <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                                                    <dxchartsui:WebChartControl 
                                                            ID="mailFileOpensWebChart" runat="server" ClientInstanceName="FileOpensWebChart" 
                                                        Height="350px" Width="440px" 
                                                            OnCustomCallback="mailFileOpensWebChart_CustomCallback" 
                                                            OnBoundDataChanged="mailFileOpensWebChart_BoundDataChanged"><fillstyle>
                                                            <optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><seriestemplate>
                                                            <viewserializable><cc1:SideBySideBarSeriesView></cc1:SideBySideBarSeriesView></viewserializable><labelserializable><cc1:SideBySideBarSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:SideBySideBarSeriesLabel></labelserializable><legendpointoptionsserializable><cc1:PointOptions></cc1:PointOptions></legendpointoptionsserializable></seriestemplate><crosshairoptions>
                                                            <commonlabelpositionserializable><cc1:CrosshairMousePosition /></commonlabelpositionserializable></crosshairoptions><tooltipoptions>
                                                            <tooltippositionserializable><cc1:ToolTipMousePosition /></tooltippositionserializable></tooltipoptions><legend maxhorizontalpercentage="30"></legend>
                                                        </dxchartsui:WebChartControl>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dx:ASPxRoundPanel>
                                        </td>
                                        <td>

                                            <dx:ASPxRoundPanel runat="server" ID="mailFileOpensRoundPanel"
                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                    GroupBoxCaptionOffsetY="-24px" HeaderText="Cumulative Mail File Open Times" 
                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%" 
                    Theme="Default" Visible="False"><ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                            <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                            <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>
                    <PanelCollection>
<dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                        <dxchartsui:WebChartControl 
                                                            ID="mailFileOpensCumulativeWebChart" runat="server" Height="350px" 
                            Width="440px" ClientInstanceName="FileOpensCumulativeWebChart"
                            OnCustomCallback="mailFileOpensCumulativeWebChart_CustomCallback" 
                                                            OnBoundDataChanged="mailFileOpensCumulativeWebChart_BoundDataChanged"><fillstyle>
                                <optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><seriestemplate>
                                <viewserializable><cc1:SideBySideBarSeriesView></cc1:SideBySideBarSeriesView></viewserializable><labelserializable><cc1:SideBySideBarSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:SideBySideBarSeriesLabel></labelserializable><legendpointoptionsserializable><cc1:PointOptions></cc1:PointOptions></legendpointoptionsserializable></seriestemplate><crosshairoptions>
                                <commonlabelpositionserializable><cc1:CrosshairMousePosition /></commonlabelpositionserializable></crosshairoptions><tooltipoptions>
                                <tooltippositionserializable><cc1:ToolTipMousePosition /></tooltippositionserializable></tooltipoptions><legend maxhorizontalpercentage="30"></legend>
                                                        </dxchartsui:WebChartControl>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dx:ASPxRoundPanel>
                                        </td>
                                    </tr>
                                </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
