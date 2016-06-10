<%@ Page Title="VitalSigns Plus" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true"
    CodeBehind="DeviceChart.aspx.cs" Inherits="VSWebUI.WebForm3" %>
	<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>

<%@ Register assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts.Web" tagprefix="dxchartsui" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table><tr><td>
<dx:ASPxRoundPanel ID="StatusRoundPanel" runat="server" 
                                        CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                        CssPostfix="Glass" 
                                        GroupBoxCaptionOffsetY="-24px" HeaderText="Status Chart" 
                                        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
                                        Width="100%" Height="250px">
                                        <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
                                        <HeaderStyle Height="23px">
                                        <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                        </HeaderStyle>
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
  
  
    <table class="style1" width="100%">
        <tr>
            <td>
                <dxchartsui:WebChartControl ID="WebChartControl1" runat="server" Height="200px" 
                    Width="600px">
                    <diagramserializable>
                <cc1:XYDiagram>
                    <axisx visibleinpanesserializable="-1" Title-Text="Time" Title-Visible="True" >
                        <range sidemarginsenabled="True" />
<Range SideMarginsEnabled="True"></Range>
                    </axisx>
                    <axisy visibleinpanesserializable="-1" Title-Text="Space(bytes)" Title-Visible="True" >
                        <range sidemarginsenabled="True" AlwaysShowZeroLevel="false" />
<Range AlwaysShowZeroLevel="false" SideMarginsEnabled="True"></Range>
                    </axisy>
                </cc1:XYDiagram>
            </diagramserializable>
                                                        <fillstyle><OptionsSerializable>
<cc1:SolidFillOptions></cc1:SolidFillOptions>
</OptionsSerializable>
</fillstyle>
                                                        <legend visible="False"></legend>
                                                        <seriesserializable>
                <cc1:Series ArgumentDataMember="Date" Name="Series 1" 
                    ValueDataMembersSerializable="StatValue">
                    <viewserializable>
                        <cc1:SplineSeriesView>
                        </cc1:SplineSeriesView>
                    </viewserializable>
                    <labelserializable>
                        <cc1:PointSeriesLabel LineVisible="True" Visible="true">
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
                    <cc1:SplineSeriesView>
                    </cc1:SplineSeriesView>
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
                &nbsp;
        
            </td>
        </tr>
    </table>
    </dx:PanelContent>
                                        </PanelCollection>
                                    </dx:ASPxRoundPanel>
   </td></tr></table>  
</asp:Content>
