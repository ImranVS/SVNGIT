<%@ Page Title="VitalSigns Plus" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="DominoServerStatisticsDetail.aspx.cs" Inherits="VSWebUI.Configurator.DominoServerStatisticsDetail"%>
<%@ MasterType virtualpath="~/DashboardSite.Master" %>

<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>






<%@ Register assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
        <tr>
            <td>
                <%--<dx:ASPxComboBox ID="hrASPxCombo" runat="server" ValueType="System.String" 
                    AutoPostBack="True" 
                    onselectedindexchanged="hrASPxCombo_SelectedIndexChanged">
                </dx:ASPxComboBox>  --%><%--</ContentTemplate>
                </asp:UpdatePanel>--%><%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">--%><%--<ContentTemplate>--%>
                <dx:ASPxButton ID="BackBtn" runat="server" onclick="BackBtn_Click" Text="Back" 
                    Theme="Office2010Blue">
                </dx:ASPxButton>
            </td>
            <td>
                
            </td>
            <td>
                
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="4">            
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="4">            
                <dx:ASPxLabel ID="ServerLabel" runat="server" Font-Bold="True">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                &nbsp;
            </td>
        </tr>
        <tr>        
        <td align="center" colspan="4">
            <%--</ContentTemplate>
                                    </asp:UpdatePanel>--%><%--<dxchartsui:WebChartControl ID="dominoServerWebChart" runat="server" 
                                                            Height="400px" Width="800px">
                                                            <FillStyle><OptionsSerializable><cc1:SolidFillOptions></cc1:SolidFillOptions></OptionsSerializable>
</FillStyle>

                <seriesserializable>
                    <cc1:Series Name="Series 1" Visible="False">
                        <viewserializable><cc1:LineSeriesView><LineMarkerOptions Size="20"></LineMarkerOptions></cc1:LineSeriesView></viewserializable>
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

                <DiagramSerializable>
                                                            <cc1:XYDiagram LabelsResolveOverlappingMinIndent="1" PaneLayoutDirection="Horizontal">
                                                                <AxisX Title-Text="Date" VisibleInPanesSerializable="-1" GridSpacingAuto="False" GridSpacing="1.5" MinorCount="15" DateTimeMeasureUnit="Hour"><label enableantialiasing="False"></label><Range SideMarginsEnabled="False"></Range><GridLines Visible="True"></GridLines><DateTimeOptions Format="Custom" FormatString="dd/MM HH:mm"></DateTimeOptions></AxisX>
                                                                <AxisY Title-Text="Space(bytes)" Title-Visible="True" VisibleInPanesSerializable="-1"><Range AlwaysShowZeroLevel="False" SideMarginsEnabled="True"></Range></AxisY>
                                                            </cc1:XYDiagram>
                                                        </DiagramSerializable>

<CrosshairOptions><CommonLabelPositionSerializable><cc1:CrosshairMousePosition></cc1:CrosshairMousePosition></CommonLabelPositionSerializable>
</CrosshairOptions>

<ToolTipOptions><ToolTipPositionSerializable><cc1:ToolTipMousePosition></cc1:ToolTipMousePosition></ToolTipPositionSerializable>
</ToolTipOptions>
                                                        </dxchartsui:WebChartControl>--%>
                                            <dx:ASPxRoundPanel ID="statisticroundpanel" 
                runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Performance" 
                SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="200px">
                                                <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                                <HeaderStyle Height="23px">
                                                    <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                                </HeaderStyle>
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                                        <table>
                                                            <tr align="center">
                                                                <td valign="middle">
                                                                    <dx:ASPxComboBox ID="hrASPxCombo" runat="server" ValueType="System.String" AutoPostBack="True" onselectedindexchanged="hrASPxCombo_SelectedIndexChanged">
                                                                    </dx:ASPxComboBox>
                                                                </td>
                                                                <td valign="middle">
                                                                    <dx:ASPxComboBox ID="dayASPxCombo" runat="server" ValueType="System.String" AutoPostBack="True" onselectedindexchanged="dayASPxCombo_SelectedIndexChanged">
                                                                    </dx:ASPxComboBox>
                                                                </td>
                                                                <td valign="middle">
                                                                    <dx:ASPxComboBox ID="monthASPxCombo" runat="server" ValueType="System.String" 
                                                                        AutoPostBack="True" 
                                                                        onselectedindexchanged="monthASPxCombo_SelectedIndexChanged" Visible="False">
                                                                    </dx:ASPxComboBox>
                                                                </td>
                                                            </tr>
                                                            <tr align="center">
                                                                <td colspan="3" valign="middle">
                                                                    <%--<dxchartsui:WebChartControl ID="dominoServerWebChart" runat="server" 
                                                            Height="400px" Width="800px">
                                                            <FillStyle><OptionsSerializable><cc1:SolidFillOptions></cc1:SolidFillOptions></OptionsSerializable>
</FillStyle>

                <seriesserializable>
                    <cc1:Series Name="Series 1" Visible="False">
                        <viewserializable><cc1:LineSeriesView><LineMarkerOptions Size="20"></LineMarkerOptions></cc1:LineSeriesView></viewserializable>
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

                <DiagramSerializable>
                                                            <cc1:XYDiagram LabelsResolveOverlappingMinIndent="1" PaneLayoutDirection="Horizontal">
                                                                <AxisX Title-Text="Date" VisibleInPanesSerializable="-1" GridSpacingAuto="False" GridSpacing="1.5" MinorCount="15" DateTimeMeasureUnit="Hour"><label enableantialiasing="False"></label><Range SideMarginsEnabled="False"></Range><GridLines Visible="True"></GridLines><DateTimeOptions Format="Custom" FormatString="dd/MM HH:mm"></DateTimeOptions></AxisX>
                                                                <AxisY Title-Text="Space(bytes)" Title-Visible="True" VisibleInPanesSerializable="-1"><Range AlwaysShowZeroLevel="False" SideMarginsEnabled="True"></Range></AxisY>
                                                            </cc1:XYDiagram>
                                                        </DiagramSerializable>

<CrosshairOptions><CommonLabelPositionSerializable><cc1:CrosshairMousePosition></cc1:CrosshairMousePosition></CommonLabelPositionSerializable>
</CrosshairOptions>

<ToolTipOptions><ToolTipPositionSerializable><cc1:ToolTipMousePosition></cc1:ToolTipMousePosition></ToolTipPositionSerializable>
</ToolTipOptions>
                                                        </dxchartsui:WebChartControl>--%>
                                                        <dxchartsui:WebChartControl ID="dominoServerWebChart" runat="server" Height="400px" Width="800px" SeriesSorting="Ascending">
                                                                        <fillstyle>
                                                                            <optionsserializable>
                                                                                <cc1:SolidFillOptions />
                                                                            </optionsserializable>
                                                                        </fillstyle>
                                                                        <seriestemplate seriespointssorting="Ascending">
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
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dx:ASPxRoundPanel>
            <%--</ContentTemplate>
                                    </asp:UpdatePanel>--%>
        </td>               
        </tr>
    </table>
</asp:Content>
