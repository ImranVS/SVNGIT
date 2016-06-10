<%@ Page Title="" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="LotusTravelerHealth.aspx.cs" Inherits="VSWebUI.Configurator.LotusTravellerHealth" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>







<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>
<%@ Register assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    function Resized() {
        var callbackState = document.getElementById('ContentPlaceHolder1_callbackState').value;
        
            if (callbackState == 0)
            DoCallback();
    }

    function DoCallback() {
        document.getElementById('ContentPlaceHolder1_chartWidth').value = document.body.offsetWidth-50;
        //cSelectServerRoundPanel.style.width = document.getElementById('ContentPlaceHolder1__chartWidth').value + "px";
       FileOpensCumulativeWebChart.PerformCallback();
       FileOpensWebChart.PerformCallback();
       chttpSessionsWebChart.PerformCallback();
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
    // <![CDATA[
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
      // ]]>
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <input id="chartWidth" type="hidden" runat="server" value="400" />
        <input id="callbackState" type="hidden" runat="server" value="0" />
    <table width="100%">
        <tr>
            <td valign="top">
                                                                            
                <dx:ASPxRoundPanel ID="SelectServerRoundPanel" runat="server" Theme="Glass" 
                    HeaderText="Select Server" Height="254px" ClientInstanceName="cSelectServerRoundPanel" Width="99%">
                    <PanelCollection>
                        <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                             <table width="100%">
                               
                                <tr>
                                    <td valign="top" colspan="2">
                                        <dx:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server" 
                                            KeyFieldName="ID"  width="100%" AutoGenerateColumns="False" 
                                            Theme="Office2010Silver" >
                                            <ClientSideEvents FocusedRowChanged="function(s, e) { edit_panel.PerformCallback(); }" />

                                            <SettingsPager PageSize="10">
                                                <PageSizeItemSettings Visible="True">
                                                </PageSizeItemSettings>
                                            </SettingsPager>
                                            
                                            <Columns>
                                                <%--<dx:GridViewCommandColumn Caption="Select" ShowSelectCheckbox="True" 
                                                    VisibleIndex="0" FixedStyle="Left">
                                                    <HeaderStyle CssClass="GridCss" />
                                                    <CellStyle VerticalAlign="Top">
                                                    </CellStyle>
                                                </dx:GridViewCommandColumn>--%>
                                                <dx:GridViewCommandColumn ShowInCustomizationForm="True" Visible="False" 
                                                    VisibleIndex="0">
                                                    <ClearFilterButton Visible="True">
                                                    </ClearFilterButton>
                                                </dx:GridViewCommandColumn>
                                                <dx:GridViewDataHyperLinkColumn Caption="Name" FieldName="Name" 
                                                    VisibleIndex="1" FixedStyle="Left" Width="150px">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCss" >
                                                    <Paddings Padding="5px" />
                                                    </HeaderStyle>
                                                    <CellStyle CssClass="GridCss" VerticalAlign="Top"></CellStyle>
                                                    <PropertiesHyperLinkEdit NavigateUrlFormatString="DominoServerDetailsPage2.aspx?Name={0}" Target="_self" TextField="Name" TextFormatString="">
                                                    </PropertiesHyperLinkEdit>
                                                </dx:GridViewDataHyperLinkColumn>                                                
                                                <dx:GridViewDataTextColumn Caption="Status" FieldName="Status" 
                                                    ShowInCustomizationForm="True" VisibleIndex="2">
                                                    <Settings AllowAutoFilter="False" />
                                                    <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCss" />
                                                    <CellStyle CssClass="GridCss1" HorizontalAlign="Center" VerticalAlign="Top">
                                                    </CellStyle> 
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Details" FieldName="Details" 
                                                    ShowInCustomizationForm="True" VisibleIndex="4" Width="400px">
                                                    <Settings AllowAutoFilter="False" />
                                                    <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCss" />
                                                    <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                    </CellStyle> 
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Users" FieldName="Users" 
                                                    ShowInCustomizationForm="True" VisibleIndex="5">
                                                    <Settings AllowAutoFilter="False" />
                                                    <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCss" />
                                                    <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                    </CellStyle> 
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Incremental Syncs" 
                                                    FieldName="IncrementalSyncs" ShowInCustomizationForm="True" 
                                                    VisibleIndex="6" Width="130px">
                                                    <Settings AllowAutoFilter="False" />
                                                    <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCss" />
                                                    <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                    </CellStyle> 
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" 
                                                    ShowInCustomizationForm="True" VisibleIndex="7" Visible="False">
                                                    <Settings AllowAutoFilter="False" />
                                                    <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCss" />
                                                    <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                    </CellStyle> 
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Devices" FieldName="Devices" 
                                                    ShowInCustomizationForm="True" VisibleIndex="8">
                                                    <Settings AllowAutoFilter="False" />
                                                    <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCss" />
                                                    <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                    </CellStyle> 
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="HTTP Status" FieldName="HTTP_Status" 
                                                    ShowInCustomizationForm="True" VisibleIndex="9">
                                                    <Settings AllowAutoFilter="False" />
                                                    <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCss" />
                                                    <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                    </CellStyle> 
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="HTTP Details" FieldName="HTTP_Details" 
                                                    ShowInCustomizationForm="True" VisibleIndex="10" Width="400px">
                                                    <Settings AllowAutoFilter="False" />
                                                    <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCss" />
                                                    <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                    </CellStyle> 
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="HTTP Peak Connections" 
                                                    FieldName="HTTP_PeakConnections" ShowInCustomizationForm="True" 
                                                    VisibleIndex="11" Width="150px">
                                                    <Settings AllowAutoFilter="False" />
                                                    <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCss" />
                                                    <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                    </CellStyle> 
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="HTTP Max Configured Connections" 
                                                    FieldName="HTTP_MaxConfiguredConnections" ShowInCustomizationForm="True" 
                                                    VisibleIndex="12" Width="200px">
                                                    <Settings AllowAutoFilter="False" />
                                                    <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCss" />
                                                    <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                    </CellStyle> 
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Traveler Version" 
                                                    FieldName="TravelerVersion" ShowInCustomizationForm="True" 
                                                    VisibleIndex="3">
                                                    <Settings AllowAutoFilter="False" />
                                                    <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCss" />
                                                    <CellStyle CssClass="GridCss" HorizontalAlign="Center" VerticalAlign="Top">
                                                    </CellStyle> 
                                                </dx:GridViewDataTextColumn>
                                            </Columns>
                                            <SettingsBehavior AllowFocusedRow="True" AllowDragDrop="False" 
                                                AutoExpandAllGroups="True" AllowSelectByRowClick="True" 
                                                ProcessSelectionChangedOnServer="True" ColumnResizeMode="NextColumn" />
                                            <Settings ShowGroupPanel="True" ShowHorizontalScrollBar="True" 
                                                ShowFilterRow="True" />
                                        </dx:ASPxGridView>                                                                                                                      
                                    </td>
                                    <td>
                                        <div style="float: left; width: 20%; visibility:hidden">
                                            <%--<div class="BottomPadding">
                                                Selected values:
                                            </div>--%>
                                            <dx:ASPxListBox ID="selList" ClientInstanceName="selList" runat="server" Height="20px" Width="100%" style="">
                                            </dx:ASPxListBox>
                                            <%--<dx:ASPxListBox ID="selList" ClientInstanceName="selList" runat="server" Height="250px" Width="100%" style=" visibility:hidden">
                                            </dx:ASPxListBox>--%>
                                            <%--<dx:ASPxListBox ID="ASPxListBox1" ClientInstanceName="selList" runat="server" Height="250px" Width="100%" />--%>
                                            <%--<div class="TopPadding">
                                                Selected count: <span id="selCount" style="font-weight: bold">0</span>
                                            </div>--%>
                                        </div>
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td colspan="2">
                                        <dx:ASPxButton ID="bttnSubmit" runat="server" Text="Submit" 
                                            OnClick="bttnSubmit_Click">
                                        </dx:ASPxButton>
                                    </td>
                                </tr>--%>
                            </table>                            
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>                                                                            
            </td>                        
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" 
                                CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                Text="Select a mail server:">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel2" runat="server" 
                                CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                Text="Select an interval:">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxComboBox ID="mailServerListComboBox" runat="server" 
                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                    SpriteCssFilePath="~/App_Themes/Glass/{0}/styles.css" Theme="Default" 
                    ValueType="System.String">
                    <ClientSideEvents SelectedIndexChanged="function(s, e) { edit_panel.PerformCallback(); }" />
                </dx:ASPxComboBox>
                        </td>
                        <td>
                            <dx:ASPxComboBox ID="travelerIntervalComboBox" runat="server" 
                                CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                SpriteCssFilePath="~/App_Themes/Glass/{0}/styles.css" ValueType="System.String">
                                <ClientSideEvents SelectedIndexChanged="function(s, e) { edit_panel.PerformCallback(); }" />
                            </dx:ASPxComboBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        </table>
        <table>
        <tr>
            <td>
                <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" ClientInstanceName="edit_panel" 
                    runat="server" Width="100%" oncallback="ASPxCallbackPanel1_Callback">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent6" runat="server">
                            <table>
                             <tr>
            <td colspan="2">
                <dx:ASPxRoundPanel runat="server" ID="mailFileOpensRoundPanel" 
                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                    GroupBoxCaptionOffsetY="-24px" HeaderText="Cumulative Mail File Open Times" 
                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="95%" 
                    Theme="Default">
                    <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                    <PanelCollection>
<dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
    <dxchartsui:WebChartControl ID="mailFileOpensCumulativeWebChart" runat="server" 
        Height="350px" Width="1030px"  ClientInstanceName="FileOpensCumulativeWebChart" 
        OnCustomCallback="mailFileOpensCumulativeWebChart_CustomCallback">
        <fillstyle>
            <optionsserializable>
                <cc1:SolidFillOptions />
            </optionsserializable>
        </fillstyle>
        <seriestemplate>
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
        </seriestemplate>
        <crosshairoptions>
            <commonlabelpositionserializable>
                <cc1:CrosshairMousePosition />
            </commonlabelpositionserializable>
        </crosshairoptions>
        <tooltipoptions>
            <tooltippositionserializable>
                <cc1:ToolTipMousePosition />
            </tooltippositionserializable>
        </tooltipoptions>
    </dxchartsui:WebChartControl>
                        </dx:PanelContent>
</PanelCollection>
                </dx:ASPxRoundPanel>
            </td>
        </tr>
        <tr>
                                <td colspan="2">
                                        <dx:ASPxRoundPanel ID="mailFileOpensDeltaRoundPanel" runat="server" 
                                            CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                            SpriteCssFilePath="~/App_Themes/Glass/{0}/styles.css" Theme="Default" 
                                            Width="95%" GroupBoxCaptionOffsetY="-24px" 
                                            HeaderText="Mail File Open Times Delta">
                                            <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                                    <dxchartsui:WebChartControl ID="mailFileOpensWebChart" runat="server"   ClientInstanceName="FileOpensWebChart" 
                                                        Height="350px" Width="1030px" 
                                                        OnCustomCallback="mailFileOpensWebChart_CustomCallback">
                                                        <fillstyle>
                                                            <optionsserializable>
                                                                <cc1:SolidFillOptions />
                                                            </optionsserializable>
                                                        </fillstyle>
                                                        <seriestemplate>
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
                                                        </seriestemplate>
                                                        <crosshairoptions>
                                                            <commonlabelpositionserializable>
                                                                <cc1:CrosshairMousePosition />
                                                            </commonlabelpositionserializable>
                                                        </crosshairoptions>
                                                        <tooltipoptions showforseries="True">
                                                            <tooltippositionserializable>
                                                                <cc1:ToolTipMousePosition />
                                                            </tooltippositionserializable>
                                                        </tooltipoptions>
                                                    </dxchartsui:WebChartControl>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxRoundPanel>
                                    </td>
                                </tr>  
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <dx:ASPxRoundPanel ID="httpSessionsASPxRoundPanel" runat="server" 
                            CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                            GroupBoxCaptionOffsetY="-24px" HeaderText="HTTP Sessions" 
                            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="95%">
                            <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                            <HeaderStyle Height="23px">
                                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                            </HeaderStyle>
                            
                            <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                                <table>
                                    <tr>
                                        <%--<td>
                                            Select Server:                                            
                                        </td>
                                        <td>
                                            <dx:ASPxComboBox ID="httpSessionsASPxCombo" runat="server" 
                                                ValueType="System.String" AutoPostBack="True" EnableTheming="True" 
                                                Theme="Glass" 
                                                OnSelectedIndexChanged="httpSessionsASPxCombo_SelectedIndexChanged" >
                                            </dx:ASPxComboBox>
                                        </td>--%>
                                        <td>
                                            <dx:ASPxRadioButtonList ID="httpSessionsASPxRadioButtonList" runat="server" 
                                            ValueType="System.String" AutoPostBack="True" CssClass="Glass" 
                                            CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                            RepeatDirection="Horizontal" SelectedIndex="0" 
                                            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
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
                                    Width="1030px"  ClientInstanceName="chttpSessionsWebChart" 
                                    OnCustomCallback="httpSessionsWebChart_CustomCallback" >
<FillStyle><OptionsSerializable><cc1:SolidFillOptions></cc1:SolidFillOptions></OptionsSerializable>
</FillStyle>

<SeriesTemplate><ViewSerializable><cc1:SideBySideBarSeriesView></cc1:SideBySideBarSeriesView></ViewSerializable>
<LabelSerializable><cc1:SideBySideBarSeriesLabel LineVisible="True"><FillStyle><OptionsSerializable><cc1:SolidFillOptions></cc1:SolidFillOptions></OptionsSerializable></FillStyle><PointOptionsSerializable><cc1:PointOptions></cc1:PointOptions></PointOptionsSerializable></cc1:SideBySideBarSeriesLabel></LabelSerializable>
<LegendPointOptionsSerializable><cc1:PointOptions></cc1:PointOptions></LegendPointOptionsSerializable>
</SeriesTemplate>

<DiagramSerializable>
                                                            <cc1:XYDiagram LabelsResolveOverlappingMinIndent="1" PaneLayoutDirection="Horizontal">
                                                                <AxisX Title-Text="Date" VisibleInPanesSerializable="-1" GridSpacingAuto="False" GridSpacing="0.5" MinorCount="15" DateTimeMeasureUnit="Hour"><label enableantialiasing="False"></label><Range SideMarginsEnabled="False"></Range><GridLines Visible="True"></GridLines><DateTimeOptions Format="Custom" FormatString="dd/MM HH:mm"></DateTimeOptions></AxisX>
                                                                <AxisY Title-Text="Space(bytes)" Title-Visible="True" VisibleInPanesSerializable="-1"><Range AlwaysShowZeroLevel="False" SideMarginsEnabled="True"></Range></AxisY>
                                                            </cc1:XYDiagram>
                                                        </DiagramSerializable>

<CrosshairOptions><CommonLabelPositionSerializable><cc1:CrosshairMousePosition></cc1:CrosshairMousePosition></CommonLabelPositionSerializable>
</CrosshairOptions>

<ToolTipOptions><ToolTipPositionSerializable><cc1:ToolTipMousePosition></cc1:ToolTipMousePosition></ToolTipPositionSerializable>
</ToolTipOptions>
                                    </dxchartsui:WebChartControl>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                    </ContentTemplate>
                </asp:UpdatePanel>            
                                    </td>
                                </tr>
                                                                                      
                                  </table>

                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxCallbackPanel>
            </td>
        </tr>        
    </table>
</asp:Content>
