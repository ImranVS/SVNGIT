<%@ Page Title="VitalSigns Plus - URL Health" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="URLHealth.aspx.cs" Inherits="VSWebUI.Dashboard.URLHealth" %>
<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
    <%@ Register assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>




<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>



<%@ Register assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    function Resized() {
        var callbackState = document.getElementById('ContentPlaceHolder1_callbackState').value;
        
        if (callbackState == 0)
            DoCallback();
    }

    function DoCallback() {
        //10/10/2013 NS modified
        document.getElementById('ContentPlaceHolder1_chartWidth').value = document.body.offsetWidth - 55;
        var chartwidth = document.getElementById('ContentPlaceHolder1_chartWidth').value;
        //FileOpensCumulativeWebChart.PerformCallback();
        //FileOpensWebChart.PerformCallback();
        //chttpSessionsWebChart.PerformCallback();
        //1/7/2013 NS commented out callbacks below due to an undefined object error
        //10/10/2013 NS uncommented
        cWebChartControl1.PerformCallback();
        cWebChartControl2.PerformCallback();
        
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
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<input id="chartWidth" type="hidden" runat="server" value="420" />
<input id="callbackState" type="hidden" runat="server" value="0" />
<table width="100%">
    <tr>
        <td>
            <div class="header" id="servernamelbldisp" runat="server">URL Health</div>
        </td>
        <td>
            &nbsp;
        </td>    
        <td align="right">
            <table>
                <tr>
                    <td>
                    </td>
                </tr>
            </table>
        </td>        
    </tr>
</table>
<table width="100%">
        
        
        <tr>
            <td colspan="5">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            <dx:ASPxGridView ID="urlhealthgrid" runat="server" AutoGenerateColumns="False" SummaryText="m" 
                    CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                    CssPostfix="Office2010Silver" Width="100%" OnPageSizeChanged="urlhealthgrid_PageSizeChanged"
                   
                    KeyFieldName="TypeANDName" 
                   Cursor="pointer" 
                    OnHtmlDataCellPrepared="urlhealthgrid_HtmlDataCellPrepared" 
                    OnHtmlRowCreated="urlhealthgrid_HtmlRowCreated" EnableCallBacks="False" Theme="Office2003Blue"   
                    >
                   
<SettingsBehavior AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True" 
                        ColumnResizeMode="NextColumn"></SettingsBehavior>

                    <SettingsPager PageSize="20">
<PageSizeItemSettings Visible="True"></PageSizeItemSettings>
                    </SettingsPager>
                    <Columns>
                    <dx:GridViewDataTextColumn Caption="Name" VisibleIndex="0"  
                            FieldName="Name" >
                       
                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
<Settings AllowDragDrop="True" AutoFilterCondition="Contains"></Settings>

                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" >
                            <Paddings Padding="5px" />
                            </HeaderStyle>
                            <CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="URL" VisibleIndex="2"  
                            FieldName="MailDetails">
                       
                            <Settings AllowDragDrop="False" AutoFilterCondition="Contains" />
<Settings AllowDragDrop="False" AutoFilterCondition="Contains"></Settings>

                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Status" FieldName="Status" Width='150px'
                            ShowInCustomizationForm="True" Visible="True" VisibleIndex="1">
                            <Settings AllowAutoFilter="False" AllowDragDrop="True" 
                                AutoFilterCondition="Contains" />
<Settings AllowDragDrop="True" AllowAutoFilter="False" AutoFilterCondition="Contains"></Settings>

                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss1"></CellStyle>
                        </dx:GridViewDataTextColumn>
                          <dx:GridViewDataTextColumn Caption="Category" VisibleIndex="3" 
                            FieldName="Category">
                           <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
<Settings AllowDragDrop="True" AllowAutoFilter="True" AutoFilterCondition="Contains"></Settings>

                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Location" VisibleIndex="4" 
                            FieldName="Location">
                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
<Settings AllowDragDrop="True" AllowAutoFilter="True" AutoFilterCondition="Contains"></Settings>

                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        
                        
                        
                       

                        

                    </Columns>

                    <SettingsBehavior AllowDragDrop="False" AutoExpandAllGroups="True"  AllowFocusedRow="true" 
                        AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True" />
                     <SettingsPager PageSize="10" SEOFriendly="Enabled" >
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
                    <Styles CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                        CssPostfix="Office2010Silver">
                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                        </Header>
                        <GroupRow Font-Bold="True" Font-Italic="False">
                        </GroupRow>
                        <GroupFooter Font-Bold="True">
                        </GroupFooter>
                        <AlternatingRow CssClass="GridAltRow" Enabled="True">
                        </AlternatingRow>
                        <GroupPanel Font-Bold="False">
                        </GroupPanel>
                        <LoadingPanel ImageSpacing="5px">
                        </LoadingPanel>
                    </Styles>
                    <StylesEditors ButtonEditCellSpacing="0">
                        <ProgressBar Height="21px">
                        </ProgressBar>
                    </StylesEditors>
                </dx:ASPxGridView>
            </ContentTemplate>
            </asp:UpdatePanel>
                
                <br />
                <br />
            </td>
           
        </tr>
        <tr>
            <td colspan="5" width="100%">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="urlhealthgrid" />
                </Triggers>
                <ContentTemplate>
                    <table>
                        <tr>
                        
                        <td>
                        <dxchartsui:WebChartControl ID="WebChartControl1" runat="server" Visible="False" 
                                Width="1000px" ClientInstanceName="cWebChartControl1" Height="300px" 
                                OnCustomCallback="WebChartControl1_CustomCallback" CrosshairEnabled="True">


                            <diagramserializable>
                                <cc2:XYDiagram>
                                    <axisx datetimegridalignment="Hour" datetimemeasureunit="Minute" 
                                        visibleinpanesserializable="-1">
                                        <tickmarks minorvisible="False" />
                                        <tickmarks minorvisible="False" /><label>
                                        <resolveoverlappingoptions allowrotate="False" />
                                        <resolveoverlappingoptions allowrotate="False" /></label>
                                        <range sidemarginsenabled="True" />
                                        <datetimeoptions format="ShortTime" />
                                    <range sidemarginsenabled="True" /><datetimeoptions format="ShortTime" /></axisx>
                                    <axisy visibleinpanesserializable="-1">
                                        <range sidemarginsenabled="True" />
                                    <range sidemarginsenabled="True" /></axisy>
                                </cc2:XYDiagram>
                            </diagramserializable>
<FillStyle><OptionsSerializable>
<cc2:SolidFillOptions></cc2:SolidFillOptions>
</OptionsSerializable>
</FillStyle>

                            <seriesserializable>
                                <cc2:Series ArgumentScaleType="DateTime" Name="Series 1">
                                    <viewserializable>
                                        <cc2:LineSeriesView>
                                        </cc2:LineSeriesView>
                                    </viewserializable>
                                    <labelserializable>
                                        <cc2:PointSeriesLabel LineVisible="True">
                                            <fillstyle>
                                                <optionsserializable>
                                                    <cc2:SolidFillOptions />
                                                </optionsserializable>
                                            </fillstyle>
                                            <pointoptionsserializable>
                                                <cc2:PointOptions>
                                                </cc2:PointOptions>
                                            </pointoptionsserializable>
                                        </cc2:PointSeriesLabel>
                                    </labelserializable>
                                    <legendpointoptionsserializable>
                                        <cc2:PointOptions>
                                        </cc2:PointOptions>
                                    </legendpointoptionsserializable>
                                </cc2:Series>
                                <cc2:Series ArgumentScaleType="DateTime" Name="Series 2">
                                    <viewserializable>
                                        <cc2:LineSeriesView>
                                        </cc2:LineSeriesView>
                                    </viewserializable>
                                    <labelserializable>
                                        <cc2:PointSeriesLabel LineVisible="True">
                                            <fillstyle>
                                                <optionsserializable>
                                                    <cc2:SolidFillOptions />
                                                </optionsserializable>
                                            </fillstyle>
                                            <pointoptionsserializable>
                                                <cc2:PointOptions>
                                                </cc2:PointOptions>
                                            </pointoptionsserializable>
                                        </cc2:PointSeriesLabel>
                                    </labelserializable>
                                    <legendpointoptionsserializable>
                                        <cc2:PointOptions>
                                        </cc2:PointOptions>
                                    </legendpointoptionsserializable>
                                </cc2:Series>
                            </seriesserializable>

<SeriesTemplate><ViewSerializable>
    <cc2:LineSeriesView>
    </cc2:LineSeriesView>
</ViewSerializable>
<LabelSerializable>
    <cc2:PointSeriesLabel LineVisible="True">
        <fillstyle>
            <optionsserializable>
                <cc2:SolidFillOptions />
            </optionsserializable>
        </fillstyle>
        <pointoptionsserializable>
            <cc2:PointOptions>
            </cc2:PointOptions>
        </pointoptionsserializable>
    </cc2:PointSeriesLabel>
</LabelSerializable>
<LegendPointOptionsSerializable>
<cc2:PointOptions></cc2:PointOptions>
</LegendPointOptionsSerializable>
</SeriesTemplate>


<CrosshairOptions><CommonLabelPositionSerializable>
<cc2:CrosshairMousePosition></cc2:CrosshairMousePosition>
</CommonLabelPositionSerializable>
</CrosshairOptions>

<ToolTipOptions><ToolTipPositionSerializable>
<cc2:ToolTipMousePosition></cc2:ToolTipMousePosition>
</ToolTipPositionSerializable>
</ToolTipOptions>
                </dxchartsui:WebChartControl>
                        </td></tr>
                       <tr>
                        <td><dxchartsui:WebChartControl ID="WebChartControl2" runat="server" Width="1000px" 
                                Visible="False" ClientInstanceName="cWebChartControl2" 
                                Height="300px" OnCustomCallback="WebChartControl2_CustomCallback" 
								CrosshairEnabled="True">
                            <diagramserializable>
                                <cc2:XYDiagram>
                                    <axisx datetimegridalignment="Hour" datetimemeasureunit="Minute" 
                                        visibleinpanesserializable="-1">
                                        <label>
                                        <resolveoverlappingoptions allowrotate="False" />
                                        <resolveoverlappingoptions allowrotate="False"></resolveoverlappingoptions>
                                        </label>
                                        <range sidemarginsenabled="True" />
                                        <range sidemarginsenabled="True" />
                                        <datetimeoptions format="Custom" />
                                        <range sidemarginsenabled="True" />
                                        <datetimeoptions format="ShortTime" />
                                    </axisx>
                                    <axisy visibleinpanesserializable="-1">
                                        <range sidemarginsenabled="True" auto="False" maxvalueserializable="100" minvalueserializable="0" />
                                        <range auto="False" maxvalueserializable="110" minvalueserializable="0" 
                                            sidemarginsenabled="True" />
                                        <range auto="False" maxvalueserializable="110" minvalueserializable="0" 
                                            sidemarginsenabled="True" />
                                    </axisy>
                                </cc2:XYDiagram>
                            </diagramserializable>
<FillStyle><OptionsSerializable>
<cc2:SolidFillOptions></cc2:SolidFillOptions>
</OptionsSerializable>
</FillStyle>

                            <seriesserializable>
                                <cc2:Series ArgumentScaleType="DateTime" Name="Series 1">
                                    <viewserializable>
                                        <cc2:LineSeriesView>
                                        </cc2:LineSeriesView>
                                    </viewserializable>
                                    <labelserializable>
                                        <cc2:PointSeriesLabel LineVisible="True">
                                            <fillstyle>
                                                <optionsserializable>
                                                    <cc2:SolidFillOptions />
                                                </optionsserializable>
                                            </fillstyle>
                                            <pointoptionsserializable>
                                                <cc2:PointOptions>
                                                </cc2:PointOptions>
                                            </pointoptionsserializable>
                                        </cc2:PointSeriesLabel>
                                    </labelserializable>
                                    <legendpointoptionsserializable>
                                        <cc2:PointOptions>
                                        </cc2:PointOptions>
                                    </legendpointoptionsserializable>
                                </cc2:Series>
                                <cc2:Series ArgumentScaleType="DateTime" Name="Series 2">
                                    <viewserializable>
                                        <cc2:LineSeriesView>
                                        </cc2:LineSeriesView>
                                    </viewserializable>
                                    <labelserializable>
                                        <cc2:PointSeriesLabel LineVisible="True">
                                            <fillstyle>
                                                <optionsserializable>
                                                    <cc2:SolidFillOptions />
                                                </optionsserializable>
                                            </fillstyle>
                                            <pointoptionsserializable>
                                                <cc2:PointOptions>
                                                </cc2:PointOptions>
                                            </pointoptionsserializable>
                                        </cc2:PointSeriesLabel>
                                    </labelserializable>
                                    <legendpointoptionsserializable>
                                        <cc2:PointOptions>
                                        </cc2:PointOptions>
                                    </legendpointoptionsserializable>
                                </cc2:Series>
                            </seriesserializable>

<SeriesTemplate><ViewSerializable>
    <cc2:LineSeriesView>
    </cc2:LineSeriesView>
</ViewSerializable>
<LabelSerializable>
    <cc2:PointSeriesLabel LineVisible="True">
        <fillstyle>
            <optionsserializable>
                <cc2:SolidFillOptions />
            </optionsserializable>
        </fillstyle>
        <pointoptionsserializable>
            <cc2:PointOptions>
            </cc2:PointOptions>
        </pointoptionsserializable>
    </cc2:PointSeriesLabel>
</LabelSerializable>
<LegendPointOptionsSerializable>
<cc2:PointOptions></cc2:PointOptions>
</LegendPointOptionsSerializable>
</SeriesTemplate>

<CrosshairOptions><CommonLabelPositionSerializable>
<cc2:CrosshairMousePosition></cc2:CrosshairMousePosition>
</CommonLabelPositionSerializable>
</CrosshairOptions>

<ToolTipOptions><ToolTipPositionSerializable>
<cc2:ToolTipMousePosition></cc2:ToolTipMousePosition>
</ToolTipPositionSerializable>
</ToolTipOptions>
                </dxchartsui:WebChartControl></td></tr>
                
                </table>
                </ContentTemplate>
                </asp:UpdatePanel>
                        
            </td>
            </tr>
        
    </table>
</asp:Content>
