<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Chart2.aspx.cs" Inherits="VSDashboard.Chart2" %>

<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>




<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <dx:ASPxLabel ID="ASPxLabel4" runat="server" Style="font-weight: 700" Text="azphxdom1/RPRWyatt">
        </dx:ASPxLabel>
        <br />
        <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
            CssPostfix="Glass" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" TabSpacing="0px">
            <TabPages>
                <dx:TabPage Text="Overall">
                    <ContentCollection>
                        <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                            <table>
                                <tr>
                                    <td>
                                        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                            CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" Height="50px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                                            HeaderText="Memory (free)">
                                            <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
                                            <ContentPaddings PaddingTop="10px" PaddingBottom="10px" Padding="2px"></ContentPaddings>
                                            <HeaderStyle Height="23px">
                                                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
<Paddings PaddingLeft="2px" PaddingTop="0px" PaddingBottom="0px"></Paddings>
                                            </HeaderStyle>
                                            <PanelCollection>
                                                <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:VSS_StatisticsConnectionString %>"
                                                        SelectCommand="SELECT  [StatName],CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date,[StatValue] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where ServerName='azphxdom1/RPRWyatt' and [StatName]='Mem.PercentUsed' and Date &gt; DATEADD (hh , -1 ,'2012-06-22 08:17:44.000' ) order by Date asc">
                                                    </asp:SqlDataSource>
                                                    <dx:ASPxRadioButtonList ID="ASPxRadioButtonList1" runat="server" AutoPostBack="true"
                                                        RepeatDirection="Horizontal" RepeatLayout="OrderedList" OnSelectedIndexChanged="ASPxRadioButtonList1_SelectedIndexChanged"
                                                        SelectedIndex="0">
                                                        <Items>
                                                            <dx:ListEditItem Text="1 Hour" Value="hh" Selected="True" />
                                                            <dx:ListEditItem Text="1 Day" Value="dd" />
                                                        </Items>
                                                    </dx:ASPxRadioButtonList>
                                                    <br />
                                                    <dx:WebChartControl ID="WebChartControl1" runat="server" DataSourceID="SqlDataSource1"
                                                        Height="300px" Width="500px">
                                                       
                                                        
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
                                                    </dx:WebChartControl>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxRoundPanel>
                                    </td>
                                    <td>
                                        <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                            CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" Height="50px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                                            HeaderText="Server Users">
                                            <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
                                            <ContentPaddings PaddingTop="10px" PaddingBottom="10px" Padding="2px"></ContentPaddings>
                                            <HeaderStyle Height="23px">
                                                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
<Paddings PaddingLeft="2px" PaddingTop="0px" PaddingBottom="0px"></Paddings>
                                            </HeaderStyle>
                                            <PanelCollection>
                                                <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:VSS_StatisticsConnectionString %>"
                                                        SelectCommand="SELECT  [StatName],CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date,[StatValue] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where ServerName='azphxdom1/RPRWyatt' and [StatName]='Server.Users' and Date &gt; DATEADD (hh , -1 ,'2012-06-22 08:17:44.000' ) order by Date asc">
                                                    </asp:SqlDataSource>
                                                    <dx:WebChartControl ID="WebChartControl2" runat="server" DataSourceID="SqlDataSource2"
                                                        Height="300px" Width="500px">
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
                                                    </dx:WebChartControl>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxRoundPanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                            CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" Height="50px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                                            HeaderText="Platform.System.PctCombinedCpuUtil">
                                            <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
                                            <ContentPaddings PaddingTop="10px" PaddingBottom="10px" Padding="2px"></ContentPaddings>
                                            <HeaderStyle Height="23px">
                                                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
<Paddings PaddingLeft="2px" PaddingTop="0px" PaddingBottom="0px"></Paddings>
                                            </HeaderStyle>
                                            <PanelCollection>
                                                <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                                    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:VSS_StatisticsConnectionString %>"
                                                        SelectCommand="SELECT  [StatName],CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date,[StatValue] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where ServerName='azphxdom1/RPRWyatt' and [StatName]='Platform.System.PctCombinedCpuUtil' and Date &gt; DATEADD (hh , -1 ,'2012-06-22 08:17:44.000' ) order by Date asc">
                                                    </asp:SqlDataSource>
                                                    <dx:WebChartControl ID="WebChartControl3" runat="server" DataSourceID="SqlDataSource3"
                                                        Height="300px" Width="500px">
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
                                                    </dx:WebChartControl>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxRoundPanel>
                                    </td>
                                    <td align="left" valign="top">
                                        <dx:ASPxRoundPanel ID="ASPxRoundPanel4" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                            CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Drives" Height="50px"
                                            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css">
                                            <ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
<ContentPaddings Padding="2px" PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                                            <HeaderStyle Height="23px">
                                                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
<Paddings PaddingLeft="2px" PaddingTop="0px" PaddingBottom="0px"></Paddings>
                                            </HeaderStyle>
                                            <PanelCollection>
                                                <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                                    <dx:WebChartControl ID="WebChartControl4" runat="server" Height="300px" Width="500px">
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
                                                <cc1:PieSeriesLabel LineVisible="True" Position="Radial" TextColor="Black" BackColor="Transparent" Font="Tahoma, 8pt, style=Bold">
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
                                            <cc1:Pie3DSeriesView>
                                            </cc1:Pie3DSeriesView>
                                        </viewserializable>
                                        <labelserializable>
                                            <cc1:Pie3DSeriesLabel LineVisible="True">
                                                <fillstyle>
                                                    <optionsserializable>
                                                        <cc1:SolidFillOptions />
                                                    </optionsserializable>
                                                </fillstyle>
                                                <pointoptionsserializable>
                                                    <cc1:PiePointOptions>
                                                    </cc1:PiePointOptions>
                                                </pointoptionsserializable>
                                            </cc1:Pie3DSeriesLabel>
                                        </labelserializable>
                                        <legendpointoptionsserializable>
                                            <cc1:PiePointOptions>
                                            </cc1:PiePointOptions>
                                        </legendpointoptionsserializable>
                                    </seriestemplate>
                                                        <titles>
                                                            <cc1:ChartTitle Alignment="Near" Text="Drive C" />
                                                        </titles>
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
                        <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
                <dx:TabPage Text="Outages">
                    <ContentCollection>
                        <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
                <dx:TabPage Text="Alert History">
                    <ContentCollection>
                        <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
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
    </form>
</body>
</html>
