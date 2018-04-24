<%@ Page Title="VitalSigns Plus - Notes Mail Probe" Language="C#" MasterPageFile="~/DashboardSite.Master"
    AutoEventWireup="true" CodeBehind="NotesMailProbeDetailsPage.aspx.cs" Inherits="VSWebUI.Dashboard.NotesMailProbeDetailsPage" %>
	<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>




<%@ Register Assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
    <script type="text/javascript">
        function BackButton_Clicked(s, e) {
            window.history.back();
        }
        function Resized() {
            var callbackState = document.getElementById('ContentPlaceHolder1_callbackState').value;

            if (callbackState == 0)
                DoCallback();
        }

        function DoCallback() {
            document.getElementById('ContentPlaceHolder1_chartWidth').value = document.body.offsetWidth - 55;
            cperformanceWebChartControl.PerformCallback();
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
            var gridCell = document.getElementById('gridCell');
            ASPxClientUtils.AttachEventToElement(gridCell, 'contextmenu', OnGridContextMenu);
            //        var imgButton = document.getElementById('popupButton');
            //        ASPxClientUtils.AttachEventToElement(imgButton, 'contextmenu', OnPreventContextMenu);
        }
        function OnGridContextMenu(evt) {
            var SortPopupMenu = popupmenu;
            SortPopupMenu.ShowAtPos(evt.clientX + ASPxClientUtils.GetDocumentScrollLeft(), evt.clientY + ASPxClientUtils.GetDocumentScrollTop());
            return OnPreventContextMenu(evt);
        }
        function OnPreventContextMenu(evt) {
            return ASPxClientUtils.PreventEventAndBubble(evt);
        }
        function OnItemClick(s, e) {
            if (e.item.parent == s.GetRootItem())
                e.processOnServer = false;
           }
           sessionStorage.setItem("Force refresh", "True");
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input id="chartWidth" type="hidden" runat="server" value="400" />
    <input id="callbackState" type="hidden" runat="server" value="0" />
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
                        <td>&nbsp;</td>
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
                    ShowAsToolbar="True" onitemclick="ASPxMenu1_ItemClick" 
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
                            </Items>
                            <Image Url="~/images/icons/Gear.png">
                            </Image>
                        </dx:MenuItem>
                    </Items>
                </dx:ASPxMenu>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <table>
                    <tr>
                        <td>
                           
                <div class="info" id="NotesInfo" runat="server">This NotesMail probe tests for message flow between
                    <asp:Label ID="lblSource" runat="server" Text="Label"></asp:Label>
                    and
                    <asp:Label ID="lblDestination" runat="server" Text="Label"></asp:Label>
                    . Every
                    <asp:Label ID="lblScanInterval" runat="server" Text="Label"></asp:Label>
                    minutes a message addressed to
                    <asp:Label ID="lblNotesMailAddress" runat="server" Text="Label"></asp:Label>
                    is deposited on
                    <asp:Label ID="lblDestination0" runat="server" Text="Label"></asp:Label>
                    .
                    <br />
                    During off hours the message is sent every
                    <asp:Label ID="lblOffHoursScanInterval" runat="server" Text="Label"></asp:Label>
                    minutes. The message has
                    <asp:Label ID="lblDeliveryThreshold" runat="server" Text="Label"></asp:Label>
                    minutes to arrive in
                    <asp:Label ID="lblDestinationDatabase" runat="server" Text="Label"></asp:Label>
                    on
                    <asp:Label ID="lblDestination1" runat="server" Text="Label"></asp:Label>
                    to be considered on time.</div>
                           
                        </td>
                    </tr>
                    <%-- Added By Gopi on 02-19-2014--%>
                    <tr>
                        <td valign="top">
                            <asp:UpdatePanel ID="PerformUpdatePanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table>
                                 
                                        <tr>
                                            <td>
                                                <div class="subheader" id="lblSubtitle1" runat="server">Performance</div>
                                                <asp:Label ID="Label1" runat="server" Text="Performance" Style="font-size: medium;
                                                    color: #000000; font-weight: 8; display: none;"></asp:Label>
                                                <dxchartsui:WebChartControl ID="performanceWebChartControl" 
													ClientInstanceName="cperformanceWebChartControl" runat="server" Height="400px"
                                                    Width="1000px" CrosshairEnabled="True">
                                                    <diagramserializable>
                                                            <cc1:XYDiagram PaneLayoutDirection="Horizontal">
                                                                <AxisX Title-Text="Date" VisibleInPanesSerializable="-1" GridSpacingAuto="True" 
                                                                    DateTimeMeasureUnit="Hour" minorcount="5"><label enableantialiasing="False" Staggered="True"><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False"></resolveoverlappingoptions></label><Range SideMarginsEnabled="True"></Range><datetimeoptions format="Custom" /><datetimeoptions format="General" /></AxisX>
                                                                <AxisY Title-Text="Space(bytes)" Title-Visible="True" VisibleInPanesSerializable="-1"><label><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False"></resolveoverlappingoptions></label><Range AlwaysShowZeroLevel="False" SideMarginsEnabled="True"></Range></AxisY>
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
                            ResolveOverlappingMode="Default"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /></cc1:PointOptions></pointoptionsserializable></cc1:PointSeriesLabel></labelserializable>
                    <legendpointoptionsserializable><cc1:PointOptions><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /><argumentdatetimeoptions format="Custom" /></cc1:PointOptions></legendpointoptionsserializable>
                </seriestemplate>
                                                    <crosshairoptions><CommonLabelPositionSerializable><cc1:CrosshairMousePosition></cc1:CrosshairMousePosition></CommonLabelPositionSerializable>
</crosshairoptions>
                                                    <tooltipoptions><ToolTipPositionSerializable><cc1:ToolTipMousePosition></cc1:ToolTipMousePosition></ToolTipPositionSerializable>
</tooltipoptions>
                                                </dxchartsui:WebChartControl>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <%--   Ends Gopi on 02-19-2014--%>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="subheader" id="Div1" runat="server">NotesMail Probe History</div>
                            <asp:Label ID="Label2" runat="server" Text="Notes Mail Probe History" Style="font-size: medium;
                                color: #000000; font-weight: 8; display: none"></asp:Label>
                            <dx:ASPxGridView ID="DiskHealthGrid" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False"
                                CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                                Width="100%" KeyFieldName="ProbeID" Cursor="pointer" OnHtmlDataCellPrepared="DiskHealthGrid_HtmlDataCellPrepared"
                                OnCustomCallback="DiskHealthGrid_CustomCallback" OnHtmlRowCreated="DiskHealthGrid_HtmlRowCreated"
                                EnableTheming="True" Theme="Office2003Blue" OnPageSizeChanged="DiskHealthGrid_PageSizeChanged">
                                <%--<ClientSideEvents RowClick="function(s, e) { s.PerformCallback(e.visibleIndex); }" />--%>
                                <ClientSideEvents FocusedRowChanged="function(s, e) { edit_panel.PerformCallback(); }">
                                </ClientSideEvents>
                                <SettingsBehavior AllowDragDrop="False" AllowFocusedRow="True" AllowSelectByRowClick="True"
                                    ColumnResizeMode="NextColumn" ProcessSelectionChangedOnServer="True" AutoExpandAllGroups="True">
                                </SettingsBehavior>
                                <Settings ShowFilterRow="True" ShowGroupPanel="True"></Settings>
                                <ClientSideEvents FocusedRowChanged="function(s, e) { edit_panel.PerformCallback(); }" />
                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="ID" VisibleIndex="10" FieldName="ProbeID" Visible="False">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Sent Date" FieldName="SentDateTime" 
                                        VisibleIndex="3" Width="200px">
                                       
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
                                    <dx:GridViewDataTextColumn Caption="Device Name" VisibleIndex="1" Width="200px" FieldName="DeviceName"
                                        Visible="false">
                                        <Settings AllowAutoFilter="True" AllowDragDrop="True" AutoFilterCondition="Contains"
                                            AllowHeaderFilter="True" HeaderFilterMode="CheckedList" ShowFilterRowMenu="True"
                                            ShowInFilterControl="True" />
                                        <Settings AllowDragDrop="True" AllowAutoFilter="True" AutoFilterCondition="Contains">
                                        </Settings>
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
                                    <dx:GridViewDataTextColumn Caption="Subject Key" VisibleIndex="2" FieldName="SubjectKey"
                                        Width="400px">
                                        <Settings AllowDragDrop="True" AutoFilterCondition="Contains" />
                                        <Settings AllowDragDrop="True" AllowAutoFilter="False"></Settings>
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                        <PropertiesTextEdit DisplayFormatString="{0:P}" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Status" VisibleIndex="2" FieldName="Status" Width='250px'>
                                        <DataItemTemplate>
                                            <a class='tooltip2'>
                                                <asp:Label ID="hfNameLabel" Text='<%# Eval("Status") %>' runat="server" onmousemove="findPos(this,event,'hfNameLabel', 'detailsspan');" />
                                            </a>
                                        </DataItemTemplate>
                                        <PropertiesTextEdit>
                                            <FocusedStyle HorizontalAlign="Center">
                                            </FocusedStyle>
                                        </PropertiesTextEdit>
                                        <Settings AllowDragDrop="True" AutoFilterCondition="Contains" />
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss1">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Details" FieldName="Details"
                                        Visible="True" VisibleIndex="5" Width="300px">
                                        <Settings AllowDragDrop="True" AutoFilterCondition="Contains" />
                                        <PropertiesTextEdit DisplayFormatString="{0:P}">
                                        </PropertiesTextEdit>
                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                                <SettingsBehavior AllowFocusedRow="True" AllowDragDrop="False" AutoExpandAllGroups="True"
                                    AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True" ColumnResizeMode="NextColumn" />
                                <SettingsPager PageSize="10" SEOFriendly="Enabled">
                                    <PageSizeItemSettings Visible="true" />
                                    <PageSizeItemSettings Visible="True">
                                    </PageSizeItemSettings>
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
                                <Templates>
                                    <GroupRowContent>
                                        <%# Container.GroupText%>
                                    </GroupRowContent>
                                </Templates>
                            </dx:ASPxGridView>
                        </td>
                    </tr>
                    <tr>
                        <td hidden="hidden">
                            <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" ClientInstanceName="edit_panel" OnCallback="ASPxCallbackPanel1_Callback"
                                runat="server" Width="1001px">
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent2" runat="server">
                                        <table>
                                            <tr>
                                                <td>
                                                    <dx:ASPxLabel ID="lblServerName" runat="server" Visible="False">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <dxchartsui:WebChartControl ID="webChartDiskHealth" runat="server" Height="300px"
                                                        Width="1000px" EnableDefaultAppearance="False" PaletteName="Palette 1" OnCustomCallback="webChartDiskHealth_CustomCallback"
                                                        ClientInstanceName="cwebChartDiskHealth">
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
                            </dx:ASPxCallbackPanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
