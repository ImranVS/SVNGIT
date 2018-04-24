<%@ Page Title="Vitalsigns Plus - Quickr Health" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true"
    CodeBehind="QuickrHealth.aspx.cs" Inherits="VSWebUI.Dashboard.QuickrHealth" %>
	
<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>




<%@ Register Assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
    
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
        }
      
        function Resized() {
            var callbackState = document.getElementById('ContentPlaceHolder1_callbackState').value;

            if (callbackState == 0)
                DoCallback();
        }

        function DoCallback() {
            document.getElementById('ContentPlaceHolder1_chartWidth').value = document.body.offsetWidth - 105;
            //cSelectServerRoundPanel.style.width = document.getElementById('ContentPlaceHolder1__chartWidth').value + "px";
           // FileOpensCumulativeWebChart.PerformCallback();
            cWebChartResponseTime.PerformCallback();
        }

        function ResizeChart(s, e) {
            document.getElementById('ContentPlaceHolder1_callbackState').value = 0;
            s.GetMainElement().style.width = document.getElementById('ContentPlaceHolder1__chartWidth').value + "px";
            //cSelectServerRoundPanel.style.width = document.getElementById('ContentPlaceHolder1__chartWidth').value + "px";
        }

        function ResetCallbackState() {
            window.document.form1.elements['ContentPlaceHolder1_callbackState'].value = 1;
        }
    </script>
    <table width="100%">
        <tr>
            <td>
                <img alt="" src="../images/icons/quickr.gif" />&nbsp;
                <asp:Label ID="ASPLabel1" runat="server" Text="IBM Quickr Server Health" 
        Font-Bold="True" Font-Size="Large" style="color: #000000; font-family: Verdana"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
            
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                <dx:ASPxGridView ID="QuickrServersGrid" ClientInstanceName="grid" runat="server"
                                            AutoGenerateColumns="False" EnableTheming="True" 
                        Theme="Office2003Blue" Width="100%" OnPageSizeChanged="QuickrServersGrid_PageSizeChanged"
                                            KeyFieldName="TypeANDName" 
                                            OnHtmlRowCreated="QuickrServersGrid_HtmlRowCreated">
                                          
                                            <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                                            <Columns>
                                              
                                                <dx:GridViewDataHyperLinkColumn Caption="Name" FieldName="Name" 
                                                    VisibleIndex="0" Width="15%">
                                                    <Settings AllowAutoFilter="False" />
                                                    <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCssHeader" >
                                                    <Paddings Padding="5px" />
                                                    </HeaderStyle>
                                                    <CellStyle CssClass="GridCss" VerticalAlign="Top">
                                                    </CellStyle>
                                                    <PropertiesHyperLinkEdit NavigateUrlFormatString="DominoServerDetailsPage2.aspx?Name={0}&Type=Domino"
                                                        Target="_self" TextField="Name" TextFormatString="">
                                                    </PropertiesHyperLinkEdit>
                                                </dx:GridViewDataHyperLinkColumn>
                                                <dx:GridViewDataTextColumn Caption="CPU" FieldName="CPU" ShowInCustomizationForm="True"
                                                    VisibleIndex="5">
                                                    <Settings AllowAutoFilter="False" />
                                                    <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                    <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                    </CellStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Details" FieldName="Details" ShowInCustomizationForm="True"
                                                    VisibleIndex="2" Width="50%">
                                                    <Settings AllowAutoFilter="False" />
                                                    <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                    <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                    </CellStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Location" FieldName="Location" ShowInCustomizationForm="True"
                                                    VisibleIndex="1">
                                                    <Settings AutoFilterCondition="Contains" />
                                                    <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                    <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                    </CellStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Domino Version" FieldName="DominoVersion" ShowInCustomizationForm="True"
                                                    VisibleIndex="4">
                                                    <Settings AllowAutoFilter="False" />
                                                    <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                    <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                    </CellStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="User Count" FieldName="UserCount" ShowInCustomizationForm="True"
                                                    VisibleIndex="3">
                                                    <Settings AllowAutoFilter="False" />
                                                    <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                    <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                    </CellStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Memory" FieldName="Memory" ShowInCustomizationForm="True"
                                                    VisibleIndex="7">
                                                    <Settings AllowAutoFilter="False" />
                                                    <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                    <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                    </CellStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="TypeANDName" FieldName="TypeANDName" ShowInCustomizationForm="True"
                                                    Visible="False" VisibleIndex="10">
                                                    <CellStyle VerticalAlign="Top">
                                                    </CellStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Last Update" FieldName="LastUpdate" 
                                                    ShowInCustomizationForm="True" Visible="False" VisibleIndex="9">
                                                </dx:GridViewDataTextColumn>
                                            </Columns>
                                            <SettingsBehavior AllowFocusedRow="True" AllowDragDrop="False" 
                                                AutoExpandAllGroups="True" AllowSelectByRowClick="True" 
                                                ProcessSelectionChangedOnServer="True" ColumnResizeMode="NextColumn" />
                                            <SettingsPager PageSize="10">
                                                <PageSizeItemSettings Visible="True">
                                                </PageSizeItemSettings>
                                            </SettingsPager>
                                            <Styles>
                                              <AlternatingRow CssClass="GridAltrow" Enabled="True">
                        </AlternatingRow>
                                            </Styles>
                                            <Settings ShowGroupPanel="True" ShowHorizontalScrollBar="True" />
                                        </dx:ASPxGridView>
                </ContentTemplate>
                </asp:UpdatePanel>
                
            </td>
        </tr>
        </table>
        <table width="100%">  
        <tr>
            <td>
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <dx:ASPxRoundPanel ID="ResponseTimeRoundPanel" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                            CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Response Time"
                            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                            <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                            <HeaderStyle Height="23px">
                                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                            </HeaderStyle>
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                                    <br />
                                    <br />
                                    <br />
                                    <dxchartsui:WebChartControl ID="WebChartResponseTime" runat="server" Height="300px" 
                                        ClientInstanceName="cWebChartResponseTime" Width="550px">
                                        <diagramserializable>
                                            <cc1:XYDiagram>
                                                <axisx datetimegridalignment="Hour" datetimemeasureunit="Minute" 
                                                    visibleinpanesserializable="-1"><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><label><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False"></resolveoverlappingoptions></label><range sidemarginsenabled="True" /><datetimeoptions format="ShortTime" /><range sidemarginsenabled="True" /><datetimeoptions format="ShortTime" /><range sidemarginsenabled="True" /><datetimeoptions format="ShortTime" /><range sidemarginsenabled="True" /><datetimeoptions format="ShortTime" /><range sidemarginsenabled="True" /><datetimeoptions format="ShortTime" /><range sidemarginsenabled="True" /><datetimeoptions format="ShortTime" /><range sidemarginsenabled="True" /><datetimeoptions format="ShortTime" /></axisx>
                                                <axisy visibleinpanesserializable="-1"><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /></axisy>
                                            </cc1:XYDiagram>
                                        </diagramserializable>
                                        <fillstyle><OptionsSerializable><cc1:SolidFillOptions></cc1:SolidFillOptions></OptionsSerializable>
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
                                        <seriestemplate><ViewSerializable><cc1:LineSeriesView></cc1:LineSeriesView></ViewSerializable>
                                        <LabelSerializable><cc1:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:PointSeriesLabel></LabelSerializable>
                                        <LegendPointOptionsSerializable><cc1:PointOptions></cc1:PointOptions></LegendPointOptionsSerializable>
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
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="QuickrServersGrid" />
                    </Triggers>
                </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <dx:ASPxRoundPanel ID="httpSessionsASPxRoundPanel" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                            CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="HTTP Sessions"
                            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                            <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                            <HeaderStyle Height="23px">
                                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                            </HeaderStyle>
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                    <table>
                                        <tr>
                                            <td>
                                                <dx:ASPxRadioButtonList ID="httpSessionsASPxRadioButtonList" runat="server" ValueType="System.String"
                                                    AutoPostBack="True" CssClass="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                    CssPostfix="Glass" RepeatDirection="Horizontal" SelectedIndex="0" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                                                    OnSelectedIndexChanged="httpSessionsASPxRadioButtonList_SelectedIndexChanged">
                                                    <Items>
                                                        <dx:ListEditItem Selected="True" Value="hh" Text="Per Hour" />
                                                        <dx:ListEditItem Value="dd" Text="Per Day" />
                                                    </Items>
                                                </dx:ASPxRadioButtonList>
                                            </td>
                                        </tr>
                                    </table>
                                    <dxchartsui:WebChartControl ID="httpSessionsWebChart" runat="server" Height="300px"
                                        ClientInstanceName="chttpSessionsWebChart" Width="550px">
                                        <diagramserializable>
                                                                                                <cc1:XYDiagram LabelsResolveOverlappingMinIndent="1" PaneLayoutDirection="Horizontal">
                                                                                                    <AxisX Title-Text="Date" VisibleInPanesSerializable="-1" 
                                                                                                        GridSpacingAuto="False" GridSpacing="0.5" MinorCount="15" 
                                                                                                        DateTimeMeasureUnit="Minute" datetimegridalignment="Hour">
                                                                                                        <tickmarks minorvisible="False" />
                                                                                                        <tickmarks minorvisible="False" />
                                                                                                        <tickmarks minorvisible="False" />
                                                                                                        <tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" />
                                                                                                        <tickmarks minorvisible="False" />
                                                                                                        <label enableantialiasing="False">
                                                                                                        <resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" />
                                                                                                        <resolveoverlappingoptions allowrotate="False"></resolveoverlappingoptions>
                                                                                                        </label>
                                                                                                        <Range SideMarginsEnabled="True"></Range>
                                                                                                        <datetimeoptions format="ShortTime" />
                                                                                                        <datetimeoptions format="ShortTime" />
                                                                                                        <datetimeoptions format="ShortTime" />
                                                                                                    <datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" />
                                                                                                        <datetimeoptions format="ShortTime" />
                                                                                                    </AxisX>
                                                                                                    <AxisY Title-Text="Space (bytes)" Title-Visible="True" 
                                                                                                        VisibleInPanesSerializable="-1" gridspacingauto="False">
                                                                                                        <Range AlwaysShowZeroLevel="False" SideMarginsEnabled="True"></Range>
                                                                                                    </AxisY>
                                                                                                </cc1:XYDiagram>
                                                                                            </diagramserializable>
                                        <fillstyle><OptionsSerializable>
                                    <cc1:SolidFillOptions></cc1:SolidFillOptions>
                                    </OptionsSerializable>
                                    </fillstyle>
                                        <seriesserializable>
                                            <cc1:Series ArgumentScaleType="DateTime" Name="Series 1">
                                                <viewserializable>
                                                    <cc1:LineSeriesView>
                                                    </cc1:LineSeriesView>
                                                </viewserializable>
                                                <labelserializable>
                                                    <cc1:PointSeriesLabel Angle="90" LineVisible="False">
                                                        <fillstyle>
                                                            <optionsserializable>
                                                                <cc1:SolidFillOptions />
                                                            </optionsserializable>
                                                        </fillstyle>
                                                        <pointoptionsserializable>
                                                            <cc1:PointOptions>
                                                            </cc1:PointOptions>
                                                        </pointoptionsserializable>
                                                    </cc1:PointSeriesLabel>
                                                </labelserializable>
                                                <legendpointoptionsserializable>
                                                    <cc1:PointOptions>
                                                    </cc1:PointOptions>
                                                </legendpointoptionsserializable>
                                            </cc1:Series>
                                            <cc1:Series ArgumentScaleType="DateTime" Name="Series 2">
                                                <viewserializable>
                                                    <cc1:LineSeriesView>
                                                    </cc1:LineSeriesView>
                                                </viewserializable>
                                                <labelserializable>
                                                    <cc1:PointSeriesLabel Angle="90" LineVisible="False">
                                                        <fillstyle>
                                                            <optionsserializable>
                                                                <cc1:SolidFillOptions />
                                                            </optionsserializable>
                                                        </fillstyle>
                                                        <pointoptionsserializable>
                                                            <cc1:PointOptions>
                                                            </cc1:PointOptions>
                                                        </pointoptionsserializable>
                                                    </cc1:PointSeriesLabel>
                                                </labelserializable>
                                                <legendpointoptionsserializable>
                                                    <cc1:PointOptions>
                                                    </cc1:PointOptions>
                                                </legendpointoptionsserializable>
                                            </cc1:Series>
                                        </seriesserializable>
                                        <seriestemplate><ViewSerializable>
                                            <cc1:LineSeriesView>
                                            </cc1:LineSeriesView>
                                    </ViewSerializable>
                                    <LabelSerializable>
                                        <cc1:PointSeriesLabel Angle="90" LineVisible="True">
                                            <fillstyle>
                                                <optionsserializable>
                                                    <cc1:SolidFillOptions />
                                                </optionsserializable>
                                            </fillstyle>
                                            <pointoptionsserializable>
                                                <cc1:PointOptions>
                                                </cc1:PointOptions>
                                            </pointoptionsserializable>
                                        </cc1:PointSeriesLabel>
                                    </LabelSerializable>
                                    <LegendPointOptionsSerializable>
                                    <cc1:PointOptions></cc1:PointOptions>
                                    </LegendPointOptionsSerializable>
                                    </seriestemplate>
                                        <crosshairoptions><CommonLabelPositionSerializable>
                                    <cc1:CrosshairMousePosition></cc1:CrosshairMousePosition>
                                    </CommonLabelPositionSerializable>
                                    </crosshairoptions>
                                        <tooltipoptions><ToolTipPositionSerializable>
                                    <cc1:ToolTipMousePosition></cc1:ToolTipMousePosition>
                                    </ToolTipPositionSerializable>
                                    </tooltipoptions>
                                    </dxchartsui:WebChartControl>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="QuickrServersGrid" />
                    </Triggers>
                </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <dx:ASPxRoundPanel ID="roundPanelCPU" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                            CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="CPU" 
                            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%"
                            >
                            <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                            <HeaderStyle Height="23px">
                                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                            </HeaderStyle>
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                                    <dxchartsui:WebChartControl ID="webChartCPU" runat="server" Height="300px" 
                                        ClientInstanceName="cwebChartCPU" Width="550px">
                                        <diagramserializable>
                                            <cc1:XYDiagram>
                                                <axisx datetimegridalignment="Hour" datetimemeasureunit="Minute" 
                                                    visibleinpanesserializable="-1">
                                                    <tickmarks minorvisible="False" />
                                                    <tickmarks minorvisible="False" />
                                                    <tickmarks minorvisible="False" />
                                                    <tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" />
                                                    <tickmarks minorvisible="False" />
                                                    <label>
                                                    <resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" />
                                                    <resolveoverlappingoptions allowrotate="False"></resolveoverlappingoptions>
                                                    </label>
                                                    <range sidemarginsenabled="True" />
                                                    <datetimeoptions format="ShortTime" />
                                                    <range sidemarginsenabled="True" />
                                                    <datetimeoptions format="ShortTime" />
                                                    <range sidemarginsenabled="True" />
                                                    <datetimeoptions format="ShortTime" />
                                                <range sidemarginsenabled="True" /><datetimeoptions format="ShortTime" /><range sidemarginsenabled="True" /><datetimeoptions format="ShortTime" /><range sidemarginsenabled="True" /><datetimeoptions format="ShortTime" />
                                                    <range sidemarginsenabled="True" />
                                                    <datetimeoptions format="ShortTime" />
                                                </axisx>
                                                <axisy visibleinpanesserializable="-1">
                                                    <range sidemarginsenabled="True" />
                                                    <range sidemarginsenabled="True" />
                                                    <range sidemarginsenabled="True" />
                                                <range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" />
                                                    <range sidemarginsenabled="True" />
                                                </axisy>
                                            </cc1:XYDiagram>
                                        </diagramserializable>
                                        <fillstyle><OptionsSerializable><cc1:SolidFillOptions></cc1:SolidFillOptions></OptionsSerializable>
                                        </fillstyle>
                                        <seriesserializable>
                                            <cc1:Series ArgumentScaleType="DateTime" Name="Series 1">
                                                <viewserializable>
                                                    <cc1:LineSeriesView>
                                                    </cc1:LineSeriesView>
                                                </viewserializable>
                                                <labelserializable>
                                                    <cc1:PointSeriesLabel LineVisible="True">
                                                        <fillstyle>
                                                            <optionsserializable>
                                                                <cc1:SolidFillOptions />
                                                            </optionsserializable>
                                                        </fillstyle>
                                                        <pointoptionsserializable>
                                                            <cc1:PointOptions>
                                                            </cc1:PointOptions>
                                                        </pointoptionsserializable>
                                                    </cc1:PointSeriesLabel>
                                                </labelserializable>
                                                <legendpointoptionsserializable>
                                                    <cc1:PointOptions>
                                                    </cc1:PointOptions>
                                                </legendpointoptionsserializable>
                                            </cc1:Series>
                                            <cc1:Series ArgumentScaleType="DateTime" Name="Series 2">
                                                <viewserializable>
                                                    <cc1:LineSeriesView>
                                                    </cc1:LineSeriesView>
                                                </viewserializable>
                                                <labelserializable>
                                                    <cc1:PointSeriesLabel LineVisible="True">
                                                        <fillstyle>
                                                            <optionsserializable>
                                                                <cc1:SolidFillOptions />
                                                            </optionsserializable>
                                                        </fillstyle>
                                                        <pointoptionsserializable>
                                                            <cc1:PointOptions>
                                                            </cc1:PointOptions>
                                                        </pointoptionsserializable>
                                                    </cc1:PointSeriesLabel>
                                                </labelserializable>
                                                <legendpointoptionsserializable>
                                                    <cc1:PointOptions>
                                                    </cc1:PointOptions>
                                                </legendpointoptionsserializable>
                                            </cc1:Series>
                                        </seriesserializable>
                                        <seriestemplate><ViewSerializable>
                                            <cc1:LineSeriesView>
                                            </cc1:LineSeriesView>
                                            </ViewSerializable>
                                        <LabelSerializable>
                                            <cc1:PointSeriesLabel LineVisible="True">
                                                <fillstyle>
                                                    <optionsserializable>
                                                        <cc1:SolidFillOptions />
                                                    </optionsserializable>
                                                </fillstyle>
                                                <pointoptionsserializable>
                                                    <cc1:PointOptions>
                                                    </cc1:PointOptions>
                                                </pointoptionsserializable>
                                            </cc1:PointSeriesLabel>
                                            </LabelSerializable>
                                        <LegendPointOptionsSerializable><cc1:PointOptions></cc1:PointOptions></LegendPointOptionsSerializable>
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
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="QuickrServersGrid" />
                    </Triggers>
                </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <dx:ASPxRoundPanel ID="roundPanelMemory" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                            CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Memory" 
                            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%"
                            >
                            <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                            <HeaderStyle Height="23px">
                                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                            </HeaderStyle>
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
                                    <dxchartsui:WebChartControl ID="webChartMemory" runat="server" Height="300px" 
                                        ClientInstanceName="cwebChartMemory" Width="550px">
                                        <diagramserializable>
                                            <cc1:XYDiagram>
                                                <axisx visibleinpanesserializable="-1">
                                                    <tickmarks minorvisible="False" />
                                                    <tickmarks minorvisible="False" />
                                                    <tickmarks minorvisible="False" />
                                                    <tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" />
                                                    <tickmarks minorvisible="False" />
                                                    <label>
                                                    <resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" />
                                                    <resolveoverlappingoptions allowrotate="False"></resolveoverlappingoptions>
                                                    </label>
                                                    <range sidemarginsenabled="True" />
                                                    <range sidemarginsenabled="True" />
                                                    <range sidemarginsenabled="True" />
                                                <range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" />
                                                    <range sidemarginsenabled="True" />
                                                </axisx>
                                                <axisy visibleinpanesserializable="-1">
                                                    <range sidemarginsenabled="True" />
                                                    <range sidemarginsenabled="True" />
                                                    <range sidemarginsenabled="True" />
                                                <range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" />
                                                    <range sidemarginsenabled="True" />
                                                </axisy>
                                            </cc1:XYDiagram>
                                        </diagramserializable>
                                        <fillstyle><OptionsSerializable><cc1:SolidFillOptions></cc1:SolidFillOptions></OptionsSerializable>
                                        </fillstyle>
                                        <seriesserializable>
                                            <cc1:Series Name="Series 1">
                                                <viewserializable>
                                                    <cc1:SideBySideBarSeriesView>
                                                    </cc1:SideBySideBarSeriesView>
                                                </viewserializable>
                                                <labelserializable>
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
                                                </labelserializable>
                                                <legendpointoptionsserializable>
                                                    <cc1:PointOptions>
                                                    </cc1:PointOptions>
                                                </legendpointoptionsserializable>
                                            </cc1:Series>
                                            <cc1:Series Name="Series 2">
                                                <viewserializable>
                                                    <cc1:SideBySideBarSeriesView>
                                                    </cc1:SideBySideBarSeriesView>
                                                </viewserializable>
                                                <labelserializable>
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
                                                </labelserializable>
                                                <legendpointoptionsserializable>
                                                    <cc1:PointOptions>
                                                    </cc1:PointOptions>
                                                </legendpointoptionsserializable>
                                            </cc1:Series>
                                        </seriesserializable>
                                        <seriestemplate><ViewSerializable><cc1:SideBySideBarSeriesView></cc1:SideBySideBarSeriesView></ViewSerializable>
                                        <LabelSerializable><cc1:SideBySideBarSeriesLabel LineVisible="True"><FillStyle><OptionsSerializable><cc1:SolidFillOptions></cc1:SolidFillOptions></OptionsSerializable></FillStyle><PointOptionsSerializable><cc1:PointOptions></cc1:PointOptions></PointOptionsSerializable></cc1:SideBySideBarSeriesLabel></LabelSerializable>
                                        <LegendPointOptionsSerializable><cc1:PointOptions></cc1:PointOptions></LegendPointOptionsSerializable>
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
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="QuickrServersGrid" />
                    </Triggers>
                </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                            <table width="100%">
        <tr>
            <td colspan="2" align="left">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <dx:ASPxRoundPanel ID="roundPanelQuickrPlaces" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                            CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Quickr Places"
                            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%" 
                            Visible="False">
                            <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                            <HeaderStyle Height="23px">
                                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                            </HeaderStyle>
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent5" runat="server" SupportsDisabledAttribute="True">
                                    <dx:ASPxGridView ID="QuickrPlacesGrid" runat="server" AutoGenerateColumns="False"
                                        EnableTheming="True" Theme="Office2003Blue" Width="100%" OnPageSizeChanged="QuickrPlacesGrid_PageSizeChanged" 
                                        KeyFieldName="PlaceKey" OnHtmlRowCreated="QuickrPlacesGrid_HtmlRowCreated">
                                        <Columns>
                                            <dx:GridViewCommandColumn ShowInCustomizationForm="True" Visible="False" 
                                                VisibleIndex="0">
                                                <ClearFilterButton Visible="True">
                                                </ClearFilterButton>
                                            </dx:GridViewCommandColumn>
                                            <dx:GridViewDataTextColumn Caption="Place Key" FieldName="PlaceKey" 
                                                ShowInCustomizationForm="True" VisibleIndex="1" FixedStyle="Left">
                                                <Settings AllowAutoFilter="False" />
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" />
                                                <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Server Name" FieldName="ServerName" ShowInCustomizationForm="True"
                                                VisibleIndex="2" FixedStyle="Left">
                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" />
                                                <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Place Name" FieldName="PlaceName" ShowInCustomizationForm="True"
                                                VisibleIndex="3">
                                                <Settings AllowAutoFilter="False" />
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" />
                                                <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Place Title" FieldName="PlaceTitle" ShowInCustomizationForm="True"
                                                VisibleIndex="4">
                                                <Settings AllowAutoFilter="False" />
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" />
                                                <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn ShowInCustomizationForm="True" VisibleIndex="5" 
                                                Caption="Place Title" FieldName="PlaceTitle">
                                                <Settings AllowAutoFilter="False" />
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" />
                                                <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Place Description" 
                                                FieldName="PlaceDescription" ShowInCustomizationForm="True" 
                                                VisibleIndex="6" Width="150px">
                                                <Settings AllowAutoFilter="False" />
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" />
                                                <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Place Type" FieldName="PlaceType" 
                                                ShowInCustomizationForm="True" VisibleIndex="7">
                                                <Settings AllowAutoFilter="False" />
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" />
                                                <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Place Owner" FieldName="PlaceOwner" 
                                                ShowInCustomizationForm="True" VisibleIndex="8">
                                                <Settings AllowAutoFilter="False" />
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" />
                                                <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Place Managers" FieldName="PlaceManagers" 
                                                ShowInCustomizationForm="True" VisibleIndex="9">
                                                <Settings AllowAutoFilter="False" />
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" />
                                                <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Place Created" FieldName="PlaceCreated" 
                                                ShowInCustomizationForm="True" VisibleIndex="10">
                                                <Settings AllowAutoFilter="False" />
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" />
                                                <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Place Host Name" FieldName="PlaceHostName" 
                                                ShowInCustomizationForm="True" VisibleIndex="11" Width="150px">
                                                <Settings AllowAutoFilter="False" />
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" />
                                                <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Place Bot Count" FieldName="PlaceBotCount" 
                                                ShowInCustomizationForm="True" VisibleIndex="12">
                                                <Settings AllowAutoFilter="False" />
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" />
                                                <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Custom Form Count" 
                                                FieldName="CustomFormCount" ShowInCustomizationForm="True" 
                                                VisibleIndex="13" Width="150px">
                                                <Settings AllowAutoFilter="False" />
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" />
                                                <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Place Last Accessed" 
                                                FieldName="PlaceLastAccessed" ShowInCustomizationForm="True" 
                                                VisibleIndex="14" Width="150px">
                                                <Settings AllowAutoFilter="False" />
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" />
                                                <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Place Last Modified" 
                                                FieldName="PlaceLastModified" ShowInCustomizationForm="True" 
                                                VisibleIndex="15" Width="150px">
                                                <Settings AllowAutoFilter="False" />
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" />
                                                <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Document Read Counts" 
                                                FieldName="DocumentReadCounts" ShowInCustomizationForm="True" 
                                                VisibleIndex="16" Width="150px">
                                                <Settings AllowAutoFilter="False" />
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" />
                                                <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Login Count" FieldName="LoginCount" 
                                                ShowInCustomizationForm="True" VisibleIndex="17">
                                                <Settings AllowAutoFilter="False" />
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" />
                                                <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Last Day Uses" FieldName="LastDayUses" 
                                                ShowInCustomizationForm="True" VisibleIndex="18">
                                                <Settings AllowAutoFilter="False" />
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" />
                                                <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Last Day Reads" FieldName="LastDayReads" 
                                                ShowInCustomizationForm="True" VisibleIndex="19">
                                                <Settings AllowAutoFilter="False" />
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" />
                                                <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Last Day Writes" FieldName="LastDayWrites" 
                                                ShowInCustomizationForm="True" VisibleIndex="20">
                                                <Settings AllowAutoFilter="False" />
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" />
                                                <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Last Week Writes" 
                                                FieldName="LastWeekWrites" ShowInCustomizationForm="True" 
                                                VisibleIndex="21" Width="150px">
                                                <Settings AllowAutoFilter="False" />
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" />
                                                <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Last Month Reads" 
                                                FieldName="LastMonthReads" ShowInCustomizationForm="True" 
                                                VisibleIndex="22" Width="150px">
                                                <Settings AllowAutoFilter="False" />
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" />
                                                <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Last Month Writes" 
                                                FieldName="LastMonthWrites" ShowInCustomizationForm="True" 
                                                VisibleIndex="23" Width="150px">
                                                <Settings AllowAutoFilter="False" />
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" />
                                                <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                        <SettingsBehavior ColumnResizeMode="NextColumn" />
                                        <SettingsPager AlwaysShowPager="True" PageSize="4">
                                        </SettingsPager>
                                        <Settings ShowHorizontalScrollBar="True" ShowGroupPanel="True" 
                                            ShowFilterRow="True" />
                                        <Styles>
                                            <Header HorizontalAlign="Center" VerticalAlign="Middle">
                                            </Header>
                                              <AlternatingRow CssClass="GridAltRow" Enabled="True">
                        </AlternatingRow>
                                        </Styles>
                                    </dx:ASPxGridView>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="QuickrServersGrid" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
            </td>
        </tr>      
        <tr>
            <td>
                
            </td>
            <td valign="top">
                
            </td>
        </tr>
        </table>

        
</asp:Content>
