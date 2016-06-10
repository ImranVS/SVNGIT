<%@ Page Title="" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="DominoServerDetailsPage.aspx.cs" Inherits="VSWebUI.Configurator.DominoServerDetailsPage" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>





<%@ Register assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts.Web" tagprefix="dxchartsui" %>
<%@ Register assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dx:ASPxLabel ID="servernameASPxLabel" runat="server" Style="font-weight: 700" Text="azphxweb1/RPRWyatt">
    </dx:ASPxLabel>
         
    <br />
    <br />
    <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
    CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" TabSpacing="0px" 
    Width="100%">
        <TabPages>
            <dx:TabPage Text="Overall">
                <ContentCollection>
                    <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                        <asp:ScriptManager ID="ScriptManager1" runat="server">
                        </asp:ScriptManager>
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <dx:ASPxRoundPanel ID="performanceASPxRoundPanel" runat="server" 
                            CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                            GroupBoxCaptionOffsetY="-24px" HeaderText="Performance" 
                            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                            <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                            <HeaderStyle Height="23px">
                            <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                            </HeaderStyle>
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                    <dx:ASPxRadioButtonList ID="performanceASPxRadioBttLst" runat="server" 
                                        AutoPostBack="True" CssClass="Glass" 
                                        CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                        OnSelectedIndexChanged="performanceASPxRadioBttLst_SelectedIndexChanged" 
                                        RepeatDirection="Horizontal" SelectedIndex="0" 
                                        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css">
                                        <Items>
                                            <dx:ListEditItem Selected="True" Value="hh" Text="Per Hour" />
                                            <dx:ListEditItem Value="dd" Text="Per Day" />
                                        </Items>
                                    </dx:ASPxRadioButtonList>
                                    <dxchartsui:WebChartControl ID="WebChartControl1" runat="server" Height="300px" 
                                        Width="600px" PaletteName="Palette 1">
                                        <fillstyle>
                                            <optionsserializable>
                                                <cc1:SolidFillOptions />
                                            </optionsserializable>
                                        </fillstyle>
                                        <seriesserializable>
                                            <cc1:Series Name="Series 1" ShowInLegend="False" Visible="False" >
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
                                        <palettewrappers>
                                            <dxchartsui:PaletteWrapper Name="Palette 1" ScaleMode="Repeat">
                                                <palette>
                                                    <cc1:PaletteEntry Color="Navy" Color2="Navy" />
                                                </palette>
                                            </dxchartsui:PaletteWrapper>
                                        </palettewrappers>
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
                                </ContentTemplate>

                                </asp:UpdatePanel>
                                
                            </td>
                            <td valign="top">
                                <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" 
                            CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                            GroupBoxCaptionOffsetY="-24px" HeaderText="Disk Space" 
                            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="200px" Height="388px">
                            <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                            <HeaderStyle Height="23px">
                            <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                            </HeaderStyle>
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                                    <dxchartsui:WebChartControl ID="DiskSpaceWebChartControl" runat="server" Height="300px" 
                                        Width="600px" PaletteName="Palette 1" EnableDefaultAppearance="False">
                                        <fillstyle>
                                            <optionsserializable>
                                                <cc1:SolidFillOptions />
                                            </optionsserializable>
                                        </fillstyle>
                                        <legend visible="False"></legend>
                                        <seriesserializable>
                                             <cc1:Series ShowInLegend="False" LegendText="ABC" Visible="False">
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
                                                        <cc1:PiePointOptions PointView="Argument">
                                                       
                                                        </cc1:PiePointOptions>
                                                    </pointoptionsserializable>
                                                </cc1:PieSeriesLabel>
                                            </labelserializable>
                                            <legendpointoptionsserializable>
                                                <cc1:PiePointOptions PointView="Argument">
                                                </cc1:PiePointOptions>
                                            </legendpointoptionsserializable>
                                        </cc1:Series>
                                        </seriesserializable>
                                        <seriestemplate>
                                            <viewserializable>
                                                <cc1:PieSeriesView RuntimeExploding="False">
                                                </cc1:PieSeriesView>
                                            </viewserializable>
                                            <labelserializable>
                                                <cc1:PieSeriesLabel LineVisible="True">
                                                    <fillstyle>
                                                        <optionsserializable>
                                                            <cc1:SolidFillOptions />
                                                        </optionsserializable>
                                                    </fillstyle>
                                                    <pointoptionsserializable>
                                                        <cc1:PiePointOptions>
                                                        </cc1:PiePointOptions>
                                                    </pointoptionsserializable>
                                                </cc1:PieSeriesLabel>
                                            </labelserializable>
                                            <legendpointoptionsserializable>
                                                <cc1:PiePointOptions>
                                                </cc1:PiePointOptions>
                                            </legendpointoptionsserializable>
                                        </seriestemplate>
                                        <palettewrappers>
                                            <dxchartsui:PaletteWrapper Name="Palette 1" ScaleMode="Repeat">
                                                <palette>
                                                    <cc1:PaletteEntry Color="0, 198, 0" Color2="0, 198, 0" />
                                                    <cc1:PaletteEntry Color="Red" Color2="Red" />
                                                </palette>
                                            </dxchartsui:PaletteWrapper>
                                        </palettewrappers>

<CrosshairOptions><CommonLabelPositionSerializable>
<cc1:CrosshairMousePosition></cc1:CrosshairMousePosition>
</CommonLabelPositionSerializable>
</CrosshairOptions>

<ToolTipOptions><ToolTipPositionSerializable>
<cc1:ToolTipMousePosition></cc1:ToolTipMousePosition>
</ToolTipPositionSerializable>
</ToolTipOptions>
                                    </dxchartsui:WebChartControl>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                            </td>
                        </tr>
                        <tr>
                            <td><asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                <dx:ASPxRoundPanel ID="cpuGraphASPxRoundPanel" runat="server" 
                            CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                            GroupBoxCaptionOffsetY="-24px" HeaderText="CPU" 
                            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                            <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                            <HeaderStyle Height="23px">
                            <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                            </HeaderStyle>
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                                    <dx:ASPxRadioButtonList ID="cpuASPxRadioButtonList" runat="server" 
                                        AutoPostBack="True" CssClass="Glass" 
                                        CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                        OnSelectedIndexChanged="cpuASPxRadioButtonList_SelectedIndexChanged" 
                                        RepeatDirection="Horizontal" SelectedIndex="0" 
                                        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css">
                                        <Items>
                                            <dx:ListEditItem Selected="True" Value="hh" Text="Per Hour" />
                                            <dx:ListEditItem Value="dd" Text="Per Day" />
                                        </Items>
                                    </dx:ASPxRadioButtonList>
                                    <dxchartsui:WebChartControl ID="cpuWebChartControl" runat="server"  
                                        Height="300px" Width="600px" PaletteName="Palette 1">
                                        <fillstyle>
                                            <optionsserializable>
                                                <cc1:SolidFillOptions />
                                            </optionsserializable>
                                        </fillstyle>
                                        <seriesserializable>
                                            <cc1:Series Name="Series 1" ShowInLegend="False" Visible="False">
                                                <viewserializable>
                                                    
                                                <cc1:SplineSeriesView></cc1:SplineSeriesView></viewserializable>
                                                <labelserializable>
                                                    
                                                <cc1:PointSeriesLabel LineVisible="True" Visible="False"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:PointSeriesLabel></labelserializable>
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
                                        <palettewrappers>
                                            <dxchartsui:PaletteWrapper Name="Palette 1" ScaleMode="Repeat">
                                                <palette>
                                                    <cc1:PaletteEntry Color="Navy" Color2="Navy" />
                                                </palette>
                                            </dxchartsui:PaletteWrapper>
                                        </palettewrappers>
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
                            </ContentTemplate>
                            </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <dx:ASPxRoundPanel ID="memASPxRoundPanel" runat="server" 
                            CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                            GroupBoxCaptionOffsetY="-24px" HeaderText="Memory" 
                            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="200px">
                            <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                            <HeaderStyle Height="23px">
                            <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                            </HeaderStyle>
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
                                    <dx:ASPxRadioButtonList ID="memASPxRadioButtonList" runat="server" 
                                        AutoPostBack="True" CssClass="Glass" 
                                        CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                        RepeatDirection="Horizontal" SelectedIndex="0" 
                                        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
                                        OnSelectedIndexChanged="memASPxRadioButtonList_SelectedIndexChanged">
                                        <Items>
                                            <dx:ListEditItem Selected="True" Value="hh" Text="Per Hour" />
                                            <dx:ListEditItem Value="dd" Text="Per Day" />
                                        </Items>
                                    </dx:ASPxRadioButtonList>
                                    <dxchartsui:WebChartControl ID="WebChartControl4" runat="server" Height="300px" 
                                        Width="600px" PaletteName="Palette 1">
                                        <fillstyle>
                                            <optionsserializable>
                                                <cc1:SolidFillOptions />
                                            </optionsserializable>
                                        </fillstyle>
                                        <seriesserializable>
                                            <cc1:Series Name="Series 1" ShowInLegend="False" Visible="False">
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
                                        <palettewrappers>
                                            <dxchartsui:PaletteWrapper Name="Palette 1" ScaleMode="Repeat">
                                                <palette>
                                                    <cc1:PaletteEntry Color="Navy" Color2="Navy" />
                                                </palette>
                                            </dxchartsui:PaletteWrapper>
                                        </palettewrappers>
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
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                <dx:ASPxRoundPanel ID="usersASPxRoundPanel" runat="server" 
                            CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                            GroupBoxCaptionOffsetY="-24px" HeaderText="Users" 
                            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                            <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                            <HeaderStyle Height="23px">
                            <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                            </HeaderStyle>
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent5" runat="server" SupportsDisabledAttribute="True">
                                    <dx:ASPxRadioButtonList ID="usersASPxRadioButtonList" runat="server" 
                                        AutoPostBack="True" CssClass="Glass" 
                                        CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                        RepeatDirection="Horizontal" SelectedIndex="0" 
                                        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" OnSelectedIndexChanged="usersASPxRadioButtonList_SelectedIndexChanged" 
                                        >
                                        <Items>
                                            <dx:ListEditItem Selected="True" Value="hh" Text="Per Hour" />
                                            <dx:ListEditItem Value="dd" Text="Per Day" />
                                        </Items>
                                    </dx:ASPxRadioButtonList>
                                    <dxchartsui:WebChartControl ID="usersWebChartControl" runat="server" 
                                        Height="300px" Width="600px" PaletteName="Palette 1">
                                        <fillstyle>
                                            <optionsserializable>
                                                <cc1:SolidFillOptions />
                                            </optionsserializable>
                                        </fillstyle>
                                        <seriesserializable>
                                            <cc1:Series Name="Series 1" ShowInLegend="False" ArgumentDataMember="Date" 
                                                ValueDataMembersSerializable="StatValue" Visible="False">
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
                                        <palettewrappers>
                                            <dxchartsui:PaletteWrapper Name="Palette 1" ScaleMode="Repeat">
                                                <palette>
                                                    <cc1:PaletteEntry Color="Navy" Color2="Navy" />
                                                </palette>
                                            </dxchartsui:PaletteWrapper>
                                        </palettewrappers>
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
                        </ContentTemplate>
                        </asp:UpdatePanel>
                            </td>
                            <td>
                                <dx:ASPxRoundPanel ID="mailASPxRoundPanel" runat="server" 
                            CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                            GroupBoxCaptionOffsetY="-24px" HeaderText="Mail" 
                            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                            <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                            <HeaderStyle Height="23px">
                            <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                            </HeaderStyle>
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent6" runat="server" SupportsDisabledAttribute="True">
                                    <dx:ASPxRadioButtonList ID="mailASPxRadioButtonList" runat="server" 
                                        AutoPostBack="True" CssClass="Glass" 
                                        CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                        OnSelectedIndexChanged="performanceASPxRadioBttLst_SelectedIndexChanged" 
                                        RepeatDirection="Horizontal" SelectedIndex="0" 
                                        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css">
                                        <Items>
                                            <dx:ListEditItem Selected="True" Value="hh" Text="Per Hour" />
                                            <dx:ListEditItem Value="dd" Text="Per Day" />
                                        </Items>
                                    </dx:ASPxRadioButtonList>
                                    <dxchartsui:WebChartControl ID="mailWebChartControl" runat="server" 
                                        Height="300px" Width="600px">
                                        <diagramserializable>
                                            <cc1:XYDiagram>
                                                <axisx visibleinpanesserializable="-1" title-text="Time" title-visible="true">
                                                    <range sidemarginsenabled="True" />

<Range SideMarginsEnabled="True"></Range>

                                                </axisx>
                                                <axisy visibleinpanesserializable="-1" title-text="Response-Time" title-visible="true">
                                                    <range sidemarginsenabled="True" />
<Range SideMarginsEnabled="True" AlwaysShowZeroLevel="False"></Range>
                                                </axisy>
                                            </cc1:XYDiagram>
                                        </diagramserializable>
                                        <fillstyle>
                                            <optionsserializable>
                                                <cc1:SolidFillOptions />
                                            </optionsserializable>
                                        </fillstyle>
                                        <seriesserializable>
                                            <cc1:Series Name="Series 1" ShowInLegend="False" ArgumentDataMember="Date" ValueDataMembersSerializable="StatValue">
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
                                    </dxchartsui:WebChartControl>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                            </td>
                        </tr>
                    </table>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
            <dx:TabPage Text="Maintenance">
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
            <dx:TabPage Text="Alerts History">
                <ContentCollection>
                    <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
        </TabPages>
        <LoadingPanelImage Url="~/App_Themes/Glass/Web/Loading.gif">
        </LoadingPanelImage>
        <Paddings PaddingLeft="0px" />
        <ContentStyle>
            <Border BorderColor="#4986A2" />
        </ContentStyle>
    </dx:ASPxPageControl>
</asp:Content>
