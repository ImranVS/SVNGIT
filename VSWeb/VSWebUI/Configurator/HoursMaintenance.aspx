<%@ Page Title="VitalSigns Plus-HoursMaintenance" Language="C#" MasterPageFile="~/Site1.Master"AutoEventWireup="true" CodeBehind="HoursMaintenance.aspx.cs" Inherits="VSWebUI.HoursMaintenance" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>






    <%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx1" %>

<%@ Register assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGauges" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGauges.Gauges" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGauges.Gauges.Linear" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGauges.Gauges.Circular" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGauges.Gauges.State" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGauges.Gauges.Digital" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">


    .dxeBase
{
	font: 12px Tahoma;
}
    
.dxeEditAreaSys
{
	width: 100%;
	background-position: 0 0; /*iOS Safari*/
}

    
.dxeEditAreaSys
{
	border: 0px!important;
	padding: 0px;
}
    
.dxWeb_edtCheckBoxUnchecked {
	background-repeat: no-repeat;
    background-color: transparent;
    width: 15px;
    height: 15px;
}

    .dxWeb_edtCheckBoxUnchecked {
	background-position: -41px -99px;
}

.dxICheckBox 
{
	margin: auto;
	display: inline-block;
	vertical-align: middle;
}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

   

    <dx:ASPxPageControl Font-Bold="True"  ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
        CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
        CssPostfix="Glass" Height="308px" 
        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
        Width="617px" TabSpacing="0px" EnableHierarchyRecreation="False">
        <TabPages>
            <dx:TabPage Text="Business Hours">
                <ContentCollection>
                    <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                        <table style="width:100%;">
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td colspan="3">
                                    <dx:ASPxLabel ID="ASPxLabel25" runat="server" 
                                        Text="Business Hours are used for determining some statistics. and also used to determine the Scan Interval of specific servers." CssClass="lblsmallFont">
                                    </dx:ASPxLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <dx:ASPxLabel ID="ASPxLabel30" runat="server" Text="Business Day Starts" CssClass="lblsmallFont">
                                    </dx:ASPxLabel>
                                </td>
                                <td>
                                    <dx:ASPxLabel ID="ASPxLabel31" runat="server" Text="Business Day Ends" CssClass="lblsmallFont">
                                    </dx:ASPxLabel>
                                </td>
                                <td>
                                    <dx:ASPxCheckBox ID="ASPxCheckBox14" runat="server" CheckState="Unchecked" 
                                        Text="Open 24 Hours" CssClass="lblsmallFont">
                                    </dx:ASPxCheckBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <dx:ASPxGaugeControl ID="ASPxGaugeControl1" runat="server" BackColor="#DDECFE" 
                                        Height="260px" Value="0" Width="150px">
                                        <Gauges>
                                            <dx:CircularGauge Bounds="0, 0, 150, 260" Name="Clock">
                                                <scales>
                                                    <dx:ArcScaleComponent Center="125, 125" EndAngle="270" MajorTickCount="13" 
                                                        MajorTickmark-FormatString="{0:F0}" MajorTickmark-ShapeType="Line" 
                                                        MajorTickmark-ShowFirst="False" MajorTickmark-TextOrientation="LeftToRight" 
                                                        MaxValue="12" MinorTickCount="0" Name="hours" RadiusX="98" RadiusY="98" 
                                                        Shader="&lt;ShaderObject Type=&quot;Gray&quot;/&gt;" StartAngle="-90">
                                                    </dx:ArcScaleComponent>
                                                    <dx:ArcScaleComponent Center="125, 125" EndAngle="270" MajorTickCount="13" 
                                                        MajorTickmark-FormatString="{0:F0}" MajorTickmark-ShapeType="Diamond" 
                                                        MajorTickmark-ShowFirst="False" MajorTickmark-TextOrientation="LeftToRight" 
                                                        MaxValue="12" MinorTickCount="0" Name="minutes" RadiusX="98" RadiusY="98" 
                                                        StartAngle="-90" Value="1" ZOrder="1001">
                                                    </dx:ArcScaleComponent>
                                                    <dx:ArcScaleComponent Center="125, 125" EndAngle="270" MajorTickCount="13" 
                                                        MajorTickmark-FormatString="{0:F0}" MajorTickmark-ShapeType="Diamond" 
                                                        MajorTickmark-ShowFirst="False" MajorTickmark-TextOrientation="LeftToRight" 
                                                        MaxValue="12" MinorTickCount="0" Name="seconds" RadiusX="98" RadiusY="98" 
                                                        StartAngle="-90" Value="1" ZOrder="1001">
                                                    </dx:ArcScaleComponent>
                                                </scales>
                                                <backgroundlayers>
                                                    <dx:ArcScaleBackgroundLayerComponent Name="bg" ScaleID="hours" 
                                                        ShapeType="CircularFull_Clock" ZOrder="1000" />
                                                </backgroundlayers>
                                                <effectlayers>
                                                    <dx:ArcScaleEffectLayerComponent Name="glass" ScaleCenterPos="0.5, 1" 
                                                        ScaleID="hours" ShapeType="CircularFull_Clock" Size="196, 98" ZOrder="-1000" />
                                                </effectlayers>
                                                <needles>
                                                    <dx:ArcScaleNeedleComponent EndOffset="10" Name="hourNeedle" ScaleID="hours" 
                                                        ShapeType="CircularFull_ClockHour" ZOrder="-50" />
                                                    <dx:ArcScaleNeedleComponent EndOffset="10" Name="minuteNeedle" 
                                                        ScaleID="minutes" ShapeType="CircularFull_ClockMinute" ZOrder="-50" />
                                                    <dx:ArcScaleNeedleComponent Name="secondNeedle" ScaleID="seconds" 
                                                        ShapeType="CircularFull_ClockSecond" ZOrder="-50" />
                                                </needles>
                                                <spindlecaps>
                                                    <dx:ArcScaleSpindleCapComponent Name="cap" ScaleID="hours" 
                                                        ShapeType="CircularFull_Clock" Size="20, 20" ZOrder="-100" />
                                                </spindlecaps>
                                            </dx:CircularGauge>
                                        </Gauges>
                                        <layoutpadding all="0" bottom="0" left="0" right="0" top="0" />

<LayoutPadding All="0" Left="0" Top="0" Right="0" Bottom="0"></LayoutPadding>
                                    </dx:ASPxGaugeControl>
                                </td>
                                <td>
                                    <dx:ASPxGaugeControl ID="ASPxGaugeControl3" runat="server" BackColor="#DDECFE" 
                                        Height="260px" Value="1" Width="150px">
                                        <Gauges>
                                            <dx:CircularGauge Bounds="0, 0, 150, 260" Name="Clock">
                                                <scales>
                                                    <dx:ArcScaleComponent Center="125, 125" EndAngle="270" MajorTickCount="13" 
                                                        MajorTickmark-FormatString="{0:F0}" MajorTickmark-ShapeType="Line" 
                                                        MajorTickmark-ShowFirst="False" MajorTickmark-TextOrientation="LeftToRight" 
                                                        MaxValue="12" MinorTickCount="0" Name="hours" RadiusX="98" RadiusY="98" 
                                                        Shader="&lt;ShaderObject Type=&quot;Gray&quot;/&gt;" StartAngle="-90" 
                                                        Value="1">
                                                    </dx:ArcScaleComponent>
                                                    <dx:ArcScaleComponent Center="125, 125" EndAngle="270" MajorTickCount="13" 
                                                        MajorTickmark-FormatString="{0:F0}" MajorTickmark-ShapeType="Diamond" 
                                                        MajorTickmark-ShowFirst="False" MajorTickmark-TextOrientation="LeftToRight" 
                                                        MaxValue="12" MinorTickCount="0" Name="minutes" RadiusX="98" RadiusY="98" 
                                                        StartAngle="-90" Value="1" ZOrder="1001">
                                                    </dx:ArcScaleComponent>
                                                    <dx:ArcScaleComponent Center="125, 125" EndAngle="270" MajorTickCount="13" 
                                                        MajorTickmark-FormatString="{0:F0}" MajorTickmark-ShapeType="Diamond" 
                                                        MajorTickmark-ShowFirst="False" MajorTickmark-TextOrientation="LeftToRight" 
                                                        MaxValue="12" MinorTickCount="0" Name="seconds" RadiusX="98" RadiusY="98" 
                                                        StartAngle="-90" Value="1" ZOrder="1001">
                                                    </dx:ArcScaleComponent>
                                                </scales>
                                                <backgroundlayers>
                                                    <dx:ArcScaleBackgroundLayerComponent Name="bg" ScaleID="hours" 
                                                        ShapeType="CircularFull_Clock" ZOrder="1000" />
                                                </backgroundlayers>
                                                <effectlayers>
                                                    <dx:ArcScaleEffectLayerComponent Name="glass" ScaleCenterPos="0.5, 1" 
                                                        ScaleID="hours" ShapeType="CircularFull_Clock" Size="196, 98" ZOrder="-1000" />
                                                </effectlayers>
                                                <needles>
                                                    <dx:ArcScaleNeedleComponent EndOffset="10" Name="hourNeedle" ScaleID="hours" 
                                                        ShapeType="CircularFull_ClockHour" ZOrder="-50" />
                                                    <dx:ArcScaleNeedleComponent EndOffset="10" Name="minuteNeedle" 
                                                        ScaleID="minutes" ShapeType="CircularFull_ClockMinute" ZOrder="-50" />
                                                    <dx:ArcScaleNeedleComponent Name="secondNeedle" ScaleID="seconds" 
                                                        ShapeType="CircularFull_ClockSecond" ZOrder="-50" />
                                                </needles>
                                                <spindlecaps>
                                                    <dx:ArcScaleSpindleCapComponent Name="cap" ScaleID="hours" 
                                                        ShapeType="CircularFull_Clock" Size="20, 20" ZOrder="-100" />
                                                </spindlecaps>
                                            </dx:CircularGauge>
                                        </Gauges>
                                        <LayoutPadding All="0" Bottom="0" Left="0" Right="0" Top="0" />

<LayoutPadding All="0" Left="0" Top="0" Right="0" Bottom="0"></LayoutPadding>
                                    </dx:ASPxGaugeControl>
                                </td>
                                <td valign="top">
                                    <dx:ASPxRoundPanel ID="ASPxRoundPanel5" runat="server" 
                                        HeaderText="Days of the week" Width="200px" 
                                        CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                        CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" 
                                        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css">
                                        <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" 
                                            PaddingTop="10px" />
                                        <HeaderStyle Height="23px">
                                        <Paddings PaddingBottom="0px" PaddingLeft="2px" 
                                            PaddingTop="0px" />
                                        </HeaderStyle>
                                        <PanelCollection>
                                            <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                            <dx:ASPxCheckBoxList ID="ASPxCheckBoxList2" runat="server">
                                                    <Items>
                                                        <dx:ListEditItem Text="Monday" />
                                                        <dx:ListEditItem Text="Tuesday" />
                                                        <dx:ListEditItem Text="Wednesday" />
                                                        <dx:ListEditItem Text="Thursday" />
                                                        <dx:ListEditItem Text="Friday" />
                                                        <dx:ListEditItem Text="Saturday" />
                                                        <dx:ListEditItem Text="Sunday" />
                                                    </Items>
                                                </dx:ASPxCheckBoxList>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dx:ASPxRoundPanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <dx:ASPxTimeEdit ID="ASPxTimeEdit5" runat="server" Width="90px">
                                    </dx:ASPxTimeEdit>
                                </td>
                                <td>
                                    <dx:ASPxTimeEdit ID="ASPxTimeEdit6" runat="server" Width="90px">
                                    </dx:ASPxTimeEdit>
                                </td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                        </table>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
            <dx:TabPage Text="Maintenance Windows">
                <ContentCollection>
                    <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                        <table style="width:100%;">
                            <tr>
                                <td valign="top" rowspan="2">
                                    &nbsp;</td>
                                <td valign="top">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                  <dx:ASPxGridView ID="MntWinDominoMaintGridView" runat="server" AutoGenerateColumns="False"
                                                                    KeyFieldName="MaintWindow" Theme="Office2003Blue"
                                        CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                                                                    Width="656px">
                                                                    <Columns>
                                                                        <dx:GridViewCommandColumn VisibleIndex="0" Width="80px">
                                                                            <EditButton Visible="True">
                                                                            </EditButton>
                                                                            <NewButton Visible="True">
                                                                            </NewButton>
                                                                            <DeleteButton Visible="True">
                                                                            </DeleteButton>
                                                                        </dx:GridViewCommandColumn>
                                                                        <dx:GridViewDataTextColumn FieldName="MaintWindow" Caption="Start Time" 
                                                                            VisibleIndex="0" />
                                                                        <dx:GridViewDataComboBoxColumn Caption="Name"
                                                                            ShowInCustomizationForm="True" VisibleIndex="0">
                                                                        </dx:GridViewDataComboBoxColumn>
                                                                        <dx:GridViewDataTextColumn Caption="End Time" ShowInCustomizationForm="True" 
                                                                            VisibleIndex="1">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataCheckColumn Caption="Sunday" ShowInCustomizationForm="True" 
                                                                            VisibleIndex="2">
                                                                        </dx:GridViewDataCheckColumn>
                                                                        <dx:GridViewDataCheckColumn Caption="Monday" ShowInCustomizationForm="True" 
                                                                            VisibleIndex="3">
                                                                        </dx:GridViewDataCheckColumn>
                                                                        <dx:GridViewDataCheckColumn Caption="Tuesday" ShowInCustomizationForm="True" 
                                                                            VisibleIndex="4">
                                                                        </dx:GridViewDataCheckColumn>
                                                                        <dx:GridViewDataCheckColumn Caption="Wednesday" ShowInCustomizationForm="True" 
                                                                            VisibleIndex="5">
                                                                        </dx:GridViewDataCheckColumn>
<dx:GridViewDataCheckColumn ShowInCustomizationForm="True" Caption="Thursday" VisibleIndex="6"></dx:GridViewDataCheckColumn>
                                                                        <dx:GridViewDataCheckColumn Caption="Friday" ShowInCustomizationForm="True" 
                                                                            VisibleIndex="7">
                                                                        </dx:GridViewDataCheckColumn>
                                                                        <dx:GridViewDataCheckColumn Caption="Saturday" ShowInCustomizationForm="True" 
                                                                            VisibleIndex="8">
                                                                        </dx:GridViewDataCheckColumn>
                                                                    </Columns>
                                                                    <SettingsBehavior ConfirmDelete="True" />
                                                                    <SettingsText ConfirmDelete="Are you sure you want to delete?" />
                                                                    <SettingsLoadingPanel ImagePosition="Top" />
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
                                                                        <LoadingPanel ImageSpacing="5px">
                                                                        </LoadingPanel>
                                                                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                        </Header>
                                                                    </Styles>
                                                                    <StylesEditors ButtonEditCellSpacing="0">
                                                                        <ProgressBar Height="21px">
                                                                        </ProgressBar>
                                                                    </StylesEditors>
                                                                </dx:ASPxGridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                        </table>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>  
        </TabPages>
        <LoadingPanelImage Url="~/App_Themes/Glass/Web/Loading.gif">
        </LoadingPanelImage>
        <Paddings PaddingLeft="0px" />
        <ContentStyle>
            <Border BorderColor="#4986A2" />
            <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px">
            </Border>
        </ContentStyle>
    </dx:ASPxPageControl>


     </asp:Content>
