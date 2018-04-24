<%@ Page Title="VitalSigns Plus - Domino Servers - Consecutive Days of Operation" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="ServerDaysUp.aspx.cs" Inherits="VSWebUI.Dashboard.ServerDaysUp" %>
<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<%@ Register assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts.Web" tagprefix="dxchartsui" %>
<%@ Register assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
    function Resized() {
        var callbackState = document.getElementById('ContentPlaceHolder1_callbackState').value;

        if (callbackState == 0)
            DoCallback();
    }

    function DoCallback() {
        document.getElementById('ContentPlaceHolder1_chartWidth').value = document.body.offsetWidth - 105;
        ServerUpChart.PerformCallback();
    }

    function ResizeChart(s, e) {
        document.getElementById('ContentPlaceHolder1_callbackState').value = 0;
        s.GetMainElement().style.width = document.getElementById('ContentPlaceHolder1__chartWidth').value + "px";
    }

    function ResetCallbackState() {
        window.document.form1.elements['ContentPlaceHolder1_callbackState'].value = 1;
    }
    </script>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input id="chartWidth" type="hidden" runat="server" value="400" />
        <input id="callbackState" type="hidden" runat="server" value="0" />
        <table width="100%">
    <tr>
        <td>
            <div class="header" id="servernamelbldisp" runat="server">Domino Servers - Consecutive Days of Operation</div>
        </td>
        <td>
            &nbsp;
        </td>    
        <td align="right">
            <table>
                <tr>
                    <td>
                    </td>
                </tr>
            </table>
        </td>        
    </tr>
</table>
<table width="100%">
    <tr>
        <td>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table>
        <tr>
            <td>
                <table>
                    <tr>
            <td>
                <dx:ASPxRadioButtonList ID="SortRadioButtonList" runat="server" 
                    RepeatDirection="Horizontal" SelectedIndex="0" AutoPostBack="True" 
                    onselectedindexchanged="SortRadioButtonList_SelectedIndexChanged">
                    <Items>
                        <dx:ListEditItem Text="By Server" Value="Server" Selected="True" />
                        <dx:ListEditItem Text="By Duration" Value="Duration" />
                    </Items>
                </dx:ASPxRadioButtonList>
            </td>
            <td>

                &nbsp;</td>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Type:" 
                                CssClass="lblsmallFont">
                            </dx:ASPxLabel>
                        </td>
            <td>
                <dx:ASPxComboBox ID="TypeComboBox" runat="server" ValueType="System.String">
                </dx:ASPxComboBox>
            </td>
            <td>
                &nbsp;
            </td>
                        <td>
                            <dx:ASPxButton ID="GoButton" runat="server" onclick="GoButton_Click" Text="Submit" CssClass="sysButton">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <dxchartsui:WebChartControl ID="ServerUpWebChartControl" 
                    ClientInstanceName="ServerUpChart" runat="server" 
                    AppearanceNameSerializable="In A Fog" Height="500px" Width="800px" 
                    OnCustomCallback="ServerUpWebChartControl_CustomCallback" 
                    CrosshairEnabled="True" 
                    onbounddatachanged="ServerUpWebChartControl_BoundDataChanged" 
                    PaletteName="Module">
                    <diagramserializable>
                        <cc1:XYDiagram Rotated="True">
                            <axisx visibleinpanesserializable="-1">
                                <tickmarks minorvisible="False" /><tickmarks minorvisible="False" />
<Tickmarks MinorVisible="False"></Tickmarks>

                                <label>
                                    <resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" />
<ResolveOverlappingOptions AllowRotate="False"></ResolveOverlappingOptions>
                                </label>
                                <range sidemarginsenabled="True" /><range sidemarginsenabled="True" />

<Range SideMarginsEnabled="True"></Range>
                            <visualrange autosidemargins="True" /><wholerange autosidemargins="True" />
                                <visualrange autosidemargins="True" />
                                <wholerange autosidemargins="True" />
                            </axisx>
                            <axisy visibleinpanesserializable="-1" title-text="Days Up" 
                                title-visible="True">
                                <tickmarks minorvisible="False" /><tickmarks minorvisible="False" />
<Tickmarks MinorVisible="False"></Tickmarks>

                                <label>
                                    <resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions 
                                allowrotate="False" allowstagger="False" />
<ResolveOverlappingOptions AllowRotate="False"></ResolveOverlappingOptions>
                                <numericoptions format="Number" precision="0" /></label>
                                <range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" />
                                <numericoptions format="Number" precision="0" />

<Range SideMarginsEnabled="True"></Range>

<NumericOptions Format="Number" Precision="0"></NumericOptions>
                            <visualrange autosidemargins="True" /><wholerange autosidemargins="True" />
                                <visualrange autosidemargins="True" />
                                <wholerange autosidemargins="True" />
                            </axisy>
                            <defaultpane backcolor="255, 255, 255">
                            </defaultpane>
                        </cc1:XYDiagram>
                    </diagramserializable>
                    <fillstyle>
                        <optionsserializable>
                            <cc1:SolidFillOptions />
                        </optionsserializable>
                    </fillstyle>
                    <legend visible="False"></legend>
                    <seriesserializable>
                        <cc1:Series LabelsVisibility="True" Name="Series 1">
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
                        <cc1:Series LabelsVisibility="True" Name="Series 2">
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
    </ContentTemplate>
    </asp:UpdatePanel>
        </td>
    </tr>
</table>
          
</asp:Content>