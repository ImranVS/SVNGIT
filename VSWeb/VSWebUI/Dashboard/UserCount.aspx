<%@ Page Title="VitalSigns Plus - User Count" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="UserCount.aspx.cs" Inherits="VSWebUI.UserCount" %>
<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>


<%@ Register assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts.Web" tagprefix="dxchartsui" %>
<%@ Register assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    .style1
    {
        width: 100%;
    }
</style>
<script type="text/javascript">
 function Resized() {
        var callbackState = document.getElementById('ContentPlaceHolder1_callbackState').value;

        if (callbackState == 0)
            DoCallback();
    }

    function DoCallback() {
        document.getElementById('ContentPlaceHolder1_chartWidth').value = document.body.offsetWidth - 105;
        //cSelectServerRoundPanel.style.width = document.getElementById('ContentPlaceHolder1__chartWidth').value + "px";
        // FileOpensCumulativeWebChart.PerformCallback();
        UserCountsChart.PerformCallback();
     
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
     <input id="chartWidth" type="hidden" runat="server" value="400" />
        <input id="callbackState" type="hidden" runat="server" value="0" />
        <table width="100%">
    <tr>
        <td>
            <div class="header" id="servernamelbldisp" runat="server">User Counts</div>
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
<table class="style1">
    <tr><td>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <table>
        <tr>
        <td>
            <dx:ASPxRadioButtonList ID="SortRadioButtonList1" runat="server" 
                AutoPostBack="True" 
                OnSelectedIndexChanged="SortRadioButtonList1_SelectedIndexChanged" 
                RepeatDirection="Horizontal" SelectedIndex="0">
                <Items>
                    <dx:ListEditItem Selected="True" Text="By Server" Value="Server" />
                    <dx:ListEditItem Text="By User Count" Value="UserCount" />
                </Items>
            </dx:ASPxRadioButtonList>
        </td>
        <td>
            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Type:" 
                CssClass="lblsmallFont">
            </dx:ASPxLabel>
        </td>
        <td>
            <dx:ASPxComboBox ID="TypeComboBox" runat="server" ValueType="System.String">
                <Items>
                                <dx:ListEditItem Text="Select Type" Value="Select Type" />
                            </Items>
            </dx:ASPxComboBox>
            
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            <dx:ASPxButton ID="GoButton" runat="server" Text="Submit" CssClass="sysButton"
                onclick="GoButton_Click">
            </dx:ASPxButton>
        </td>
        </tr>
        </table>
        </ContentTemplate>
        </asp:UpdatePanel>

    </td></tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                <dxchartsui:WebChartControl ID="UserCountWebChartControl" runat="server" 
                    Height="500px" Width="800px" ClientInstanceName="UserCountsChart" 
                    OnCustomCallback="UserCountWebChartControl_CustomCallback" 
                        CrosshairEnabled="True" 
                        onbounddatachanged="UserCountWebChartControl_BoundDataChanged">
                    <diagramserializable>
                        <cc1:XYDiagram>
                            <axisx gridspacingauto="False" visibleinpanesserializable="-1">
                                <tickmarks minorvisible="False" />
                                <tickmarks minorvisible="False" />
                                <tickmarks minorvisible="False" /><tickmarks minorvisible="False" />
                                <tickmarks minorvisible="False" />
                                <label>
                                    <resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" />
                                <resolveoverlappingoptions allowrotate="False"></resolveoverlappingoptions>
                                <numericoptions format="Number" precision="0" />
                                </label>
                                <range sidemarginsenabled="True" />
                                <numericoptions format="Number" precision="0" />
                                <range sidemarginsenabled="True" />
                                <numericoptions format="Number" precision="0" />
                            <range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" /><range sidemarginsenabled="True" /><numericoptions format="Number" precision="0" />
                                <visualrange autosidemargins="True" />
                                <wholerange autosidemargins="True" />
                                <numericscaleoptions autogrid="False" />
                                <datetimescaleoptions autogrid="False">
                                </datetimescaleoptions>
                            </axisx>
                            <axisy visibleinpanesserializable="-1">
                                <range sidemarginsenabled="True" />
                                <range sidemarginsenabled="True" />
                            <range sidemarginsenabled="True" /><range sidemarginsenabled="True" />
                                <label>
                                <numericoptions format="Number" precision="0" />
                                </label>
                                <visualrange autosidemargins="True" />
                                <wholerange autosidemargins="True" />
                            </axisy>
                        </cc1:XYDiagram>
                    </diagramserializable>
                    <fillstyle>
                        <optionsserializable>
                            <cc1:SolidFillOptions />
                        </optionsserializable>
                    </fillstyle>
                    <seriesserializable>
                        <cc1:Series Name="Series 1" LabelsVisibility="True" 
                            SynchronizePointOptions="False">
                            <viewserializable>
                                <cc1:SideBySideBarSeriesView>
                                </cc1:SideBySideBarSeriesView>
                            </viewserializable>
                            <labelserializable>
                                <cc1:SideBySideBarSeriesLabel LineVisible="True" TextOrientation="TopToBottom">
                                    <fillstyle>
                                        <optionsserializable>
                                            <cc1:SolidFillOptions />
                                        </optionsserializable>
                                    </fillstyle>
                                    <pointoptionsserializable>
                                        <cc1:PointOptions>
                                            <valuenumericoptions format="Number" precision="0" />
                                        <valuenumericoptions format="Number" precision="0" />
                                            <valuenumericoptions format="Number" precision="0" />
                                        </cc1:PointOptions>
                                    </pointoptionsserializable>
                                </cc1:SideBySideBarSeriesLabel>
                            </labelserializable>
                            <legendpointoptionsserializable>
                                <cc1:PointOptions>
                                    <valuenumericoptions precision="0" />
                                <valuenumericoptions precision="0" />
                                    <valuenumericoptions precision="0" />
                                </cc1:PointOptions>
                            </legendpointoptionsserializable>
                        </cc1:Series>
                        <cc1:Series Name="Series 2" LabelsVisibility="True">
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
                    <seriestemplate labelsvisibility="True" synchronizepointoptions="False">
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
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="SortRadioButtonList1" />
                    <asp:AsyncPostBackTrigger ControlID="GoButton" />
                </Triggers>
                </asp:UpdatePanel>
                
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>
