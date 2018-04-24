<%@ Page Title="Microsoft SharePoint Server Health" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true"
    CodeBehind="SharepointServerHealth.aspx.cs" Inherits="VSWebUI.Dashboard.SharepointServerHealth" %>
	
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
        function SharePointHealthGridView_ContextMenu(s, e) {
            if (e.objectType == "row") {
                s.SetFocusedRowIndex(e.index);
                StatusListPopup.ShowAtPos(ASPxClientUtils.GetEventX(e.htmlEvent), ASPxClientUtils.GetEventY(e.htmlEvent));
            }
        }
        function SharePointHealthGridView_FocusedRowChanged(s, e) {
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
            //alert(obj.offsetParent.offsetLeft);
            var ispan = obj.id;
            ispan = ispan.replace(replacestring, replacewith);
            var ispanCtl = document.getElementById(ispan);

            var xOffset = Math.max(document.documentElement.scrollLeft, document.body.scrollLeft);
            var yOffset = Math.max(document.documentElement.scrollTop, document.body.scrollTop);

            ispanCtl.style.left = (event.clientX + xOffset + 25) + "px"; //obj.offsetParent.offsetLeft + "px";
            ispanCtl.style.top = (event.clientY + yOffset + -40) + "px";
        }

        function InitPopupMenuHandler(s, e) {
            //var menu1 = document.getElementById('ContentPlaceHolder1_ASPxPageControl1_UserDetailsMenu');
            //alert(menu1.style.visibility);
            //if (menu1.style.visibility == "visible") {
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

            //        lPendingMail=document.getElementById("lblPendingMail");
            //        if (pav == 0) {
            //        lPendingMail.style.visibility = "hidden";
            //    } 
            //        else
            //        {
            //         lPendingMail.style.visibility = "visible";
            //         }
            var str = '';
            lPendingMail = document.getElementById("TDp");
            lPendingMail.style.visibility = "visible";
            //        ldeadmail = document.getElementById("TDd");
            //        lHeldmail = document.getElementById("TDh");
            //        if (pav == 0) {
            //            //lPendingMail.style.visibility = "hidden";
            //        }
            //        else if (pav > 0) {
            // lPendingMail.style.visibility = "visible";
            //lPendingMail.innerHTML = "Pending Mails : " + pav + "";
            str += "Pending Messages : " + pav + "/" + pth + "<br>";
            //        }
            //        if (dav == 0) {
            //            //ldeadmail.style.visibility = "hidden";
            //        }
            //        else if (dav > 0) {
            //            ldeadmail.style.visibility = "visible";
            //           ldeadmail.innerHTML="Dead Mails :" + dav + "";
            str += "Dead Messages : " + dav + "/" + dth + "<br>";

            //        }
            //        if (hav == 0) {
            //            //lHeldmail.style.visibility = "hidden";
            //        }
            //        else if (hav > 0) {
            //            lHeldmail.style.visibility = "visible";
            //            lHeldmail.innerHTML = "Held Mails :" + hav + "";
            str += "Held Messages : " + hav + "/" + hth;

            //}
            lPendingMail.innerHTML = str;

            //alert(lPendingMail.innerHTML);
        }
        // Simple follow the mouse script
        //document.onmousemove = follow;
        var divName = 'div1'; // div that is to follow the mouse
        // (must be position:absolute)
        var offX = -120;          // X offset from mouse position
        var offY = 23;          // Y offset from mouse position

        function mouseX(evt) { if (!evt) evt = window.event; if (evt.pageX) return evt.pageX; else if (evt.clientX) return evt.clientX + (document.documentElement.scrollLeft ? document.documentElement.scrollLeft : document.body.scrollLeft); else return 0; }
        function mouseY(evt) { if (!evt) evt = window.event; if (evt.pageY) return evt.pageY; else if (evt.clientY) return evt.clientY + (document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop); else return 0; }

        function follow(evt) {

            if (document.getElementById) {
                var obj = document.getElementById(divName).style; // obj.visibility = 'visible';
                obj.left = (parseInt(mouseX(evt)) + offX) + 'px';
                obj.top = (parseInt(mouseY(evt)) + offY) + 'px';
                //alert(obj.left + ' ' + obj.top);

            }
        }
    </script>
   
    <style type="text/css">
        .dxtc-leftIndent
        {
            display: none !important;
        }
        
        .dxtc-rightIndent
        {
            display: none !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="99%">
        <tr>
            <td valign="top">
                <table>
                    <tr>
                        <td align="center">
                            <img alt="" src="../images/icons/SharePoint_2013.jpg" style="width: 31px; height: 31px;" />
                        </td>
                        <td>
                            <div class="header" id="servernamelbldisp" runat="server">
                                Microsoft SharePoint Server Health</div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <dx:ASPxPanel ID="ASPxPanel1" runat="server" Theme="MetropolisBlue" Width="100%"
                    BackColor="White">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <table width="99%">
                                <tr>
                                    <td align="center" width="33%">
                                        <%-- <dxchartsui:WebChartControl ID="OSWebChart" runat="server" Height="200px" Width="350px"
                                            CrosshairEnabled="True" PaletteName="Paper" BackColor="#FFFFFF">
                                            <borderoptions visible="False" />
                                            <borderoptions visible="False"></borderoptions>
                                            <diagramserializable>
                                <cc2:SimpleDiagram3D RotationMatrixSerializable="1;0;0;0;0;0.5;-0.866025403784439;0;0;0.866025403784439;0.5;0;0;0;0;1">
                                </cc2:SimpleDiagram3D>
                            </diagramserializable>
                                            <legend alignmenthorizontal="Center" alignmentvertical="BottomOutside" backcolor="Transparent">
                                            </legend>
                                            <seriesserializable>
                                <cc2:Series ArgumentScaleType="Qualitative" LabelsVisibility="True" Name="OS" 
                                    SynchronizePointOptions="False">
                                    <viewserializable>
                                        <cc2:Pie3DSeriesView SizeAsPercentage="100">
                                        </cc2:Pie3DSeriesView>
                                    </viewserializable>
                                    <labelserializable>
                                        <cc2:Pie3DSeriesLabel Position="Inside">
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
                                <cc2:ChartTitle Font="Tahoma, 12pt" Text="Operating Systems" 
                                    TextColor="64, 64, 64" />
                            </titles>
                                        </dxchartsui:WebChartControl> --%>
                                        <div style="width: 180px; float: none; height: 100px;
                                            background-position: left bottom; background-color: #42B4E6">
                                            <table style="float: none; width: 170px;">
                                                <tr>
                                                    <td>
                                                        <dx:ASPxLabel ID="ASPxLabel4" runat="server" Font-Bold="false" ForeColor="White"
                                                            Text="Total Sites" Font-Size="20px">
                                                        </dx:ASPxLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <dx:ASPxLabel ID="UsersLabel" runat="server" Font-Bold="True" ForeColor="White" Font-Size="22px">
                                                        </dx:ASPxLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <dx:ASPxLabel ID="ASPxLabel143" runat="server" Font-Bold="True" ForeColor="White"
                                                            Font-Size="11px">
                                                        </dx:ASPxLabel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                    <td align="center" width="33%">
                                        <dxchartsui:WebChartControl ID="SpaceWebChart" runat="server" AppearanceNameSerializable="Pastel Kit"
                                            CrosshairEnabled="True" Height="200px" OnCustomDrawSeriesPoint="StatusWebChart_CustomDrawSeriesPoint"
                                            Width="350px" BackColor="#FFFFFF" PaletteName="Module">
                                            <borderoptions visible="False" />
                                            <borderoptions visible="False"></borderoptions>
                                            <diagramserializable>
                                <cc2:SimpleDiagram3D RotationMatrixSerializable="1;0;0;0;0;0.5;-0.866025403784439;0;0;0.866025403784439;0.5;0;0;0;0;1">
                                </cc2:SimpleDiagram3D>
                            </diagramserializable>
                                            <legend backcolor="Transparent"></legend>
                                            <seriesserializable>
                                <cc2:Series ArgumentScaleType="Qualitative" Name="Spaces" SynchronizePointOptions="False">
                                    <viewserializable>
                                        <cc2:Pie3DSeriesView SizeAsPercentage="100">
                                        </cc2:Pie3DSeriesView>
                                    </viewserializable>
                                    <labelserializable>
                                        <cc2:Pie3DSeriesLabel Position="Inside">
                                            <pointoptionsserializable>
                                                <cc2:PiePointOptions PercentOptions-ValueAsPercent="False">
<ValueNumericOptions Format="Number" Precision="0"></ValueNumericOptions>
                                                </cc2:PiePointOptions>
                                            </pointoptionsserializable>
                                        </cc2:Pie3DSeriesLabel>
                                    </labelserializable>
                                    <legendpointoptionsserializable>
                                        <cc2:PiePointOptions Pattern="{A} - {V}" PercentOptions-ValueAsPercent="False" 
                                            PointView="ArgumentAndValues">
<ValueNumericOptions Format="Number" Precision="0"></ValueNumericOptions>
                                        </cc2:PiePointOptions>
                                    </legendpointoptionsserializable>
                                </cc2:Series>
                            </seriesserializable>
                                            <seriestemplate synchronizepointoptions="False">
                                <viewserializable>
                                    <cc2:Pie3DSeriesView SizeAsPercentage="100">
                                    </cc2:Pie3DSeriesView>
                                </viewserializable>
                                <labelserializable>
                                    <cc2:Pie3DSeriesLabel>
                                        <pointoptionsserializable>
                                            <cc2:PiePointOptions PercentOptions-ValueAsPercent="False" 
                                                PointView="ArgumentAndValues">
<ValueNumericOptions Format="General"></ValueNumericOptions>
                                            </cc2:PiePointOptions>
                                        </pointoptionsserializable>
                                    </cc2:Pie3DSeriesLabel>
                                </labelserializable>
                                <legendpointoptionsserializable>
                                    <cc2:PiePointOptions PercentOptions-ValueAsPercent="False">
<ValueNumericOptions Format="General"></ValueNumericOptions>
                                    </cc2:PiePointOptions>
                                </legendpointoptionsserializable>
                            </seriestemplate>
                                            <titles>
                                <cc2:ChartTitle Font="Tahoma, 12pt" Text="Site Distribution" TextColor="64, 64, 64" />
                            </titles>
                                        </dxchartsui:WebChartControl>
                                    </td>
                                    <td align="center" width="33%">
                                        <dxchartsui:WebChartControl ID="SrvRolesWebChart" runat="server" CrosshairEnabled="True"
                                            Height="200px" PaletteName="Module" Width="350px" BackColor="#FFFFFF">
                                            <borderoptions visible="False" />
                                            <borderoptions visible="False"></borderoptions>
                                            <diagramserializable>
                                                                            <cc2:SimpleDiagram3D RotationMatrixSerializable="1;0;0;0;0;0.5;-0.866025403784439;0;0;0.866025403784439;0.5;0;0;0;0;1">
                                                                            </cc2:SimpleDiagram3D>
                                                                        </diagramserializable>
                                            <legend backcolor="Transparent"></legend>
                                            <seriesserializable>
                                                                            <cc2:Series ArgumentScaleType="Qualitative" Name="Roles" 
                                                                                SynchronizePointOptions="False">
                                                                                <viewserializable>
                                                                                    <cc2:Pie3DSeriesView SizeAsPercentage="100">
                                                                                    </cc2:Pie3DSeriesView>
                                                                                </viewserializable>
                                                                                <labelserializable>
                                                                                    <cc2:Pie3DSeriesLabel Position="Inside">
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
                                                                            <cc2:ChartTitle Font="Tahoma, 12pt" Text="Total Server Roles" 
                                                                                TextColor="64, 64, 64" />
                                                                        </titles>
                                        </dxchartsui:WebChartControl>
                                    </td>
                                </tr>
                            </table>
                        </dx:PanelContent>
                    </PanelCollection>
                    <Border BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" />
                </dx:ASPxPanel>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                    CssPostfix="Glass" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%"
                    TabSpacing="0px" EnableHierarchyRecreation="False">
                    <TabPages>
                        <dx:TabPage Text="Servers">
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl1" runat="server">
                                    <table width="100%">
                                        <tr>
                                            <td valign="top">
                                                <asp:UpdatePanel ID="EXGServerListUpdatePanel" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <dx:ASPxGridView ID="SharePointHealthGridView" runat="server" AutoGenerateColumns="False"
                                                            CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                                                            Cursor="pointer" KeyFieldName="ID" OnSelectionChanged="SharePointHealthGridView_SelectionChanged"
                                                            SummaryText="m" Theme="Office2003Blue" Width="100%" OnPageSizeChanged="SharePointHealthGridView_OnPageSizeChanged"
                                                            onhtmldatacellprepared="SharePointHealthGridView_HtmlDataCellPrepared">
                                                            <Columns>
                                                                <dx:GridViewDataTextColumn Caption="Server Name" FieldName="ServerName" ShowInCustomizationForm="True"
                                                                    VisibleIndex="0" Width="200px">
                                                                    <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    <EditCellStyle CssClass="GridCss">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader">
                                                                    </HeaderStyle>
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Redirect To" FieldName="redirectto" ShowInCustomizationForm="True"
                                                                    Visible="False" VisibleIndex="11">
                                                                    <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    <EditCellStyle CssClass="GridCss">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                                    <CellStyle CssClass="GridCss1">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>

                                                                <dx:GridViewDataTextColumn Caption="Role" FieldName="rolename" ShowInCustomizationForm="True"
                                                                    VisibleIndex="2">
                                                                    <Settings AllowAutoFilter="False" />
                                                                    <EditCellStyle CssClass="GridCss1">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss1">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader1" />
                                                                    <CellStyle CssClass="GridCss1">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>

																<dx:GridViewDataTextColumn Caption="Farm(s)" FieldName="Farm" ShowInCustomizationForm="True"
                                                                    VisibleIndex="3">
                                                                    <Settings AllowAutoFilter="False" />
                                                                    <EditCellStyle CssClass="GridCss1">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss1">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader1" />
                                                                    <CellStyle CssClass="GridCss1">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="CPU" ShowInCustomizationForm="True" 
                                                                    VisibleIndex="7">
                                                                    <EditCellStyle CssClass="GridCss2">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <DataItemTemplate>
                                                                        <asp:Label ID="Type" runat="server" align="center" Font-Bold="true" Text='<%# Eval("Type")%>'
                                                                            Visible="false"></asp:Label>
                                                                        <asp:Label ID="Sec" runat="server" align="center" Font-Bold="true" Text='<%# Eval("Type")%>'
                                                                            Visible="false"></asp:Label>
                                                                        <asp:Label ID="poplbl" runat="server" align="center" Font-Bold="true" Text="" Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblCPUTH" runat="server" align="center" Font-Bold="true" Text='<%#Eval("CPUThreshold")%>'
                                                                            Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblCPU" runat="server" align="center" Font-Bold="true" Text='<%#Eval("CPU")%>'
                                                                            Visible="false"></asp:Label>
                                                                        <a id="ahover" runat="server" class="tooltip1">
                                                                            <asp:Label ID="msgLabel" runat="server" Font-Bold="true" Text="" Visible="true"></asp:Label>
                                                                        </a>
                                                                        <dx:ASPxHyperLink ID="ASPxHyperLink2" runat="server" NavigateUrl="<%# SetCPU(Container) %>"
                                                                            Text='<%# Eval("ServerName") %>' Visible="false" Width="100%">
                                                                        </dx:ASPxHyperLink>
                                                                    </DataItemTemplate>
                                                                    <HeaderStyle CssClass="GridCssHeader2" />
                                                                    <CellStyle CssClass="GridCss2" HorizontalAlign="Right">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Memory" ShowInCustomizationForm="True" 
                                                                    VisibleIndex="8">
                                                                    <EditCellStyle CssClass="GridCss2">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <DataItemTemplate>
                                                                        <asp:Label ID="Type" runat="server" align="center" Font-Bold="true" Text='<%# Eval("Type")%>'
                                                                            Visible="false"></asp:Label>
                                                                        <asp:Label ID="Sec" runat="server" align="center" Font-Bold="true" Text='<%# Eval("Type")%>'
                                                                            Visible="false"></asp:Label>
                                                                        <asp:Label ID="poplbl" runat="server" align="center" Font-Bold="true" Text="" Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblMemTH" runat="server" align="center" Font-Bold="true" Text='<%#Eval("MemThreshold")%>'
                                                                            Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblMem" runat="server" align="center" Font-Bold="true" Text='<%#Eval("MemoryPercent")%>'
                                                                            Visible="false"></asp:Label>
                                                                        <a id="ahover" runat="server" class="tooltip1">
                                                                            <asp:Label ID="msgLabel" runat="server" Font-Bold="true" Text="" Visible="true"></asp:Label>
                                                                        </a>
                                                                        <dx:ASPxHyperLink ID="ASPxHyperLink2" runat="server" NavigateUrl="<%# SetMemory(Container) %>"
                                                                            Text='<%# Eval("ServerName") %>' Visible="false" Width="100%">
                                                                        </dx:ASPxHyperLink>
                                                                    </DataItemTemplate>
                                                                    <HeaderStyle CssClass="GridCssHeader2" />
                                                                    <CellStyle CssClass="GridCss2" HorizontalAlign="Right">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Response Time" ShowInCustomizationForm="True"
                                                                    VisibleIndex="9" Width="100px">
                                                                    <EditCellStyle CssClass="GridCss2">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <DataItemTemplate>
                                                                        <asp:Label ID="lblResponseTH" runat="server" align="center" Font-Bold="true" Text='<%#Eval("ResponseThreshold")%>'
                                                                            Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblResponse" runat="server" align="center" Font-Bold="true" Text='<%#Eval("ResponseTime")%>'
                                                                            Visible="false"></asp:Label>
                                                                        <asp:Label ID="Type" runat="server" align="center" Font-Bold="true" Text='<%# Eval("Type")%>'
                                                                            Visible="false"></asp:Label>
                                                                        <asp:Label ID="Sec" runat="server" align="center" Font-Bold="true" Text='<%# Eval("Type")%>'
                                                                            Visible="false"></asp:Label>
                                                                        <asp:Label ID="poplbl" runat="server" align="center" Font-Bold="true" Text="" Visible="false"></asp:Label>
                                                                        <a id="ahover" runat="server" class="tooltip1">
                                                                            <asp:Label ID="msgLabel" runat="server" Font-Bold="true" Text="" Visible="true"></asp:Label>
                                                                        </a>
                                                                        <dx:ASPxHyperLink ID="ASPxHyperLink2" runat="server" NavigateUrl="<%# SetResponse(Container) %>"
                                                                            Text='<%# Eval("ServerName") %>' Visible="false" Width="100%">
                                                                        </dx:ASPxHyperLink>
                                                                    </DataItemTemplate>
                                                                    <HeaderStyle CssClass="GridCssHeader2" />
                                                                    <CellStyle CssClass="GridCss2" HorizontalAlign="Right">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="OS" FieldName="OperatingSystem" VisibleIndex="12"
                                                                    Width="250px">
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Version" FieldName="DominoVersion" VisibleIndex="13"
                                                                    Width="150px">
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Status" FieldName="Status" VisibleIndex="1">
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
                        <dx:TabPage Text="Farms">
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl2" runat="server">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="DAGUpdatePanel" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <dx:ASPxGridView ID="FarmGridView" runat="server" AutoGenerateColumns="False" SummaryText="m"
                                                            CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                                                            Width="100%" KeyFieldName="Farm" Cursor="pointer" Theme="Office2003Blue" OnPageSizeChanged="FarmGridView_OnPageSizeChanged" OnHtmlDataCellPrepared="FarmGridView_HtmlDataCellPrepared">
                                                            <SettingsPager PageSize="20">
                                                                <PageSizeItemSettings Visible="True">
                                                                </PageSizeItemSettings>
                                                            </SettingsPager>
                                                            <Columns>
                                                                <dx:GridViewDataTextColumn Caption="Farm" VisibleIndex="0" FieldName="Farm">
                                                                    <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    <EditCellStyle CssClass="GridCss">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader">
                                                                    </HeaderStyle>
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Log On Test" VisibleIndex="1" FieldName="LogOnTest"
                                                                    Width="160px">
                                                                    <Settings AllowAutoFilter="False" />
                                                                    <EditCellStyle CssClass="GridCss">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader2" />
                                                                    <CellStyle CssClass="GridCss2">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Upload Test" FieldName="UploadTest" Width='60px' Visible="True"
                                                                    VisibleIndex="3">
                                                                    <Settings AllowAutoFilter="False" />
                                                                    <EditCellStyle CssClass="GridCss">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader2" />
                                                                    <CellStyle CssClass="GridCss2">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Site Collection Creation Test" FieldName="SiteCollectionTest" Visible="True"
                                                                    VisibleIndex="4">
                                                                    <Settings AllowAutoFilter="False" />
                                                                    <EditCellStyle CssClass="GridCss">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader2" />
                                                                    <CellStyle CssClass="GridCss2">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                            </Columns>
                                                            <SettingsBehavior AllowDragDrop="False" AllowFocusedRow="true" AllowSelectByRowClick="True"
                                                                ProcessSelectionChangedOnServer="True" />
                                                            <SettingsPager PageSize="10" SEOFriendly="Enabled">
                                                                <PageSizeItemSettings Visible="true" />
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
                                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                </Header>
                                                                <GroupRow Font-Bold="True" Font-Italic="False">
                                                                </GroupRow>
                                                                <GroupFooter Font-Bold="True">
                                                                </GroupFooter>
                                                                <AlternatingRow CssClass="GridAltRow" Enabled="True">
                                                                </AlternatingRow>
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
						<dx:TabPage Text="Web Apps">
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl3" runat="server">
                                    <table width="100%">
                                        <tr>
                                            <td valign="top">
                                                <asp:UpdatePanel ID="DBUpdatePanel" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <dx:ASPxGridView ID="DBServersGridView" runat="server" AutoGenerateColumns="False"
                                                            CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                                                            Cursor="pointer" KeyFieldName="ID" OnSelectionChanged="DBServersGridView_SelectionChanged" OnPageSizeChanged="DBServersGridView_OnPageSizeChanged"
                                                            SummaryText="m" Theme="Office2003Blue" Width="100%">
                                                            <Columns>
                                                                <dx:GridViewDataTextColumn Caption="Server Name" FieldName="DatabaseServer" ShowInCustomizationForm="True"
                                                                    VisibleIndex="0" Width="200px" Visible="false">
                                                                    <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    <EditCellStyle CssClass="GridCss">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader">
                                                                    </HeaderStyle>
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
																<dx:GridViewDataTextColumn Caption="App Name" FieldName="WebAppName" ShowInCustomizationForm="True"
                                                                    VisibleIndex="1" Width="200px">
                                                                    <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    <EditCellStyle CssClass="GridCss">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader">
                                                                    </HeaderStyle>
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
																<dx:GridViewDataTextColumn Caption="Database Name" FieldName="DatabaseName" ShowInCustomizationForm="True"
                                                                    VisibleIndex="2" Width="200px">
                                                                    <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    <EditCellStyle CssClass="GridCss">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader">
                                                                    </HeaderStyle>
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Redirect To" FieldName="redirectto" ShowInCustomizationForm="True"
                                                                    Visible="false" VisibleIndex="10">
                                                                    <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    <EditCellStyle CssClass="GridCss">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                                    <CellStyle CssClass="GridCss1">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
																
																<dx:GridViewDataTextColumn Caption="Database Site Count" FieldName="DatabaseSiteCount" ShowInCustomizationForm="True"
                                                                    VisibleIndex="3" Width="200px">
                                                                    <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    <EditCellStyle CssClass="GridCss">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader2">
                                                                    </HeaderStyle>
                                                                    <CellStyle CssClass="GridCss2">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
																<dx:GridViewDataTextColumn Caption="Max Site Count" FieldName="MaxSiteCountThreshold" ShowInCustomizationForm="True"
                                                                    VisibleIndex="4" Width="200px">
                                                                    <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    <EditCellStyle CssClass="GridCss">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader2">
                                                                    </HeaderStyle>
                                                                    <CellStyle CssClass="GridCss2">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
																<dx:GridViewDataTextColumn Caption="Warning Site Count" FieldName="WarningSiteCountThreshold" ShowInCustomizationForm="True"
                                                                    VisibleIndex="5" Width="200px">
                                                                    <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    <EditCellStyle CssClass="GridCss">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader2">
                                                                    </HeaderStyle>
                                                                    <CellStyle CssClass="GridCss2">
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
						<dx:TabPage Text="Site Collections" >
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl4" runat="server">
                                    <table width="100%">
                                        
                                        <tr>
                                            <td valign="top">
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
														
														<table>
															<tr>
                                                            <td>
                                                            <table>
                                                            <tr>
																<td>
                                                               
																	<dx:ASPxLabel runat="server" Text="Select a farm: " CssClass="lblsmallFont" />
																</td>
																<td>
																	<dx:ASPxComboBox ID="FarmForSiteCollComboBox" runat="server" OnSelectedIndexChanged="FarmForSiteCollComboBox_SelectedIndexChanged"
																	 AutoPostBack="true" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" />
                                                                    
																</td>
                                                                </tr>
                                                                </table>
                                                                </td>
															</tr>
															<tr>
																<td>
																	<dx:ASPxGridView ID="SiteCollectionSizeGridView" runat="server" 
                                                                        AutoGenerateColumns="False" 
                                                                        CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                                                                        CssPostfix="Office2010Silver" Cursor="pointer" KeyFieldName="ID" 
                                                                        OnPageSizeChanged="SiteCollectionSizeGridView_OnPageSizeChanged" 
                                                                        SummaryText="m" Theme="Office2003Blue" Width="100%">
                                                                        <Columns>
                                                                            <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" Visible="false" 
                                                                                VisibleIndex="0">
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn Caption="Site Collection" FieldName="SiteCollection" 
                                                                                ShowInCustomizationForm="True" VisibleIndex="1" Width="100px">
                                                                                <Settings AllowAutoFilterTextInputTimer="False" 
                                                                                    AutoFilterCondition="Contains" />
                                                                                <EditCellStyle CssClass="GridCss">
                                                                                </EditCellStyle>
                                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                                </EditFormCaptionStyle>
                                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                                <CellStyle CssClass="GridCss">
                                                                                </CellStyle>
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn Caption="Size (MB)" FieldName="SizeMB" 
                                                                                ShowInCustomizationForm="True" VisibleIndex="2" Width="100px">
                                                                                <Settings AllowAutoFilterTextInputTimer="False" 
                                                                                    AutoFilterCondition="Contains" />
                                                                                <EditCellStyle CssClass="GridCss">
                                                                                </EditCellStyle>
                                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                                </EditFormCaptionStyle>
                                                                                <HeaderStyle CssClass="GridCssHeader2" />
                                                                                <CellStyle CssClass="GridCss2">
                                                                                </CellStyle>
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn Caption="Owner" FieldName="Owner" 
                                                                                ShowInCustomizationForm="True" VisibleIndex="3" Width="100px">
                                                                                <Settings AllowAutoFilterTextInputTimer="False" 
                                                                                    AutoFilterCondition="Contains" />
                                                                                <EditCellStyle CssClass="GridCss">
                                                                                </EditCellStyle>
                                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                                </EditFormCaptionStyle>
                                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                                <CellStyle CssClass="GridCss">
                                                                                </CellStyle>
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn Caption="Site Count" FieldName="SiteCount" 
                                                                                ShowInCustomizationForm="True" VisibleIndex="4" Width="100px">
                                                                                <Settings AllowAutoFilterTextInputTimer="False" 
                                                                                    AutoFilterCondition="Contains" />
                                                                                <EditCellStyle CssClass="GridCss">
                                                                                </EditCellStyle>
                                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                                </EditFormCaptionStyle>
                                                                                <HeaderStyle CssClass="GridCssHeader2" />
                                                                                <CellStyle CssClass="GridCss2">
                                                                                </CellStyle>
                                                                            </dx:GridViewDataTextColumn>
                                                                        </Columns>
                                                                        <SettingsBehavior AllowDragDrop="False" AllowFocusedRow="True" 
                                                                            AllowSelectByRowClick="True" AutoExpandAllGroups="True" 
                                                                            ProcessSelectionChangedOnServer="True" />
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
                                                                        <Styles CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                                                                            CssPostfix="Office2010Silver">
                                                                            <Header CssClass="GridCssHeader" ImageSpacing="5px" SortingImageSpacing="5px">
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
																</td>
                                                               
															</tr>
														</table>

													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
										</tr>
									</table>
								</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>
                         <dx:TabPage Text="Timer Jobs">
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl5" runat="server">
                                    <table width="100%">
                                        <tr>
                                            <td valign="top">
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <dx:ASPxGridView ID="SharePointTimerJobsGridView" runat="server" AutoGenerateColumns="False"
                                                            CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                                                            Cursor="pointer" KeyFieldName="ID"
                                                           Theme="Office2003Blue" Width="100%" OnPageSizeChanged="SharePointTimerJobsGridView_OnPageSizeChanged"
                                                            onhtmldatacellprepared="SharePointTimerJobsGridView_HtmlDataCellPrepared">
                                                            <Columns>
                                                                <dx:GridViewDataTextColumn Caption="Job Name" FieldName="JobName" ShowInCustomizationForm="True"
                                                                    VisibleIndex="2">
                                                                    <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    <EditCellStyle CssClass="GridCss">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader">
                                                                    </HeaderStyle>
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Server Name" FieldName="ServerName" ShowInCustomizationForm="True"
                                                                      VisibleIndex="1">
                                                                    <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    <EditCellStyle CssClass="GridCss">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>

                                                                <dx:GridViewDataTextColumn Caption="Web Application Name" 
                                                                    FieldName="WebApplicationName" ShowInCustomizationForm="True"
                                                                    VisibleIndex="3">
                                                                    <Settings AllowAutoFilter="False" />
                                                                    <EditCellStyle CssClass="GridCss1">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss1">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>

																<dx:GridViewDataTextColumn Caption="Status" FieldName="Status" ShowInCustomizationForm="True"
                                                                    VisibleIndex="4">
                                                                    <Settings AllowAutoFilter="False" />
                                                                    <EditCellStyle CssClass="GridCss1">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss1">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader1" />
                                                                    <CellStyle CssClass="GridCss1">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                             	<dx:GridViewDataTextColumn Caption="Start Time" FieldName="StartTime" ShowInCustomizationForm="True"
                                                                    VisibleIndex="5" Width="80px">
                                                                    <Settings AllowAutoFilter="False" />
                                                                    <EditCellStyle CssClass="GridCss1">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss1">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader1" />
                                                                    <CellStyle CssClass="GridCss1">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                	<dx:GridViewDataTextColumn Caption="End Time" FieldName="EndTime" ShowInCustomizationForm="True"
                                                                    VisibleIndex="6" Width="80px">
                                                                    <Settings AllowAutoFilter="False" />
                                                                    <EditCellStyle CssClass="GridCss1">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss1">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader1" />
                                                                    <CellStyle CssClass="GridCss1">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                            	<dx:GridViewDataTextColumn Caption="Database Name" FieldName="DataBaseName" ShowInCustomizationForm="True"
                                                                    VisibleIndex="7">
                                                                    <Settings AllowAutoFilter="False" />
                                                                    <EditCellStyle CssClass="GridCss1">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss1">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                              	<dx:GridViewDataTextColumn Caption="Error Message" FieldName="ErrorMessage" ShowInCustomizationForm="True"
                                                                    VisibleIndex="8">
                                                                    <Settings AllowAutoFilter="False" />
                                                                    <EditCellStyle CssClass="GridCss1">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss1">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                              	<dx:GridViewDataTextColumn Caption="Farm" FieldName="Farm" ShowInCustomizationForm="True"
                                                                    VisibleIndex="9">
                                                                    <Settings AllowAutoFilter="False" />
                                                                    <EditCellStyle CssClass="GridCss1">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss1">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                             	<dx:GridViewDataTextColumn Caption="Schedule" FieldName="Schedule" ShowInCustomizationForm="True"
                                                                    VisibleIndex="10">
                                                                    <Settings AllowAutoFilter="False" />
                                                                    <EditCellStyle CssClass="GridCss1">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss1">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader" />
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
                                            </td>
                                        </tr>
                                    </table>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                    </TabPages>
                </dx:ASPxPageControl>
            </td>
        </tr>
    </table>
</asp:Content>
