<%@ Page Title="VitalSigns Plus - ExchangeServer Details page" Language="C#" MasterPageFile="~/DashboardSite.Master"
    AutoEventWireup="true" CodeBehind="ExchangeServerDetailsPage2.aspx.cs" Inherits="VSWebUI.Configurator.ExchangeServerDetailsPage2" %>
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
    <script type="text/javascript">
    // <![CDATA[
        function BackButton_Clicked(s, e) {
            window.history.back();
        }
    // ]]>
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
        .dxeTextBoxSys, .dxeMemoSys
        {
            border-collapse: separate !important;
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblServerType" runat="server" Text="IBM Domino Server: " Font-Bold="True"
        Font-Size="Large" Style="color: #000000; font-family: Verdana"></asp:Label>
    <asp:Label ID="servernamelbl" runat="server" Text="Label" Font-Bold="True"
        Font-Size="Large" Style="color: #000000; font-family: Verdana"></asp:Label>
    <br />
    <asp:Label ID="lbltext" runat="server" Text="Last scan date: " Font-Size="Small" Style="color: #000000; font-family: Verdana">
    </asp:Label>
    <asp:Label ID="Lastscanned" runat="server" Text="Not Mentioned" Font-Size="Small" Style="color: #000000; font-family: Verdana">
    </asp:Label>
    <br />
    <br />
    <dx:ASPxButton ID="BackButton" runat="server" Text="Back" Theme="Office2010Blue"
        AutoPostBack="false" CausesValidation="False">
        <ClientSideEvents Click="BackButton_Clicked" />
    </dx:ASPxButton>
    <br />
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
                                                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
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
                                                                            <dxchartsui:WebChartControl ID="performanceWebChartControl" runat="server" Height="300px"
                                                                                Width="600px">
                                                                                <diagramserializable>
                                                            <cc1:XYDiagram PaneLayoutDirection="Horizontal">
                                                                <AxisX Title-Text="Date" VisibleInPanesSerializable="-1" GridSpacingAuto="True" 
                                                                    DateTimeMeasureUnit="Hour" minorcount="5"><label enableantialiasing="False" Staggered="True"><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions></label><Range SideMarginsEnabled="True"></Range><datetimeoptions format="Custom" /><datetimeoptions format="General" /><datetimeoptions format="Custom" /><datetimeoptions format="General" /></AxisX>
                                                                <AxisY Title-Text="Space(bytes)" Title-Visible="True" VisibleInPanesSerializable="-1"><label><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions 
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
                            ResolveOverlappingMode="Default"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /></cc1:PointOptions></pointoptionsserializable></cc1:PointSeriesLabel></labelserializable>
                    <legendpointoptionsserializable><cc1:PointOptions><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /></cc1:PointOptions></legendpointoptionsserializable>
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
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
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
                                                                <table>
                                                                    <tr>
                                                                        <td style="padding-left: 5px">
                                                                            <dx:ASPxHyperLink ID="linkDiskSpaceMore" runat="server" Text="More">
                                                                            </dx:ASPxHyperLink>
                                                                            <br />
                                                                            <br />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <dxchartsui:WebChartControl ID="diskspaceWebChartControl" runat="server" Height="300px"
                                                                                Width="600px" EnableDefaultAppearance="False" PaletteName="Palette 1">
                                                                                <fillstyle>
                                            <optionsserializable><cc1:SolidFillOptions /></optionsserializable>
                                        </fillstyle>
                                                                                <legend visible="False"></legend>
                                                                                <seriesserializable>
                                             <cc1:Series ShowInLegend="False" LegendText="ABC" Visible="False">
                                            <viewserializable><cc1:PieSeriesView RuntimeExploding="true"></cc1:PieSeriesView></viewserializable>
                                            <labelserializable><cc1:PieSeriesLabel LineVisible="True" Position="Radial" TextColor="Black" 
                                                    BackColor="Transparent" Font="Tahoma, 8pt, style=Bold" Visible="False"><Border Visible="False"></Border><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PiePointOptions PointView="Argument"></cc1:PiePointOptions></pointoptionsserializable></cc1:PieSeriesLabel></labelserializable>
                                            <legendpointoptionsserializable><cc1:PiePointOptions PointView="Argument"></cc1:PiePointOptions></legendpointoptionsserializable>
                                        </cc1:Series>
                                        </seriesserializable>
                                                                                <seriestemplate>
                                            <viewserializable><cc1:PieSeriesView RuntimeExploding="False"></cc1:PieSeriesView></viewserializable>
                                            <labelserializable><cc1:PieSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PiePointOptions></cc1:PiePointOptions></pointoptionsserializable></cc1:PieSeriesLabel></labelserializable>
                                            <legendpointoptionsserializable><cc1:PiePointOptions></cc1:PiePointOptions></legendpointoptionsserializable>
                                        </seriestemplate>
                                                                                <palettewrappers>
                                            <dxchartsui:PaletteWrapper Name="Palette 1" ScaleMode="Repeat">
                                                <palette><cc1:PaletteEntry Color="0, 198, 0" Color2="0, 198, 0" /><cc1:PaletteEntry Color="Red" Color2="Red" /></palette>
                                            </dxchartsui:PaletteWrapper>
                                        </palettewrappers>
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
                                                                    GridSpacingAuto="False" MinorCount="5" DateTimeMeasureUnit="Hour"><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><label 
                                                                    enableantialiasing="False" Staggered="True"><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /></label><Range 
                                                                    SideMarginsEnabled="True"></Range><datetimeoptions format="Custom" /><datetimeoptions format="Custom" /><datetimeoptions format="Custom" /><datetimeoptions format="Custom" /></AxisX>
                                                                <AxisY Title-Text="Space (bytes)" Title-Visible="True" 
                                                                    VisibleInPanesSerializable="-1"><label><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions></label><Range AlwaysShowZeroLevel="False" SideMarginsEnabled="True"></Range></AxisY>
                                                            </cc1:XYDiagram>
                                                        </diagramserializable>
                                                                                <fillstyle><OptionsSerializable><cc1:SolidFillOptions></cc1:SolidFillOptions></OptionsSerializable>
</fillstyle>
                                                                                <seriesserializable>
                    <cc1:Series Name="Series 1" ArgumentScaleType="DateTime">
                        <viewserializable><cc1:LineSeriesView></cc1:LineSeriesView></viewserializable>
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
                                                                    GridSpacingAuto="False" MinorCount="5" DateTimeMeasureUnit="Hour"><label 
                                                                    enableantialiasing="False" Staggered="True"><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False"></resolveoverlappingoptions><resolveoverlappingoptions 
                                                                    allowrotate="False"></resolveoverlappingoptions></label><Range 
                                                                    SideMarginsEnabled="True"></Range><datetimeoptions format="General" /><datetimeoptions format="General" /></AxisX>
                                                                <AxisY Title-Text="Space(bytes)" Title-Visible="True" VisibleInPanesSerializable="-1"><Range AlwaysShowZeroLevel="False" SideMarginsEnabled="True"></Range></AxisY>
                                                            </cc1:XYDiagram>
                                                        </diagramserializable>
                                                                                <fillstyle><OptionsSerializable><cc1:SolidFillOptions></cc1:SolidFillOptions></OptionsSerializable>
</fillstyle>
                                                                                <seriesserializable>
                    <cc1:Series Name="Series 1" ArgumentScaleType="DateTime">
                        <viewserializable><cc1:LineSeriesView></cc1:LineSeriesView></viewserializable>
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
                                                        CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" 
                                                        HeaderText="RPC Client Access Users" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
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
                                                                <dxchartsui:WebChartControl ID="RPCusersWebChartControl" runat="server" Height="300px"
                                                                    Width="600px">
                                                                    <diagramserializable>
                                                            <cc1:XYDiagram LabelsResolveOverlappingMinIndent="1" PaneLayoutDirection="Horizontal">
                                                                <AxisX Title-Text="Date" VisibleInPanesSerializable="-1" GridSpacingAuto="False" GridSpacing="0.5" MinorCount="5" DateTimeMeasureUnit="Hour"><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><label><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False"></resolveoverlappingoptions></label><Range SideMarginsEnabled="True"></Range><datetimeoptions format="General" /><datetimeoptions format="General" /><datetimeoptions format="General" /><datetimeoptions format="General" /><datetimeoptions format="General" /></AxisX>
                                                                <AxisY Title-Text="Space(bytes)" Title-Visible="True" VisibleInPanesSerializable="-1"><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><label><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False"></resolveoverlappingoptions></label><Range AlwaysShowZeroLevel="False" SideMarginsEnabled="True"></Range><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /></AxisY>
                                                            </cc1:XYDiagram>
                                                        </diagramserializable>
                                                                    <fillstyle><OptionsSerializable><cc1:SolidFillOptions></cc1:SolidFillOptions></OptionsSerializable>
</fillstyle>
                                                                    <seriesserializable>
                    <cc1:Series Name="Series 1" ArgumentScaleType="DateTime">
                        <viewserializable><cc1:LineSeriesView></cc1:LineSeriesView></viewserializable>
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
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <dx:ASPxRoundPanel ID="usersASPxRoundPanel0" runat="server" 
                                                        CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                                        GroupBoxCaptionOffsetY="-24px" HeaderText="Outlook Web App Users" 
                                                        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                                                        <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                                        <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                                        <HeaderStyle Height="23px">
                                                        <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                                        <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                                        </HeaderStyle>
                                                        <PanelCollection>
                                                            <dx:PanelContent ID="PanelContent8" runat="server" 
                                                                SupportsDisabledAttribute="True">
                                                                <dx:ASPxRadioButtonList ID="OutlookusersASPxRadioButtonList" runat="server" 
                                                                    AutoPostBack="True" CssClass="Glass" 
                                                                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                                                    OnSelectedIndexChanged="OutlookusersASPxRadioButtonList_SelectedIndexChanged" 
                                                                    RepeatDirection="Horizontal" SelectedIndex="0" 
                                                                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" ValueType="System.String" 
                                                                    Visible="False">
                                                                    <Items>
                                                                        <dx:ListEditItem Selected="True" Text="Per Hour" Value="hh" />
                                                                        <dx:ListEditItem Text="Per Day" Value="dd" />
                                                                    </Items>
                                                                </dx:ASPxRadioButtonList>
                                                                <dxchartsui:WebChartControl ID="OutlookusersWebChartControl" runat="server" 
                                                                    Height="300px" Width="600px">
                                                                    <diagramserializable>
                                                                        <cc1:XYDiagram LabelsResolveOverlappingMinIndent="1" 
                                                                            PaneLayoutDirection="Horizontal">
                                                                            <axisx datetimemeasureunit="Hour" gridspacing="0.5" gridspacingauto="False" 
                                                                                minorcount="5" title-text="Date" visibleinpanesserializable="-1"><range sidemarginsenabled="True" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><label><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False"></resolveoverlappingoptions></label><range sidemarginsenabled="True" /><datetimeoptions format="General" /><datetimeoptions format="General" /><datetimeoptions format="General" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /></axisx>
                                                                            <axisy title-text="Space(bytes)" title-visible="True" 
                                                                                visibleinpanesserializable="-1"><range alwaysshowzerolevel="False" sidemarginsenabled="True" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><label><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False"></resolveoverlappingoptions></label><range alwaysshowzerolevel="False" sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /><range alwaysshowzerolevel="False" sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range alwaysshowzerolevel="False" sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /></axisy>
                                                                        </cc1:XYDiagram>
                                                                    </diagramserializable>
                                                                    <fillstyle>
                                                                        <optionsserializable><cc1:SolidFillOptions /></optionsserializable>
                                                                    </fillstyle>
                                                                    <seriesserializable>
                                                                        <cc1:Series ArgumentScaleType="DateTime" Name="Series 1">
                                                                            <viewserializable><cc1:LineSeriesView></cc1:LineSeriesView></viewserializable>
                                                                            <labelserializable><cc1:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:PointSeriesLabel></labelserializable>
                                                                            <legendpointoptionsserializable><cc1:PointOptions></cc1:PointOptions></legendpointoptionsserializable>
                                                                        </cc1:Series>
                                                                        <cc1:Series ArgumentScaleType="DateTime" Name="Series 2">
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
                                                                    <crosshairoptions>
                                                                        <commonlabelpositionserializable><cc1:CrosshairMousePosition /></commonlabelpositionserializable>
                                                                    </crosshairoptions>
                                                                    <tooltipoptions>
                                                                        <tooltippositionserializable><cc1:ToolTipMousePosition /></tooltippositionserializable>
                                                                    </tooltipoptions>
                                                                </dxchartsui:WebChartControl>
                                                            </dx:PanelContent>
                                                        </PanelCollection>
                                                    </dx:ASPxRoundPanel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <dx:ASPxRoundPanel ID="mailASPxRoundPanel0" runat="server" 
                                                        CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                                        GroupBoxCaptionOffsetY="-24px" HeaderText="Mail" Height="400px" 
                                                        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                                                        <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                                        <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                                        <HeaderStyle Height="23px">
                                                        <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                                        <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                                        </HeaderStyle>
                                                        <PanelCollection>
                                                            <dx:PanelContent ID="PanelContent7" runat="server" 
                                                                SupportsDisabledAttribute="True">
                                                                <dxchartsui:WebChartControl ID="MailWebChartControl" runat="server" 
                                                                    Height="300px" Width="600px">
                                                                    <fillstyle>
                                                                        <optionsserializable><cc1:SolidFillOptions /></optionsserializable>
                                                                    </fillstyle>
                                                                    <seriestemplate>
                                                                        <viewserializable><cc1:SideBySideBarSeriesView></cc1:SideBySideBarSeriesView></viewserializable>
                                                                        <labelserializable><cc1:SideBySideBarSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:SideBySideBarSeriesLabel></labelserializable>
                                                                        <legendpointoptionsserializable><cc1:PointOptions></cc1:PointOptions></legendpointoptionsserializable>
                                                                    </seriestemplate>
                                                                    <crosshairoptions>
                                                                        <commonlabelpositionserializable><cc1:CrosshairMousePosition /></commonlabelpositionserializable>
                                                                    </crosshairoptions>
                                                                    <tooltipoptions>
                                                                        <tooltippositionserializable><cc1:ToolTipMousePosition /></tooltippositionserializable>
                                                                    </tooltipoptions>
                                                                </dxchartsui:WebChartControl>
                                                            </dx:PanelContent>
                                                        </PanelCollection>
                                                    </dx:ASPxRoundPanel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                </table>
                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
                    <dx:TabPage Text="Exchange Server ">
                        <TabImage Url="~/images/icons/exchange.jpg">
                        </TabImage>
                        <ContentCollection>
                            <dx:ContentControl ID="ContentControl2" runat="server" SupportsDisabledAttribute="True">
                                <%--  <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                        <ContentTemplate>--%>
                                <%--  </ContentTemplate>
                        </asp:UpdatePanel>--%>
                                <table width="100%">
                                    <tr>
                                        <td style="font-size: small; font-weight: 700">
                                            <asp:Label ID="Label3" runat="server" Text="ExchangeServerHealth"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="gridCell0">
                                            <dx:ASPxGridView ID="ExchangeServerHealthGrid" runat="server" AutoGenerateColumns="False"
                                                CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                                                Cursor="pointer" KeyFieldName="ServerName" Theme="Office2003Blue" Width="100%" OnPageSizeChanged="ExchangeServerHealthGrid_PageSizeChanged">
                                                <Columns>
                                                    <dx:GridViewDataTextColumn Caption="DNS" FieldName="DNS" ShowInCustomizationForm="True"
                                                        VisibleIndex="2">
                                                        <EditCellStyle CssClass="GridCss">
                                                        </EditCellStyle>
                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                        </EditFormCaptionStyle>
                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                        <CellStyle CssClass="GridCss">
                                                        </CellStyle>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Name" FieldName="ServerName" ShowInCustomizationForm="True"
                                                        Visible="False" VisibleIndex="0">
                                                        <Settings AutoFilterCondition="Contains" />
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
                                                    
                                                    <dx:GridViewDataTextColumn Caption="Ping" FieldName="Ping" ShowInCustomizationForm="True"
                                                        VisibleIndex="3">
                                                        <Settings AutoFilterCondition="Contains" />
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
                                                    <dx:GridViewDataTextColumn Caption="Uptime" FieldName="Uptime" ShowInCustomizationForm="True"
                                                        VisibleIndex="4">
                                                       <Settings AutoFilterCondition="Contains" />
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
                                                    <dx:GridViewDataTextColumn Caption="Version" FieldName="Version" ShowInCustomizationForm="True"
                                                        VisibleIndex="5">
                                                        <EditCellStyle CssClass="GridCss">
                                                        </EditCellStyle>
                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                        </EditFormCaptionStyle>
                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                        <CellStyle CssClass="GridCss">
                                                        </CellStyle>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Roles" FieldName="Roles" ShowInCustomizationForm="True"
                                                        VisibleIndex="6">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="MailboxServerRoleServices" FieldName="MailboxServerRoleServices"
                                                        ShowInCustomizationForm="True" VisibleIndex="7">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="ClientAccessServerRoleServices" FieldName="ClientAccessServerRoleServices"
                                                        ShowInCustomizationForm="True" VisibleIndex="8">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="HubTransportServerRoleServices" FieldName="HubTransportServerRoleServices"
                                                        ShowInCustomizationForm="True" VisibleIndex="9">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="QueueLength" FieldName="QueueLength" ShowInCustomizationForm="True"
                                                        VisibleIndex="10">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="TransportQueue" FieldName="TransportQueue" ShowInCustomizationForm="True"
                                                        VisibleIndex="11">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="PFDBsMounted" FieldName="PFDBsMounted" ShowInCustomizationForm="True"
                                                        VisibleIndex="12">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="MAPITest" FieldName="MAPITest" ShowInCustomizationForm="True"
                                                        VisibleIndex="13">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="MailFlowTest" FieldName="MailFlowTest" ShowInCustomizationForm="True"
                                                        VisibleIndex="14">
                                                    </dx:GridViewDataTextColumn>
                                                </Columns>
                                                <SettingsBehavior AllowDragDrop="False" AllowFocusedRow="True" AllowSelectByRowClick="True"
                                                    AutoExpandAllGroups="True" ColumnResizeMode="NextColumn" />
                                                <SettingsPager PageSize="20" SEOFriendly="Enabled">
                                                    <PageSizeItemSettings Visible="True">
                                                    </PageSizeItemSettings>
                                                </SettingsPager>
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
                                                <Styles CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver">
                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px" CssClass="GridCssHeader">
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
                                    <tr>
                                        <td style="font-size: small; font-weight: 700">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="font-size: small; font-weight: 700">
                                            <asp:Label ID="Label4" runat="server" Text="Exchange MailBox Report"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxGridView ID="ExchangeMailBoxReportGrid" runat="server" AutoGenerateColumns="False"
                                                CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                                                Cursor="pointer" KeyFieldName="ServerName" Theme="Office2003Blue" Width="100%" OnPageSizeChanged="ExchangeMailBoxReportGrid_PageSizeChanged">
                                                <Columns>
                                                    <dx:GridViewDataTextColumn Caption="Description" FieldName="DisplayName" ShowInCustomizationForm="True"
                                                        VisibleIndex="2">
                                                        <EditCellStyle CssClass="GridCss">
                                                        </EditCellStyle>
                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                        </EditFormCaptionStyle>
                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                        <CellStyle CssClass="GridCss">
                                                        </CellStyle>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Name" FieldName="ServerName" ShowInCustomizationForm="True"
                                                        Visible="False" VisibleIndex="0">
                                                        <Settings AutoFilterCondition="Contains" />
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
                                                    <dx:GridViewDataTextColumn Caption="Service Name" FieldName="Service_Name" ShowInCustomizationForm="True"
                                                        VisibleIndex="1">
                                                        <EditCellStyle CssClass="GridCss">
                                                        </EditCellStyle>
                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                        </EditFormCaptionStyle>
                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                        <CellStyle CssClass="GridCss">
                                                        </CellStyle>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Status" FieldName="Status" ShowInCustomizationForm="True"
                                                        VisibleIndex="3">
                                                        <EditCellStyle CssClass="GridCss">
                                                        </EditCellStyle>
                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                        </EditFormCaptionStyle>
                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                        <CellStyle CssClass="GridCss1">
                                                        </CellStyle>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="StartMode" FieldName="StartMode" ShowInCustomizationForm="True"
                                                        VisibleIndex="4">
                                                        <EditCellStyle CssClass="GridCss">
                                                        </EditCellStyle>
                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                        </EditFormCaptionStyle>
                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                        <CellStyle CssClass="GridCss">
                                                        </CellStyle>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" ShowInCustomizationForm="True"
                                                        Visible="False" VisibleIndex="5">
                                                        <EditCellStyle CssClass="GridCss">
                                                        </EditCellStyle>
                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                        </EditFormCaptionStyle>
                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                        <CellStyle CssClass="GridCss">
                                                        </CellStyle>
                                                    </dx:GridViewDataTextColumn>
                                                </Columns>
                                                <SettingsBehavior AllowDragDrop="False" AllowFocusedRow="True" AllowSelectByRowClick="True"
                                                    AutoExpandAllGroups="True" ColumnResizeMode="NextColumn" />
                                                <SettingsPager PageSize="20">
                                                    <PageSizeItemSettings Visible="True">
                                                    </PageSizeItemSettings>
                                                </SettingsPager>
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
                                     <tr>
                                        <td style="font-size: small; font-weight: 700">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="font-size: small; font-weight: 700">
                                            <asp:Label ID="Label5" runat="server" 
                                                Text="Exchange DAG Health Copy Status Report"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxGridView ID="ExDAGHealthCopyStatusGrid" runat="server" AutoGenerateColumns="False"
                                                CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                                                Cursor="pointer" KeyFieldName="ServerName" Theme="Office2003Blue" 
                                                Width="100%" OnPageSizeChanged="ExDAGHealthCopyStatusGrid_PageSizeChanged">
                                                <Columns>
                                                    <dx:GridViewDataTextColumn Caption="ActiveCopy" FieldName="ActiveCopy" ShowInCustomizationForm="True"
                                                        VisibleIndex="4">
                                                        <EditCellStyle CssClass="GridCss">
                                                        </EditCellStyle>
                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                        </EditFormCaptionStyle>
                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                        <CellStyle CssClass="GridCss">
                                                        </CellStyle>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Date" FieldName="InfoDate" 
                                                        ShowInCustomizationForm="True" VisibleIndex="0">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="DatabaseName" FieldName="DatabaseName" 
                                                        ShowInCustomizationForm="True" VisibleIndex="1">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="ReplayQueueLength" 
                                                        FieldName="ReplayQueueLength" ShowInCustomizationForm="True"
                                                        VisibleIndex="2">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="DatabaseCopy" FieldName="DatabaseCopy" ShowInCustomizationForm="True"
                                                        VisibleIndex="3">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="MailboxServer" FieldName="MailboxServer" ShowInCustomizationForm="True"
                                                        VisibleIndex="5">
                                                        <EditCellStyle CssClass="GridCss">
                                                        </EditCellStyle>
                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                        </EditFormCaptionStyle>
                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                        <CellStyle CssClass="GridCss">
                                                        </CellStyle>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Status" FieldName="Status" ShowInCustomizationForm="True"
                                                        Visible="False" VisibleIndex="12">
                                                        <Settings AutoFilterCondition="Contains" />
                                                        <EditCellStyle CssClass="GridCss">
                                                        </EditCellStyle>
                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                        </EditFormCaptionStyle>
                                                        <HeaderStyle CssClass="GridCssHeader" >
                                                        <Paddings Padding="5px" />
                                                        </HeaderStyle>
                                                        <CellStyle CssClass="GridCss">
                                                        </CellStyle>
                                                        <EditCellStyle CssClass="GridCss">
                                                        </EditCellStyle>
                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                        </EditFormCaptionStyle>
                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                        <CellStyle CssClass="GridCss">
                                                        </CellStyle>
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
                                                    <dx:GridViewDataTextColumn Caption="ContentIndex" FieldName="ContentIndex" 
                                                        ShowInCustomizationForm="True" VisibleIndex="6">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="CopyQueueLength" 
                                                        FieldName="CopyQueueLength" ShowInCustomizationForm="True" 
                                                        VisibleIndex="7">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="ActivationPreference" 
                                                        FieldName="ActivationPreference" ShowInCustomizationForm="True" 
                                                        VisibleIndex="8">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="ReplayLagged" FieldName="ReplayLagged" 
                                                        ShowInCustomizationForm="True" VisibleIndex="9">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="TruncationLagged" 
                                                        FieldName="TruncationLagged" ShowInCustomizationForm="True" 
                                                        VisibleIndex="10">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="ServerId" FieldName="ServerId" 
                                                        ShowInCustomizationForm="True" Visible="False" VisibleIndex="11">
                                                    </dx:GridViewDataTextColumn>
                                                </Columns>
                                                <SettingsBehavior AllowDragDrop="False" AllowFocusedRow="True" AllowSelectByRowClick="True"
                                                    AutoExpandAllGroups="True" ColumnResizeMode="NextColumn" />
                                                <SettingsPager PageSize="20">
                                                    <PageSizeItemSettings Visible="True">
                                                    </PageSizeItemSettings>
                                                </SettingsPager>
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
                                                <Styles CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver">
                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px" CssClass="GridCssHeader">
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
                                     <tr>
                                        <td style="font-size: small; font-weight: 700">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="font-size: small; font-weight: 700">
                                            <asp:Label ID="Label8" runat="server" 
                                                Text="Exchange DAG Health Copy Summary Report"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="margin-left: 40px">
                                            <dx:ASPxGridView ID="ExDAGHealthCopySummaryGrid" runat="server" AutoGenerateColumns="False"
                                                CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                                                Cursor="pointer" KeyFieldName="ServerName" Theme="Office2003Blue" 
                                                Width="100%" OnPageSizeChanged="ExDAGHealthCopySummaryGrid_PageSizeChanged">
                                                <Columns>
                                                    <dx:GridViewDataTextColumn Caption="Preference" FieldName="Preference" ShowInCustomizationForm="True"
                                                        VisibleIndex="3">
                                                        <EditCellStyle CssClass="GridCss">
                                                        </EditCellStyle>
                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                        </EditFormCaptionStyle>
                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                        <CellStyle CssClass="GridCss">
                                                        </CellStyle>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Date" FieldName="InfoDate" 
                                                        ShowInCustomizationForm="True" VisibleIndex="0">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Database" FieldName="Databasename" 
                                                        ShowInCustomizationForm="True" VisibleIndex="1">
                                                        <Settings AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Mountedon" FieldName="Mountedon" ShowInCustomizationForm="True"
                                                        VisibleIndex="2">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="TotalCopies" FieldName="TotalCopies" ShowInCustomizationForm="True"
                                                        VisibleIndex="4">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="HealthyCopies" FieldName="HealthyCopies" ShowInCustomizationForm="True"
                                                        VisibleIndex="5">
                                                        <EditCellStyle CssClass="GridCss">
                                                        </EditCellStyle>
                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                        </EditFormCaptionStyle>
                                                        <HeaderStyle CssClass="GridCssHeader" >
                                                        <Paddings Padding="5px" />
                                                        </HeaderStyle>
                                                        <CellStyle CssClass="GridCss">
                                                        </CellStyle>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="UnhealthyCopies" 
                                                        FieldName="UnhealthyCopies" ShowInCustomizationForm="True"
                                                        Visible="False" VisibleIndex="11">
                                                        <EditCellStyle CssClass="GridCss">
                                                        </EditCellStyle>
                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                        </EditFormCaptionStyle>
                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                        <CellStyle CssClass="GridCss">
                                                        </CellStyle>
                                                        <EditCellStyle CssClass="GridCss">
                                                        </EditCellStyle>
                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                        </EditFormCaptionStyle>
                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                        <CellStyle CssClass="GridCss1">
                                                        </CellStyle>
                                                        <EditCellStyle CssClass="GridCss">
                                                        </EditCellStyle>
                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                        </EditFormCaptionStyle>
                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                        <CellStyle CssClass="GridCss">
                                                        </CellStyle>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="HealthyQueues" 
                                                        ShowInCustomizationForm="True" VisibleIndex="6">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="UnhealthyQueues" 
                                                        FieldName="UnhealthyQueues" ShowInCustomizationForm="True" 
                                                        VisibleIndex="7">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="LaggedQueues" FieldName="LaggedQueues" 
                                                        ShowInCustomizationForm="True" VisibleIndex="8">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="HealthyIndexes" FieldName="HealthyIndexes" 
                                                        ShowInCustomizationForm="True" VisibleIndex="9">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="UnhealthyIndexes" 
                                                        FieldName="UnhealthyIndexes" ShowInCustomizationForm="True" 
                                                        VisibleIndex="10">
                                                    </dx:GridViewDataTextColumn>
                                                </Columns>
                                                <SettingsBehavior AllowDragDrop="False" AllowFocusedRow="True" AllowSelectByRowClick="True"
                                                    AutoExpandAllGroups="True" ColumnResizeMode="NextColumn" />
                                                <SettingsPager PageSize="20">
                                                    <PageSizeItemSettings Visible="True">
                                                    </PageSizeItemSettings>
                                                </SettingsPager>
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
                                                <Styles CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver">
                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px" CssClass="GridCssHeader">
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
                                       <tr>
                                        <td style="font-size: small; font-weight: 700">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="font-size: small; font-weight: 700">
                                            <asp:Label ID="Label9" runat="server" Text="Exchange DAG Health Member Report"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxGridView ID="ExDAGHealthMemberReportGrid" runat="server" AutoGenerateColumns="False"
                                                CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                                                Cursor="pointer" KeyFieldName="ServerName" Theme="Office2003Blue" 
                                                Width="100%" OnPageSizeChanged="ExDAGHealthMemberReportGrid_PageSizeChanged">
                                                <Columns>
                                                    <dx:GridViewDataTextColumn Caption="ClusterService" FieldName="ClusterService" ShowInCustomizationForm="True"
                                                        VisibleIndex="2">
                                                        <EditCellStyle CssClass="GridCss">
                                                        </EditCellStyle>
                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                        </EditFormCaptionStyle>
                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                        <CellStyle CssClass="GridCss">
                                                        </CellStyle>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Date" FieldName="InfoDate" 
                                                        ShowInCustomizationForm="True" VisibleIndex="0">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Server" FieldName="Server" 
                                                        ShowInCustomizationForm="True" VisibleIndex="1">
                                                        <Settings AutoFilterCondition="Contains" />
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
                                                    <dx:GridViewDataTextColumn Caption="ReplayService" FieldName="ReplayService" ShowInCustomizationForm="True"
                                                        VisibleIndex="3">
                                                        <EditCellStyle CssClass="GridCss">
                                                        </EditCellStyle>
                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                        </EditFormCaptionStyle>
                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                        <CellStyle CssClass="GridCss1">
                                                        </CellStyle>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="ActiveManager" FieldName="ActiveManager" ShowInCustomizationForm="True"
                                                        VisibleIndex="4">
                                                        <EditCellStyle CssClass="GridCss">
                                                        </EditCellStyle>
                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                        </EditFormCaptionStyle>
                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                        <CellStyle CssClass="GridCss">
                                                        </CellStyle>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="TcpListener" FieldName="TcpListener" 
                                                        ShowInCustomizationForm="True" VisibleIndex="5">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="ServerLocatorService" 
                                                        FieldName="ServerLocatorService" ShowInCustomizationForm="True" 
                                                        VisibleIndex="6">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="DagMembersUp" FieldName="DagMembersUp" 
                                                        ShowInCustomizationForm="True" VisibleIndex="7">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="ClusterNetwork" FieldName="ClusterNetwork" 
                                                        ShowInCustomizationForm="True" VisibleIndex="8">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="QuorumGroup" FieldName="QuorumGroup" 
                                                        ShowInCustomizationForm="True" VisibleIndex="9">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="FileShareQuorum" 
                                                        FieldName="FileShareQuorum" ShowInCustomizationForm="True" VisibleIndex="10">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="DBCopySuspended" 
                                                        FieldName="DBCopySuspended" ShowInCustomizationForm="True" VisibleIndex="11">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="DBCopyFailed" FieldName="DBCopyFailed" 
                                                        ShowInCustomizationForm="True" VisibleIndex="12">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="DBInitializing" FieldName="DBInitializing" 
                                                        ShowInCustomizationForm="True" VisibleIndex="13">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="DBDisconnected" FieldName="DBDisconnected" 
                                                        ShowInCustomizationForm="True" VisibleIndex="14">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="DBLogCopyKeepingUp" 
                                                        FieldName="DBLogCopyKeepingUp" ShowInCustomizationForm="True" VisibleIndex="15">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="DBLogReplayKeepingUp" 
                                                        FieldName="DBLogReplayKeepingUp" ShowInCustomizationForm="True" 
                                                        VisibleIndex="16">
                                                    </dx:GridViewDataTextColumn>
                                                </Columns>
                                                <SettingsBehavior AllowDragDrop="False" AllowFocusedRow="True" AllowSelectByRowClick="True"
                                                    AutoExpandAllGroups="True" ColumnResizeMode="NextColumn" />
                                                <SettingsPager PageSize="20">
                                                    <PageSizeItemSettings Visible="True">
                                                    </PageSizeItemSettings>
                                                </SettingsPager>
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
                                                <Styles CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver">
                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px" CssClass="GridCssHeader">
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
                                         <tr>
                                        <td style="font-size: small; font-weight: 700">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="font-size: small; font-weight: 700">
                                            <asp:Label ID="Label6" runat="server" Text="Exchange Queues Report"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxGridView ID="ExQueuesGrid" runat="server" AutoGenerateColumns="False"
                                                CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                                                Cursor="pointer" KeyFieldName="ServerName" Theme="Office2003Blue" 
                                                Width="100%" OnPageSizeChanged="ExQueuesGrid_PageSizeChanged">
                                                <Columns>
                                                    <dx:GridViewDataTextColumn Caption="IdentityAction" FieldName="IdentityAction" ShowInCustomizationForm="True"
                                                        VisibleIndex="2">
                                                        <EditCellStyle CssClass="GridCss">
                                                        </EditCellStyle>
                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                        </EditFormCaptionStyle>
                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                        <CellStyle CssClass="GridCss">
                                                        </CellStyle>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Date" FieldName="InfoDate" 
                                                        ShowInCustomizationForm="True" VisibleIndex="0">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="IdentityName" FieldName="IdentityName" 
                                                        ShowInCustomizationForm="True" VisibleIndex="1">
                                                        <Settings AutoFilterCondition="Contains" />
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
                                                    <dx:GridViewDataTextColumn Caption="DeliveryType" FieldName="DeliveryType" ShowInCustomizationForm="True"
                                                        VisibleIndex="3">
                                                        <EditCellStyle CssClass="GridCss">
                                                        </EditCellStyle>
                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                        </EditFormCaptionStyle>
                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                        <CellStyle CssClass="GridCss1">
                                                        </CellStyle>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Status" FieldName="Status" ShowInCustomizationForm="True"
                                                        VisibleIndex="4">
                                                        <EditCellStyle CssClass="GridCss">
                                                        </EditCellStyle>
                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                        </EditFormCaptionStyle>
                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                        <CellStyle CssClass="GridCss">
                                                        </CellStyle>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="MessageCount" FieldName="MessageCount" 
                                                        ShowInCustomizationForm="True" VisibleIndex="5">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="NextHopDomain" 
                                                        FieldName="NextHopDomain" ShowInCustomizationForm="True" 
                                                        VisibleIndex="8">
                                                    </dx:GridViewDataTextColumn>
                                                </Columns>
                                                <SettingsBehavior AllowDragDrop="False" AllowFocusedRow="True" AllowSelectByRowClick="True"
                                                    AutoExpandAllGroups="True" ColumnResizeMode="NextColumn" />
                                                <SettingsPager PageSize="20">
                                                    <PageSizeItemSettings Visible="True">
                                                    </PageSizeItemSettings>
                                                </SettingsPager>
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
                                                <Styles CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver">
                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px" CssClass="GridCssHeader">
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
                                                    <asp:Label ID="Label2" runat="server" Text="Monitored Services"></asp:Label>
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
                                                <td id="gridCell">
                                                    <dx:ASPxGridView ID="MonitoredServicesGrid" runat="server" AutoGenerateColumns="False"
                                                        CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                                                        Width="100%" Cursor="pointer" KeyFieldName="ID" Theme="Office2003Blue" OnPageSizeChanged="MonitoredServicesGrid_PageSizeChanged">
                                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" ColumnResizeMode="NextColumn">
                                                        </SettingsBehavior>
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn Caption="Description" VisibleIndex="2" FieldName="DisplayName">
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Name" VisibleIndex="0" FieldName="ServerName"
                                                                Visible="False">
                                                                <Settings AutoFilterCondition="Contains" />
                                                                <Settings AutoFilterCondition="Contains" />
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
                                                            <dx:GridViewDataTextColumn Caption="Service Name" VisibleIndex="1" FieldName="Service_Name">
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Status" FieldName="Status" VisibleIndex="3">
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss1">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="StartMode" FieldName="StartMode" VisibleIndex="4">
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" ShowInCustomizationForm="True"
                                                                Visible="False" VisibleIndex="5">
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                        </Columns>
                                                        <SettingsBehavior AllowDragDrop="False" AutoExpandAllGroups="True" AllowSelectByRowClick="True" />
                                                        <SettingsPager PageSize="20" SEOFriendly="Enabled">
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
                                            <tr>
                                                <td style="font-size: small; font-weight: 700">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="font-size: small; font-weight: 700">
                                                    <asp:Label ID="Label1" runat="server" Text="Non-Monitored Services"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dx:ASPxGridView ID="NonMonitoredServicesGrid" runat="server" AutoGenerateColumns="False"
                                                        CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                                                        Cursor="pointer" KeyFieldName="ID" Theme="Office2003Blue" Width="100%" OnPageSizeChanged="NonMonitoredServicesGrid_PageSizeChanged">
                                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" ColumnResizeMode="NextColumn" />
                                                        <SettingsPager PageSize="20">
                                                            <PageSizeItemSettings Visible="True">
                                                            </PageSizeItemSettings>
                                                        </SettingsPager>
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn Caption="Description" FieldName="DisplayName" VisibleIndex="2">
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Name" FieldName="ServerName" Visible="False"
                                                                VisibleIndex="0">
                                                                <Settings AutoFilterCondition="Contains" />
                                                                <Settings AutoFilterCondition="Contains" />
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
                                                            <dx:GridViewDataTextColumn Caption="Service Name" FieldName="Service_Name" VisibleIndex="1">
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Status" FieldName="Status" VisibleIndex="3">
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss1">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="StartMode" FieldName="StartMode" VisibleIndex="4">
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" ShowInCustomizationForm="True"
                                                                Visible="False" VisibleIndex="5">
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                        </Columns>
                                                        <SettingsBehavior AllowDragDrop="False" AllowSelectByRowClick="True" AutoExpandAllGroups="True" />
                                                        <Settings ShowFilterRow="True" ShowGroupPanel="True" />
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
                                                        Width="100%" KeyFieldName="TypeandName" Theme="Office2003Blue" OnPageSizeChanged="maintenancegrid_PageSizeChanged">
                                                        <SettingsBehavior ColumnResizeMode="NextColumn"></SettingsBehavior>
                                                        <SettingsPager PageSize="50">
                                                            <PageSizeItemSettings Visible="True">
                                                            </PageSizeItemSettings>
                                                        </SettingsPager>
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn Caption="Server Name" VisibleIndex="0" FieldName="servername">
                                                                <Settings AllowDragDrop="True" AutoFilterCondition="Contains" />
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
                                                    <br />
                                                    <br />
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
                                                                <dx:ASPxLabel ID="ASPxLabel9" runat="server" CssClass="lblsmallFont" Text="The list of configured alerts that apply to the server are listed below. The list includes the last 7 days worth of information.">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxGridView ID="AlertGridView" runat="server" AutoGenerateColumns="False" EnableTheming="True"
                                                                    Theme="Office2003Blue" Width="100%" OnPageSizeChanged="AlertGridView_PageSizeChanged">
                                                                    <Columns>
                                                                        <dx:GridViewCommandColumn ShowInCustomizationForm="True" Visible="False" VisibleIndex="0">
                                                                            <ClearFilterButton Visible="True">
                                                                            </ClearFilterButton>
                                                                        </dx:GridViewCommandColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Device Name" FieldName="DeviceName" ShowInCustomizationForm="True"
                                                                            VisibleIndex="1">
                                                                            <Settings AutoFilterCondition="Contains" />
                                                                            <Settings AutoFilterCondition="Contains"></Settings>
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
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Date/Time of Alert" FieldName="DateTimeOfAlert"
                                                                            ShowInCustomizationForm="True" VisibleIndex="4">
                                                                            <EditCellStyle CssClass="GridCss">
                                                                            </EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                                            </EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                                            <CellStyle CssClass="GridCss">
                                                                            </CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                       <%-- <dx:GridViewDataTextColumn Caption="Date/Time Sent" FieldName="DateTimeSent" ShowInCustomizationForm="True"
                                                                            VisibleIndex="5">
                                                                            <EditCellStyle CssClass="GridCss">
                                                                            </EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                                            </EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                                            <CellStyle CssClass="GridCss">
                                                                            </CellStyle>
                                                                        </dx:GridViewDataTextColumn>--%>
                                                                        <dx:GridViewDataTextColumn Caption="Date/Time Alert Cleared" FieldName="DateTimeAlertCleared"
                                                                            ShowInCustomizationForm="True" VisibleIndex="6">
                                                                            <EditCellStyle CssClass="GridCss">
                                                                            </EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                                            </EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                                            <CellStyle CssClass="GridCss">
                                                                            </CellStyle>
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
                                    <tr>
                                        <td>
                                            &nbsp;
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
                                                        Theme="Office2003Blue" Width="100%" OnPageSizeChanged="OutageGridView_PageSizeChanged">
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn Caption="Server Name" FieldName="ServerName" ShowInCustomizationForm="True"
                                                                VisibleIndex="1">
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
                                                                <CellStyle CssClass="GridCss">
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
                                                                <CellStyle CssClass="GridCss">
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
                                                                <CellStyle CssClass="GridCss">
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
                    <dx:TabPage Text="Web Admin">
                        <TabImage Url="~/images/icons/domserver2.png">
                        </TabImage>
                        <ContentCollection>
                            <dx:ContentControl ID="ContentControl7" runat="server" SupportsDisabledAttribute="True">
                                <iframe runat="server" id="fraHtml"></iframe>
                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
                </TabPages>
            </dx:ASPxPageControl>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
