﻿<%@ Page Title="VitalSigns Plus - DominoServerHealthPage" Language="C#" MasterPageFile="~/DashboardSite.Master"
    AutoEventWireup="true" CodeBehind="DominoServerHealthPage.aspx.cs" Inherits="VSWebUI.Dashboard.DominoServerHealthPage" %>
<%@ MasterType virtualpath="~/DashboardSite.Master" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>




<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts" TagPrefix="cc2" %>




<%@ Register Src="../Controls/MailStatus.ascx" TagName="MailStatus" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGauges" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGauges.Gauges" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGauges.Gauges.Linear" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGauges.Gauges.Circular" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGauges.Gauges.State" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGauges.Gauges.Digital" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet'
        type='text/css' />
    <link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
    <style type="text/css">
        .headerCell
        {
            color: Black;
            background-color: #045FB4;
        }
        .rowCell
        {
            color: Black;
            background-color: White;
            text-align: right;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function EXGHealthGridView_ContextMenu(s, e) {
            if (e.objectType == "row") {
                s.SetFocusedRowIndex(e.index);
                StatusListPopup.ShowAtPos(ASPxClientUtils.GetEventX(e.htmlEvent), ASPxClientUtils.GetEventY(e.htmlEvent));
            }
        }
        function EXGHealthGridView_FocusedRowChanged(s, e) {
            if (e.objectType != "row") return;
            alert(e.index);
            s.SetFocusedRowIndex(e.index);
        }

        function findPos0(obj, event) {

            var ispan = obj.id;
            ispan = ispan.replace("detailsLabel", "detailsspan");
            var ispanCtl = document.getElementById(ispan);
            ispanCtl.style.left = (event.clientX + 25) + "px"; //obj.offsetParent.offsetLeft + "px";
            ispanCtl.style.top = (event.clientY - 40) + "px";
        }
        function findPos(obj, event, replacestring, replacewith) {
            var ispan = obj.id;
            ispan = ispan.replace(replacestring, replacewith);
            var ispanCtl = document.getElementById(ispan);

            var xOffset = Math.max(document.documentElement.scrollLeft, document.body.scrollLeft);
            var yOffset = Math.max(document.documentElement.scrollTop, document.body.scrollTop);

            ispanCtl.style.left = (event.clientX + xOffset + 25) + "px"; //obj.offsetParent.offsetLeft + "px";
            ispanCtl.style.top = (event.clientY + yOffset + -40) + "px";
        }

        function InitPopupMenuHandler(s, e) {
            var gridCell = document.getElementById('gridCell');
            ASPxClientUtils.AttachEventToElement(gridCell, 'contextmenu', OnGridContextMenu);
        }
        function OnGridContextMenu(evt) {
            DeviceGridView.SetFocusedRowIndex(e.index);
            var SortPopupMenu = StatusListPopup;
            SortPopupMenu.ShowAtPos(evt.clientX + ASPxClientUtils.GetDocumentScrollLeft(), evt.clientY + ASPxClientUtils.GetDocumentScrollTop());
            return OnPreventContextMenu(evt);
        }
        function OnPreventContextMenu(evt) {
            return ASPxClientUtils.PreventEventAndBubble(evt);
        }
        function DeviceGridView_ContextMenu(s, e) {
            if (e.objectType == "row") {
                s.SetFocusedRowIndex(e.index);
                StatusListPopup.ShowAtPos(ASPxClientUtils.GetEventX(e.htmlEvent), ASPxClientUtils.GetEventY(e.htmlEvent));
            }
        }
        function DeviceGridView_FocusedRowChanged(s, e) {
            if (e.objectType != "row") return;
            s.SetFocusedRowIndex(e.index);
        }


        function tblmouseout() {
            idiv = document.getElementById("div1");
            idiv.style.visibility = "hidden";
            lPendingMail = document.getElementById("TDp");
            ldeadmail = document.getElementById("TDd");
            lHeldmail = document.getElementById("TDh");
            lPendingMail.style.visibility = "hidden";
            ldeadmail.style.visibility = "hidden";
            lHeldmail.style.visibility = "hidden";

        }
        function tblmouseover(pav, pth, dav, dth, hav, hth) {

            idiv = document.getElementById("div1");

            idiv.style.visibility = "visible";


            var str = '';
            lPendingMail = document.getElementById("TDp");
            lPendingMail.style.visibility = "visible";

            str += "Pending Messages : " + pav + "/" + pth + "<br>";

            str += "Dead Messages : " + dav + "/" + dth + "<br>";


            str += "Held Messages : " + hav + "/" + hth;


            lPendingMail.innerHTML = str;

        }

        var divName = 'div1';

        function mouseX(evt) { if (!evt) evt = window.event; if (evt.pageX) return evt.pageX; else if (evt.clientX) return evt.clientX + (document.documentElement.scrollLeft ? document.documentElement.scrollLeft : document.body.scrollLeft); else return 0; }
        function mouseY(evt) { if (!evt) evt = window.event; if (evt.pageY) return evt.pageY; else if (evt.clientY) return evt.clientY + (document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop); else return 0; }
        function follow(evt) {

            if (document.getElementById) {
                var obj = document.getElementById(divName).style; // obj.visibility = 'visible';
                obj.left = (parseInt(mouseX(evt)) + offX) + 'px';
                obj.top = (parseInt(mouseY(evt)) + offY) + 'px';

            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%">
        <tr>
            <td valign="top">
                <table>
                    <tr>
                        <td align="center">
                            <img alt="" src="../images/icons/dominoserver.gif" />
                        </td>
                        <td>
                            <div class="header" id="servernamelbldisp" runat="server">
                                IBM Domino Server Health</div>
                        </td>
                        <td>
                            <asp:Label ID="servernamelbl" runat="server" Text="Label" Font-Bold="True" Font-Size="Large"
                                Style="color: #000000; font-family: Verdana; display: none"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <table>
                    <tr>
                        <td>
                            <dxchartsui:WebChartControl ID="SrvRolesWebChart" runat="server" CrosshairEnabled="True"
                                            Height="200px" PaletteName="Module" Width="320px" 
                                            AppearanceNameSerializable="Light">
                                            <borderoptions visible="False" />
<BorderOptions Visibility="True"></BorderOptions>
                                            <diagramserializable>
                                                                            <cc2:SimpleDiagram3D RotationMatrixSerializable="1;0;0;0;0;0.5;-0.866025403784439;0;0;0.866025403784439;0.5;0;0;0;0;1">
                                                                            </cc2:SimpleDiagram3D>
                                                                        </diagramserializable>
                                            <legend backcolor="Transparent" AlignmentHorizontal="Center" 
                                                AlignmentVertical="BottomOutside" MaxVerticalPercentage="30"></legend>
                                            <seriesserializable>
                                                                            <cc2:Series ArgumentScaleType="Qualitative" Name="Roles" 
                                                                                SynchronizePointOptions="False">
                                                                                <viewserializable>
                                                                                    <cc2:Pie3DSeriesView SizeAsPercentage="100">
                                                                                    </cc2:Pie3DSeriesView>
                                                                                </viewserializable>
                                                                                <labelserializable>
                                                                                    <cc2:Pie3DSeriesLabel Position="TwoColumns">
                                                                                        <pointoptionsserializable>
                                                                                            <cc2:PiePointOptions PercentOptions-ValueAsPercent="False">
                                                                                                <valuenumericoptions format="Number" precision="0" />
<ValueNumericOptions Format="Number" Precision="0"></ValueNumericOptions>
                                                                                            </cc2:PiePointOptions>
                                                                                        </pointoptionsserializable>
                                                                                    </cc2:Pie3DSeriesLabel>
                                                                                </labelserializable>
                                                                                <legendpointoptionsserializable>
                                                                                    <cc2:PiePointOptions Pattern="{A} - {V}" PercentOptions-ValueAsPercent="False" 
                                                                                        PointView="ArgumentAndValues">
                                                                                        <valuenumericoptions format="Number" precision="0" />
<ValueNumericOptions Format="Number" Precision="0"></ValueNumericOptions>
                                                                                    </cc2:PiePointOptions>
                                                                                </legendpointoptionsserializable>
                                                                            </cc2:Series>
                                                                        </seriesserializable>
                                            <seriestemplate>
                                                                            <viewserializable>
                                                                                <cc2:Pie3DSeriesView SizeAsPercentage="100">
                                                                                </cc2:Pie3DSeriesView>
                                                                            </viewserializable>
                                                                            <labelserializable>
                                                                                <cc2:Pie3DSeriesLabel>
                                                                                    <pointoptionsserializable>
                                                                                        <cc2:PiePointOptions>
                                                                                            <valuenumericoptions format="General" />
<ValueNumericOptions Format="General"></ValueNumericOptions>
                                                                                        </cc2:PiePointOptions>
                                                                                    </pointoptionsserializable>
                                                                                </cc2:Pie3DSeriesLabel>
                                                                            </labelserializable>
                                                                            <legendpointoptionsserializable>
                                                                                <cc2:PiePointOptions>
                                                                                    <valuenumericoptions format="General" />
<ValueNumericOptions Format="General"></ValueNumericOptions>
                                                                                </cc2:PiePointOptions>
                                                                            </legendpointoptionsserializable>
                                                                        </seriestemplate>
                                            <titles>
                                                <cc2:ChartTitle Font="Tahoma, 12pt" Text="Server Roles" />
                                            </titles>
                                        </dxchartsui:WebChartControl>
                        </td>
                        <td>
                                        <dxchartsui:WebChartControl ID="StatusWebChart" runat="server" AppearanceNameSerializable="Light"
                                            CrosshairEnabled="True" Height="200px" OnCustomDrawSeriesPoint="StatusWebChart_CustomDrawSeriesPoint"
                                            Width="320px" PaletteName="Module">
                                            <borderoptions visible="False" />
                                            <borderoptions visible="False" visibility="True"></borderoptions>
                                            <diagramserializable>
                                <cc2:SimpleDiagram3D RotationMatrixSerializable="1;0;0;0;0;0.5;-0.866025403784439;0;0;0.866025403784439;0.5;0;0;0;0;1">
                                </cc2:SimpleDiagram3D>
                            </diagramserializable>
                                            <legend backcolor="Transparent" AlignmentHorizontal="Center" 
                                                AlignmentVertical="BottomOutside" MaxVerticalPercentage="30" 
                                                Direction="LeftToRight" EquallySpacedItems="False"></legend>
                                            <seriesserializable>
                                <cc2:Series ArgumentScaleType="Qualitative" Name="Statuses" 
                                    SynchronizePointOptions="False">
                                    <viewserializable>
                                        <cc2:Pie3DSeriesView SizeAsPercentage="100">
                                        </cc2:Pie3DSeriesView>
                                    </viewserializable>
                                    <labelserializable>
                                        <cc2:Pie3DSeriesLabel Position="TwoColumns">
                                            <pointoptionsserializable>
                                                <cc2:PiePointOptions PercentOptions-ValueAsPercent="False">
                                                    <valuenumericoptions format="Number" precision="0" />
<ValueNumericOptions Format="Number" Precision="0"></ValueNumericOptions>
                                                </cc2:PiePointOptions>
                                            </pointoptionsserializable>
                                        </cc2:Pie3DSeriesLabel>
                                    </labelserializable>
                                    <legendpointoptionsserializable>
                                        <cc2:PiePointOptions Pattern="{A} - {V}" PercentOptions-ValueAsPercent="False" 
                                            PointView="ArgumentAndValues">
                                            <valuenumericoptions format="Number" precision="0" />
<ValueNumericOptions Format="Number" Precision="0"></ValueNumericOptions>
                                        </cc2:PiePointOptions>
                                    </legendpointoptionsserializable>
                                </cc2:Series>
                            </seriesserializable>
                                            <seriestemplate>
                                <viewserializable>
                                    <cc2:Pie3DSeriesView SizeAsPercentage="100">
                                    </cc2:Pie3DSeriesView>
                                </viewserializable>
                                <labelserializable>
                                    <cc2:Pie3DSeriesLabel>
                                        <pointoptionsserializable>
                                            <cc2:PiePointOptions>
                                                <valuenumericoptions format="General" />
<ValueNumericOptions Format="General"></ValueNumericOptions>
                                            </cc2:PiePointOptions>
                                        </pointoptionsserializable>
                                    </cc2:Pie3DSeriesLabel>
                                </labelserializable>
                                <legendpointoptionsserializable>
                                    <cc2:PiePointOptions>
                                        <valuenumericoptions format="General" />
<ValueNumericOptions Format="General"></ValueNumericOptions>
                                    </cc2:PiePointOptions>
                                </legendpointoptionsserializable>
                            </seriestemplate>
                                            <titles>
                                                <cc2:ChartTitle Font="Tahoma, 12pt" Text="Status" />
                                            </titles>
                                        </dxchartsui:WebChartControl>
                                    </td>
                                    <td>
                                        <dxchartsui:WebChartControl ID="VersionWebChart" runat="server" 
                                            CrosshairEnabled="True" Height="200px" PaletteName="Module" 
                                            Width="320px" AppearanceNameSerializable="Light">
                                            <borderoptions visibility="False" />
<BorderOptions Visibility="True"></BorderOptions>
                                            <diagramserializable>
                                                <cc2:SimpleDiagram3D RotationMatrixSerializable="1;0;0;0;0;0.5;-0.866025403784439;0;0;0.866025403784439;0.5;0;0;0;0;1">
                                                </cc2:SimpleDiagram3D>
                                            </diagramserializable>
                                            <legend alignmenthorizontal="Center" alignmentvertical="BottomOutside" 
                                                maxverticalpercentage="30" Direction="LeftToRight" 
                                                EquallySpacedItems="False"></legend>
                                            <seriesserializable>
                                                <cc2:Series ArgumentScaleType="Qualitative" LegendTextPattern="{A} - {V:N0}" 
                                                    Name="Versions">
                                                    <viewserializable>
                                                        <cc2:Pie3DSeriesView>
                                                        </cc2:Pie3DSeriesView>
                                                    </viewserializable>
                                                    <labelserializable>
                                                        <cc2:Pie3DSeriesLabel Position="TwoColumns">
                                                        </cc2:Pie3DSeriesLabel>
                                                    </labelserializable>
                                                </cc2:Series>
                                            </seriesserializable>
                                            <seriestemplate>
                                                <viewserializable>
                                                    <cc2:Pie3DSeriesView>
                                                    </cc2:Pie3DSeriesView>
                                                </viewserializable>
                                            </seriestemplate>
                                            <titles>
                                                <cc2:ChartTitle Font="Tahoma, 12pt" Text="Version" />
                                            </titles>
                                        </dxchartsui:WebChartControl>
                                    </td>
                                    <td>
                                        <dxchartsui:WebChartControl ID="OSWebChart" runat="server" 
                                            CrosshairEnabled="True" Height="200px" PaletteName="Module" Width="320px" 
                                            AppearanceNameSerializable="Light">
                                            <borderoptions visibility="False" />
<BorderOptions Visibility="True"></BorderOptions>
                                            <diagramserializable>
                                                <cc2:SimpleDiagram3D RotationMatrixSerializable="1;0;0;0;0;0.5;-0.866025403784439;0;0;0.866025403784439;0.5;0;0;0;0;1">
                                                </cc2:SimpleDiagram3D>
                                            </diagramserializable>
                                            <legend alignmenthorizontal="Center" alignmentvertical="BottomOutside" 
                                                maxverticalpercentage="30" Direction="LeftToRight" 
                                                EquallySpacedItems="False"></legend>
                                            <seriesserializable>
                                                <cc2:Series ArgumentScaleType="Qualitative" LegendTextPattern="{A} - {V:N0}" 
                                                    Name="OS">
                                                    <viewserializable>
                                                        <cc2:Pie3DSeriesView>
                                                        </cc2:Pie3DSeriesView>
                                                    </viewserializable>
                                                    <labelserializable>
                                                        <cc2:Pie3DSeriesLabel Position="TwoColumns">
                                                        </cc2:Pie3DSeriesLabel>
                                                    </labelserializable>
                                                </cc2:Series>
                                            </seriesserializable>
                                            <seriestemplate>
                                                <viewserializable>
                                                    <cc2:Pie3DSeriesView>
                                                    </cc2:Pie3DSeriesView>
                                                </viewserializable>
                                            </seriestemplate>
                                            <titles>
                                                <cc2:ChartTitle Font="Tahoma, 12pt" Text="OS" />
                                            </titles>
                                        </dxchartsui:WebChartControl>
                                    </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td>
                <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                    CssPostfix="Glass" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
                    TabSpacing="0px" EnableHierarchyRecreation="False" Width="100%">
                    <TabPages>
                        <dx:TabPage Text="Issues">
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl1" runat="server">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <dx:ASPxGridView ID="DominoIssuesGrid" runat="server" AutoGenerateColumns="False"
                                                            CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                                                            Cursor="pointer" KeyFieldName="ServerName" 
                                                            OnHtmlDataCellPrepared="DominoIssues_HtmlDataCellPrepared" OnPageSizeChanged="DominoIssuesGrid_PageSizeChanged"
                                                            Theme="Office2003Blue" Width="100%" >
                                                            <Columns>
                                                                <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" ShowInCustomizationForm="True"
                                                                    Visible="False" VisibleIndex="0">
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Server Name" FieldName="ServerName" ShowInCustomizationForm="True"
                                                                    VisibleIndex="1" >
                                                                   <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    <EditCellStyle CssClass="GridCss">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader">
                                                                        <Paddings Padding="5px" />
                                                                    </HeaderStyle>
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Test Name" FieldName="TestName" ShowInCustomizationForm="True"
                                                                    VisibleIndex="2">
                                                                    <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    <EditCellStyle CssClass="GridCss">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader">
                                                                        <Paddings Padding="5px" />
                                                                    </HeaderStyle>
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Result" FieldName="Result" ShowInCustomizationForm="True"
                                                                    VisibleIndex="3" Visible="false">
                                                                   <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    <EditCellStyle CssClass="GridCss" HorizontalAlign="Center">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader">
                                                                        <Paddings Padding="5px" />
                                                                    </HeaderStyle>
                                                                    <CellStyle CssClass="GridCss1" HorizontalAlign="Center">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Details" FieldName="Details" ShowInCustomizationForm="True"
                                                                    VisibleIndex="4">
                                                                    <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    <EditCellStyle CssClass="GridCss">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader">
                                                                        <Paddings Padding="5px" />
                                                                    </HeaderStyle>
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Last Update" FieldName="LastUpdate" ShowInCustomizationForm="True"
                                                                    VisibleIndex="5">
                                                               <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    <EditCellStyle CssClass="GridCss">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader">
                                                                        <Paddings Padding="5px" />
                                                                    </HeaderStyle>
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                            </Columns>
                                                            <SettingsBehavior AllowDragDrop="False" AllowFocusedRow="True" AllowSelectByRowClick="True"
                                                                AutoExpandAllGroups="True" ProcessSelectionChangedOnServer="True" />
                                                            <SettingsPager SEOFriendly="Enabled">
                                                                <PageSizeItemSettings Visible="True">
                                                                </PageSizeItemSettings>
                                                            </SettingsPager>
                                                            <Settings ShowFilterRow="True" />
                                                            <Styles CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver">
                                                                <Header ImageSpacing="5px" SortingImageSpacing="5px" CssClass="GridCssHeader">
                                                                </Header>
                                                                <GroupRow Font-Bold="True" Font-Italic="False">
                                                                </GroupRow>
                                                                <AlternatingRow CssClass="GridAltRow" Enabled="True">
                                                                </AlternatingRow>
                                                                <Cell CssClass="GridCss">
                                                                </Cell>
                                                                <GroupFooter Font-Bold="True">
                                                                </GroupFooter>
                                                                <GroupPanel Font-Bold="False">
                                                                </GroupPanel>
                                                                <LoadingPanel ImageSpacing="5px">
                                                                </LoadingPanel>
                                                            </Styles>
                                                            <StylesEditors ButtonEditCellSpacing="0">
                                                                <ProgressBar Height="21px">
                                                                </ProgressBar>
                                                            </StylesEditors>
                                                        </dx:ASPxGridView>
                                                        
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Monitored Tasks">
                            <ContentCollection>
                                <dx:ContentControl runat="server">
                                    <asp:UpdatePanel ID="MoniteredUpdatePanel" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                            <dx:ASPxGridView ID="MaintanceTasksgrid" runat="server" AutoGenerateColumns="False"
                                                                CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                                                                Cursor="pointer" OnHtmlDataCellPrepared="MaintanceTasksgrid_HtmlDataCellPrepared" 
                                                                KeyFieldName="ServerName" Theme="Office2003Blue" Width="100%">
                                                                <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" ColumnResizeMode="NextColumn">
                                                                </SettingsBehavior>
                                                                <SettingsPager PageSize="50">
                                                                    <PageSizeItemSettings Visible="True">
                                                                    </PageSizeItemSettings>
                                                                </SettingsPager>
                                                                <Columns>
                                                                    <dx:GridViewDataTextColumn Caption="Server Name" VisibleIndex="1" FieldName="ServerName">
                                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                     
                                                                        <EditCellStyle CssClass="GridCss">
                                                                        </EditCellStyle>
                                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                                        </EditFormCaptionStyle>
                                                                        <HeaderStyle CssClass="GridCssHeader">
                                                                            <Paddings Padding="5px" />
                                                                            <Paddings Padding="5px"></Paddings>
                                                                            <Paddings Padding="5px" />
                                                                        </HeaderStyle>
                                                                        <CellStyle CssClass="GridCss">
                                                                        </CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Task Name" VisibleIndex="2" FieldName="TaskName">
                                                                        <EditCellStyle CssClass="GridCss">
                                                                        </EditCellStyle>
                                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                                        </EditFormCaptionStyle>
                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                        <CellStyle CssClass="GridCss">
                                                                        </CellStyle>
                                                                        
                                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Status" VisibleIndex="3" FieldName="StatusSummary">
                                                                        <EditCellStyle CssClass="GridCss">
                                                                        </EditCellStyle>
                                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                                        </EditFormCaptionStyle>
                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                        <CellStyle CssClass="GridCss1">
                                                                        </CellStyle>
                                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                      
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Date" VisibleIndex="4" FieldName="LastUpdate">
                                                                        <EditCellStyle CssClass="GridCss">
                                                                        </EditCellStyle>
                                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                                        </EditFormCaptionStyle>
                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                        <CellStyle CssClass="GridCss">
                                                                        </CellStyle>
                                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Primary Status" VisibleIndex="5" FieldName="PrimaryStatus">
                                                                        <EditCellStyle CssClass="GridCss">
                                                                        </EditCellStyle>
                                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                                        </EditFormCaptionStyle>
                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                        <CellStyle CssClass="GridCss">
                                                                        </CellStyle>
                                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Secondary Status" FieldName="SecondaryStatus"
                                                                        ShowInCustomizationForm="True" Visible="True" VisibleIndex="6">
                                                                        <EditCellStyle CssClass="GridCss">
                                                                        </EditCellStyle>
                                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                                        </EditFormCaptionStyle>
                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                        <CellStyle CssClass="GridCss">
                                                                        </CellStyle>
                                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" ShowInCustomizationForm="True"
                                                                        Visible="false" VisibleIndex="7">
                                                                        <EditCellStyle CssClass="GridCss">
                                                                        </EditCellStyle>
                                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                                        </EditFormCaptionStyle>
                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                        <CellStyle CssClass="GridCss">
                                                                        </CellStyle>
                                                                       <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    </dx:GridViewDataTextColumn>
                                                                </Columns>
                                                                <Settings VerticalScrollableHeight="10" />
                                                                <SettingsBehavior AllowSelectByRowClick="True" AutoExpandAllGroups="True" />
                                                                <SettingsBehavior AllowSelectByRowClick="True" AutoExpandAllGroups="True" />
                                                                <SettingsPager Mode="ShowAllRecords" PageSize="12">
                                                                </SettingsPager>
                                                                <Styles CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver">
                                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px" CssClass="GridCssHeader">
                                                                    </Header>
                                                                    <AlternatingRow CssClass="GridAltRow">
                                                                    </AlternatingRow>
                                                                    <Cell CssClass="GridCss">
                                                                    </Cell>
                                                                    <LoadingPanel ImageSpacing="5px">
                                                                    </LoadingPanel>
                                                                </Styles>
                                                                <StylesEditors ButtonEditCellSpacing="0">
                                                                    <ProgressBar Height="21px">
                                                                    </ProgressBar>
                                                                </StylesEditors>
                                                            </dx:ASPxGridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="System Information">
                            <ContentCollection>
                                <dx:ContentControl runat="server">
                                    <dx:ASPxGridView ID="SysInfoGrid" runat="server" AutoGenerateColumns="False" 
                                        EnableTheming="True" OnPageSizeChanged="SysInfoGrid_PageSizeChanged" 
                                        Theme="Office2003Blue" Width="100%">
                                        <Columns>
                                            <dx:GridViewDataTextColumn Caption="Server Name" FieldName="ServerName" 
                                                ShowInCustomizationForm="True" VisibleIndex="0">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="OS" FieldName="OperatingSystem" 
                                                ShowInCustomizationForm="True" VisibleIndex="1">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Domino Version" FieldName="DominoVersion" 
                                                ShowInCustomizationForm="True" VisibleIndex="2">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Architecture" 
                                                FieldName="VersionArchitecture" ShowInCustomizationForm="True" VisibleIndex="3">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="CPU Count" FieldName="CPUCount" 
                                                ShowInCustomizationForm="True" VisibleIndex="4">
                                                <HeaderStyle CssClass="GridCssHeader2" />
                                                <CellStyle CssClass="GridCss2">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" 
                                                ShowInCustomizationForm="True" Visible="False" VisibleIndex="6">
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                        <SettingsPager AlwaysShowPager="True">
                                            <PageSizeItemSettings Visible="True">
                                            </PageSizeItemSettings>
                                        </SettingsPager>
                                        <Styles>
                                            <Header CssClass="GridCssHeader">
                                            </Header>
                                            <AlternatingRow CssClass="GridAltRow">
                                            </AlternatingRow>
                                            <Cell CssClass="GridCss">
                                            </Cell>
                                        </Styles>
                                    </dx:ASPxGridView>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                    </TabPages>
                </dx:ASPxPageControl>
                <dxchartsui:WebChartControl ID="MailWebChart" runat="server" 
                                                            AppearanceNameSerializable="Pastel Kit" BackColor="White" 
                                                            CrosshairEnabled="True" Height="200px" PaletteName="Paper" Visible="False" 
                                                            Width="350px">
                                                            <borderoptions visibility="False" />
                                                            <diagramserializable>
                                                                <cc2:SimpleDiagram3D RotationMatrixSerializable="1;0;0;0;0;0.5;-0.866025403784439;0;0;0.866025403784439;0.5;0;0;0;0;1">
                                                                </cc2:SimpleDiagram3D>
                                                            </diagramserializable>
                                                            <legend backcolor="Transparent"></legend>
                                                            <seriesserializable>
                                                                <cc2:Series ArgumentScaleType="Qualitative" LegendTextPattern="{A} - {V:N0}" 
                                                                    Name="Statuses">
                                                                    <viewserializable>
                                                                        <cc2:Pie3DSeriesView>
                                                                        </cc2:Pie3DSeriesView>
                                                                    </viewserializable>
                                                                    <labelserializable>
                                                                        <cc2:Pie3DSeriesLabel Position="Inside">
                                                                        </cc2:Pie3DSeriesLabel>
                                                                    </labelserializable>
                                                                </cc2:Series>
                                                            </seriesserializable>
                                                            <seriestemplate legendtextpattern="{VP:G4}">
                                                                <viewserializable>
                                                                    <cc2:Pie3DSeriesView>
                                                                    </cc2:Pie3DSeriesView>
                                                                </viewserializable>
                                                            </seriestemplate>
                                                            <titles>
                                                                <cc2:ChartTitle Font="Tahoma, 12pt" Text="By Mail" TextColor="64, 64, 64" />
                                                            </titles>
                                                        </dxchartsui:WebChartControl>
                <dx:ASPxPageControl ID="DominoServerHealthGrid" runat="server" 
                    ActiveTabIndex="0"  Width="400px"
                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                    TabSpacing="0px" EnableHierarchyRecreation="False" Visible="False">
                    <TabPages>
                        <dx:TabPage Text="Domino">
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl2" runat="server">
                                    <table>
                                        <tr>
                                            <td valign="top">
                                                <asp:UpdatePanel ID="DominoServerListUpdatePanel" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <dx:ASPxGridView ID="DominoHealthGridView" runat="server" AutoGenerateColumns="False"
                                                            CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                                                            Cursor="pointer" KeyFieldName="ServerName" SummaryText="m" Theme="Office2003Blue"
                                                            OnHtmlDataCellPrepared="DominoHealthGridView_HtmlDataCellPrepared" OnPageSizeChanged="DominoHealthGridView_PageSizeChanged">
                                                            <Columns>
                                                                <dx:GridViewDataTextColumn Caption="Server Name" FieldName="ServerName" Width="100px"
                                                                    VisibleIndex="1" >
                                                                   <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    <EditCellStyle CssClass="GridCss">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader">
                                                                        <Paddings Padding="5px" />
                                                                    </HeaderStyle>
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Status" FieldName="Status" VisibleIndex="2" Width="50px">
                                                                    <HeaderStyle CssClass="GridCssHeader1" />
                                                                    <CellStyle CssClass="GridCss1">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="CPU" FieldName="CPU" Width="50px"
                                                                    VisibleIndex="3">
                                                                    <EditCellStyle CssClass="GridCss2">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader2" />
                                                                    <CellStyle CssClass="GridCss2" HorizontalAlign="Right">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Memory" FieldName="Memory" Width="50px"
                                                                    VisibleIndex="4">
                                                                    <EditCellStyle CssClass="GridCss2">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader2" />
                                                                    <CellStyle CssClass="GridCss2" HorizontalAlign="Right">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Response Time" FieldName="ResponseTime"
                                                                    VisibleIndex="5" Width="50px">
                                                                    <EditCellStyle CssClass="GridCss2">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader2" />
                                                                    <CellStyle CssClass="GridCss2" HorizontalAlign="Right">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                            </Columns>
                                                            <SettingsBehavior AllowDragDrop="False" AllowFocusedRow="True" AllowSelectByRowClick="True"
                                                                AutoExpandAllGroups="True" ProcessSelectionChangedOnServer="True" />
                                                            <SettingsPager SEOFriendly="Enabled">
                                                                <PageSizeItemSettings Visible="True">
                                                                </PageSizeItemSettings>
                                                            </SettingsPager>
                                                            <Settings ShowFilterRow="True" />
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
                                                            <Styles CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver">
                                                                <Header ImageSpacing="5px" SortingImageSpacing="5px" CssClass="GridCssHeader">
                                                                </Header>
                                                                <GroupRow Font-Bold="True" Font-Italic="False">
                                                                </GroupRow>
                                                                <AlternatingRow CssClass="GridAltRow" Enabled="True">
                                                                </AlternatingRow>
                                                                <Cell CssClass="GridCss">
                                                                </Cell>
                                                                <GroupFooter Font-Bold="True">
                                                                </GroupFooter>
                                                                <GroupPanel Font-Bold="False">
                                                                </GroupPanel>
                                                                <LoadingPanel ImageSpacing="5px">
                                                                </LoadingPanel>
                                                            </Styles>
                                                            <StylesEditors ButtonEditCellSpacing="0">
                                                                <ProgressBar Height="21px">
                                                                </ProgressBar>
                                                            </StylesEditors>
                                                        </dx:ASPxGridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Traveler">
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl3" runat="server">
                                    <table >
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="TravelerpdatePanel" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <dx:ASPxGridView ID="TravelerGridView" runat="server" AutoGenerateColumns="False"
                                                            CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                                                            Cursor="pointer" KeyFieldName="ServerName" SummaryText="m" Theme="Office2003Blue"
                                                             OnHtmlDataCellPrepared="TravelerGridView_HtmlDataCellPrepared"  OnPageSizeChanged="TravelerGridView_PageSizeChanged">
                                                            <Columns>
                                                                <dx:GridViewDataTextColumn Caption="Server Name" FieldName="ServerName" ShowInCustomizationForm="True"
                                                                    VisibleIndex="1" >
                                                                    <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    <EditCellStyle CssClass="GridCss">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader">
                                                                        <Paddings Padding="5px" />
                                                                    </HeaderStyle>
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="CPU" FieldName="CPU" ShowInCustomizationForm="True"  Width="50px"
                                                                    VisibleIndex="3">
                                                                    <EditCellStyle CssClass="GridCss2">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader2" />
                                                                    <CellStyle CssClass="GridCss2" HorizontalAlign="Right">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Memory" FieldName="Memory" ShowInCustomizationForm="True"  Width="50px"
                                                                    VisibleIndex="4">
                                                                    <EditCellStyle CssClass="GridCss2">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader2" />
                                                                    <CellStyle CssClass="GridCss2" HorizontalAlign="Right">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Response Time" FieldName="ResponseTime" ShowInCustomizationForm="True"
                                                                    VisibleIndex="5" Width="50px">
                                                                    <EditCellStyle CssClass="GridCss2">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader2" />
                                                                    <CellStyle CssClass="GridCss2" HorizontalAlign="Right">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Status" FieldName="Status" VisibleIndex="2"  Width="50px">
                                                                    <HeaderStyle CssClass="GridCssHeader1" />
                                                                    <CellStyle CssClass="GridCss1">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                            </Columns>
                                                            <SettingsBehavior AllowDragDrop="False" AllowFocusedRow="True" AllowSelectByRowClick="True"
                                                                AutoExpandAllGroups="True" ProcessSelectionChangedOnServer="True" />
                                                            <SettingsPager SEOFriendly="Enabled">
                                                                <PageSizeItemSettings Visible="True">
                                                                </PageSizeItemSettings>
                                                            </SettingsPager>
                                                            <Settings ShowFilterRow="True" />
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
                                                            <Styles CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver">
                                                                <Header ImageSpacing="5px" SortingImageSpacing="5px" CssClass="GridCssHeader">
                                                                </Header>
                                                                <GroupRow Font-Bold="True" Font-Italic="False">
                                                                </GroupRow>
                                                                <AlternatingRow CssClass="GridAltRow" Enabled="True">
                                                                </AlternatingRow>
                                                                <Cell CssClass="GridCss">
                                                                </Cell>
                                                                <GroupFooter Font-Bold="True">
                                                                </GroupFooter>
                                                                <GroupPanel Font-Bold="False">
                                                                </GroupPanel>
                                                                <LoadingPanel ImageSpacing="5px">
                                                                </LoadingPanel>
                                                            </Styles>
                                                            <StylesEditors ButtonEditCellSpacing="0">
                                                                <ProgressBar Height="21px">
                                                                </ProgressBar>
                                                            </StylesEditors>
                                                        </dx:ASPxGridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Sametime">
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl4" runat="server">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="SametimeUpdatePanel" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <dx:ASPxGridView ID="SameTimeGridView" runat="server" AutoGenerateColumns="False"
                                                            CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                                                            Cursor="pointer" KeyFieldName="ServerName" SummaryText="m" Theme="Office2003Blue"
                                                             OnHtmlDataCellPrepared="SameTimeGridView_HtmlDataCellPrepared" OnPageSizeChanged="SameTimeGridView_PageSizeChanged">
                                                            <Columns>
                                                                <dx:GridViewDataTextColumn Caption="Server Name" FieldName="ServerName" ShowInCustomizationForm="True"
                                                                    VisibleIndex="1" >
                                                                    <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    <EditCellStyle CssClass="GridCss">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader">
                                                                        <Paddings Padding="5px" />
                                                                    </HeaderStyle>
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="CPU" FieldName="CPU" ShowInCustomizationForm="True"  Width="50px"
                                                                    VisibleIndex="3">
                                                                    <EditCellStyle CssClass="GridCss2">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader2" />
                                                                    <CellStyle CssClass="GridCss2" HorizontalAlign="Right">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Memory" FieldName="Memory" ShowInCustomizationForm="True"  Width="50px"
                                                                    VisibleIndex="4">
                                                                    <EditCellStyle CssClass="GridCss2">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader2" />
                                                                    <CellStyle CssClass="GridCss2" HorizontalAlign="Right">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Response Time" FieldName="ResponseTime" ShowInCustomizationForm="True"
                                                                    VisibleIndex="5" Width="50px">
                                                                    <EditCellStyle CssClass="GridCss2">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader2" />
                                                                    <CellStyle CssClass="GridCss2" HorizontalAlign="Right">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Status" FieldName="Status" VisibleIndex="2" Width="50px">
                                                                    <HeaderStyle CssClass="GridCssHeader1" />
                                                                    <CellStyle CssClass="GridCss1">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                            </Columns>
                                                            <SettingsBehavior AllowDragDrop="False" AllowFocusedRow="True" AllowSelectByRowClick="True"
                                                                AutoExpandAllGroups="True" ProcessSelectionChangedOnServer="True" />
                                                            <SettingsPager SEOFriendly="Enabled">
                                                                <PageSizeItemSettings Visible="True">
                                                                </PageSizeItemSettings>
                                                            </SettingsPager>
                                                            <Settings ShowFilterRow="True" />
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
                                                            <Styles CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver">
                                                                <Header ImageSpacing="5px" SortingImageSpacing="5px" CssClass="GridCssHeader">
                                                                </Header>
                                                                <GroupRow Font-Bold="True" Font-Italic="False">
                                                                </GroupRow>
                                                                <AlternatingRow CssClass="GridAltRow" Enabled="True">
                                                                </AlternatingRow>
                                                                <Cell CssClass="GridCss">
                                                                </Cell>
                                                                <GroupFooter Font-Bold="True">
                                                                </GroupFooter>
                                                                <GroupPanel Font-Bold="False">
                                                                </GroupPanel>
                                                                <LoadingPanel ImageSpacing="5px">
                                                                </LoadingPanel>
                                                            </Styles>
                                                            <StylesEditors ButtonEditCellSpacing="0">
                                                                <ProgressBar Height="21px">
                                                                </ProgressBar>
                                                            </StylesEditors>
                                                        </dx:ASPxGridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Monitored Tasks">
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl5" runat="server">
                                    
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                    </TabPages>
                </dx:ASPxPageControl>
            </td>
        </tr>
    </table>
</asp:Content>
