﻿<%@ Page Title="VitalSigns Plus - Sharepoint details page" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true"
    CodeBehind="Sharepointdetailspage.aspx.cs" Inherits="VSWebUI.Dashboard.Sharepointdetailspage" %>
	
<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>



<%@ Register Assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.alert-success').delay(10000).fadeOut("slow", function () {
            });
        });
        function BackButton_Clicked(s, e) {
            window.history.back();
        }
        function WebAdminButton_Clicked(s, e) {
            var urllink = document.getElementById('ContentPlaceHolder1_ASPxPageControl1_webAdminLabel');
            if (urllink.innerHTML != '') {
                window.open(urllink.innerHTML, '_blank');
            }
        }
        //7/14/2014 NS added for VSPLUS-813
        function OnSelectedItemChanged(s, e) {
            var txt = rblDiskSpace.GetSelectedItem().value;
            //alert(txt);
            if (txt == 'Chart') {
                LyncchartDiskSpace.SetVisible(true);
                grid.SetVisible(false);
            }
            else {
                if (txt == 'Grid') {
                    LyncchartDiskSpace.SetVisible(false);
                    grid.SetVisible(true);
                }
            }
        }
        function OnItemClick(s, e) {
            if (e.item.parent == s.GetRootItem())
                e.processOnServer = false;
        }
    </script>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        iframe
        {
            margin: 0;
            padding: 0;
            height: 500px;
        }
        iframe
        {
            display: block;
            width: 100%;
            border: none;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function PopupCenter(pageURL) {
            //alert(pageURL);
            var w = 500;
            var h = 550;
            var left = (screen.width / 2) - (w / 2);
            var top = (screen.height / 2) - (h / 2);
            var targetWin;
            //1/4/2013 NS modified the second parameter in the window.open call - IE8 doesn't recognize the second parameter if 
            //it's anything other than an empty string
            targetWin = (window.open(pageURL, '', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left));
        } 
    </script>
    <script type="text/javascript">

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
        //    $("#SuspendTemporarily").click(function () {
        //        //        $("#Suspend").dialog({
        //        //            title: "jQuery Modal Dialog Popup",
        //        //            buttons: {
        //        //                Close: function () {
        //        //                    $(this).dialog('close');
        //        //                }
        //        //            },
        //        //            modal: true
        //        //        });
        //        //        return false;
        //        alert('ok');
        //    });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript" src="../js/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.10.3.custom.js"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.10.3.custom.min.js"></script>
    <link rel="stylesheet" type="text/css" media="screen" href="../css/jquery-ui-1.10.3.custom.css" />
    <link rel="stylesheet" type="text/css" media="screen" href="../css/jquery-ui-1.10.3.custom.min.css" />
    <script type="text/javascript">
    	$(function () {
    		$('.date-picker').datepicker({
    			changeMonth: true,
    			changeYear: true,
    			showButtonPanel: true,
    			dateFormat: 'MM yy',
    			onClose: function (dateText, inst) {
    				var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
    				var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
    				$(this).datepicker('setDate', new Date(year, month, 1));
    				DoCallback();
    			}
    		});
    	});
    	function DoCallback() {
    		EventsHistory.PerformCallback();
    	}
    	function OnItemClick(s, e) {
    		if (e.item.parent == s.GetRootItem())
    			e.processOnServer = false;
    	}
		</script>
   <table width="100%">
        <tr>
            <td valign="top">
                <table>
                    <tr>
                        <td>
                            <div id="StatusHolder">
<div class="StatusLabel">Overall Status</div><div class="OK" id="serverstatus" runat="server">OK</div>
</div>
                        </td>
                        <td>
                            <div id="NameHolder">
<div class="header" id="servernamelbldisp" runat="server">azphxdom1/RPRWyatt</div>
<div class="scan" id="Lastscanned" runat="server">9/16/2014 2:59:00 PM</div>
</div>
                                                <asp:Label ID="lblServerType" runat="server" Text="IBM Domino Server: " Font-Bold="True"
                                                   Font-Size="Large" Style="color: #000000; font-family: Verdana; display: none"></asp:Label>             
 
    <asp:Label ID="servernamelbl" runat="server" Text="Label" Font-Bold="True"
        Font-Size="Large" Style="color: #000000; font-family: Verdana; display: none"></asp:Label>

                        </td>
                    </tr>
                </table>
                
                                                                    </td>
            <td>
                &nbsp;
            </td>
            <td valign="top" align="right">
                <dx:ASPxMenu ID="ASPxMenu1" runat="server" HorizontalAlign="Right" 
                    ShowAsToolbar="True" onitemclick="ASPxMenu1_ItemClick1" 
                    style="height: 33px" Theme="Moderno">
                    <ClientSideEvents ItemClick="OnItemClick" />
                    <Items>
                        <dx:MenuItem BeginGroup="True" Name="EditConfigItem2" Text="">
                            <Items>
                                <dx:MenuItem Name="BackItem" Text="Back">
                                </dx:MenuItem>
                                <dx:MenuItem Name="ScanItem" Text="Scan Now">
                                </dx:MenuItem>
                                <dx:MenuItem Name="EditConfigItem" Text="Edit in Configurator">
                                </dx:MenuItem>
                                <dx:MenuItem Name="SuspendItem" Text="Suspend Temporarily">
                                </dx:MenuItem>
                            </Items>
                            <Image Url="~/images/icons/Gear.png">
                            </Image>
                        </dx:MenuItem>
                    </Items>
                </dx:ASPxMenu>
            </td>
                                                                      
                                                       <%--<ClientSideEvents RowClick="function(s, e) { s.PerformCallback(e.visibleIndex); }" />--%>
                                                    </tr>

             <tr>
                                                        <td colspan="3">
                                                            <div id="ErrorMsg" class="alert alert-danger" runat="server" style="display: none">The settings were not updated</div>
                                                            <div id="SuccessMsg" class="alert alert-success" runat="server" style="display: none">Temporarily Suspended monitoring</div>
                                                        </td>
                                                   
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <dx:ASPxPageControl ID="ASPxPageControl1" ActiveTabIndex="0" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                CssPostfix="Glass" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" TabSpacing="0px"
                runat="server" Width="100%" EnableHierarchyRecreation="False">
                <TabPages>
                    <dx:TabPage Text="Overall">
                        <TabImage Url="~/images/icons/information.png">
                        </TabImage>
                        <ContentCollection>
                            <dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">
                                <table width="100%">
                                    <tr>
                                        <td valign="top">
                                            <asp:UpdatePanel ID="PerformUpdatePanel" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <dx:ASPxRoundPanel ID="performanceASPxRoundPanel" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                        CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Performance" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                                                        Width="100%">
                                                        <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                                        <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                                        <HeaderStyle Height="23px">
                                                            <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                                            <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                                        </HeaderStyle>
                                                        <PanelCollection>
                                                            <dx:PanelContent ID="PanelContent10" runat="server" SupportsDisabledAttribute="True">
                                                                <table>
                                                                    <tr>
                                                                        <td style="padding-left: 10px">
                                                                            <dx:ASPxHyperLink ID="PerfrmHyperLink" runat="server" Text="More" />
                                                                            <br />
                                                                            <br />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <dxchartsui:WebChartControl ID="performanceWebChartControl" runat="server" Height="465px"
                                                                                Width="600px">
                                                                                <diagramserializable>
                                                            <cc1:XYDiagram PaneLayoutDirection="Horizontal">
                                                                <AxisX Title-Text="Date" VisibleInPanesSerializable="-1" GridSpacingAuto="True" 
                                                                    DateTimeMeasureUnit="Hour" minorcount="5"><label enableantialiasing="False" Staggered="True"><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions></label><Range SideMarginsEnabled="True"></Range><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /></AxisX>
                                                                <AxisY Title-Text="Space(bytes)" Title-Visible="True" VisibleInPanesSerializable="-1"><label><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions></label><Range AlwaysShowZeroLevel="False" SideMarginsEnabled="True"></Range></AxisY>
                                                            </cc1:XYDiagram>
                                                        </diagramserializable>
                                                                                <fillstyle><OptionsSerializable><cc1:SolidFillOptions></cc1:SolidFillOptions></OptionsSerializable>
</fillstyle>
                                                                                <seriesserializable>
                    <cc1:Series Name="Series 1" ArgumentScaleType="DateTime">
                        <viewserializable><cc1:LineSeriesView></cc1:LineSeriesView></viewserializable>
                        <labelserializable><cc1:PointSeriesLabel LineVisible="True" 
                            ResolveOverlappingMode="HideOverlapped"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:PointSeriesLabel></labelserializable>
                        <legendpointoptionsserializable><cc1:PointOptions></cc1:PointOptions></legendpointoptionsserializable>
                    </cc1:Series>
                    <cc1:Series Name="Series 2" Visible="False">
                        <viewserializable><cc1:LineSeriesView></cc1:LineSeriesView></viewserializable>
                        <labelserializable><cc1:PointSeriesLabel LineVisible="True" 
                            ResolveOverlappingMode="HideOverlapped"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:PointSeriesLabel></labelserializable>
                        <legendpointoptionsserializable><cc1:PointOptions></cc1:PointOptions></legendpointoptionsserializable>
                    </cc1:Series>
                </seriesserializable>
                                                                                <seriestemplate argumentscaletype="DateTime">
                    <viewserializable><cc1:LineSeriesView></cc1:LineSeriesView></viewserializable>
                    <labelserializable><cc1:PointSeriesLabel LineVisible="True" 
                            ResolveOverlappingMode="Default"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /></cc1:PointOptions></pointoptionsserializable></cc1:PointSeriesLabel></labelserializable>
                    <legendpointoptionsserializable><cc1:PointOptions><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /></cc1:PointOptions></legendpointoptionsserializable>
                </seriestemplate>
                                                                                <crosshairoptions><CommonLabelPositionSerializable><cc1:CrosshairMousePosition></cc1:CrosshairMousePosition></CommonLabelPositionSerializable>
</crosshairoptions>
                                                                                <tooltipoptions><ToolTipPositionSerializable><cc1:ToolTipMousePosition></cc1:ToolTipMousePosition></ToolTipPositionSerializable>
</tooltipoptions>
                                                                            </dxchartsui:WebChartControl>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </dx:PanelContent>
                                                        </PanelCollection>
                                                    </dx:ASPxRoundPanel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td valign="top">
                                            <dx:ASPxRoundPanel ID="diskspaceASPxRoundPanel" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Disk Space" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                                                Width="100%">
                                                <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                                <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                                <HeaderStyle Height="23px">
                                                    <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                                    <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                                </HeaderStyle>
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <dx:ASPxHyperLink ID="linkDiskSpaceMore" runat="server" Text="More" Visible="False">
                                                                    </dx:ASPxHyperLink>
                                                                    <dx:ASPxRadioButtonList ID="DiskSpaceRadioButtonList" runat="server" ClientInstanceName="rblDiskSpace"
                                                                        RepeatDirection="Horizontal" SelectedIndex="0">
                                                                        <Items>
                                                                            <dx:ListEditItem Selected="True" Text="View Chart" Value="Chart" />
                                                                            <dx:ListEditItem Text="View Grid" Value="Grid" />
                                                                        </Items>
                                                                        <ClientSideEvents SelectedIndexChanged="OnSelectedItemChanged" />
                                                                    </dx:ASPxRadioButtonList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <dx:ASPxGridView ID="DiskSpaceLync" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid"
                                                                                CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                                                                                Cursor="pointer" EnableTheming="True" KeyFieldName="ID" OnCustomCallback="DiskSpaceLync_CustomCallback"
                                                                                OnHtmlDataCellPrepared="DiskSpaceLync_HtmlDataCellPrepared" OnHtmlRowCreated="DiskSpaceLync_HtmlRowCreated"
                                                                                OnPageSizeChanged="DiskSpaceLync_PageSizeChanged" Theme="Office2003Blue">
                                                                                <Columns>
                                                                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" ShowInCustomizationForm="True"
                                                                                        Visible="False" VisibleIndex="11">
                                                                                        <Settings AllowAutoFilter="False" />
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn Caption="Disk Name" FieldName="DiskName" ShowInCustomizationForm="True"
                                                                                        VisibleIndex="2">
                                                                                        <Settings AllowAutoFilter="False" AllowDragDrop="True" AutoFilterCondition="Contains" />
                                                                                        <EditCellStyle CssClass="GridCss">
                                                                                        </EditCellStyle>
                                                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                                                        </EditFormCaptionStyle>
                                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                                        <CellStyle CssClass="GridCss">
                                                                                        </CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn Caption="Percent Free" FieldName="PercentFree" ShowInCustomizationForm="True"
                                                                                        VisibleIndex="5" Width="100px">
                                                                                        <PropertiesTextEdit DisplayFormatString="{0:P}">
                                                                                        </PropertiesTextEdit>
                                                                                        <Settings AllowAutoFilter="False" AllowDragDrop="True" />
                                                                                        <EditCellStyle CssClass="GridCss">
                                                                                        </EditCellStyle>
                                                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                                                        </EditFormCaptionStyle>
                                                                                        <HeaderStyle CssClass="GridCssHeader2" />
                                                                                        <CellStyle CssClass="GridCss2">
                                                                                        </CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn Caption="Disk Free (GB)" FieldName="DiskFree" ShowInCustomizationForm="True"
                                                                                        VisibleIndex="4" Width="120px">
                                                                                        <PropertiesTextEdit DisplayFormatString="0.00">
                                                                                        </PropertiesTextEdit>
                                                                                        <Settings AllowDragDrop="True" AutoFilterCondition="Contains" />
                                                                                        <EditCellStyle CssClass="GridCss">
                                                                                        </EditCellStyle>
                                                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                                                        </EditFormCaptionStyle>
                                                                                        <HeaderStyle CssClass="GridCssHeader2" />
                                                                                        <CellStyle CssClass="GridCss2" HorizontalAlign="Center">
                                                                                        </CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn Caption="Disk Size (GB)" FieldName="DiskSize" ShowInCustomizationForm="True"
                                                                                        VisibleIndex="3" Width="120px">
                                                                                        <PropertiesTextEdit DisplayFormatString="0.00">
                                                                                        </PropertiesTextEdit>
                                                                                        <Settings AllowAutoFilter="False" AllowDragDrop="True" AutoFilterCondition="Contains" />
                                                                                        <EditCellStyle CssClass="GridCss">
                                                                                        </EditCellStyle>
                                                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                                                        </EditFormCaptionStyle>
                                                                                        <HeaderStyle CssClass="GridCssHeader2" />
                                                                                        <CellStyle CssClass="GridCss2">
                                                                                        </CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn Caption="Disk Utilization" FieldName="PercentUtilization"
                                                                                        ShowInCustomizationForm="True" Visible="False" VisibleIndex="8" Width="120px">
                                                                                        <Settings AllowAutoFilter="False" AllowDragDrop="True" AutoFilterCondition="Contains" />
                                                                                        <EditCellStyle CssClass="GridCss">
                                                                                        </EditCellStyle>
                                                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                                                        </EditFormCaptionStyle>
                                                                                        <HeaderStyle CssClass="GridCssHeader2" />
                                                                                        <CellStyle CssClass="GridCss2">
                                                                                        </CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn Caption="Threshold" FieldName="ThresholdDisp" ShowInCustomizationForm="True"
                                                                                        VisibleIndex="6" Width="100px">
                                                                                        <Settings AllowAutoFilter="False" AutoFilterCondition="Contains" />
                                                                                        <EditCellStyle CssClass="GridCss">
                                                                                        </EditCellStyle>
                                                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                                                        </EditFormCaptionStyle>
                                                                                        <HeaderStyle CssClass="GridCssHeader2" />
                                                                                        <CellStyle CssClass="GridCss2">
                                                                                        </CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                </Columns>
                                                                                <SettingsBehavior AllowDragDrop="False" AllowFocusedRow="True" AllowSelectByRowClick="True"
                                                                                    AutoExpandAllGroups="True" ColumnResizeMode="NextColumn" ProcessSelectionChangedOnServer="True" />
                                                                                <SettingsPager SEOFriendly="Enabled">
                                                                                    <PageSizeItemSettings Visible="True">
                                                                                    </PageSizeItemSettings>
                                                                                </SettingsPager>
                                                                                <Images SpriteCssFilePath="~/App_Themes/Office2010Silver/{0}/sprite.css">
                                                                                    <LoadingPanelOnStatusBar Url="~/App_Themes/Office2010Silver/GridView/Loading.gif">
                                                                                    </LoadingPanelOnStatusBar>
                                                                                    <LoadingPanel Url="~/App_Themes/Office2010Silver/GridView/Loading.gif">
                                                                                    </LoadingPanel>
                                                                                </Images>
                                                                                <ImagesFilterControl>
                                                                                    <LoadingPanel Url="~/App_Themes/Office2010Silver/GridView/Loading.gif">
                                                                                    </LoadingPanel>
                                                                                </ImagesFilterControl>
                                                                                <Styles CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver">
                                                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                                    </Header>
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
                                                                            <dx:ASPxLabel ID="lblServerName" runat="server" Visible="False">
                                                                            </dx:ASPxLabel>
                                                                            <dxchartsui:WebChartControl ID="DiskSpaceWebChartControl1" runat="server" ClientInstanceName="LyncchartDiskSpace"
                                                                                CrosshairEnabled="True" Height="465px" Width="600px">
                                                                                <diagramserializable>
                                                                <cc1:XYDiagram Rotated="True">
                                                                    <axisx visibleinpanesserializable="-1" reverse="True"><label><resolveoverlappingoptions allowhide="False" allowrotate="False" /><resolveoverlappingoptions allowhide="False" allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions 
                                                                        allowhide="False" allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions 
                                                                        allowhide="False" allowrotate="False"></resolveoverlappingoptions></label></axisx>
                                                                    <axisy visibleinpanesserializable="-1" title-text="Disk Size (GB)" 
                                                                        title-visible="True"><label><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions 
                                                                        allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions 
                                                                        allowrotate="False"></resolveoverlappingoptions></label></axisy>
                                                                </cc1:XYDiagram>
                                                            </diagramserializable>
                                                                                <legend visible="False"></legend>
                                                                                <seriesserializable>
                                                                <cc1:Series ArgumentScaleType="Qualitative" LabelsVisibility="True" 
                                                                    Name="Disk Used" CrosshairEnabled="True" CrosshairLabelPattern="{V}" 
                                                                    SynchronizePointOptions="False">
                                                                    <viewserializable>
                                                                        <cc1:StackedBarSeriesView Color="Red" 
                                                                        Transparency="0"><fillstyle 
                                                                        fillmode="Solid"></fillstyle></cc1:StackedBarSeriesView></viewserializable>
                                                                    <labelserializable><cc1:StackedBarSeriesLabel ResolveOverlappingMode="HideOverlapped"><pointoptionsserializable><cc1:PointOptions><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" />
                                                                        <valuenumericoptions format="Number" />
                                                                        <valuenumericoptions format="Number" />
                                                                        <valuenumericoptions format="Number" />
                                                                        <valuenumericoptions format="Number" />
                                                                        </cc1:PointOptions></pointoptionsserializable></cc1:StackedBarSeriesLabel></labelserializable>
                                                                    <legendpointoptionsserializable><cc1:PointOptions Pattern="{V}"><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" />
                                                                        <valuenumericoptions format="Number" />
                                                                        <valuenumericoptions format="Number" />
                                                                        <valuenumericoptions format="Number" />
                                                                        <valuenumericoptions format="Number" />
                                                                        </cc1:PointOptions></legendpointoptionsserializable>
                                                                </cc1:Series>
                                                                <cc1:Series ArgumentScaleType="Qualitative" LabelsVisibility="True" 
                                                                    Name="Disk Free" CrosshairEnabled="True" CrosshairLabelPattern="{V}" 
                                                                    SynchronizePointOptions="False">
                                                                    <viewserializable>
                                                                        <cc1:StackedBarSeriesView Color="0, 128, 0" 
                                                                        Transparency="0"><fillstyle 
                                                                        fillmode="Solid"></fillstyle></cc1:StackedBarSeriesView></viewserializable>
                                                                    <labelserializable><cc1:StackedBarSeriesLabel><pointoptionsserializable><cc1:PointOptions><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" />
                                                                        <valuenumericoptions format="Number" />
                                                                        <valuenumericoptions format="Number" />
                                                                        <valuenumericoptions format="Number" />
                                                                        <valuenumericoptions format="Number" />
                                                                        </cc1:PointOptions></pointoptionsserializable></cc1:StackedBarSeriesLabel></labelserializable>
                                                                    <legendpointoptionsserializable><cc1:PointOptions Pattern="{V}"><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" />
                                                                        <valuenumericoptions format="Number" />
                                                                        <valuenumericoptions format="Number" />
                                                                        <valuenumericoptions format="Number" />
                                                                        <valuenumericoptions format="Number" />
                                                                        </cc1:PointOptions></legendpointoptionsserializable>
                                                                </cc1:Series>
                                                            </seriesserializable>
                                                                                <seriestemplate>
                                                                <viewserializable><cc1:StackedBarSeriesView></cc1:StackedBarSeriesView></viewserializable>
                                                            </seriestemplate>
                                                                            </dxchartsui:WebChartControl>
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="DiskSpaceRadioButtonList" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
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
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <dx:ASPxRoundPanel ID="cpuASPxRoundPanel" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                        CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="CPU" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                                                        Width="100%">
                                                        <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                                        <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                                        <HeaderStyle Height="23px">
                                                            <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                                            <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                                        </HeaderStyle>
                                                        <PanelCollection>
                                                            <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <dx:ASPxHyperLink ID="CpuHyperLink" runat="server" Text="More">
                                                                            </dx:ASPxHyperLink>
                                                                            <br />
                                                                            <br />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <dxchartsui:WebChartControl ID="cpuWebChartControl" runat="server" Height="300px"
                                                                                Width="600px">
                                                                                <diagramserializable>
                                                            <cc1:XYDiagram LabelsResolveOverlappingMinIndent="1" PaneLayoutDirection="Horizontal">
                                                                <AxisX Title-Text="Date" VisibleInPanesSerializable="-1" 
                                                                    GridSpacingAuto="True" MinorCount="5" DateTimeMeasureUnit="Hour"><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><label 
                                                                    enableantialiasing="False" Staggered="True"><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="Custom" /><datetimeoptions autoformat="False" format="Custom" /><datetimeoptions autoformat="False" format="Custom" /><datetimeoptions autoformat="False" format="Custom" /><datetimeoptions autoformat="False" format="Custom" /><datetimeoptions autoformat="False" format="Custom" /><datetimeoptions autoformat="False" format="Custom" /><datetimeoptions autoformat="False" format="Custom" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="Custom" /><datetimeoptions autoformat="False" format="Custom" /><datetimeoptions autoformat="False" format="Custom" /><datetimeoptions autoformat="False" format="Custom" /><datetimeoptions autoformat="False" format="Custom" /><datetimeoptions autoformat="False" format="Custom" /><datetimeoptions autoformat="False" format="Custom" /><datetimeoptions autoformat="False" format="Custom" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="Custom" /><datetimeoptions autoformat="False" format="Custom" /><datetimeoptions autoformat="False" format="Custom" /><datetimeoptions autoformat="False" format="Custom" /><datetimeoptions autoformat="False" format="Custom" /><datetimeoptions autoformat="False" format="Custom" /><datetimeoptions autoformat="False" format="Custom" /><datetimeoptions autoformat="False" format="Custom" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="Custom" /><datetimeoptions autoformat="False" format="Custom" /><datetimeoptions autoformat="False" format="Custom" /><datetimeoptions autoformat="False" format="Custom" /><datetimeoptions autoformat="False" format="Custom" /><datetimeoptions autoformat="False" format="Custom" /><datetimeoptions autoformat="False" format="Custom" /><datetimeoptions autoformat="False" format="Custom" /></label><Range 
                                                                    SideMarginsEnabled="True"></Range><datetimeoptions format="Custom" /><datetimeoptions format="Custom" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><datetimeoptions format="Custom" /><datetimeoptions format="Custom" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><datetimeoptions format="Custom" /><datetimeoptions format="Custom" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><datetimeoptions format="Custom" /><datetimeoptions format="Custom" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><datetimeoptions format="Custom" /><datetimeoptions format="Custom" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><datetimeoptions format="Custom" /><datetimeoptions format="Custom" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><datetimeoptions format="Custom" /><datetimeoptions format="Custom" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><datetimeoptions format="Custom" /><datetimeoptions format="Custom" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><datetimeoptions format="Custom" /><datetimeoptions format="Custom" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><datetimeoptions format="Custom" /><datetimeoptions format="Custom" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><datetimeoptions format="Custom" /><datetimeoptions format="Custom" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><datetimeoptions format="Custom" /><datetimeoptions format="Custom" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><datetimeoptions format="Custom" /><datetimeoptions format="Custom" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><datetimeoptions format="Custom" /><datetimeoptions format="Custom" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><datetimeoptions format="Custom" /><datetimeoptions format="Custom" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><datetimeoptions format="Custom" /><datetimeoptions format="Custom" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><datetimescaleoptions measureunit="Hour"></datetimescaleoptions></AxisX>
                                                                <AxisY Title-Text="Space (bytes)" Title-Visible="True" 
                                                                    VisibleInPanesSerializable="-1"><label><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions></label><Range AlwaysShowZeroLevel="False" SideMarginsEnabled="True"></Range><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /></AxisY>
                                                            </cc1:XYDiagram>
                                                        </diagramserializable>
                                                                                <fillstyle><OptionsSerializable><cc1:SolidFillOptions></cc1:SolidFillOptions></OptionsSerializable>
</fillstyle>
                                                                                <seriesserializable>
                    <cc1:Series Name="Series 1" ArgumentScaleType="DateTime">
                        <viewserializable><cc1:LineSeriesView MarkerVisibility="True"></cc1:LineSeriesView></viewserializable>
                        <labelserializable><cc1:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:PointSeriesLabel></labelserializable>
                        <legendpointoptionsserializable><cc1:PointOptions></cc1:PointOptions></legendpointoptionsserializable>
                    </cc1:Series>
                    <cc1:Series Name="Series 2" Visible="False">
                        <viewserializable><cc1:LineSeriesView></cc1:LineSeriesView></viewserializable>
                        <labelserializable><cc1:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:PointSeriesLabel></labelserializable>
                        <legendpointoptionsserializable><cc1:PointOptions></cc1:PointOptions></legendpointoptionsserializable>
                    </cc1:Series>
                </seriesserializable>
                                                                                <seriestemplate>
                    <viewserializable><cc1:LineSeriesView></cc1:LineSeriesView></viewserializable>
                    <labelserializable><cc1:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:PointSeriesLabel></labelserializable>
                    <legendpointoptionsserializable><cc1:PointOptions></cc1:PointOptions></legendpointoptionsserializable>
                </seriestemplate>
                                                                                <crosshairoptions><CommonLabelPositionSerializable><cc1:CrosshairMousePosition></cc1:CrosshairMousePosition></CommonLabelPositionSerializable>
</crosshairoptions>
                                                                                <tooltipoptions><ToolTipPositionSerializable><cc1:ToolTipMousePosition></cc1:ToolTipMousePosition></ToolTipPositionSerializable>
</tooltipoptions>
                                                                            </dxchartsui:WebChartControl>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </dx:PanelContent>
                                                        </PanelCollection>
                                                    </dx:ASPxRoundPanel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <dx:ASPxRoundPanel ID="memASPxRoundPanel" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                        CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Memory" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                                                        Width="100%">
                                                        <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                                        <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                                        <HeaderStyle Height="23px">
                                                            <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                                            <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                                        </HeaderStyle>
                                                        <PanelCollection>
                                                            <dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <dx:ASPxHyperLink ID="MemHyperLink" runat="server" Text="More" />
                                                                            <br />
                                                                            <br />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <dxchartsui:WebChartControl ID="memWebChartControl" runat="server" Height="300px"
                                                                                Width="600px">
                                                                                <diagramserializable>
                                                            <cc1:XYDiagram LabelsResolveOverlappingMinIndent="1" PaneLayoutDirection="Horizontal">
                                                                <AxisX Title-Text="Date" VisibleInPanesSerializable="-1" 
                                                                    GridSpacingAuto="True" MinorCount="5" DateTimeMeasureUnit="Hour"><label 
                                                                    enableantialiasing="False" Staggered="True"><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /></label><Range 
                                                                    SideMarginsEnabled="True"></Range><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" /><datetimescaleoptions measureunit="Hour"></datetimescaleoptions></AxisX>
                                                                <AxisY Title-Text="Space(bytes)" Title-Visible="True" VisibleInPanesSerializable="-1"><Range AlwaysShowZeroLevel="False" SideMarginsEnabled="True"></Range><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /></AxisY>
                                                            </cc1:XYDiagram>
                                                        </diagramserializable>
                                                                                <fillstyle><OptionsSerializable><cc1:SolidFillOptions></cc1:SolidFillOptions></OptionsSerializable>
</fillstyle>
                                                                                <seriesserializable>
                    <cc1:Series Name="Series 1" ArgumentScaleType="DateTime">
                        <viewserializable><cc1:LineSeriesView MarkerVisibility="True"></cc1:LineSeriesView></viewserializable>
                        <labelserializable><cc1:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:PointSeriesLabel></labelserializable>
                        <legendpointoptionsserializable><cc1:PointOptions></cc1:PointOptions></legendpointoptionsserializable>
                    </cc1:Series>
                    <cc1:Series Name="Series 2" Visible="False">
                        <viewserializable><cc1:LineSeriesView></cc1:LineSeriesView></viewserializable>
                        <labelserializable><cc1:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:PointSeriesLabel></labelserializable>
                        <legendpointoptionsserializable><cc1:PointOptions></cc1:PointOptions></legendpointoptionsserializable>
                    </cc1:Series>
                </seriesserializable>
                                                                                <seriestemplate>
                    <viewserializable><cc1:LineSeriesView></cc1:LineSeriesView></viewserializable>
                    <labelserializable><cc1:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:PointSeriesLabel></labelserializable>
                    <legendpointoptionsserializable><cc1:PointOptions></cc1:PointOptions></legendpointoptionsserializable>
                </seriestemplate>
                                                                                <crosshairoptions><CommonLabelPositionSerializable><cc1:CrosshairMousePosition></cc1:CrosshairMousePosition></CommonLabelPositionSerializable>
</crosshairoptions>
                                                                                <tooltipoptions><ToolTipPositionSerializable><cc1:ToolTipMousePosition></cc1:ToolTipMousePosition></ToolTipPositionSerializable>
</tooltipoptions>
                                                                            </dxchartsui:WebChartControl>
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
                                    <tr valign="top">
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <dx:ASPxRoundPanel ID="usersASPxRoundPanel" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                        CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Users" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                                                        Width="100%">
                                                        <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                                        <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                                        <HeaderStyle Height="23px">
                                                            <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                                            <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                                        </HeaderStyle>
                                                        <PanelCollection>
                                                            <dx:PanelContent ID="PanelContent5" runat="server" SupportsDisabledAttribute="True">
                                                                <dx:ASPxRadioButtonList ID="usersASPxRadioButtonList" runat="server" ValueType="System.String"
                                                                    AutoPostBack="True" CssClass="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                                    CssPostfix="Glass" RepeatDirection="Horizontal" SelectedIndex="0" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                                                                    OnSelectedIndexChanged="usersASPxRadioButtonList_SelectedIndexChanged" Visible="False">
                                                                    <Items>
                                                                        <dx:ListEditItem Selected="True" Value="hh" Text="Per Hour" />
                                                                        <dx:ListEditItem Value="dd" Text="Per Day" />
                                                                    </Items>
                                                                </dx:ASPxRadioButtonList>
                                                                <dxchartsui:WebChartControl ID="usersWebChartControl" runat="server" Height="300px"
                                                                    Width="600px">
                                                                    <diagramserializable>
                                                            <cc1:XYDiagram LabelsResolveOverlappingMinIndent="1" PaneLayoutDirection="Horizontal">
                                                                <AxisX Title-Text="Date" VisibleInPanesSerializable="-1" GridSpacingAuto="True" GridSpacing="0.5" MinorCount="5" DateTimeMeasureUnit="Hour"><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><label><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /></label><Range SideMarginsEnabled="True"></Range><datetimeoptions format="General" /><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="0.5" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="0.5" /><datetimeoptions format="General" /><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="0.5" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="0.5" /><datetimeoptions format="General" /><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="0.5" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="0.5" /><datetimeoptions format="General" /><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="0.5" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="0.5" /><datetimeoptions format="General" /><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="0.5" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="0.5" /><datetimeoptions format="General" /><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="0.5" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="0.5" /><datetimeoptions format="General" /><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="0.5" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="0.5" /><datetimeoptions format="General" /><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="0.5" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="0.5" /><datetimeoptions format="General" /><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="0.5" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="0.5" /><datetimeoptions format="General" /><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="0.5" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="0.5" /><datetimeoptions format="General" /><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="0.5" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="0.5" /><datetimeoptions format="General" /><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="0.5" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="0.5" /><datetimeoptions format="General" /><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="0.5" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="0.5" /><datetimeoptions format="General" /><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="0.5" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="0.5" /><datetimeoptions format="General" /><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="0.5" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="0.5" /><datetimeoptions format="General" /><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="0.5" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="0.5" /><datetimescaleoptions measureunit="Hour"></datetimescaleoptions></AxisX>
                                                                <AxisY Title-Text="Space(bytes)" Title-Visible="True" VisibleInPanesSerializable="-1"><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><label><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /></label><Range AlwaysShowZeroLevel="False" SideMarginsEnabled="True"></Range><numericoptions format="Number" precision="0" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><numericoptions format="Number" precision="0" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><numericoptions format="Number" precision="0" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><numericoptions format="Number" precision="0" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><numericoptions format="Number" precision="0" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><numericoptions format="Number" precision="0" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><numericoptions format="Number" precision="0" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><numericoptions format="Number" precision="0" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><numericoptions format="Number" precision="0" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><numericoptions format="Number" precision="0" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><numericoptions format="Number" precision="0" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><numericoptions format="Number" precision="0" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><numericoptions format="Number" precision="0" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><numericoptions format="Number" precision="0" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><numericoptions format="Number" precision="0" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><numericoptions format="Number" precision="0" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /></AxisY>
                                                            </cc1:XYDiagram>
                                                        </diagramserializable>
                                                                    <fillstyle><OptionsSerializable><cc1:SolidFillOptions></cc1:SolidFillOptions></OptionsSerializable>
</fillstyle>
                                                                    <seriesserializable>
                    <cc1:Series Name="Series 1" ArgumentScaleType="DateTime">
                        <viewserializable><cc1:LineSeriesView MarkerVisibility="True"></cc1:LineSeriesView></viewserializable>
                        <labelserializable><cc1:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:PointSeriesLabel></labelserializable>
                        <legendpointoptionsserializable><cc1:PointOptions></cc1:PointOptions></legendpointoptionsserializable>
                    </cc1:Series>
                    <cc1:Series Name="Series 2" ArgumentScaleType="DateTime">
                        <viewserializable><cc1:LineSeriesView></cc1:LineSeriesView></viewserializable>
                        <labelserializable><cc1:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:PointSeriesLabel></labelserializable>
                        <legendpointoptionsserializable><cc1:PointOptions></cc1:PointOptions></legendpointoptionsserializable>
                    </cc1:Series>
                </seriesserializable>
                                                                    <seriestemplate>
                    <viewserializable><cc1:LineSeriesView></cc1:LineSeriesView></viewserializable>
                    <labelserializable><cc1:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:PointSeriesLabel></labelserializable>
                    <legendpointoptionsserializable><cc1:PointOptions></cc1:PointOptions></legendpointoptionsserializable>
                </seriestemplate>
                                                                    <crosshairoptions><CommonLabelPositionSerializable><cc1:CrosshairMousePosition></cc1:CrosshairMousePosition></CommonLabelPositionSerializable>
</crosshairoptions>
                                                                    <tooltipoptions><ToolTipPositionSerializable><cc1:ToolTipMousePosition></cc1:ToolTipMousePosition></ToolTipPositionSerializable>
</tooltipoptions>
                                                                </dxchartsui:WebChartControl>
                                                            </dx:PanelContent>
                                                        </PanelCollection>
                                                    </dx:ASPxRoundPanel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        
                                    </tr>

                                </table>
                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
                     <dx:TabPage Text="Health Assessment">
                        <TabImage Url="~/images/icons/overallhealth.gif">
                        </TabImage>
                        <ContentCollection>
                            <dx:ContentControl ID="ContentControl2" runat="server" SupportsDisabledAttribute="True">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel20" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <dx:ASPxRoundPanel ID="ASPxRoundPanel13" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                        CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Status" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                                                        Width="100%">
                                                        <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                                        <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                                        <HeaderStyle Height="23px">
                                                            <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                                            <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                                        </HeaderStyle>
                                                        <PanelCollection>
                                                            <dx:PanelContent ID="PanelContent18" runat="server" SupportsDisabledAttribute="True">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td id="Td1">
                                                                            <dx:ASPxGridView ID="HealthAssessmentGrid11" runat="server" AutoGenerateColumns="False"
                                                                                CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                                                                                Cursor="pointer" KeyFieldName="ID" Theme="Office2003Blue" Width="100%" OnHtmlDataCellPrepared="HealthAssessmentGrid11_HtmlDataCellPrepared">
                                                                                <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" ColumnResizeMode="NextColumn" />
                                                                                <Columns>
                                                                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" Visible="false" VisibleIndex="0">
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn Caption="Test Name" FieldName="TestName" ShowInCustomizationForm="True"
                                                                                        VisibleIndex="1" Width="200px">
                                                                                        <Settings AllowDragDrop="True" AutoFilterCondition="Contains"></Settings>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn Caption="Details" FieldName="Details" ShowInCustomizationForm="True"
                                                                                        VisibleIndex="3">
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn Caption="Result" FieldName="Result" VisibleIndex="2" Width="80px">
                                                                                        <HeaderStyle CssClass="GridCssHeader1" />
                                                                                        <CellStyle CssClass="GridCss1">
                                                                                        </CellStyle>
                                                                                        <Settings AllowDragDrop="True" AutoFilterCondition="Contains"></Settings>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    
                                                                                    <dx:GridViewDataTextColumn Caption="Category" FieldName="Category" ShowInCustomizationForm="True"
                                                                                        VisibleIndex="0" Width="80px" Visible="true">
                                                                                        <Settings AllowDragDrop="True" AutoFilterCondition="Contains"></Settings>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn Caption="Last Update" FieldName="LastUpdate" ShowInCustomizationForm="True"
                                                                                        VisibleIndex="4" Width="200px">
                                                                                    </dx:GridViewDataTextColumn>
                                                                                </Columns>
                                                                                <Settings ShowGroupPanel="True" />
                                                                                <SettingsBehavior AllowDragDrop="False" AllowSelectByRowClick="True" AutoExpandAllGroups="True" />
                                                                                <SettingsPager Mode="ShowAllRecords" PageSize="12">
                                                                                </SettingsPager>
                                                                                <Images SpriteCssFilePath="~/App_Themes/Office2010Silver/{0}/sprite.css">
                                                                                    <LoadingPanelOnStatusBar Url="~/App_Themes/Office2010Silver/GridView/Loading.gif">
                                                                                    </LoadingPanelOnStatusBar>
                                                                                    <LoadingPanel Url="~/App_Themes/Office2010Silver/GridView/Loading.gif">
                                                                                    </LoadingPanel>
                                                                                </Images>
                                                                                <ImagesFilterControl>
                                                                                    <LoadingPanel Url="~/App_Themes/Office2010Silver/GridView/Loading.gif">
                                                                                    </LoadingPanel>
                                                                                </ImagesFilterControl>
                                                                                <Styles CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver">
                                                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px" CssClass="GridCssHeader">
                                                                                    </Header>
                                                                                    <AlternatingRow CssClass="GridAltRow">
                                                                                    </AlternatingRow>
                                                                                    <Cell CssClass="GridCss">
                                                                                    </Cell>
                                                                                    <LoadingPanel ImageSpacing="5px">
                                                                                    </LoadingPanel>
                                                                                </Styles>
                                                                                <StylesEditors ButtonEditCellSpacing="0">
                                                                                    <ProgressBar Height="21px">
                                                                                    </ProgressBar>
                                                                                </StylesEditors>
                                                                            </dx:ASPxGridView>
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
                                </table>
                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
                    <dx:TabPage Text="Windows Services">
                        <TabImage Url="~/images/icons/windows.gif">
                        </TabImage>
                        <ContentCollection>
                            <dx:ContentControl ID="ContentControl3" runat="server" SupportsDisabledAttribute="True">
                                <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td style="font-size: small; font-weight: 700">
                                                    <dx:ASPxPopupMenu ID="ServerTasksPopupMenu" runat="server" PopupAction="LeftMouseClick"
                                                        PopupHorizontalAlign="RightSides" PopupVerticalAlign="TopSides" ClientInstanceName="popupmenu">
                                                        <ClientSideEvents Init="InitPopupMenuHandler"></ClientSideEvents>
                                                        <Items>
                                                            <dx:MenuItem Text="Start" Name="ServerTaskStart">
                                                            </dx:MenuItem>
                                                            <dx:MenuItem Text="Stop" Name="ServerTaskStop">
                                                            </dx:MenuItem>
                                                            <dx:MenuItem Text="Restart" Name="ServerTaskRestart">
                                                            </dx:MenuItem>
                                                        </Items>
                                                    </dx:ASPxPopupMenu>
                                                </td>
                                            </tr>
                                                  
                                            <tr>
                                            <td style="font-size: small; font-weight: 700">
                                                    <asp:Label ID="Label2" runat="server" Text="Monitored Services"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td id="gridCell">
                                                
                                                    <dx:ASPxGridView ID="MonitoredServicesGrid" runat="server" AutoGenerateColumns="False"
                                                        CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                                                        Width="100%" Cursor="pointer" KeyFieldName="ID" Theme="Office2003Blue" OnPageSizeChanged="MonitoredServicesGrid_PageSizeChanged"
                                                        OnHtmlDataCellPrepared="MonitoredServicesGrid_HtmlDataCellPrepared">
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn Caption="Current State" VisibleIndex="2" FieldName="Result" Width="200px">
                                                                <Settings AutoFilterCondition="Contains" />
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <Settings AllowAutoFilter="False"  AllowDragDrop="True" />
                                                                <CellStyle CssClass="GridCss1" HorizontalAlign="Center">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            
                                                            <dx:GridViewDataTextColumn Caption="Service Name" VisibleIndex="1" FieldName="ServiceName">
                                                                <Settings AutoFilterCondition="Contains"  AllowDragDrop="True" />
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Start Mode" FieldName="StartMode"
                                                                VisibleIndex="3" Width="180px">
                                                                <Settings AutoFilterCondition="Contains"  AllowDragDrop="True" />
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss1">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            
                                                             <dx:GridViewDataTextColumn Caption="Last Checked" FieldName="DateStamp"
                                                                VisibleIndex="5" Width="180px">
                                                                <Settings AutoFilterCondition="Contains" AllowDragDrop="True"  />
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss1">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Type" FieldName="Type" 
																VisibleIndex="4" Visible="false">
															</dx:GridViewDataTextColumn>
                                                        </Columns>
														
														<Templates>
															<GroupRowContent>
																<%# Container.GroupText %>
															</GroupRowContent>
														</Templates>
														<Settings ShowFilterRow="True" />


													    <SettingsBehavior AllowDragDrop="False" AutoExpandAllGroups="True"/>
                                                        <SettingsPager PageSize="20"> 
                                                            <PageSizeItemSettings Visible="true" />
                                                        </SettingsPager>
                                                        
													    <Images SpriteCssFilePath="~/App_Themes/Office2010Silver/{0}/sprite.css">
                                                            <LoadingPanelOnStatusBar Url="~/App_Themes/Office2010Silver/GridView/Loading.gif">
                                                            </LoadingPanelOnStatusBar>
                                                            <LoadingPanel Url="~/App_Themes/Office2010Silver/GridView/Loading.gif">
                                                            </LoadingPanel>
                                                        </Images>
                                                        <ImagesFilterControl>
                                                            <LoadingPanel Url="~/App_Themes/Office2010Silver/GridView/Loading.gif">
                                                            </LoadingPanel>
                                                        </ImagesFilterControl>
                                                        <Styles CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver">
                                                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                            </Header>
                                                            <AlternatingRow CssClass="GridAltRow">
                                                            </AlternatingRow>
                                                            <LoadingPanel ImageSpacing="5px">
                                                            </LoadingPanel>
															<GroupRow Font-Bold="true" />
                                                        </Styles>

                                                    </dx:ASPxGridView>
                                                </td>
                                            </tr>
                                                    <tr>
                                                <td style="font-size: small; font-weight: 700">
                                                    &nbsp;
           
          
                                                    </td>
                                                </tr>
                                                <tr>
                                                <td style="font-size: small; font-weight: 700">
                                                        <asp:Label ID="Label1" runat="server" Text="All Windows Services"></asp:Label>
                                                    </td>
                                                </tr>
                                                                                        <tr>
                                                <td id="Td2">
                                                  

                                                    <dx:ASPxGridView ID="UnMonitoredServicesGrid" runat="server" AutoGenerateColumns="False"
                                                        CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                                                        Width="100%" Cursor="pointer" KeyFieldName="ID" Theme="Office2003Blue" OnPageSizeChanged="MonitoredServicesGrid_PageSizeChanged"
                                                       >
                                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" ColumnResizeMode="NextColumn">
                                                        </SettingsBehavior>
                                                        <Columns>
                                                          
                                                      
                                                            <dx:GridViewDataTextColumn Caption="Service Name" VisibleIndex="0" FieldName="ServiceName">
                                                                <Settings AutoFilterCondition="Contains"  AllowDragDrop="True" />
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>    
                                                                 <dx:GridViewDataTextColumn Caption="Startup Mode" VisibleIndex="2" FieldName="StartMode" Width="180px">
                                                                <Settings AutoFilterCondition="Contains"  AllowDragDrop="True" />
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                                 <dx:GridViewDataTextColumn Caption="Current State" VisibleIndex="1" FieldName="Result" Width="200px">
                                                                <Settings AutoFilterCondition="Contains"  AllowDragDrop="True" />
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <Settings AllowAutoFilter="False" />
                                                                <CellStyle CssClass="GridCss1" HorizontalAlign="Center">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Last Checked" FieldName="DateStamp"
                                                                VisibleIndex="3" Width="180px">
                                                                <Settings AutoFilterCondition="Contains" AllowDragDrop="True"  />
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss1">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                        </Columns>
                                                        <SettingsBehavior AllowDragDrop="False" AutoExpandAllGroups="True" AllowSelectByRowClick="True" />
                                                        <SettingsPager PageSize="20">
                                                            <PageSizeItemSettings Visible="true" />
                                                        </SettingsPager>
                                                        <Settings ShowGroupPanel="True" ShowFilterRow="True" />
                                                        <Images SpriteCssFilePath="~/App_Themes/Office2010Silver/{0}/sprite.css">
                                                            <LoadingPanelOnStatusBar Url="~/App_Themes/Office2010Silver/GridView/Loading.gif">
                                                            </LoadingPanelOnStatusBar>
                                                            <LoadingPanel Url="~/App_Themes/Office2010Silver/GridView/Loading.gif">
                                                            </LoadingPanel>
                                                        </Images>
                                                        <ImagesFilterControl>
                                                            <LoadingPanel Url="~/App_Themes/Office2010Silver/GridView/Loading.gif">
                                                            </LoadingPanel>
                                                        </ImagesFilterControl>
                                                        <Styles CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver">
                                                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                            </Header>
                                                            <AlternatingRow CssClass="GridAltRow">
                                                            </AlternatingRow>
                                                            <LoadingPanel ImageSpacing="5px">
                                                            </LoadingPanel>
                                                        </Styles>
                                                        <StylesEditors ButtonEditCellSpacing="0">
                                                            <ProgressBar Height="21px">
                                                            </ProgressBar>
                                                        </StylesEditors>
                                                    </dx:ASPxGridView>
                                                </td>
                                            </tr>
                                        </table>
                                        <dx:ASPxPopupControl ID="msgPopupControl1" runat="server" HeaderText="VitalSigns"
                                            Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                            Width="300px" Height="120px" Theme="Glass">
                                            <ContentCollection>
                                                <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server" SupportsDisabledAttribute="True">
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
                                                                <dx:ASPxButton ID="YesButton" runat="server" AutoPostBack="true" Text="Yes" Theme="Office2010Blue"
                                                                    Width="70px">
                                                                </dx:ASPxButton>
                                                            </td>
                                                            <td align="left">
                                                                <dx:ASPxButton ID="NOButton" runat="server" Text="No" AutoPostBack="true" Theme="Office2010Blue"
                                                                    Width="70px">
                                                                </dx:ASPxButton>
                                                            </td>
                                                            <td align="left">
                                                                <dx:ASPxButton ID="CancelButton1" runat="server" AutoPostBack="true" Text="Cancel"
                                                                    Theme="Office2010Blue" Visible="False" Width="70px">
                                                                </dx:ASPxButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </dx:PopupControlContentControl>
                                            </ContentCollection>
                                        </dx:ASPxPopupControl>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
					<dx:TabPage Text="Windows Events Log History">
						<TabImage Url="~/images/icons/windows.gif">
						</TabImage>
						<ContentCollection>
							<dx:ContentControl ID="ContentControl8" runat="server" SupportsDisabledAttribute="True">
								<asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<table width="100%">
											<tr>
												<td>
													<div class="header" id="Div1" runat="server">
														Windows Event History</div>
												</td>
												<td>
													&nbsp;
												</td>
												<td align="right">
													<table>
														<tr>
															<td>
																<dx:ASPxMenu ID="ASPxMenu2" runat="server" EnableTheming="True" HorizontalAlign="Right" Visible="false"
																	OnItemClick="EventSettings_ItemClick" ShowAsToolbar="True" Theme="Moderno">
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
										<table width="100%">
											<tr>
												<td>
													<table>
														<tr>
															<td>
																<dx:ASPxLabel ID="ASPxLabel5" runat="server" CssClass="lblsmallFont" Text="Month/Year:">
																</dx:ASPxLabel>
															</td>
															<td>
																<input id="startDate" runat="server" class="date-picker" type="text" />
															</td>
															<td>
																&nbsp;
															</td>
															<td>
																<dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Filter by Event Log:" CssClass="lblsmallFont">
																</dx:ASPxLabel>
															</td>
															<td>
																<dx:ASPxComboBox ID="EventTypeComboBox" runat="server" ValueType="System.String"
																	AutoPostBack="True" OnSelectedIndexChanged="EventTypeComboBox_SelectedIndexChanged">
																</dx:ASPxComboBox>
															</td>
															<td>
															</td>
															<%-- <td style="width:450px"></td>
                       
                       
                    
                     <td valign="top" align="right">
                            <dx:ASPxMenu ID="ASPxMenu1" runat="server" EnableTheming="True" 
                    HorizontalAlign="Right" onitemclick="AlertSettings_ItemClick" ShowAsToolbar="True" 
                    Theme="Moderno">
                    <Items>
                        <dx:MenuItem Name="MainItem">
                            <Items>
                                <dx:MenuItem Name="AlertSettingsItem" Text="Alert Settings">
                                </dx:MenuItem>
                                
                            </Items>
                            <Image Url="~/images/icons/Gear.png">
                            </Image>
                        </dx:MenuItem>
                    </Items>
                </dx:ASPxMenu>
                        </td>--%>
														</tr>
													</table>
												</td>
											</tr>
											<tr>
												<td>
													<asp:UpdatePanel ID="UpdatePanel13" runat="server" UpdateMode="Conditional">
														<ContentTemplate>
															<dx:ASPxGridView ID="EventsHistory" runat="server" AutoGenerateColumns="False" Theme="Office2003Blue"
																Width="100%" OnHtmlDataCellPrepared="EventsHistory_HtmlDataCellPrepared" OnPageSizeChanged="EventsHistoryGridView_PageSizeChanged"
																ClientInstanceName="EventsHistory">
																<TotalSummary>
																	<dx:ASPxSummaryItem FieldName="MailDeliv" ShowInColumn="Total Mail Delivered" SummaryType="Sum"
																		DisplayFormat="{0:n0}" />
																	<dx:ASPxSummaryItem DisplayFormat="{0:n0}" FieldName="DownTime" ShowInColumn="Down Time (min)"
																		SummaryType="Sum" />
																	<dx:ASPxSummaryItem DisplayFormat="{0:n0}" FieldName="WebOpenDoc" ShowInColumn="Web Databases Opened"
																		SummaryType="Sum" />
																	<dx:ASPxSummaryItem DisplayFormat="{0:n0}" FieldName="WebCreateDoc" ShowInColumn="Web Documents Created"
																		ShowInGroupFooterColumn="Web Documents Created" SummaryType="Sum" />
																	<dx:ASPxSummaryItem DisplayFormat="{0:n0}" FieldName="WebOpenView" ShowInColumn="Web Views Opened"
																		SummaryType="Sum" />
																	<dx:ASPxSummaryItem DisplayFormat="{0:n0}" FieldName="WebComTotal" ShowInColumn="Web Commands Total"
																		SummaryType="Sum" />
																</TotalSummary>
																<Columns>
																	<dx:GridViewDataTextColumn Caption="Alias Name" FieldName="AliasName" VisibleIndex="0"
																		Width="100px">
																		<Settings AllowHeaderFilter="True" AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"
																			HeaderFilterMode="CheckedList" />
																		<CellStyle CssClass="GridCss">
																		</CellStyle>
																	</dx:GridViewDataTextColumn>
																	<dx:GridViewDataTextColumn Caption="Server Name" FieldName="DeviceName" VisibleIndex="0"
																		Width="200px">
																		<Settings AllowHeaderFilter="True" AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"
																			HeaderFilterMode="CheckedList" />
																		<CellStyle CssClass="GridCss">
																		</CellStyle>
																	</dx:GridViewDataTextColumn>
																	<dx:GridViewDataTextColumn Caption="DeviceName" FieldName="DeviceName" Visible="false"
																		VisibleIndex="6">
																		<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																		<CellStyle CssClass="GridCss">
																		</CellStyle>
																	</dx:GridViewDataTextColumn>
																	<dx:GridViewDataTextColumn Caption="DeviceType" Width="80px" FieldName="DeviceType"
																		VisibleIndex="7">
																		<Settings AllowHeaderFilter="True" AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"
																			HeaderFilterMode="CheckedList" />
																		<CellStyle CssClass="GridCss">
																		</CellStyle>
																	</dx:GridViewDataTextColumn>
																	<dx:GridViewDataTextColumn Caption="Log Name" FieldName="LogName" Width="100px" VisibleIndex="1">
																		<Settings AllowHeaderFilter="True" AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"
																			HeaderFilterMode="CheckedList" />
																		<CellStyle CssClass="GridCss">
																		</CellStyle>
																	</dx:GridViewDataTextColumn>
																	<dx:GridViewDataTextColumn Caption="Event Time" FieldName="EventTime" Width="170px"
																		VisibleIndex="5">
																		<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																		<CellStyle CssClass="GridCss" Wrap="False">
																		</CellStyle>
																	</dx:GridViewDataTextColumn>
																	<dx:GridViewDataTextColumn Caption="Source" Width="80px" FieldName="Source" VisibleIndex="4">
																		<Settings AllowHeaderFilter="True" AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"
																			HeaderFilterMode="CheckedList" />
																		<CellStyle CssClass="GridCss" Wrap="False">
																		</CellStyle>
																	</dx:GridViewDataTextColumn>
																	<dx:GridViewDataTextColumn Caption="Message" FieldName="MessageDetails" Width="600px"
																		VisibleIndex="3">
																		<CellStyle CssClass="GridCss">
																		</CellStyle>
																	</dx:GridViewDataTextColumn>
																	<dx:GridViewDataTextColumn Caption="Entry Type" FieldName="EntryType" Width="85px"
																		VisibleIndex="2">
																		<Settings AllowHeaderFilter="True" AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"
																			HeaderFilterMode="CheckedList" />
																		<CellStyle CssClass="GridCss">
																		</CellStyle>
																	</dx:GridViewDataTextColumn>
																</Columns>
																<SettingsBehavior ColumnResizeMode="NextColumn" />
																<SettingsPager AlwaysShowPager="True" PageSize="20">
																	<PageSizeItemSettings Visible="True">
																	</PageSizeItemSettings>
																</SettingsPager>
																<Settings ShowFilterRow="True" ShowFooter="True" ShowGroupPanel="True" />
																<Styles>
																	<Header CssClass="GridCssHeader">
																	</Header>
																	<AlternatingRow CssClass="GridAltRow" Enabled="True">
																	</AlternatingRow>
																	<Footer CssClass="GridCss2">
																	</Footer>
																</Styles>
															</dx:ASPxGridView>
														</ContentTemplate>
														<Triggers>
															<asp:AsyncPostBackTrigger ControlID="startDate" />
															<asp:AsyncPostBackTrigger ControlID="EventTypeComboBox" />
														</Triggers>
													</asp:UpdatePanel>
												</td>
											</tr>
											<tr>
												<td>
													<dx:ASPxGridViewExporter ID="ServerGridViewExporter" runat="server" GridViewID="EventsHistory">
													</dx:ASPxGridViewExporter>
												</td>
											</tr>
										</table>
									</ContentTemplate>
								</asp:UpdatePanel>
							</dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
                    <dx:TabPage Text="Maintenance">
                        <TabImage Url="~/images/icons/wrench.png">
                        </TabImage>
                        <ContentCollection>
                            <dx:ContentControl ID="ContentControl4" runat="server" SupportsDisabledAttribute="True">
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td colspan="8">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" Text="From Date:"
                                                                    Wrap="False">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtfromdate" runat="server" Font-Size="12px"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="calextender" runat="server" Format="MM/dd/yyyy" TargetControlID="txtfromdate">
                                                                </cc1:CalendarExtender>
                                                            </td>
                                                            <td valign="top">
                                                                <asp:RequiredFieldValidator ID="rfvtxtfromdate" runat="server" ControlToValidate="txtfromdate"
                                                                    ErrorMessage="Enter From Date" Font-Size="12px" ForeColor="#FF3300"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td valign="top">
                                                                &nbsp;
                                                            </td>
                                                            <td valign="top">
                                                                <dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" Text="From Time:"
                                                                    Wrap="False">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td valign="top">
                                                                <dx:ASPxTimeEdit ID="ASPxTimeEdit1" runat="server">
                                                                </dx:ASPxTimeEdit>
                                                            </td>
                                                            <td valign="top">
                                                                &nbsp;
                                                            </td>
                                                            <td valign="top">
                                                                <dx:ASPxButton ID="ClearButton" runat="server" OnClick="ClearButton_Click" Text="Clear"
                                                                    Theme="Office2010Blue" Width="80px">
                                                                </dx:ASPxButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="lblsmallFont" Text="To Date:"
                                                                    Wrap="False">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td valign="top">
                                                                <asp:TextBox ID="txttodate" runat="server" Font-Size="12px"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="MM/dd/yyyy" TargetControlID="txttodate">
                                                                </cc1:CalendarExtender>
                                                            </td>
                                                            <td valign="top">
                                                                <asp:RequiredFieldValidator ID="RFvtxttodate" runat="server" ControlToValidate="txttodate"
                                                                    ErrorMessage="Enter To Date" Font-Size="12px" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td valign="top">
                                                                &nbsp;
                                                            </td>
                                                            <td valign="top">
                                                                <dx:ASPxLabel ID="ASPxLabel4" runat="server" CssClass="lblsmallFont" Text="To Time:"
                                                                    Wrap="False">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td valign="top">
                                                                <dx:ASPxTimeEdit ID="ASPxTimeEdit2" runat="server">
                                                                </dx:ASPxTimeEdit>
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <dx:ASPxButton ID="btnsearch" runat="server" OnClick="btnsearch_Click" Text="Search"
                                                                    Theme="Office2010Blue" Width="80px">
                                                                </dx:ASPxButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <dx:ASPxGridView ID="maintenancegrid" runat="server" AutoGenerateColumns="False"
                                                        CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                                                        Width="100%" KeyFieldName="TypeandName" Theme="Office2003Blue">
                                                        <SettingsBehavior ColumnResizeMode="NextColumn"></SettingsBehavior>
                                                        <SettingsPager PageSize="50">
                                                            <PageSizeItemSettings Visible="True">
                                                            </PageSizeItemSettings>
                                                        </SettingsPager>
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn Caption="Server Name" VisibleIndex="0" FieldName="servername">
                                                                <Settings AllowDragDrop="True" AutoFilterCondition="Contains" />
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
                                                            <dx:GridViewDataTextColumn Caption="Server Type" FieldName="ServerType" Visible="True"
                                                                VisibleIndex="1">
                                                                <Settings AllowAutoFilter="True" AllowDragDrop="True" AutoFilterCondition="Contains" />
                                                                <Settings AllowDragDrop="True" AllowAutoFilter="True" AutoFilterCondition="Contains">
                                                                </Settings>
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Start Date" VisibleIndex="2" FieldName="StartDate">
                                                                <Settings AllowAutoFilter="False" AllowDragDrop="True" AutoFilterCondition="Contains" />
                                                                <Settings AllowDragDrop="True" AllowAutoFilter="False" AutoFilterCondition="Contains">
                                                                </Settings>
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Start Time" VisibleIndex="3" FieldName="StartTime">
                                                                <Settings AllowAutoFilter="False" AllowDragDrop="True" AutoFilterCondition="Contains" />
                                                                <Settings AllowDragDrop="True" AllowAutoFilter="False" AutoFilterCondition="Contains">
                                                                </Settings>
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Duration" VisibleIndex="4" FieldName="Duration">
                                                                <Settings AllowAutoFilter="False" AllowDragDrop="True" />
                                                                <Settings AllowDragDrop="True" AllowAutoFilter="False"></Settings>
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="End Date" VisibleIndex="5" FieldName="EndDate">
                                                                <Settings AllowAutoFilter="False" AllowDragDrop="True" />
                                                                <Settings AllowDragDrop="True" AllowAutoFilter="False"></Settings>
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Maintenance Type" VisibleIndex="6" FieldName="MaintType">
                                                                <Settings AllowAutoFilter="False" AllowDragDrop="True" />
                                                                <Settings AllowDragDrop="True" AllowAutoFilter="False"></Settings>
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Maintenance Days List" VisibleIndex="7" FieldName="MaintDaysList">
                                                                <Settings AllowAutoFilter="False" AllowDragDrop="True" />
                                                                <Settings AllowDragDrop="True" AllowAutoFilter="False"></Settings>
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                        </Columns>
                                                        <SettingsBehavior AllowDragDrop="False" AutoExpandAllGroups="True" AllowSelectByRowClick="True"
                                                            ProcessSelectionChangedOnServer="True" />
                                                        <SettingsPager PageSize="50" SEOFriendly="Enabled">
                                                            <PageSizeItemSettings Visible="true" />
                                                        </SettingsPager>
                                                        <Settings ShowGroupPanel="True" ShowFilterRow="True" />
                                                        <Settings ShowFilterRow="True" ShowGroupPanel="True"></Settings>
                                                        <Images SpriteCssFilePath="~/App_Themes/Office2010Silver/{0}/sprite.css">
                                                            <LoadingPanelOnStatusBar Url="~/App_Themes/Office2010Silver/GridView/Loading.gif">
                                                            </LoadingPanelOnStatusBar>
                                                            <LoadingPanel Url="~/App_Themes/Office2010Silver/GridView/Loading.gif">
                                                            </LoadingPanel>
                                                        </Images>
                                                        <ImagesFilterControl>
                                                            <LoadingPanel Url="~/App_Themes/Office2010Silver/GridView/Loading.gif">
                                                            </LoadingPanel>
                                                        </ImagesFilterControl>
                                                        <Styles CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver">
                                                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                            </Header>
                                                            <GroupRow Font-Bold="True" Font-Italic="False">
                                                            </GroupRow>
                                                            <GroupFooter Font-Bold="True">
                                                            </GroupFooter>
                                                            <GroupPanel Font-Bold="False">
                                                            </GroupPanel>
                                                            <LoadingPanel ImageSpacing="5px">
                                                            </LoadingPanel>
                                                            <AlternatingRow CssClass="GridAltRow" Enabled="True">
                                                            </AlternatingRow>
                                                        </Styles>
                                                        <StylesEditors ButtonEditCellSpacing="0">
                                                            <ProgressBar Height="21px">
                                                            </ProgressBar>
                                                        </StylesEditors>
                                                    </dx:ASPxGridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dx:ASPxPopupControl ID="MsgPopupControl" runat="server" HeaderText="Information"
                                                        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                                        Theme="Glass">
                                                        <ContentCollection>
                                                            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                                                                <table class="style1">
                                                                    <tr>
                                                                        <td>
                                                                            <dx:ASPxLabel ID="ErrmsgLabel" runat="server">
                                                                            </dx:ASPxLabel>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <dx:ASPxButton ID="OKButton" runat="server" OnClick="OKButton_Click" Text="OK" Theme="Office2010Blue">
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
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
                    <dx:TabPage Text="Alerts History">
                        <TabImage Url="~/images/icons/sounds.gif">
                        </TabImage>
                        <ContentCollection>
                            <dx:ContentControl ID="ContentControl5" runat="server" SupportsDisabledAttribute="True">
                                <table class="style1" width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <div id="infoDiv" class="info">
                                                                    The list of configured alerts that apply to the server are listed below. The list
                                                                    includes the last 7 days worth of information.
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxGridView ID="AlertGridView" runat="server" AutoGenerateColumns="False" EnableTheming="True"
                                                                    Theme="Office2003Blue" Width="100%">
                                                                    <Columns>
                                                                        <dx:GridViewCommandColumn ShowInCustomizationForm="True" Visible="False" VisibleIndex="0">
                                                                            <ClearFilterButton Visible="True">
                                                                            </ClearFilterButton>
                                                                        </dx:GridViewCommandColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Device Name" FieldName="DeviceName" ShowInCustomizationForm="True"
                                                                            VisibleIndex="1">
                                                                            <Settings AutoFilterCondition="Contains" />
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
                                                                        <dx:GridViewDataTextColumn Caption="Device Type" FieldName="DeviceType" ShowInCustomizationForm="True"
                                                                            VisibleIndex="2">
                                                                            <EditCellStyle CssClass="GridCss">
                                                                            </EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                                            </EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                                            <CellStyle CssClass="GridCss">
                                                                            </CellStyle>
                                                                            <Settings AutoFilterCondition="Contains" />
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Alert Type" FieldName="AlertType" ShowInCustomizationForm="True"
                                                                            VisibleIndex="3">
                                                                            <EditCellStyle CssClass="GridCss">
                                                                            </EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                                            </EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                                            <CellStyle CssClass="GridCss">
                                                                            </CellStyle>
                                                                            <Settings AutoFilterCondition="Contains" />
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Date/Time of Alert" FieldName="DateTimeOfAlert"
                                                                            ShowInCustomizationForm="True" VisibleIndex="4">
                                                                            <EditCellStyle CssClass="GridCss">
                                                                            </EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                                            </EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                                            <CellStyle CssClass="GridCss1">
                                                                            </CellStyle>
                                                                            <Settings AutoFilterCondition="Contains" />
                                                                        </dx:GridViewDataTextColumn>
                                                                        <%-- <dx:GridViewDataTextColumn Caption="Date/Time Sent" FieldName="DateTimeSent" 
                                                                        ShowInCustomizationForm="True" VisibleIndex="5">
                                                                         <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                    <CellStyle CssClass="GridCss1">
                                                    </CellStyle>
                                                    <Settings AutoFilterCondition="Contains" />
                                                                    </dx:GridViewDataTextColumn>--%>
                                                                        <dx:GridViewDataTextColumn Caption="Date/Time Alert Cleared" FieldName="DateTimeAlertCleared"
                                                                            ShowInCustomizationForm="True" VisibleIndex="6">
                                                                            <EditCellStyle CssClass="GridCss">
                                                                            </EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                                            </EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                                            <CellStyle CssClass="GridCss1">
                                                                            </CellStyle>
                                                                            <Settings AutoFilterCondition="Contains" />
                                                                        </dx:GridViewDataTextColumn>
                                                                    </Columns>
                                                                    <SettingsBehavior ColumnResizeMode="NextColumn" />
                                                                    <SettingsBehavior ColumnResizeMode="NextColumn"></SettingsBehavior>
                                                                    <SettingsPager PageSize="50">
                                                                        <PageSizeItemSettings Visible="True">
                                                                        </PageSizeItemSettings>
                                                                    </SettingsPager>
                                                                    <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                                                                    <Settings ShowFilterRow="True" ShowGroupPanel="True"></Settings>
                                                                    <Styles>
                                                                        <AlternatingRow CssClass="GridAltRow">
                                                                        </AlternatingRow>
                                                                    </Styles>
                                                                </dx:ASPxGridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
                    <dx:TabPage Text="Outages">
                        <TabImage Url="../images/icons/exclamation.png">
                        </TabImage>
                        <ContentCollection>
                            <dx:ContentControl ID="ContentControl6" runat="server" SupportsDisabledAttribute="True">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table class="tableWidth100Percent">
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dx:ASPxGridView ID="OutageGridView" runat="server" AutoGenerateColumns="False" EnableTheming="True"
                                                        Theme="Office2003Blue" Width="100%">
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn Caption="Server Name" FieldName="ServerName" ShowInCustomizationForm="True"
                                                                VisibleIndex="1" Width="300px">
                                                                <Settings AllowAutoFilter="False" AutoFilterCondition="Contains" />
                                                                <Settings AllowAutoFilter="False" AutoFilterCondition="Contains" />
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader">
                                                                    <Paddings Padding="5px" />
                                                                    <Paddings Padding="5px" />
                                                                </HeaderStyle>
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Date/Time Down" FieldName="DateTimeDown" ShowInCustomizationForm="True"
                                                                VisibleIndex="2">
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss1">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Date/Time Up" FieldName="DateTimeUp" ShowInCustomizationForm="True"
                                                                VisibleIndex="3">
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss1">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Duration (mins)" FieldName="Description" ShowInCustomizationForm="True"
                                                                VisibleIndex="4">
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss2">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                            </dx:GridViewDataTextColumn>
                                                        </Columns>
                                                        <SettingsBehavior ColumnResizeMode="NextColumn" />
                                                        <SettingsPager PageSize="50">
                                                            <PageSizeItemSettings Visible="True">
                                                            </PageSizeItemSettings>
                                                        </SettingsPager>
                                                        <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                                                        <SettingsBehavior ColumnResizeMode="NextColumn" />
                                                        <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                                                        <Styles>
                                                            <AlternatingRow CssClass="GridAltRow">
                                                            </AlternatingRow>
                                                        </Styles>
                                                    </dx:ASPxGridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
					<dx:TabPage Text="SharePoint Health">
						<TabImage Url="~/images/icons/overallhealth.gif">
						</TabImage>
						<ContentCollection>
							<dx:ContentControl ID="ContentControl7" runat="server" SupportsDisabledAttribute="True">
								<table width="100%">
									<tr>
										<td width="50%" valign="top" align="center">
											<asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
												<ContentTemplate>
													<dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
														CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Configuration"
														SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="95%" Height="199px">
														<ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
														<ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
														<HeaderStyle Height="23px">
															<Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
															<Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
														</HeaderStyle>
														<PanelCollection>
															<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
																<table width="100%">
																	<tr>
																		<td id="Td3">
																			<asp:Label runat="server" ID="lbl1" Text="Virtual Server Manufacturer:" Visible="false"></asp:Label>
																		</td>
																		<td>
																			<asp:Label runat="server" ID="lblvrtlsvr" Visible="false"></asp:Label>
																		</td>
																	</tr>
																	<tr>
																		<td id="Td5">
																			<asp:Label runat="server" ID="Label4" Text="Model:" Visible="false"></asp:Label>
																		</td>
																		<td>
																			<asp:Label runat="server" ID="lblmodel" Visible="false"></asp:Label>
																		</td>
																	</tr>
																	<tr>
																		<td id="Td6">
																			<asp:Label runat="server" ID="Label5" Text="Physical Processors:" Visible="false"></asp:Label>
																		</td>
																		<td>
																			<asp:Label runat="server" ID="Labelprocessrs" Visible="false"></asp:Label>
																		</td>
																	</tr>
																	<tr>
																		<td id="Td7">
																			<asp:Label runat="server" ID="Label6" Text="Logical Processors:" Visible="false"></asp:Label>
																		</td>
																		<td>
																			<asp:Label runat="server" ID="Labellprcessrs" Visible="false"></asp:Label>
																		</td>
																	</tr>
																	<tr>
																		<td id="Td8">
																			<asp:Label runat="server" ID="Label8" Text="Total Physical Memory:" Visible="false"></asp:Label>
																		</td>
																		<td>
																			<asp:Label runat="server" ID="Labelttlphmry" Visible="false"></asp:Label>
																		</td>
																	</tr>
																	<tr>
																		<td id="Td9">
																			<asp:Label runat="server" ID="Label10" Text="Total Logical Memory:" Visible="false"></asp:Label>
																		</td>
																		<td>
																			<asp:Label runat="server" ID="Labelttlmry" Visible="false"></asp:Label>
																		</td>
																	</tr>
																	<tr>
																		<td id="Td10">
																			<asp:Label runat="server" ID="Label7" Text="IIS Version:"></asp:Label>
																		</td>
																		<td>
																			<asp:Label runat="server" ID="Labeliisvrsn"></asp:Label>
																		</td>
																	</tr>
																	<tr>
																		<td id="Td11">
																			<asp:Label runat="server" ID="Label11" Text="IIS Service State:"></asp:Label>
																		</td>
																		<td>
																			<asp:Label runat="server" ID="Labeliissrvstate"></asp:Label>
																		</td>
																	</tr>
																	<tr>
																		<td id="Td12">
																			<asp:Label runat="server" ID="Label13" Text="ASP.NET Version:"></asp:Label>
																		</td>
																		<td>
																			<asp:Label runat="server" ID="lblaspntvsn"></asp:Label>
																		</td>
																	</tr>
																</table>
															</dx:PanelContent>
														</PanelCollection>
													</dx:ASPxRoundPanel>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
										<td width="50%" valign="top" align="center">
											<asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional">
												<ContentTemplate>
													<dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
														CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Network/IIS/Web Services"
														SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%" Height="199px">
														<ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
														<ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
														<HeaderStyle Height="23px">
															<Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
															<Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
														</HeaderStyle>
														<PanelCollection>
															<dx:PanelContent ID="PanelContent6" runat="server" SupportsDisabledAttribute="True">
																<table width="95%" height="190">
																	<tr>
																		<td>
																			<table align="center">
																				<tr>
																					<td>
																						<table cellspacing="1px">
																							<tr>
																								<td align="center">
																									<asp:Label runat="server" ID="lblnw" Text="Network" Font-Bold="true" align="centre"></asp:Label>
																								</td>
																							</tr>
																							<tr>
																								<td>
																									<asp:Label runat="server" ID="lblstatus" Width="100%" Height="500%" CssClass="StatusLabel"></asp:Label>
																								</td>
																							</tr>
																							<tr>
																								<td>
																									<asp:Label runat="server" ID="lblnwmsg" Width="150%"></asp:Label>
																								</td>
																							</tr>
																							<tr>
																								<td>
																									<asp:Label runat="server" ID="lblbytesrecvd" Text="Bytes Received/Sec:"></asp:Label>
																								</td>
																								<td>
																									<asp:Label runat="server" ID="lblnwbyr"></asp:Label>
																								</td>
																							</tr>
																							<tr>
																								<td>
																									<asp:Label runat="server" ID="Label3" Text="Bytes Sent/Sec:"></asp:Label>
																								</td>
																								<td>
																									<asp:Label runat="server" ID="lblnbysnt"></asp:Label>
																								</td>
																							</tr>
																						</table>
																					</td>
																				</tr>
																			</table>
																			<td align="center" >
																				<table>
																					<tr>
																						<td  style="text-align:center"; >
																							<asp:Label runat="server" ID="lbliis" Text="IIS" Font-Bold="true" ></asp:Label>
																							<table>
																								<tr>
																									<td>
																										<asp:Label runat="server" ID="lbliistatus" CssClass="StatusLabel" Width="100%" Height="500%"></asp:Label>
																									</td>
																								</tr>
																								<tr>
																									<td>
																										<asp:Label runat="server" ID="lbliimsg" Width="150%"></asp:Label>
																									</td>
																								</tr>
																								<tr>
																									<td>
																										<asp:Label runat="server" ID="Label9" Text="App Requests:"></asp:Label>
																									</td>
																									<td>
																										<asp:Label runat="server" ID="Labelappre"></asp:Label>
																									</td>
																								</tr>
																								<tr>
																									<td>
																										<asp:Label runat="server" ID="Label14" Text="App Requests Rejected:"></asp:Label>
																									</td>
																									<td>
																										<asp:Label runat="server" ID="Labelapprerj"></asp:Label>
																									</td>
																								</tr>
																								<tr>
																									<td>
																										<asp:Label runat="server" ID="Label16" Text="Current Connections:"></asp:Label>
																									</td>
																									<td>
																										<asp:Label runat="server" ID="Labelccntns"></asp:Label>
																									</td>
																								</tr>
																							</table>
																						</td>
																					</tr>
																				</table>
																			</td>
																			<td align="center">
																				<table>
																					<tr>
																						<td>
																							<table>
																								<asp:Label runat="server" ID="Lblwebsrvs" Text="Web Services" Font-Bold="true"></asp:Label>
																								<table>
																									<tr>
																										<td>
																											<asp:Label runat="server" ID="lblwebsts" CssClass="StatusLabel" Width="100%" Height="500%"></asp:Label>
																										</td>
																									</tr>
																									<tr>
																										<td>
																											<asp:Label runat="server" ID="lblmsg" Width="150%"></asp:Label>
																										</td>
																									</tr>
																									<tr>
																										<td>
																											<asp:Label runat="server" ID="Label12" Text="Bytes Received/Sec:"></asp:Label>
																										</td>
																										<td>
																											<asp:Label runat="server" ID="lblwebbrevd"></asp:Label>
																										</td>
																									</tr>
																									<tr>
																										<td>
																											<asp:Label runat="server" ID="Label17" Text="Bytes Sent/Sec:"></asp:Label>
																										</td>
																										<td>
																											<asp:Label runat="server" ID="lblwebbysnt"></asp:Label>
																										</td>
																									</tr>
																								</table>
																						</td>
																				</table>
																			</td>
																		</td>
																	</tr>
																</table>
															</dx:PanelContent>
														</PanelCollection>
														<PanelCollection>
														</PanelCollection>
													</dx:ASPxRoundPanel>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
								</table>
							</dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
                </TabPages>
            </dx:ASPxPageControl>
        </ContentTemplate>
    </asp:UpdatePanel>
    <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" AllowDragging="True" AllowResize="True"
        CloseAction="CloseButton"
        EnableViewState="False" PopupElementID="popupArea" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" ShowFooter="True" ShowOnPageLoad="False" Width="400px"
        Height="70px" FooterText="To resize the control use the resize grip or the control's edges"
        HeaderText="Suspend Monitoring" ClientInstanceName="FeedPopupControl" 
        EnableHierarchyRecreation="True" Theme="MetropolisBlue">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControlPopup" runat="server">
                <table id="FeedBackTable" class="EditorsTable" style="width: 100%; height: 100%;">
            <tr>
                <td class="Label">
                    <dx:ASPxLabel ID="lblDuration" runat="server" Text="Duration (mins):" Wrap="False"></dx:ASPxLabel>
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
                    <dx:ASPxLabel ID="ASPxLabel8" runat="server" 
                        Text="You can temporarily suspend the monitoring for a maximum duration of two hours. If you need to suspend monitoring for more than two hours, please use the Maintenance window functionality in VitalSigns Configurator."></dx:ASPxLabel>
                </td>
            </tr>        
            <tr>
            <td colspan="2">
                <dx:ASPxButton ID="test123" runat="server" onclick="BtnApply_Click"
                                                        AutoPostBack="False" 
                                                                CausesValidation="False" EnableTheming="True" 
                                                                Text="Apply" 
                                                                Theme="Office2010Blue"></dx:ASPxButton>
                                                                </td>
            </tr>
        </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <br />
</asp:Content>
