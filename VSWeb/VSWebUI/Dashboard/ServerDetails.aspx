<%@ Page Title="VitalSigns Plus" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="ServerDetails.aspx.cs" Inherits="VSWebUI.Dashboard.ServerDetails" %>

<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>





<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    function Resized() {
        var callbackState = document.getElementById('ContentPlaceHolder1_callbackState').value;

        if (callbackState == 0)
            DoCallback();
    }

    function DoCallback() {

        document.getElementById('ContentPlaceHolder1_chartWidth').value = document.body.offsetWidth - 105;

        //cSelectServerRoundPanel.style.width = document.getElementById('ContentPlaceHolder1__chartWidth').value + "px";
        //
        //cResponseWebChartControl.Performcallback();
//        cActiveNwayWebChartControl.Performcallback();
//        cActiveMeetingWebChartControl.Performcallback();
        //        cActiveNwayWebChartControl.Performcallback();
        cWebChartControl1.Performcallback();
        cWebChartControl2.Performcallback();
        cWebChartControl3.Performcallback();
        cWebChartControl4.Performcallback();


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
<input id="chartWidth" type="hidden" runat="server" value="400" />
<input id="callbackState" type="hidden" runat="server" value="0" />

   <div>
        <dx:ASPxLabel ID="ASPxLabel4" runat="server" Style="font-weight: 700" Text="azphxdom1/RPRWyatt">
        </dx:ASPxLabel>
        <br />
        <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
            CssPostfix="Glass" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
            TabSpacing="0px" Width="700px" EnableHierarchyRecreation="False">
            <TabPages>
                <dx:TabPage Text="Overall">
                    <ContentCollection>
                        <dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">
                            <table style="vertical-align: top; text-align: left">
                                <tr>
                                    <td style="vertical-align: top; text-align: left; width: 50%;">
                                        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                            CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" Height="300px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                                            HeaderText="Memory (free)" Width="100%">
                                            <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
                                            <ContentPaddings PaddingTop="10px" PaddingBottom="10px" Padding="2px"></ContentPaddings>
                                            <HeaderStyle Height="23px">
                                                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
<Paddings PaddingLeft="2px" PaddingTop="0px" PaddingBottom="0px"></Paddings>
                                            </HeaderStyle>
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:VSS_StatisticsConnectionString %>"
                                                        SelectCommand="SELECT  [StatName],CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date,[StatValue] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where ServerName='azphxdom1/RPRWyatt' and [StatName]='Mem.PercentUsed' and Date &gt; DATEADD (hh , -1 ,'2012-06-22 08:17:44.000' ) order by Date asc">
                                                    </asp:SqlDataSource>
                                                    <dx:ASPxRadioButtonList ID="ASPxRadioButtonList1" runat="server" AutoPostBack="true"
                                                        RepeatDirection="Horizontal" RepeatLayout="OrderedList" OnSelectedIndexChanged="ASPxRadioButtonList1_SelectedIndexChanged"
                                                        SelectedIndex="0" Width="100%">
                                                        <Items>
                                                            <dx:ListEditItem Text="1 Hour" Value="hh" Selected="True" />
                                                            <dx:ListEditItem Text="1 Day" Value="dd" />
                                                        </Items>
                                                    </dx:ASPxRadioButtonList>
                                                    <dx:WebChartControl ID="WebChartControl1" runat="server" DataSourceID="SqlDataSource1"
                                                        Height="200px" Width="300px" ClientInstanceName="cWebChartControl1" 
                                                        OnCustomCallback="WebChartControl1_CustomCallback">
                                                       
                                                        
                                                        <SeriesSerializable>
                <cc1:Series ArgumentDataMember="Date" Name="Series 1" 
                    ValueDataMembersSerializable="StatValue">

                    <ViewSerializable>
                        <cc1:SplineSeriesView>
                            <linemarkeroptions size="4">
                            </linemarkeroptions>
                        </cc1:SplineSeriesView>
</ViewSerializable>
<LabelSerializable>
<cc1:PointSeriesLabel LineLength="12" LineVisible="True" 
        ResolveOverlappingMode="JustifyAroundPoint" Visible="False">
<FillStyle><OptionsSerializable>
<cc1:SolidFillOptions></cc1:SolidFillOptions>
</OptionsSerializable>
</FillStyle>
<PointOptionsSerializable>
<cc1:PointOptions></cc1:PointOptions>
</PointOptionsSerializable>
</cc1:PointSeriesLabel>
</LabelSerializable>
<LegendPointOptionsSerializable>
<cc1:PointOptions></cc1:PointOptions>
</LegendPointOptionsSerializable>
</cc1:Series>
</SeriesSerializable>
                                                <SeriesTemplate
                                                    ><ViewSerializable>
                                                        <cc1:SplineSeriesView>
                                                        </cc1:SplineSeriesView>
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
<LegendPointOptionsSerializable>
<cc1:PointOptions></cc1:PointOptions>
</LegendPointOptionsSerializable>
</SeriesTemplate>
                                                <FillStyle ><OptionsSerializable>
<cc1:SolidFillOptions></cc1:SolidFillOptions>
</OptionsSerializable>
</FillStyle>
                                                <Legend Visible="False"></Legend>
                                                <BorderOptions Visible="False" />
                                                <Titles>

</Titles>
<BorderOptions Visible="False"></BorderOptions>
                                                <DiagramSerializable>
<cc1:XYDiagram LabelsResolveOverlappingMinIndent="1">
<AxisX Title-Text="Date" VisibleInPanesSerializable="-1" GridSpacingAuto="False" GridSpacing="0.5" MinorCount="5" DateTimeMeasureUnit="Hour">
<Range SideMarginsEnabled="False"></Range>
<GridLines Visible="True"></GridLines>
<DateTimeOptions Format="Custom" FormatString="dd/MM HH:mm"></DateTimeOptions>
</AxisX>
<AxisY Title-Text="Space(bytes)" Title-Visible="True" VisibleInPanesSerializable="-1">
<Range AlwaysShowZeroLevel="False" SideMarginsEnabled="True"></Range>
</AxisY>
</cc1:XYDiagram>
</DiagramSerializable>

<CrosshairOptions><CommonLabelPositionSerializable>
<cc1:CrosshairMousePosition></cc1:CrosshairMousePosition>
</CommonLabelPositionSerializable>
</CrosshairOptions>

<ToolTipOptions><ToolTipPositionSerializable>
<cc1:ToolTipMousePosition></cc1:ToolTipMousePosition>
</ToolTipPositionSerializable>
</ToolTipOptions>
                                                    </dx:WebChartControl>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxRoundPanel>
                                    </td>
                                    <td style="vertical-align: top; text-align: left; width: 50%;">
                                        <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                            CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" Height="300px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                                            HeaderText="Server Users" Width="100%">
                                            <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
                                            <ContentPaddings PaddingTop="10px" PaddingBottom="10px" Padding="2px"></ContentPaddings>
                                            <HeaderStyle Height="23px">
                                                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
<Paddings PaddingLeft="2px" PaddingTop="0px" PaddingBottom="0px"></Paddings>
                                            </HeaderStyle>
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:VSS_StatisticsConnectionString %>"
                                                        SelectCommand="SELECT  [StatName],CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date,[StatValue] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where ServerName='azphxdom1/RPRWyatt' and [StatName]='Server.Users' and Date &gt; DATEADD (hh , -1 ,'2012-06-22 08:17:44.000' ) order by Date asc">
                                                    </asp:SqlDataSource>
                                                    <dx:ASPxRadioButtonList ID="ASPxRadioButtonList2" runat="server" 
                                                        AutoPostBack="True" 
                                                        OnSelectedIndexChanged="ASPxRadioButtonList1_SelectedIndexChanged" 
                                                        RepeatDirection="Horizontal" RepeatLayout="OrderedList" SelectedIndex="0" 
                                                        Width="100%">
                                                        <Items>
                                                            <dx:ListEditItem Selected="True" Text="1 Hour" Value="hh" />
                                                            <dx:ListEditItem Text="1 Day" Value="dd" />
                                                        </Items>
                                                    </dx:ASPxRadioButtonList>
                                                    <dx:WebChartControl ID="WebChartControl2" runat="server" DataSourceID="SqlDataSource2"
                                                        Height="200px" Width="300px" ClientInstanceName="cWebChartControl2" 
                                                        OnCustomCallback="WebChartControl2_CustomCallback">
                                                        <diagramserializable>
                <cc1:XYDiagram>
                    <AxisX Title-Text="Date" VisibleInPanesSerializable="-1" GridSpacingAuto="False" GridSpacing="0.5" MinorCount="5" DateTimeMeasureUnit="Hour">
<Range SideMarginsEnabled="False"></Range>
<GridLines Visible="True"></GridLines>
<DateTimeOptions Format="Custom" FormatString="dd/MM HH:mm"></DateTimeOptions>
</AxisX>
                   
                    <axisy visibleinpanesserializable="-1" Title-Text="No of Users" Title-Visible="True" >
                        <range sidemarginsenabled="True" AlwaysShowZeroLevel="False" />
<Range AlwaysShowZeroLevel="False" SideMarginsEnabled="True"></Range>
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
                        <LineMarkerOptions Size="4"></LineMarkerOptions>
                        </cc1:SplineSeriesView>
                    </viewserializable>
                    <labelserializable>
                        <cc1:PointSeriesLabel LineLength="12" LineVisible="True" ResolveOverlappingMode="JustifyAroundPoint">
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
                                                    </dx:WebChartControl>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxRoundPanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; text-align: left">
                                        <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                            CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" Height="300px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                                            HeaderText="Platform.System.PctCombinedCpuUtil" Width="100%">
                                            <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
                                            <ContentPaddings PaddingTop="10px" PaddingBottom="10px" Padding="2px"></ContentPaddings>
                                            <HeaderStyle Height="23px">
                                                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
<Paddings PaddingLeft="2px" PaddingTop="0px" PaddingBottom="0px"></Paddings>
                                            </HeaderStyle>
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                                                    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:VSS_StatisticsConnectionString %>"
                                                        SelectCommand="SELECT  [StatName],CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date,[StatValue] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where ServerName='azphxdom1/RPRWyatt' and [StatName]='Platform.System.PctCombinedCpuUtil' and Date &gt; DATEADD (hh , -1 ,'2012-06-22 08:17:44.000' ) order by Date asc">
                                                    </asp:SqlDataSource>
                                                    <dx:ASPxRadioButtonList ID="ASPxRadioButtonList3" runat="server" 
                                                        AutoPostBack="True" 
                                                        OnSelectedIndexChanged="ASPxRadioButtonList1_SelectedIndexChanged" 
                                                        RepeatDirection="Horizontal" RepeatLayout="OrderedList" SelectedIndex="0" 
                                                        Width="100%">
                                                        <Items>
                                                            <dx:ListEditItem Selected="True" Text="1 Hour" Value="hh" />
                                                            <dx:ListEditItem Text="1 Day" Value="dd" />
                                                        </Items>
                                                    </dx:ASPxRadioButtonList>
                                                    <dx:WebChartControl ID="WebChartControl3" runat="server" DataSourceID="SqlDataSource3"
                                                        Height="200px" Width="300px"  ClientInstanceName="cWebChartControl3"
                                                        OnCustomCallback="WebChartControl3_CustomCallback">
                                                        <diagramserializable>
                <cc1:XYDiagram>
                    <axisx visibleinpanesserializable="-1" Title-Text="Time" Title-Visible="True" >
                        <range sidemarginsenabled="True" />
<Range SideMarginsEnabled="True"></Range>
                    </axisx>
                    <axisy visibleinpanesserializable="-1" Title-Text="CPU" Title-Visible="True" >
                        <range sidemarginsenabled="True" AlwaysShowZeroLevel="False" />
<Range AlwaysShowZeroLevel="False" SideMarginsEnabled="True"></Range>
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
                        <cc1:SideBySideBarSeriesView>
                        </cc1:SideBySideBarSeriesView>
                    </viewserializable>
                    <labelserializable>
                        <cc1:SideBySideBarSeriesLabel LineVisible="True" Visible="False">
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

<CrosshairOptions><CommonLabelPositionSerializable>
<cc1:CrosshairMousePosition></cc1:CrosshairMousePosition>
</CommonLabelPositionSerializable>
</CrosshairOptions>

<ToolTipOptions><ToolTipPositionSerializable>
<cc1:ToolTipMousePosition></cc1:ToolTipMousePosition>
</ToolTipPositionSerializable>
</ToolTipOptions>
                                                    </dx:WebChartControl>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxRoundPanel>
                                    </td>
                                    <td style="vertical-align: top; text-align: left">
                                        <dx:ASPxRoundPanel ID="ASPxRoundPanel4" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                            CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Drives" Height="300px"
                                            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                                            <ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
<ContentPaddings Padding="2px" PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                                            <HeaderStyle Height="23px">
                                                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
<Paddings PaddingLeft="2px" PaddingTop="0px" PaddingBottom="0px"></Paddings>
                                            </HeaderStyle>
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
                                                    <dx:ASPxRadioButtonList ID="ASPxRadioButtonList4" runat="server" 
                                                        AutoPostBack="True" 
                                                        OnSelectedIndexChanged="ASPxRadioButtonList1_SelectedIndexChanged" 
                                                        RepeatDirection="Horizontal" RepeatLayout="OrderedList" SelectedIndex="0" 
                                                        Width="100%">
                                                        <Items>
                                                            <dx:ListEditItem Selected="True" Text="1 Hour" Value="hh" />
                                                            <dx:ListEditItem Text="1 Day" Value="dd" />
                                                        </Items>
                                                    </dx:ASPxRadioButtonList>
                                                    <dx:WebChartControl ID="WebChartControl4" runat="server" Height="200px"  ClientInstanceName="cWebChartControl4"
                                                        Width="300px" OnCustomCallback="WebChartControl4_CustomCallback">
                                                        <diagramserializable>
                                                            <cc1:SimpleDiagram>
                                                            </cc1:SimpleDiagram>
                                    </diagramserializable>
                                                        <fillstyle>
                                        <optionsserializable>
                                            <cc1:SolidFillOptions />
                                        </optionsserializable>
                                    </fillstyle>
                                                        <seriesserializable>
                                        <cc1:Series Name="Drive C">
                                            <points>
                                                <cc1:SeriesPoint ArgumentSerializable="FreeSpace" SeriesPointID="0" 
                                                    Values="26616678912">
                                                </cc1:SeriesPoint>
                                                <cc1:SeriesPoint ArgumentSerializable="TotalSpace" SeriesPointID="1" 
                                                    Values="100000000000">
                                                </cc1:SeriesPoint>
                                            </points>
                                            <viewserializable>
                                                <cc1:PieSeriesView RuntimeExploding="true">
                                                </cc1:PieSeriesView>
                                            </viewserializable>
                                            <labelserializable>
                                                <cc1:PieSeriesLabel LineVisible="True" Position="Radial" TextColor="Black" 
                                                BackColor="Transparent" Font="Tahoma, 8pt, style=Bold" Visible="False">
                                                <Border Visible="False"></Border>
                                                    <fillstyle>
                                                        <optionsserializable>
                                                            <cc1:SolidFillOptions />
                                                        </optionsserializable>
                                                    </fillstyle>
                                                    <pointoptionsserializable>
                                                        <cc1:PiePointOptions PointView="ArgumentAndValues">
                                                       
                                                        </cc1:PiePointOptions>
                                                    </pointoptionsserializable>
                                                </cc1:PieSeriesLabel>
                                            </labelserializable>
                                            <legendpointoptionsserializable>
                                                <cc1:PiePointOptions>
                                                </cc1:PiePointOptions>
                                            </legendpointoptionsserializable>
                                        </cc1:Series>
                                    </seriesserializable>
                                                        <seriestemplate>
                                        <viewserializable>
                                            
                                        <cc1:PieSeriesView RuntimeExploding="False"></cc1:PieSeriesView></viewserializable>
                                        <labelserializable>
                                            
                                        <cc1:PieSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PiePointOptions></cc1:PiePointOptions></pointoptionsserializable></cc1:PieSeriesLabel></labelserializable>
                                        <legendpointoptionsserializable>
                                            <cc1:PiePointOptions>
                                            </cc1:PiePointOptions>
                                        </legendpointoptionsserializable>
                                    </seriestemplate>
                                                        <titles>
                                                            <cc1:ChartTitle Alignment="Near" Text="Drive C" />
                                                        </titles>

<CrosshairOptions><CommonLabelPositionSerializable>
<cc1:CrosshairMousePosition></cc1:CrosshairMousePosition>
</CommonLabelPositionSerializable>
</CrosshairOptions>

<ToolTipOptions><ToolTipPositionSerializable>
<cc1:ToolTipMousePosition></cc1:ToolTipMousePosition>
</ToolTipPositionSerializable>
</ToolTipOptions>
                                                    </dx:WebChartControl>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxRoundPanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
                <dx:TabPage Text="Maintenance Windows">
                    <ContentCollection>
                        <dx:ContentControl ID="ContentControl2" runat="server" SupportsDisabledAttribute="True">
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
                <dx:TabPage Text="Outages">
                    <ContentCollection>
                        <dx:ContentControl ID="ContentControl3" runat="server" SupportsDisabledAttribute="True">
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
                <dx:TabPage Text="Alert History">
                    <ContentCollection>
                        <dx:ContentControl ID="ContentControl4" runat="server" SupportsDisabledAttribute="True">
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
            </TabPages>
            <LoadingPanelImage Url="~/App_Themes/Glass/Web/Loading.gif">
            </LoadingPanelImage>
            <Paddings PaddingLeft="0px" />

<Paddings PaddingLeft="0px"></Paddings>

            <ContentStyle>
                <Border BorderColor="#4986A2" />
<Border BorderColor="#4986A2"></Border>
            </ContentStyle>
        </dx:ASPxPageControl>
    </div>
</asp:Content>
