﻿<%@ Page Title="VitalSigns Plus - Cost Per User Served" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="CostperUserserved.aspx.cs" Inherits="VSWebUI.CostperUserserved" %>

<%@ MasterType virtualpath="~/DashboardSite.Master" %>
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
   

<script type="text/javascript">
    function Resized() {
        var callbackState = document.getElementById('ContentPlaceHolder1_callbackState').value;

        if (callbackState == 0)
            DoCallback();
    }

    function DoCallback() {
        document.getElementById('ContentPlaceHolder1_chartWidth').value = document.body.offsetWidth - 105;
        //cSelectServerRoundPanel.style.width = document.getElementById('ContentPlaceHolder1__chartWidth').value + "px";
        //        FileOpensCumulativeWebChart.PerformCallback();
        //        FileOpensWebChart.PerformCallback();
        //        chttpSessionsWebChart.PerformCallback();
        //        DeviceTypeChart.PerformCallback();
        //        OSTypeChart.PerformCallback();

        CostperUserservedWebChartControl.PerformCallback();


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
    //10/30/2013 NS added - fix for when an Enter key is pressed within the text box on the page - redirect the
    //submit function to the actual Go button on the page instead of performing a whole page submit
    function OnKeyDown(s, e) {
        //alert(window.event.keyCode);
        //var keyCode = (window.event) ? e.which : e.keyCode;
        //alert(keyCode);
        var keyCode = window.event.keyCode;
        if (keyCode == 13)
            goButton.DoClick();
    }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input id="chartWidth" type="hidden" runat="server" value="1200" />
<input id="callbackState" type="hidden" runat="server" value="0" />
<table width="100%">
    <tr>
        <td>
            <div class="header" id="servernamelbldisp" runat="server">Cost Per User Served</div>
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
    <tr>
        <td colspan="3">
            <div class="info">In order to filter the chart below by server name, enter a part of the server name 
            and the chart will be trimmed accordingly. You do not need to enter a complete server name, any number of identifying
            characters will be sufficient.</div>
        </td>
    </tr>
</table>
<table>
    <tr>
    <td valign="top">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <table>
    <tr>
    <td>
            <dx:ASPxRadioButtonList ID="SortRadioButtonList1" runat="server" 
                RepeatDirection="Horizontal" SelectedIndex="0" AutoPostBack="True" 
                OnSelectedIndexChanged="SortRadioButtonList1_SelectedIndexChanged" 
                Width="250px">
                <Items>
                    <dx:ListEditItem Selected="True" Text="By Server" Value="1" />
                    <dx:ListEditItem Text="By Cost" Value="2" />
                </Items>
            </dx:ASPxRadioButtonList>
        </td>
                <td>
                    <dx:ASPxLabel ID="SearchLabel" runat="server" CssClass="lblsmallFont" 
                        Text="Specify a part of the server name to filter by:">
                    </dx:ASPxLabel>
        </td>
        <td>
            <dx:ASPxTextBox ID="SearchTextBox" runat="server" 
                ClientInstanceName="searchTxtBox" Width="170px">
                <ClientSideEvents KeyDown="function(s, e) {OnKeyDown(s, e);}" />
            </dx:ASPxTextBox>
        </td>
        <td>
            <dx:ASPxLabel ID="TypeLabel" runat="server" CssClass="lblsmallFont" 
                Text="Type:">
            </dx:ASPxLabel>
        </td>
        <td>
            <dx:ASPxComboBox ID="TypeComboBox" runat="server" onselectedindexchanged="TypeComboBox_SelectedIndexChanged"  AutoPostBack="true">
                <Items>
                    <dx:ListEditItem Text="Select Type" Value="Select Type" />
                </Items>
            </dx:ASPxComboBox>
        </td>
       <td>
            <dx:ASPxLabel ID="UserTypelabel" runat="server" CssClass="lblsmallFont" 
                Text="User Type:" >
            </dx:ASPxLabel>
        </td>
                <td>
                    <dx:ASPxComboBox ID="UserTypeComboBox" runat="server" ValueType="System.String" onselectedindexchanged="UserTypeComboBox_SelectedIndexChanged">
                    </dx:ASPxComboBox>
        </td>
        <td>
            &nbsp;
        </td>
       
        <td>
            <dx:ASPxButton ID="GoButton" runat="server" ClientInstanceName="goButton" 
                OnClick="GoButton_Click" Text="Submit" CssClass="sysButton">
            </dx:ASPxButton>
        </td>
                </tr>
            </table>
    </ContentTemplate>
        </asp:UpdatePanel>
        </td>
        </tr>
    <tr>
    <td valign="top">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <dxchartsui:WebChartControl ID="CostperUserservedWebChartControl" 
                    runat="server" CrosshairEnabled="True" Height="500px" PaletteName="Oriel" 
                    Width="1000px">
                    <diagramserializable>
                        <cc1:XYDiagram>
                            <axisx visibleinpanesserializable="-1">
                                <label>
                                <resolveoverlappingoptions allowrotate="False" />
                                </label>
                            </axisx>
                            <axisy visibleinpanesserializable="-1">
                                <tickmarks minorvisible="False" />
                                <label>
                                <resolveoverlappingoptions allowrotate="False" />
                                </label>
                            </axisy>
                        </cc1:XYDiagram>
                    </diagramserializable>
                    <seriesserializable>
                        <cc1:Series ArgumentScaleType="Qualitative" Name="Series 1">
                        </cc1:Series>
                        <cc1:Series ArgumentScaleType="Qualitative" Name="Series 2">
                        </cc1:Series>
                    </seriesserializable>
                    <seriestemplate argumentscaletype="Qualitative">
                    </seriestemplate>
                </dxchartsui:WebChartControl>
                <dxchartsui:WebChartControl ID="CostperUserservedWebChartControl1" 
                    runat="server" ClientInstanceName="CostperUserservedWebChartControl1" 
                    Height="500px" Width="1000px" 
                    OnCustomCallback="CostperUserservedWebChartControl_CustomCallback" 
                    CrosshairEnabled="True"   
                    onbounddatachanged="CostperUserservedWebChartControl_BoundDataChanged" Visible="False"  
                    >
                    <diagramserializable>
                        <cc1:XYDiagram>
                            <axisx visibleinpanesserializable="-1">
                                <range sidemarginsenabled="True" />
                                <tickmarks minorvisible="False" />
                                <range sidemarginsenabled="True" />
                                <tickmarks minorvisible="False" />
                                <range sidemarginsenabled="True" />
                                <tickmarks minorvisible="False" />
                                <tickmarks minorvisible="False" />
                                <label>
                                <resolveoverlappingoptions allowhide="False" allowrotate="False" />
                                <resolveoverlappingoptions allowhide="False" allowrotate="False"></resolveoverlappingoptions>
                                </label>
                                <range sidemarginsenabled="True" />
                                <visualrange autosidemargins="True" />
                                <wholerange autosidemargins="True" />
                            </axisx>
                            <axisy visibleinpanesserializable="-1">
                                <range sidemarginsenabled="True" />
                                <tickmarks minorvisible="False" />
                                <range sidemarginsenabled="True" />
                                <numericoptions format="Number" precision="0" />
                                <tickmarks minorvisible="False" />
                                <tickmarks minorvisible="False" />
                                <tickmarks minorvisible="False" />
                                <label>
                                <resolveoverlappingoptions allowrotate="False" />
                                <resolveoverlappingoptions allowrotate="False" allowstagger="False"></resolveoverlappingoptions>
                                </label>
                                <range sidemarginsenabled="True" />
                                <numericoptions format="Number" precision="0" />
                                <range sidemarginsenabled="True" />
                                <numericoptions format="Number" precision="0" />
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
                        <cc1:Series Name="Series 1" ArgumentScaleType="Qualitative" >
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
                        <cc1:Series Name="Series 2" ArgumentScaleType="Qualitative">
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
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="GoButton" />
            </Triggers>
        </asp:UpdatePanel>
            
        </td>
        </tr>
    </table>
</asp:Content>
