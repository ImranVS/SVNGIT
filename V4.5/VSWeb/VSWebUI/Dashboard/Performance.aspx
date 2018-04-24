<%@ Page Title="VitalSigns Plus" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="Performance.aspx.cs" Inherits="VSWebUI.WebForm5" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<%@ Register assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts.Web" tagprefix="dxchartsui" %>
<%@ Register assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="style1">
        <tr>
            <td>
                <dx:ASPxRoundPanel ID="NotesRoundPanel" runat="server" 
                    HeaderText="Notes Database Performance" Theme="Glass" Width="600px">
                    <PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
    <table class="style1">
        <tr>
            <td colspan="2">
                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Name:">
                </dx:ASPxLabel>
            </td>
            <td colspan="3">
                <dx:ASPxComboBox ID="NameComboBox" runat="server" Theme="Office2010Blue">
                </dx:ASPxComboBox>
            </td>
            <td colspan="2">
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Start:">
                </dx:ASPxLabel>
            </td>
            <td colspan="2">
                <dx:ASPxDateEdit ID="StartDateEdit" runat="server" Theme="Office2010Blue">
                </dx:ASPxDateEdit>
            </td>
            <td>
                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="End:">
                </dx:ASPxLabel>
            </td>
            <td colspan="2">
                <dx:ASPxDateEdit ID="EndDateEdit" runat="server" Theme="Office2010Blue">
                </dx:ASPxDateEdit>
            </td>
            <td>
                <dx:ASPxButton ID="GraphButton" runat="server" Text="Draw Graph" 
                    Theme="Office2010Blue" Width="147px" OnClick="GraphButton_Click">
                </dx:ASPxButton>
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
                <dx:ASPxLabel ID="ResponseLabel" runat="server">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td>
                <dxchartsui:WebChartControl ID="DBPerformanceWebChartControl" runat="server" Height="300px" 
                    Width="609px">
                    <diagramserializable>
                         <cc1:XYDiagram LabelsResolveOverlappingMinIndent="1" PaneLayoutDirection="Horizontal">
                                                                <AxisX Title-Text="Date" VisibleInPanesSerializable="-1" GridSpacingAuto="False" GridSpacing="0.5" MinorCount="15" DateTimeMeasureUnit="Hour">
                                                                    <label enableantialiasing="False">
                                                                    </label>
                                                                    <Range SideMarginsEnabled="False"></Range>
                                                                    <GridLines Visible="True"></GridLines>
                                                                    <DateTimeOptions Format="Custom" FormatString="dd/MM HH:mm"></DateTimeOptions>
                                                                </AxisX>
                                                                <AxisY Title-Text="ResponseTime(Milliseconds)" Title-Visible="True" VisibleInPanesSerializable="-1" >
                                                                    <Range AlwaysShowZeroLevel="False" SideMarginsEnabled="True"></Range>
                                                                </AxisY>
                                                            </cc1:XYDiagram>
                    </diagramserializable>
<FillStyle><OptionsSerializable>
<cc1:SolidFillOptions></cc1:SolidFillOptions>
</OptionsSerializable>
</FillStyle>

                    <seriesserializable>
                        <cc1:Series Name="Series 1">
                            <viewserializable>
                                <cc1:LineSeriesView >
                                <LineMarkerOptions Size="20"></LineMarkerOptions>
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
                        <cc1:Series Name="Series 2">
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
                    <seriestemplate>
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

<CrosshairOptions><CommonLabelPositionSerializable>
<cc1:CrosshairMousePosition></cc1:CrosshairMousePosition>
</CommonLabelPositionSerializable>
</CrosshairOptions>

<ToolTipOptions><ToolTipPositionSerializable>
<cc1:ToolTipMousePosition></cc1:ToolTipMousePosition>
</ToolTipPositionSerializable>
</ToolTipOptions>
                </dxchartsui:WebChartControl>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxPopupControl ID="MsgPopupControl" runat="server" 
                    HeaderText="Information" Modal="True" PopupHorizontalAlign="WindowCenter" 
                    PopupVerticalAlign="WindowCenter" Theme="Glass">
                    <ContentCollection>
<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
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
</asp:Content>
