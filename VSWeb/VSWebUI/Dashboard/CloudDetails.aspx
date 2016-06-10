<%@ Page Title="VitalSigns Plus - Cloud Apps Details Page" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="CloudDetails.aspx.cs" Inherits="VSWebUI.Configurator.CloudDetails" %>

<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>

            




    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
    


<%@ Register assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>

<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
    <script type="text/javascript" language="javascript" >
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
            chartDiskSpace.SetVisible(true);
            grid.SetVisible(false);
        }
        else {
            chartDiskSpace.SetVisible(false);
            grid.SetVisible(true);
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
        iframe { margin:0; padding:0; height:500px; }
        iframe { display:block; width:100%; border:none; }
    </style>
    <style type="text/css">
        #NameHolder {
            float: left;
        }
        #StatusHolder {
            width:100px; 
            float: left;
            margin: 0 10px 0 10px;
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
    sessionStorage.setItem("Force refresh", "True");
</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
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
                                                                    <td valign="top">
<div id="NameHolder">
<div class="header" id="servernamelbldisp" runat="server">azphxdom1/RPRWyatt</div>
<div class="scan" id="Lastscanned" runat="server">9/16/2014 2:59:00 PM</div>
</div>
                                                <asp:Label ID="lblServerType" runat="server" Text="Cloud Apps: " Font-Bold="True"
                                                   Font-Size="Large" Style="color: #000000; font-family: Verdana; display: none"></asp:Label>             
 
    <asp:Label ID="servernamelbl" runat="server" Text="Label" Font-Bold="True"
        Font-Size="Large" Style="color: #000000; font-family: Verdana; display: none"></asp:Label>
                                                        </td>

                                                                </tr>
                                                            </table>
                                                        
</td>
                                                        <td>
                                                            &nbsp;&nbsp;
                                                        </td>
                                                        <td valign="top" align="right">
                                                            <dx:ASPxMenu ID="ASPxMenu1" runat="server" ShowAsToolbar="True" 
                                                                onitemclick="ASPxMenu1_ItemClick" Theme="Moderno" HorizontalAlign="Right">
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
                                                            <div id="ErrorMsg" class="alert alert-danger" runat="server" style="display: none">The settings were not updated
                                                            <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                                                            </div>
                                                            <div id="SuccessMsg" class="alert alert-success" runat="server" style="display: none">Temporarily Suspended monitoring
                                                            <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
   
    <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <dx:ASPxPageControl ID="ASPxPageControl1" ActiveTabIndex="0" 
        CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" TabSpacing="0px" 
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
                                    <asp:UpdatePanel ID="PerformUpdatePanel" runat="server" 
                                        UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <dx:ASPxRoundPanel ID="performanceASPxRoundPanel" runat="server" 
                                                CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                                GroupBoxCaptionOffsetY="-24px" HeaderText="Performance" 
                                                SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                                                <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                                <contentpaddings paddingbottom="10px" paddingleft="4px" 
                                                    paddingtop="10px" />
                                                <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                                <contentpaddings paddingbottom="10px" paddingleft="4px" paddingtop="10px" />
                                                <HeaderStyle Height="23px">
                                                    <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                                <paddings paddingbottom="0px" paddingleft="2px" paddingtop="0px" />
                                                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                                <paddings paddingbottom="0px" paddingleft="2px" paddingtop="0px" />
                                                </HeaderStyle>
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">

                                                    <table>
                                                    <tr>
                                                    <td>
                                                    <dx:ASPxHyperLink ID="PerfrmHyperLink" runat="server" 
                                                                Text="More" Visible="False" />
                                                    <dxchartsui:WebChartControl ID="performanceWebChartControl" runat="server" 
                                                            Height="300px" Width="600px">

                <DiagramSerializable>
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
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><datetimeoptions autoformat="False" format="General" /><datetimeoptions autoformat="False" format="General" /></label><Range SideMarginsEnabled="True"></Range><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><datetimescaleoptions measureunit="Hour"></datetimescaleoptions></AxisX>
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
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions></label><Range AlwaysShowZeroLevel="False" SideMarginsEnabled="True"></Range><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /></AxisY>
                                                            </cc1:XYDiagram>
                                                        </DiagramSerializable>

                                                            <FillStyle><OptionsSerializable><cc1:SolidFillOptions></cc1:SolidFillOptions></OptionsSerializable>
</FillStyle>

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
                            ResolveOverlappingMode="Default"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions autoformat="False" format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions autoformat="False" format="Custom" /></cc1:PointOptions></pointoptionsserializable></cc1:PointSeriesLabel></labelserializable>
                    <legendpointoptionsserializable><cc1:PointOptions><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions autoformat="False" format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions autoformat="False" format="Custom" /></cc1:PointOptions></legendpointoptionsserializable>
                </seriestemplate>

<CrosshairOptions><CommonLabelPositionSerializable><cc1:CrosshairMousePosition></cc1:CrosshairMousePosition></CommonLabelPositionSerializable>
</CrosshairOptions>

<ToolTipOptions><ToolTipPositionSerializable><cc1:ToolTipMousePosition></cc1:ToolTipMousePosition></ToolTipPositionSerializable>
</ToolTipOptions>
                                                        </dxchartsui:WebChartControl>
                                                    </td></tr>
                                                    </table>
                                                         
                                                        
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dx:ASPxRoundPanel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>    
                                </td>
                               <%-- <td valign="top">
								
							
									<dx:ASPxRoundPanel ID="diskspaceASPxRoundPanel" runat="server" 
                                            CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                            GroupBoxCaptionOffsetY="-24px" HeaderText="Disk Space" 
                                            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                                        <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                        <contentpaddings paddingbottom="10px" paddingleft="4px" 
                                            paddingtop="10px" />
                                        <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                        <HeaderStyle Height="23px">
                                            <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                        <paddings paddingbottom="0px" paddingleft="2px" paddingtop="0px" />
                                        <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                        </HeaderStyle>
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
											
                                            <table width="100%">
											    <tr>
                                                    <td>
                                                        <dx:ASPxHyperLink ID="linkDiskSpaceMore" runat="server" Text="More" 
                                                        Visible="False">
                                                    </dx:ASPxHyperLink>
                                                    <dx:ASPxRadioButtonList ID="DiskSpaceRadioButtonList" runat="server" ClientInstanceName="rblDiskSpace"
                                                        RepeatDirection="Horizontal" SelectedIndex="0" Visible="False" 
                                                        >
                                                        <ClientSideEvents SelectedIndexChanged="OnSelectedItemChanged" />
                                                        <ClientSideEvents SelectedIndexChanged="OnSelectedItemChanged" />
                                                        <Items>
                                                            <dx:ListEditItem Selected="True" Text="Chart" Value="Chart" 
                                                                ImageUrl="~/images/icons/chart_bar.png" />
                                                            <dx:ListEditItem Text="Grid" Value="Grid" ImageUrl="~/images/icons/table.png" />
                                                        </Items>
                                                    </dx:ASPxRadioButtonList>
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <dx:ASPxGridView ID="DiskHealthGrid" runat="server" AutoGenerateColumns="False" 
                                                            ClientInstanceName="grid" 
                                                            CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                                                            CssPostfix="Office2010Silver" Cursor="pointer" EnableTheming="True" 
                                                            KeyFieldName="ID" OnCustomCallback="DiskHealthGrid_CustomCallback" 
                                                            OnHtmlDataCellPrepared="DiskHealthGrid_HtmlDataCellPrepared" 
                                                            OnHtmlRowCreated="DiskHealthGrid_HtmlRowCreated" 
                                                            OnPageSizeChanged="DiskHealthGrid_PageSizeChanged" 
                                                            Theme="Office2003Blue">
                                                            <Columns>
                                                                <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" Visible="False" 
                                                                    VisibleIndex="11">
                                                                    <Settings AllowAutoFilter="False" />
                                                                    <Settings AllowAutoFilter="False" />
                                                                    <EditCellStyle CssClass="GridCss">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader2" />
                                                                    <CellStyle CssClass="GridCss2">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Disk Name" FieldName="DiskName" 
                                                                    VisibleIndex="2" Width="200px">
                                                                    <Settings AllowAutoFilter="False" AllowDragDrop="True" 
                                                                        AutoFilterCondition="Contains" />
                                                                    <Settings AllowAutoFilter="False" AllowDragDrop="True" 
                                                                        AutoFilterCondition="Contains" />
                                                                    <EditCellStyle CssClass="GridCss">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Percent Free" FieldName="PercentFree" 
                                                                    VisibleIndex="5" Width="100px">
                                                                    <PropertiesTextEdit DisplayFormatString="{0:P}"></PropertiesTextEdit>
                                                                    <Settings AllowAutoFilter="False" AllowDragDrop="True" />
                                                                    <Settings AllowAutoFilter="False" AllowDragDrop="True" />
                                                                    <EditCellStyle CssClass="GridCss">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader2" />
                                                                    <CellStyle CssClass="GridCss2">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Disk Free (GB)" FieldName="DiskFree" 
                                                                    VisibleIndex="4" Width="100px">
                                                                    <Settings AutoFilterCondition="Contains" AllowAutoFilter="False" />
                                                                    <Settings AutoFilterCondition="Contains" AllowAutoFilter="False" />
                                                                    <EditCellStyle CssClass="GridCss">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader2" />
                                                                    <CellStyle CssClass="GridCss2">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Disk Size (GB)" FieldName="DiskSize" 
                                                                    VisibleIndex="3" Width="100px">
                                                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                                                    <Settings AllowDragDrop="True" 
                                                                        AutoFilterCondition="Contains" />
                                                                    <Settings AllowDragDrop="True" 
                                                                        AutoFilterCondition="Contains" />
                                                                    <EditCellStyle CssClass="GridCss">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader2" />
                                                                    <CellStyle CssClass="GridCss2" HorizontalAlign="Center">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Disk Utilization" 
                                                                    FieldName="PercentUtilization" Visible="False" 
                                                                    VisibleIndex="8">
                                                                    <Settings AllowAutoFilter="False" AllowDragDrop="True" 
                                                                        AutoFilterCondition="Contains" />
                                                                    <Settings AllowAutoFilter="False" AllowDragDrop="True" 
                                                                        AutoFilterCondition="Contains" />
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Threshold" FieldName="ThresholdDisp" 
                                                                    VisibleIndex="6" Width="100px">
                                                                    <PropertiesTextEdit DisplayFormatString="0.00">
                                                                    </PropertiesTextEdit>
                                                                    <Settings AllowAutoFilter="False" AutoFilterCondition="Contains" 
                                                                        AllowDragDrop="True" />
                                                                    <Settings AllowAutoFilter="False" AutoFilterCondition="Contains" 
                                                                        AllowDragDrop="True" />
                                                                    <EditCellStyle CssClass="GridCss">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader2" />
                                                                    <CellStyle CssClass="GridCss2">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                            </Columns>
                                                                    <SettingsBehavior AllowDragDrop="False" AllowFocusedRow="True" 
                                                                        AllowSelectByRowClick="True" AutoExpandAllGroups="True" 
                                                                        ColumnResizeMode="NextColumn" ProcessSelectionChangedOnServer="True" />
                                                                    <SettingsBehavior AllowDragDrop="False" AllowFocusedRow="True" 
                                                                        AllowSelectByRowClick="True" AutoExpandAllGroups="True" 
                                                                        ColumnResizeMode="NextColumn" ProcessSelectionChangedOnServer="True" />
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
                                                            <Styles CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                                                                CssPostfix="Office2010Silver">
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
                                                                <dxchartsui:WebChartControl ID="DiskSpaceWebChartControl1" runat="server" ClientInstanceName="chartDiskSpace" 
                                                            CrosshairEnabled="True" Height="265px" Width="600px">
                                                            <diagramserializable>
                                                                <cc1:XYDiagram Rotated="True">
                                                                    <axisx visibleinpanesserializable="-1" reverse="True"><label><resolveoverlappingoptions allowhide="False" allowrotate="False" /><resolveoverlappingoptions allowhide="False" allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions 
                                                                        allowhide="False" allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions 
                                                                        allowhide="False" allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions 
                                                                        allowhide="False" allowrotate="False"></resolveoverlappingoptions></label></axisx>
                                                                    <axisy visibleinpanesserializable="-1" title-text="Disk Size (GB)" 
                                                                        title-visible="True"><label><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions 
                                                                        allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions 
                                                                        allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions 
                                                                        allowrotate="False"></resolveoverlappingoptions></label></axisy>
                                                                </cc1:XYDiagram>
                                                            </diagramserializable>
                                                            <legend visible="False"></legend>
                                                            <seriesserializable>
                                                                <cc1:Series ArgumentScaleType="Qualitative" LabelsVisibility="True" 
                                                                    Name="Disk Used" CrosshairEnabled="True" CrosshairLabelPattern="{V}" 
                                                                    SynchronizePointOptions="False">
                                                                    <viewserializable><cc1:StackedBarSeriesView Color="Red" 
                                                                        Transparency="0"><fillstyle 
                                                                        fillmode="Solid"></fillstyle></cc1:StackedBarSeriesView></viewserializable>
                                                                    <labelserializable><cc1:StackedBarSeriesLabel ResolveOverlappingMode="HideOverlapped"><pointoptionsserializable><cc1:PointOptions><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /></cc1:PointOptions></pointoptionsserializable></cc1:StackedBarSeriesLabel></labelserializable>
                                                                    <legendpointoptionsserializable><cc1:PointOptions Pattern="{V}"><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /></cc1:PointOptions></legendpointoptionsserializable>
                                                                </cc1:Series>
                                                                <cc1:Series ArgumentScaleType="Qualitative" LabelsVisibility="True" 
                                                                    Name="Disk Free" CrosshairEnabled="True" CrosshairLabelPattern="{V}" 
                                                                    SynchronizePointOptions="False">
                                                                    <viewserializable><cc1:StackedBarSeriesView Color="0, 128, 0" 
                                                                        Transparency="0"><fillstyle 
                                                                        fillmode="Solid"></fillstyle></cc1:StackedBarSeriesView></viewserializable>
                                                                    <labelserializable><cc1:StackedBarSeriesLabel><pointoptionsserializable><cc1:PointOptions><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /></cc1:PointOptions></pointoptionsserializable></cc1:StackedBarSeriesLabel></labelserializable>
                                                                    <legendpointoptionsserializable><cc1:PointOptions Pattern="{V}"><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /><valuenumericoptions format="Number" /></cc1:PointOptions></legendpointoptionsserializable>
                                                                </cc1:Series>
                                                            </seriesserializable>
                                                            <seriestemplate>
                                                                <viewserializable><cc1:StackedBarSeriesView></cc1:StackedBarSeriesView></viewserializable>
                                                            </seriestemplate>
                                                        </dxchartsui:WebChartControl>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="DiskSpaceRadioButtonList" />
                                                                <asp:AsyncPostBackTrigger ControlID="ASPxButton2" />
                                                                <asp:AsyncPostBackTrigger ControlID="ASPxButton3" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                        
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <dx:ASPxButton ID="ASPxButton2" runat="server" Theme="Office2003Blue" 
                                                        ToolTip="View Chart">
                                                        <ClientSideEvents Click="function(s, e) {
	chartDiskSpace.SetVisible(true);
    grid.SetVisible(false);
}" />
                                                        <Image Url="~/images/icons/chart_bar.png">
                                                        </Image>
                                                    </dx:ASPxButton>
                                                    <dx:ASPxButton ID="ASPxButton3" runat="server" Theme="Office2003Blue" 
                                                        ToolTip="View Grid">
                                                        <ClientSideEvents Click="function(s, e) {
     chartDiskSpace.SetVisible(false);
     grid.SetVisible(true);
}" />
                                                        <Image Url="~/images/icons/table.png">
                                                        </Image>
                                                    </dx:ASPxButton>
                                                    </td>
                                                </tr>
											</table>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dx:ASPxRoundPanel>
								
                                   
                                </td>--%>
                            </tr>
                        
                              
                       
                        </table>
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
                    <td colspan="8"></td>
                </tr>
        <tr>
            <td valign="top">
               <dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" 
                    Text="From Date:" Wrap="False">
                </dx:ASPxLabel>
            </td>
            <td>
                <asp:TextBox ID="txtfromdate" runat="server" Font-Size="12px"></asp:TextBox>
                <cc1:CalendarExtender ID="calextender" runat="server" Format="MM/dd/yyyy" 
                    TargetControlID="txtfromdate">
                </cc1:CalendarExtender>
            </td>
            <td valign="top">
                <asp:RequiredFieldValidator ID="rfvtxtfromdate" runat="server" 
                    ControlToValidate="txtfromdate" ErrorMessage="Enter From Date" 
                    Font-Size="12px" ForeColor="#FF3300"></asp:RequiredFieldValidator>
            </td>
            <td valign="top">
                &nbsp;</td>
            <td valign="top">
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" 
                    Text="From Time:" Wrap="False">
                </dx:ASPxLabel>
            </td>
            <td valign="top">
                <dx:ASPxTimeEdit ID="ASPxTimeEdit1" runat="server">
                </dx:ASPxTimeEdit>
            </td>
            <td valign="top">
                &nbsp;</td>
            <td valign="top">
                <dx:ASPxButton ID="ClearButton" runat="server" OnClick="ClearButton_Click" 
                    Text="Clear" Theme="Office2010Blue" Width="80px">
                </dx:ASPxButton>
            </td>
        </tr>
        <tr>
            <td valign="top">

                <dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="lblsmallFont" 
                    Text="To Date:" Wrap="False">
                </dx:ASPxLabel>
            </td>
            <td valign="top">
             
                <asp:TextBox ID="txttodate" runat="server" Font-Size="12px"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="MM/dd/yyyy" 
                    TargetControlID="txttodate">
                </cc1:CalendarExtender>
          
               </td>
            <td valign="top">
        
                <asp:RequiredFieldValidator ID="RFvtxttodate" runat="server" 
                    ControlToValidate="txttodate" ErrorMessage="Enter To Date" 
                    Font-Size="12px" ForeColor="Red"></asp:RequiredFieldValidator>
            </td> 
                <td valign="top">
                    &nbsp;</td>
                <td valign="top">
                    <dx:ASPxLabel ID="ASPxLabel4" runat="server" CssClass="lblsmallFont" 
                        Text="To Time:" Wrap="False">
                    </dx:ASPxLabel>
            </td>
                <td valign="top">
                    <dx:ASPxTimeEdit ID="ASPxTimeEdit2" runat="server">
                    </dx:ASPxTimeEdit>
                </td>
                <td>
           
                    &nbsp;</td>
            <td>
                <dx:ASPxButton ID="btnsearch" runat="server" OnClick="btnsearch_Click" 
                    Text="Search" Theme="Office2010Blue" Width="80px">
                </dx:ASPxButton>
            </td>
        </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="5">
                <dx:ASPxGridView ID="maintenancegrid" runat="server" AutoGenerateColumns="False" 
                    CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                    CssPostfix="Office2010Silver" Width="100%" 
                    
                    KeyFieldName="TypeandName" Theme="Office2003Blue" 
                    >
                   
<SettingsBehavior 
                        ColumnResizeMode="NextColumn"></SettingsBehavior>

                    <SettingsPager PageSize="50">
<PageSizeItemSettings Visible="True"></PageSizeItemSettings>
                    </SettingsPager>
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="Server Name" VisibleIndex="0"  
                            FieldName="servername" >
                       
                            <Settings AllowDragDrop="True" AutoFilterCondition="Contains" />

                            <Settings AllowDragDrop="True" AutoFilterCondition="Contains" />

                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" >
                            <Paddings Padding="5px" />
<Paddings Padding="5px"></Paddings>
                            <Paddings Padding="5px" />
                            </HeaderStyle>
                            <CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Server Type" FieldName="ServerType" 
                            Visible="True" VisibleIndex="1">
                            <Settings AllowAutoFilter="True" AllowDragDrop="True" 
                                AutoFilterCondition="Contains" />
<Settings AllowDragDrop="True" AllowAutoFilter="True" AutoFilterCondition="Contains"></Settings>

                            <Settings AllowAutoFilter="True" AllowDragDrop="True" 
                                AutoFilterCondition="Contains" />

                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                          <dx:GridViewDataTextColumn Caption="Start Date" VisibleIndex="2" 
                            FieldName="StartDate">
                            <Settings AllowAutoFilter="False" AllowDragDrop="True" 
                                AutoFilterCondition="Contains" />
<Settings AllowDragDrop="True" AllowAutoFilter="False" AutoFilterCondition="Contains"></Settings>

                              <Settings AllowAutoFilter="False" AllowDragDrop="True" 
                                  AutoFilterCondition="Contains" />

                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Start Time" VisibleIndex="3" 
                            FieldName="StartTime">
                            <Settings AllowAutoFilter="False" AllowDragDrop="True" 
                                AutoFilterCondition="Contains" />
<Settings AllowDragDrop="True" AllowAutoFilter="False" AutoFilterCondition="Contains"></Settings>

                            <Settings AllowAutoFilter="False" AllowDragDrop="True" 
                                AutoFilterCondition="Contains" />

                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Duration" VisibleIndex="4" 
                            FieldName="Duration">
                            <Settings AllowAutoFilter="False" AllowDragDrop="True" />
<Settings AllowDragDrop="True" AllowAutoFilter="False"></Settings>

                            <Settings AllowAutoFilter="False" AllowDragDrop="True" />

                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        
                        
                        <dx:GridViewDataTextColumn Caption="End Date" VisibleIndex="5" 
                            FieldName="EndDate">
                            <Settings AllowAutoFilter="False" AllowDragDrop="True" />
<Settings AllowDragDrop="True" AllowAutoFilter="False"></Settings>

                            <Settings AllowAutoFilter="False" AllowDragDrop="True" />

                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        
                        <dx:GridViewDataTextColumn Caption="Maintenance Type" VisibleIndex="6" 
                            FieldName="MaintType">
                            <Settings AllowAutoFilter="False" AllowDragDrop="True" />
<Settings AllowDragDrop="True" AllowAutoFilter="False"></Settings>

                            <Settings AllowAutoFilter="False" AllowDragDrop="True" />

                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Maintenance Days List" VisibleIndex="7" 
                            FieldName="MaintDaysList">
                            <Settings AllowAutoFilter="False" AllowDragDrop="True" />
<Settings AllowDragDrop="True" AllowAutoFilter="False"></Settings>

                            <Settings AllowAutoFilter="False" AllowDragDrop="True" />

                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>                     
                        
                    </Columns>
                    <SettingsBehavior AllowDragDrop="False" AutoExpandAllGroups="True" 
                        AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True" />
                     <SettingsBehavior AllowDragDrop="False" AllowSelectByRowClick="True" 
                        AutoExpandAllGroups="True" ProcessSelectionChangedOnServer="True" />
                     <SettingsPager PageSize="50" SEOFriendly="Enabled" >
                         <PageSizeItemSettings Visible="true" />
                         <PageSizeItemSettings Visible="true" />
        </SettingsPager>
                    <Settings ShowGroupPanel="True" ShowFilterRow="True" />

<Settings ShowFilterRow="True" ShowGroupPanel="True"></Settings>

                    <Settings ShowFilterRow="True" ShowGroupPanel="True" />

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
                    <Styles CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                        CssPostfix="Office2010Silver">
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
     
            <dx:ASPxPopupControl ID="MsgPopupControl" runat="server" 
                    HeaderText="Information" Modal="True" PopupHorizontalAlign="WindowCenter" 
                    PopupVerticalAlign="WindowCenter" Theme="Glass">
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
                <dx:ASPxButton ID="OKButton" runat="server" OnClick="OKButton_Click" Text="OK" 
                    Theme="Office2010Blue">
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
                                                <table  width="100%">
                                                    <tr>
                                                        <td>
                                                            <div id="infoDiv" class="info">The list of configured alerts that apply to the server are listed below. 
                                                            The list includes the last 7 days worth of information.
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxGridView ID="AlertGridView" runat="server" AutoGenerateColumns="False" 
                                                                EnableTheming="True" Theme="Office2003Blue" Width="100%">
                                                                <Columns>
                                                                    <dx:GridViewCommandColumn ShowInCustomizationForm="True" Visible="False" 
                                                                        VisibleIndex="0">
                                                                        <ClearFilterButton Visible="True">
                                                                        </ClearFilterButton>
                                                                    </dx:GridViewCommandColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Device Name" FieldName="DeviceName" 
                                                                        ShowInCustomizationForm="True" VisibleIndex="1">
                                                                         <Settings AutoFilterCondition="Contains" />

                                                                         <Settings AutoFilterCondition="Contains" />

                                                                         <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCssHeader" >
                                                                         <Paddings Padding="5px" />
<Paddings Padding="5px"></Paddings>
                                                                         <Paddings Padding="5px" />
                                                                         </HeaderStyle>
                                                    <CellStyle CssClass="GridCss">
                                                    </CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Device Type" FieldName="DeviceType" 
                                                                        ShowInCustomizationForm="True" VisibleIndex="2">
                                                                         <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                    <CellStyle CssClass="GridCss">
                                                    </CellStyle>
                                                    <Settings AutoFilterCondition="Contains" />
                                                                         <Settings AutoFilterCondition="Contains" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Alert Type" FieldName="AlertType" 
                                                                        ShowInCustomizationForm="True" VisibleIndex="3">
                                                                         <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                    <CellStyle CssClass="GridCss">
                                                    </CellStyle>
                                                    <Settings AutoFilterCondition="Contains" />
                                                                         <Settings AutoFilterCondition="Contains" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Date/Time of Alert" 
                                                                        FieldName="DateTimeOfAlert" ShowInCustomizationForm="True" 
                                                                        VisibleIndex="4">
                                                                         <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                    <CellStyle CssClass="GridCss1">
                                                    </CellStyle>
                                                    <Settings AutoFilterCondition="Contains" />
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
                                                                    <dx:GridViewDataTextColumn Caption="Date/Time Alert Cleared" 
                                                                        FieldName="DateTimeAlertCleared" ShowInCustomizationForm="True" 
                                                                        VisibleIndex="6">
                                                                         <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                    <CellStyle CssClass="GridCss1">
                                                    </CellStyle>
                                                    <Settings AutoFilterCondition="Contains" />
                                                                         <Settings AutoFilterCondition="Contains" />
                                                                    </dx:GridViewDataTextColumn>
                                                                </Columns>
                                                                <SettingsBehavior ColumnResizeMode="NextColumn" />

<SettingsBehavior ColumnResizeMode="NextColumn"></SettingsBehavior>

                                                                <SettingsBehavior ColumnResizeMode="NextColumn" />

                                                                <SettingsPager PageSize="50">
                                                                    <PageSizeItemSettings Visible="True" >
                                                                    </PageSizeItemSettings>
                                                                </SettingsPager>
                                                                <Settings ShowFilterRow="True" ShowGroupPanel="True" />

<Settings ShowFilterRow="True" ShowGroupPanel="True"></Settings>
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
                                                <dx:ASPxGridView ID="OutageGridView" runat="server" AutoGenerateColumns="False" 
                                                    EnableTheming="True" Theme="Office2003Blue" Width="100%">
                                                    <Columns>
                                                        <dx:GridViewDataTextColumn Caption="Server Name" FieldName="ServerName" 
                                                            ShowInCustomizationForm="True" VisibleIndex="1" Width="300px">
                                                            <Settings AllowAutoFilter="False" AutoFilterCondition="Contains" />
                                                            <Settings AllowAutoFilter="False" AutoFilterCondition="Contains" />
                                                            <Settings AllowAutoFilter="False" AutoFilterCondition="Contains" />
                                                            <Settings AllowAutoFilter="False" AutoFilterCondition="Contains" />
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader">
                                                            <Paddings Padding="5px" />
                                                            <Paddings Padding="5px" />
                                                            <Paddings Padding="5px" />
                                                            <Paddings Padding="5px" />
                                                            </HeaderStyle>
                                                            <CellStyle CssClass="GridCss">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Date/Time Down" FieldName="DateTimeDown" 
                                                            ShowInCustomizationForm="True" VisibleIndex="2">
                                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                            <CellStyle CssClass="GridCss1">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Date/Time Up" FieldName="DateTimeUp" 
                                                            ShowInCustomizationForm="True" VisibleIndex="3">
                                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                            <CellStyle CssClass="GridCss1">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Duration (mins)" FieldName="Description" 
                                                            ShowInCustomizationForm="True" VisibleIndex="4">
                                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                            <CellStyle CssClass="GridCss2">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                        </dx:GridViewDataTextColumn>
                                                    </Columns>
                                                    <SettingsBehavior ColumnResizeMode="NextColumn" />
                                                    <SettingsBehavior ColumnResizeMode="NextColumn" />
                                                    <SettingsPager PageSize="50">
                                                        <PageSizeItemSettings Visible="True">
                                                        </PageSizeItemSettings>
                                                    </SettingsPager>
                                                    <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                                                    <SettingsBehavior ColumnResizeMode="NextColumn" />
                                                    <Settings ShowFilterRow="True" ShowGroupPanel="True" />
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
           </TabPages>
    </dx:ASPxPageControl>
    </ContentTemplate>
    </asp:UpdatePanel>   
    <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" AllowDragging="True" AllowResize="True"
        CloseAction="CloseButton"
        EnableViewState="False" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" ShowFooter="True" ShowOnPageLoad="False" Width="400px"
        Height="70px" FooterText="Try to resize the control using the resize grip or the control's edges"
        HeaderText="Suspend Monitoring" ClientInstanceName="FeedPopupControl" EnableHierarchyRecreation="True" 
                                        Theme="MetropolisBlue">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControlPopup" runat="server">
                <table id="FeedBackTable" class="EditorsTable" style="width: 100%; height: 100%;">
            <tr>
                <td class="Label">
                    <dx:ASPxLabel ID="lblDuration" runat="server" Text="Duration (mins):" 
                        Wrap="False"></dx:ASPxLabel>
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
                    <dx:ASPxLabel ID="ASPxLabel9" runat="server" 
                        Text="You may temporarily suspend monitoring for a maximum duration of two hours. If you need to suspend monitoring for more than two hours, please use the Maintenance window functionality in VitalSigns Configurator."></dx:ASPxLabel>
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
