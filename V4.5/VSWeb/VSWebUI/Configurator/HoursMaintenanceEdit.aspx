<%@ Page Title="VitalSigns Plus" Language="C#" MasterPageFile="~/Site1.Master"AutoEventWireup="true" CodeBehind="HoursMaintenanceEdit.aspx.cs" Inherits="VSWebUI.HoursMaintenanceEdit" %>
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
            background-color: #FFFFFF;
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
       <table width="400">
                            <tr>
                                  <td colspan="4">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center" style="background-color: #FFFFFF">
                                    <dx:ASPxLabel ID="ASPxLabel32" runat="server" 
                                        Text="Edit Maintenance Window: Friday Night 10 to 11" 
                                        Font-Bold="False" Font-Size="Large" Width="100%">
                                    </dx:ASPxLabel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="4" style="background-color: #FFFFFF">
                                    <dx:ASPxLabel ID="ASPxLabel25" runat="server" 
                                        
                                        Text="Maintenance Windows are specific days and times when servers should not be monitored.  For example, you should define a Maintenance Window for those times when the server is shut down for backup or scheduled maintenance." style="color: #15428B">
                                    </dx:ASPxLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td valign="middle">
                                    s
                                </td>
                                <td>
                                    <dx:ASPxTextBox runat="server" CssClass="txtbig" ID="ASPxTextBox16">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td valign="top">
                                    <dx:ASPxRoundPanel ID="ASPxRoundPanel6" runat="server" 
                                        HeaderText="Time Settings" Width="200px" 
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
                                            <dx:PanelContent ID="PanelContent2" runat="server" 
                                                SupportsDisabledAttribute="True">
                                                <table style="width:100%;">
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxLabel ID="ASPxLabel30" runat="server" Text="Start Time" CssClass="lblsmallFont">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxLabel ID="ASPxLabel31" runat="server" Text="End Time" CssClass="lblsmallFont">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                        <td>
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxGaugeControl ID="ASPxGaugeControl1" runat="server" BackColor="White" 
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
                                                                <LayoutPadding All="0" Bottom="0" Left="0" Right="0" Top="0" />
                                                            </dx:ASPxGaugeControl>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxGaugeControl ID="ASPxGaugeControl3" runat="server" BackColor="White" 
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
                                                                <LayoutPadding All="0" Bottom="0" Left="0" Right="0" Top="0" />
                                                            </dx:ASPxGaugeControl>
                                                        </td>
                                                        <td>
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
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
                                                </table>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dx:ASPxRoundPanel>
                                </td>
                                <td valign="top">
                                    <dx:ASPxRoundPanel ID="ASPxRoundPanel5" runat="server" 
                                        HeaderText="Days of the Week" Width="200px" 
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
                                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                            <dx:ASPxCheckBoxList ID="ASPxCheckBoxList2" runat="server" CssClass="lblsmallFont">
                                                    <Items>
                                                         <dx:ListEditItem Text="Select All" />
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
                    <table>
                        <tr>
                            <td>
                                <dx:ASPxButton ID="FormOkButton" runat="server" Text="Ok" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
                                    CssPostfix="Office2010Blue" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
                                    Width="75px" >
                                </dx:ASPxButton>
                            </td>
                            <td>
                                <dx:ASPxButton ID="FormCancelButton" runat="server" Text="Cancel" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
                                    CssPostfix="Office2010Blue" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
                                    Width="75px">
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                                </td>
                            </tr>
                            </table>
 </td>
        </tr>
        <tr>            
                
                <td align=right>
                </td>
        </tr>
    </table>

</asp:Content>
