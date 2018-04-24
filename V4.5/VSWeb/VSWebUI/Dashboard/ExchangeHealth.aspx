<%@ Page Title="VitalSigns Plus - Exchange Server Health" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="ExchangeHealth.aspx.cs" Inherits="VSWebUI.Dashboard.ExchangeHealth" %>
<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
    <%@ Register assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>




<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>



<%@ Register assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc2" %>





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
    <link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
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
<script type="text/javascript" language="javascript" >
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
        //        var imgButton = document.getElementById('popupButton');
        //        ASPxClientUtils.AttachEventToElement(imgButton, 'contextmenu', OnPreventContextMenu);
        //}
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="99%"> 
    <tr>
        <td valign="top">
            <table>
                <tr>
                    <td align="center">
                        <img alt="" src="../images/icons/exchange%20icon%203.jpg" style="width: 31px; height: 31px;" />
                    </td>
                    <td>
                        <div class="header" id="servernamelbldisp" runat="server">Microsoft Exchange Server Health</div>
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
                        <dxchartsui:WebChartControl ID="SrvRolesWebChart" runat="server" 
                                                                        CrosshairEnabled="True" 
                            Height="200px" PaletteName="Module" Width="400px" BackColor="White">
                                                                        <borderoptions visible="False" />
<BorderOptions Visible="False" visibility="True"></BorderOptions>
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
                    <td>
                        <dxchartsui:WebChartControl ID="StatusWebChart" runat="server" 
                            AppearanceNameSerializable="Pastel Kit" CrosshairEnabled="True" Height="200px" 
                            oncustomdrawseriespoint="StatusWebChart_CustomDrawSeriesPoint" 
                            Width="400px" BackColor="White" PaletteName="Module">
                            <borderoptions visible="False" />
<BorderOptions Visible="False" visibility="True"></BorderOptions>
                            <diagramserializable>
                                <cc2:SimpleDiagram3D RotationMatrixSerializable="1;0;0;0;0;0.5;-0.866025403784439;0;0;0.866025403784439;0.5;0;0;0;0;1">
                                </cc2:SimpleDiagram3D>
                            </diagramserializable>
                            <legend backcolor="Transparent" AlignmentHorizontal="Center" 
                                AlignmentVertical="BottomOutside" MaxVerticalPercentage="30"></legend>
                            <seriesserializable>
                                <cc2:Series ArgumentScaleType="Qualitative" Name="Statuses" 
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
                                <cc2:ChartTitle Font="Tahoma, 12pt" Text="By Status" TextColor="64, 64, 64" />
                            </titles>
                        </dxchartsui:WebChartControl>   
                    </td>
                    <td>
                        <dxchartsui:WebChartControl ID="OSWebChart" runat="server" Height="200px" 
                            Width="400px" CrosshairEnabled="True" PaletteName="Module" 
                            BackColor="White">
                            <borderoptions visible="False" />
<BorderOptions Visible="False" visibility="True"></BorderOptions>
                            <diagramserializable>
                                <cc2:SimpleDiagram3D RotationMatrixSerializable="1;0;0;0;0;0.5;-0.866025403784439;0;0;0.866025403784439;0.5;0;0;0;0;1">
                                </cc2:SimpleDiagram3D>
                            </diagramserializable>
                            <legend alignmenthorizontal="Center" alignmentvertical="BottomOutside" 
                                BackColor="Transparent" MaxVerticalPercentage="30"></legend>
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
                        </dxchartsui:WebChartControl>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
                            <tr>
                                <td valign="top">  
                                    <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
                                        CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%" TabSpacing="0px" EnableHierarchyRecreation="False">
                                        <TabPages>
                                            <dx:TabPage Text="Servers">
                                                <ContentCollection>
                                                    <dx:ContentControl runat="server">
                                                        <table width="100%">
                                                            <tr>
                                                            <td valign="top">
                                                                    <asp:UpdatePanel ID="EXGServerListUpdatePanel" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <dx:ASPxGridView ID="EXGHealthGridView" runat="server" 
                                                                AutoGenerateColumns="False" 
                                                                CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                                                                CssPostfix="Office2010Silver" Cursor="pointer" 
                                                                KeyFieldName="ID" OnSelectionChanged="EXGHealthGridView_SelectionChanged" 
                                                                SummaryText="m" Theme="Office2003Blue" Width="100%" 
                                                                        onhtmldatacellprepared="EXGHealthGridView_HtmlDataCellPrepared" OnPageSizeChanged="EXGHealthGridView_OnPageSizeChanged">
                                                                <Columns>
                                                                    <dx:GridViewDataTextColumn Caption="Server Name" FieldName="ServerName" 
                                                                        ShowInCustomizationForm="True" VisibleIndex="0" Width="200px">
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
                                                                    <dx:GridViewDataTextColumn Caption="Redirect To" FieldName="redirectto" 
                                                                        ShowInCustomizationForm="True" Visible="False" VisibleIndex="11">
                                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                        <EditCellStyle CssClass="GridCss">
                                                                        </EditCellStyle>
                                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                                        </EditFormCaptionStyle>
                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                        <CellStyle CssClass="GridCss1">
                                                                        </CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataCheckColumn Caption="CAS" FieldName="CAS" 
                                                                        ShowInCustomizationForm="True" VisibleIndex="2">
                                                                        <Settings AllowAutoFilter="False" />
                                                                        <EditCellStyle CssClass="GridCss1">
                                                                        </EditCellStyle>
                                                                        <EditFormCaptionStyle CssClass="GridCss1">
                                                                        </EditFormCaptionStyle>
                                                                        <DataItemTemplate>
                                                                            <dx:ASPxCheckBox ID="CAS" runat="server" 
                                                                                Checked='<%# Convert.ToInt32(Eval("CAS")) > 0? true : false %>' Enabled="false">
                                                                            </dx:ASPxCheckBox>
                                                                        </DataItemTemplate>
                                                                        <HeaderStyle CssClass="GridCssHeader1" />
                                                                        <CellStyle CssClass="GridCss1">
                                                                        </CellStyle>
                                                                    </dx:GridViewDataCheckColumn>
                                                                    <dx:GridViewDataCheckColumn Caption="MBX" FieldName="MailBox" 
                                                                        ShowInCustomizationForm="True" UnboundType="Boolean" VisibleIndex="3">
                                                                        <Settings AllowAutoFilter="False" />
                                                                        <EditCellStyle CssClass="GridCss1">
                                                                        </EditCellStyle>
                                                                        <EditFormCaptionStyle CssClass="GridCss1">
                                                                        </EditFormCaptionStyle>
                                                                        <DataItemTemplate>
                                                                            <dx:ASPxCheckBox ID="MailBox" runat="server" 
                                                                                Checked='<%# Convert.ToInt32(Eval("Mailbox")) > 0 ? true : false %>' 
                                                                                Enabled="false">
                                                                            </dx:ASPxCheckBox>
                                                                        </DataItemTemplate>
                                                                        <HeaderStyle CssClass="GridCssHeader1" />
                                                                        <CellStyle CssClass="GridCss1">
                                                                        </CellStyle>
                                                                    </dx:GridViewDataCheckColumn>
                                                                    <dx:GridViewDataCheckColumn Caption="EDGE" FieldName="EDGE" 
                                                                        ShowInCustomizationForm="True" VisibleIndex="4">
                                                                        <Settings AllowAutoFilter="False" />
                                                                        <EditCellStyle CssClass="GridCss1">
                                                                        </EditCellStyle>
                                                                        <EditFormCaptionStyle CssClass="GridCss1">
                                                                        </EditFormCaptionStyle>
                                                                        <DataItemTemplate>
                                                                            <dx:ASPxCheckBox ID="EDGE" runat="server" 
                                                                                Checked='<%# Convert.ToInt32(Eval("EDGE")) > 0 ? true : false %>' 
                                                                                Enabled="false">
                                                                            </dx:ASPxCheckBox>
                                                                        </DataItemTemplate>
                                                                        <HeaderStyle CssClass="GridCssHeader1" />
                                                                        <CellStyle CssClass="GridCss1">
                                                                        </CellStyle>
                                                                    </dx:GridViewDataCheckColumn>
                                                                    <dx:GridViewDataCheckColumn Caption="HUB" FieldName="HUB" 
                                                                        ShowInCustomizationForm="True" VisibleIndex="5">
                                                                        <PropertiesCheckEdit>
                                                                            

                                                                            <CheckBoxStyle HorizontalAlign="Left" Wrap="True" />
                                                                            

                                                                            <Style HorizontalAlign="Left" Wrap="True">
                                                                                

                                                                            </Style>
                                                                            

                                                                        </PropertiesCheckEdit>
                                                                        <Settings AllowAutoFilter="False" />
                                                                        <EditCellStyle CssClass="GridCss1">
                                                                        </EditCellStyle>
                                                                        <EditFormCaptionStyle CssClass="GridCss1">
                                                                        </EditFormCaptionStyle>
                                                                        <DataItemTemplate>
                                                                            <dx:ASPxCheckBox ID="HUB" runat="server" 
                                                                                Checked='<%# Convert.ToInt32(Eval("HUB")) > 0 ? true : false %>' 
                                                                                Enabled="false">
                                                                            </dx:ASPxCheckBox>
                                                                        </DataItemTemplate>
                                                                        <HeaderStyle CssClass="GridCssHeader1" />
                                                                        <CellStyle CssClass="GridCss1">
                                                                        </CellStyle>
                                                                    </dx:GridViewDataCheckColumn>
                                                                    <dx:GridViewDataTextColumn Caption="CPU" ShowInCustomizationForm="True" 
                                                                        VisibleIndex="7">
                                                                        <EditCellStyle CssClass="GridCss2">
                                                                        </EditCellStyle>
                                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                                        </EditFormCaptionStyle>
                                                                        <DataItemTemplate>
                                                                            <asp:Label ID="Type" runat="server" align="center" Font-Bold="true" 
                                                                                Text='<%# Eval("Type")%>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="Sec" runat="server" align="center" Font-Bold="true" 
                                                                                Text='<%# Eval("Type")%>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="poplbl" runat="server" align="center" Font-Bold="true" Text="" 
                                                                                Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblCPUTH" runat="server" align="center" Font-Bold="true" 
                                                                                Text='<%#Eval("CPUThreshold")%>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblCPU" runat="server" align="center" Font-Bold="true" 
                                                                                Text='<%#Eval("CPU")%>' Visible="false"></asp:Label>
                                                                            <a ID="ahover" runat="server" class="tooltip1">
                                                                            <asp:Label ID="msgLabel" runat="server" Font-Bold="true" Text="" Visible="true"></asp:Label>
                                                                            </a>
                                                                            <dx:ASPxHyperLink ID="ASPxHyperLink2" runat="server" 
                                                                                NavigateUrl="<%# SetCPU(Container) %>" Text='<%# Eval("ServerName") %>' 
                                                                                Visible="false" Width="100%">
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
                                                                            <asp:Label ID="Type" runat="server" align="center" Font-Bold="true" 
                                                                                Text='<%# Eval("Type")%>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="Sec" runat="server" align="center" Font-Bold="true" 
                                                                                Text='<%# Eval("Type")%>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="poplbl" runat="server" align="center" Font-Bold="true" Text="" 
                                                                                Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblMemTH" runat="server" align="center" Font-Bold="true" 
                                                                                Text='<%#Eval("MemThreshold")%>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblMem" runat="server" align="center" Font-Bold="true" 
                                                                                Text='<%#Eval("MemoryPercent")%>' Visible="false"></asp:Label>
                                                                            <a ID="ahover" runat="server" class="tooltip1">
                                                                            <asp:Label ID="msgLabel" runat="server" Font-Bold="true" Text="" Visible="true"></asp:Label>
                                                                            
                                                                            </a>
                                                                            <dx:ASPxHyperLink ID="ASPxHyperLink2" runat="server" 
                                                                                NavigateUrl="<%# SetMemory(Container) %>" Text='<%# Eval("ServerName") %>' 
                                                                                Visible="false" Width="100%">
                                                                            </dx:ASPxHyperLink>
                                                                        </DataItemTemplate>
                                                                        <HeaderStyle CssClass="GridCssHeader2" />
                                                                        <CellStyle CssClass="GridCss2" HorizontalAlign="Right">
                                                                        </CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Response Time" 
                                                                        ShowInCustomizationForm="True" VisibleIndex="9" Width="100px">
                                                                        <EditCellStyle CssClass="GridCss2">
                                                                        </EditCellStyle>
                                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                                        </EditFormCaptionStyle>
                                                                        <DataItemTemplate>
                                                                            <asp:Label ID="lblResponseTH" runat="server" align="center" Font-Bold="true" 
                                                                                Text='<%#Eval("ResponseThreshold")%>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblResponse" runat="server" align="center" Font-Bold="true" 
                                                                                Text='<%#Eval("ResponseTime")%>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="Type" runat="server" align="center" Font-Bold="true" 
                                                                                Text='<%# Eval("Type")%>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="Sec" runat="server" align="center" Font-Bold="true" 
                                                                                Text='<%# Eval("Type")%>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="poplbl" runat="server" align="center" Font-Bold="true" Text="" 
                                                                                Visible="false"></asp:Label>
                                                                            <a ID="ahover" runat="server" class="tooltip1">
                                                                            <asp:Label ID="msgLabel" runat="server" Font-Bold="true" Text="" Visible="true"></asp:Label>
                                                                            </a>
                                                                            <dx:ASPxHyperLink ID="ASPxHyperLink2" runat="server" 
                                                                                NavigateUrl="<%# SetResponse(Container) %>" Text='<%# Eval("ServerName") %>' 
                                                                                Visible="false" Width="100%">
                                                                            </dx:ASPxHyperLink>
                                                                        </DataItemTemplate>
                                                                        <HeaderStyle CssClass="GridCssHeader2" />
                                                                        <CellStyle CssClass="GridCss2" HorizontalAlign="Right">
                                                                        </CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="OS" FieldName="OperatingSystem" 
                                                                        VisibleIndex="12">
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Version" FieldName="DominoVersion" 
                                                                        VisibleIndex="13">
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Status" FieldName="Status" VisibleIndex="1">
                                                                        <HeaderStyle CssClass="GridCssHeader1" />
                                                                        <CellStyle CssClass="GridCss1">
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
                                            <dx:TabPage Text="DAG">
                                                <ContentCollection>
                                                    <dx:ContentControl runat="server">
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:UpdatePanel ID="DAGUpdatePanel" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <dx:ASPxGridView ID="DAGGridView" runat="server" AutoGenerateColumns="False" SummaryText="m" 
                                                                        CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                                                                        CssPostfix="Office2010Silver" Width="100%"                 
                                                                        KeyFieldName="DAGName" OnSelectionChanged="DAGGridView_SelectionChanged" OnPageSizeChanged="DAGGridView_OnPageSizeChanged"
                                                                       Cursor="pointer"  Theme="Office2003Blue" 
                                                                    onhtmldatacellprepared="DAGGridView_HtmlDataCellPrepared" >                   
                                                                      
                                                                       <SettingsPager PageSize="20">
                                                                            <PageSizeItemSettings Visible="True"></PageSizeItemSettings>
                                                                       </SettingsPager>
                                                                       <Columns>
                                                                        <dx:GridViewDataTextColumn Caption="DAG Name" VisibleIndex="0" FieldName="DAGName" >                       
                                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader">
                                                                                <Paddings Padding="5px" />
                                                                            </HeaderStyle>
                                                                            <CellStyle CssClass="GridCss"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>                                                                        
                                                                        <dx:GridViewDataTextColumn Caption="File Witness Server Name" VisibleIndex="1" 
                                                                               FieldName="FileWitnessSereverName" Width="160px">
                                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                         <dx:GridViewDataTextColumn Caption="Members" FieldName="Members" Width='60px' 
                                                                               Visible="True" VisibleIndex="3">
                                                                            <Settings AllowAutoFilter="False" />
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader2" />
                                                                            <CellStyle CssClass="GridCss2"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                         <dx:GridViewDataTextColumn Caption="Total Databases" FieldName="TotalDatabases" 
                                                                               Visible="True" VisibleIndex="4">
                                                                            <Settings AllowAutoFilter="False" />
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader2" />
                                                                            <CellStyle CssClass="GridCss2"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                         <dx:GridViewDataTextColumn Caption="Total MailBoxes" FieldName="TotalMailBoxes" 
                                                                               Visible="True" VisibleIndex="5">
                                                                            <Settings AllowAutoFilter="False" />
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader2" />
                                                                            <CellStyle CssClass="GridCss2"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="ID" VisibleIndex="6" FieldName="ID" 
                                                                               Visible="false">
                                                                           
                                                                        </dx:GridViewDataTextColumn>
                                                                         <dx:GridViewDataTextColumn Caption="Status" FieldName="Status" VisibleIndex="2">
                                                                                <PropertiesTextEdit EnableFocusedStyle="False">
                                                                                    

                                                                                    <Style HorizontalAlign="Left" Wrap="True">
                                                                                        

                                                                                    </Style>
                                                                                    

                                                                                </PropertiesTextEdit>
                                                                                <Settings AllowAutoFilter="False" />
                                                                                <EditCellStyle CssClass="GridCss1">
                                                                                </EditCellStyle>
                                                                                <EditFormCaptionStyle CssClass="GridCss1">
                                                                                </EditFormCaptionStyle>
                                                                             
                                                                                <HeaderStyle CssClass="GridCssHeader1" />
                                                                                <CellStyle CssClass="GridCss1">
                                                                                </CellStyle>
                                                                            </dx:GridViewDataTextColumn>
                                                                        </Columns>

                                                                        <SettingsBehavior AllowDragDrop="False" AllowFocusedRow="true" 
                                                                            AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True" />
                                                                        <SettingsPager PageSize="10" SEOFriendly="Enabled" >
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
                                                                        <Styles CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                                                                            CssPostfix="Office2010Silver">
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
                                            <dx:TabPage Text="CAS">
                                                <ContentCollection>
                                                    <dx:ContentControl runat="server">
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:UpdatePanel ID="CASUpdatePanel" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <dx:ASPxGridView ID="CASGridView" runat="server" AutoGenerateColumns="False" SummaryText="m" 
                                                                        CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                                                                        CssPostfix="Office2010Silver" Width="100%"                   
                                                                        KeyFieldName="Name"  OnSelectionChanged="CASGrid_SelectionChanged" OnPageSizeChanged="CASGridView_OnPageSizeChanged"
                                                                       Cursor="pointer" EnableCallBacks="False" Theme="Office2003Blue" 
                                                                    onhtmldatacellprepared="CASGridView_HtmlDataCellPrepared" >  
                                                                       <%--<ClientSideEvents FocusedRowChanged="EXGHealthGridView_FocusedRowChanged" />--%>                
                                                                       <SettingsBehavior AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True" 
                                                                            ColumnResizeMode="NextColumn"></SettingsBehavior>
                                                                       <SettingsPager PageSize="20">
                                                                            <PageSizeItemSettings Visible="True"></PageSizeItemSettings>
                                                                       </SettingsPager>
                                                

                                                                       <Columns>
                                                                        <dx:GridViewDataTextColumn Caption="Server Name" VisibleIndex="0" 
                                                                               FieldName="Name"  Width="180px">                       
                                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader">
                                                                                <Paddings Padding="5px" />
                                                                            </HeaderStyle>
                                                                            <CellStyle CssClass="GridCss"></CellStyle>
                                                                        </dx:GridViewDataTextColumn> 
                                                                        <dx:GridViewDataTextColumn Caption="RPC" FieldName="RPC" Visible="True" 
                                                                               VisibleIndex="1">
                                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                                            <CellStyle CssClass="GridCss1"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Redirect To" FieldName="redirectto" 
                                                                               Visible="False" VisibleIndex="10">
                                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                                            <CellStyle CssClass="GridCss1"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="IMAP" VisibleIndex="2" FieldName="IMAP">
                                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss1"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="OWA" VisibleIndex="3" FieldName="OWA">
                                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss1"></CellStyle>
                                                                        </dx:GridViewDataTextColumn> 
                                                                        <dx:GridViewDataTextColumn Caption="POP3" VisibleIndex="4" FieldName="POP3">
                                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss1"></CellStyle>
                                                                        </dx:GridViewDataTextColumn> 
                                                                        <dx:GridViewDataTextColumn Caption="Active Sync" VisibleIndex="5" 
                                                                               FieldName="Active Sync">
                                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" /><CellStyle CssClass="GridCss1"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="SMTP" VisibleIndex="6" FieldName="SMTP">
                                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss1"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Outlook Anywhere" VisibleIndex="7" FieldName="EWS">
                                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss1"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Auto Discovery" VisibleIndex="8" FieldName="Auto Discovery">
                                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss1"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Services" VisibleIndex="9" FieldName="Services">
                                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss1"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        </Columns>
                                                                        <SettingsBehavior AllowDragDrop="False"  AllowFocusedRow="true" 
                                                                            AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True" 
                                                                           AllowGroup="False" />
                                                                        <SettingsPager PageSize="10" SEOFriendly="Enabled" >
                                                                            <PageSizeItemSettings Visible="true" />
                                                                        </SettingsPager>
                                                                        <Settings ShowFilterRow="True"></Settings>

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
                                            <dx:TabPage Text="Queues">
                                                <ContentCollection>
                                                    <dx:ContentControl runat="server">
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:UpdatePanel ID="HubEdgeUpdatePanel" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <dx:ASPxGridView ID="HubEdgeGridView" runat="server" 
                                                                    AutoGenerateColumns="False" SummaryText="m" 
                                                                        CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                                                                        CssPostfix="Office2010Silver" Width="100%"                    
                                                                        KeyFieldName="CID" OnSelectionChanged="HubEdgeGridView_SelectionChanged"  OnPageSizeChanged="HubEdgeGridView_OnPageSizeChanged"
                                                                       Cursor="pointer" EnableCallBacks="False" Theme="Office2003Blue" >                   
                                                                       <SettingsBehavior AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True" 
                                                                            ColumnResizeMode="NextColumn" AllowGroup="False"></SettingsBehavior>
                                                                       <SettingsPager PageSize="20">
                                                                            <PageSizeItemSettings Visible="True"></PageSizeItemSettings>
                                                                       </SettingsPager>
                                                                       <Columns>
                                                                        <%--<dx:GridViewDataTextColumn Caption="Submission Queue" FieldName="SubmissionQ" Width='150px'
                                                                                ShowInCustomizationForm="True" Visible="True" VisibleIndex="1">
                                                                            <Settings AllowAutoFilter="True" AllowDragDrop="True" AutoFilterCondition="Contains" />
                                                                            <Settings AllowDragDrop="True" AllowAutoFilter="True" AutoFilterCondition="Contains"></Settings>
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                                            <CellStyle CssClass="GridCss1"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>--%>
                                                                        <%--<dx:GridViewDataTextColumn Caption="Submission Queue" 
                            VisibleIndex="1" Width="150px" FieldName="SubmissionQ" ><DataItemTemplate>
                                            <asp:Label ID="lblSubQTH" runat="server" Text='<%#Eval("SubQThreshold")%>' Visible="false" align="center" Font-Bold="true"></asp:Label>
                                            <asp:Label ID="lblSubQ" runat="server" Text='<%#Eval("SubmissionQ")%>' Visible="false" align="center" Font-Bold="true"></asp:Label>
                                            
                                            <a  class='tooltip1' runat="server" id="ahover" ><div class="parent"  runat="server" id="parentdiv">
                                                <div class="msg">
                                                    <asp:Label ID="msgLabel" runat="server" Text="" Visible="true" Font-Bold="true"></asp:Label></div><div class="b">
                                                <dx:ASPxGaugeControl runat="server" Width="145px" Height="30px" BackColor="Transparent"
                                                    ID="gControl_Page3" ClientInstanceName="Gauge3" SaveStateOnCallbacks="False"
                                                    AutoLayout="False" visible=false>                                                   
                                                        
                                                    <LayoutPadding All="0" Left="0" Top="0" Right="0" Bottom="0"></LayoutPadding>
                                                </dx:ASPxGaugeControl>
                                                 </div></div>
                                                    <span class="tip">
                                                    <img class='callout1' src='../images/callout_2.gif' style="visibility:hidden" />
                                                   
                                                    <asp:Label ID="lblSubQDetails" runat="server" Visible="false"></asp:Label></span></a><dx:ASPxHyperLink ID="ASPxHyperLink2" runat="server" NavigateUrl="<%# SetSubQ(Container) %>"
                                                Text='<%# Eval("ServerName") %>' Width="100%" Visible="false">
                                            </dx:ASPxHyperLink>
                                        </DataItemTemplate>
                               <EditCellStyle CssClass="GridCss"></EditCellStyle>
                               <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                               <HeaderStyle 
                                CssClass="GridCssHeader" /><CellStyle CssClass="GridCss" HorizontalAlign="Right"></CellStyle>
                        </dx:GridViewDataTextColumn>--%>
                                                                        <dx:GridViewDataTextColumn Caption="Server Name" VisibleIndex="0" 
                                                                               FieldName="ServerName" Width="180px" >                       
                                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader">
                                                                                <Paddings Padding="5px" />
                                                                            </HeaderStyle>
                                                                            <CellStyle CssClass="GridCss"></CellStyle>
                                                                        </dx:GridViewDataTextColumn> 
                                                                        <dx:GridViewDataTextColumn Caption="Redirect To" FieldName="redirectto" 
                                                                               Visible="False" VisibleIndex="10">
                                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                                            <CellStyle CssClass="GridCss1"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Submission Queue" VisibleIndex="1" FieldName="SubmissionQ">
                                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader2" Wrap="True" /><CellStyle CssClass="GridCss2"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Shadow Queue" VisibleIndex="2" 
                                                                               FieldName="ShadowQ">
                                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader2" Wrap="True" /><CellStyle CssClass="GridCss2"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Total" VisibleIndex="4">
                                                                            <EditFormCaptionStyle CssClass="GridCss2">
                                                                            </EditFormCaptionStyle>
                                                                            <DataItemTemplate>
                                                                                <asp:Label ID="aspLabel1" runat="server" Text='<%# Convert.ToInt32(Eval("SubmissionQ"))+Convert.ToInt32(Eval("ShadowQ"))+Convert.ToInt32(Eval("UnreachableQ")) %>'></asp:Label>
                                                                            
                                                                            </DataItemTemplate>
                                                                            <EditCellStyle CssClass="GridCss2"></EditCellStyle>
                                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            <HeaderStyle CssClass="GridCssHeader2" />
                                                                            <CellStyle CssClass="GridCss2"></CellStyle>
                                                                        </dx:GridViewDataTextColumn> 
                                                                        <dx:GridViewDataTextColumn Caption="Unreachable Queue" VisibleIndex="3" 
                                                                               FieldName="UnreachableQ">
                                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader2" Wrap="True" /><CellStyle CssClass="GridCss2"></CellStyle>
                                                                        </dx:GridViewDataTextColumn> 
                                                                        <dx:GridViewDataTextColumn Caption="Services" VisibleIndex="5" Visible="false">
                                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="rolename" VisibleIndex="6" Visible ="false" FieldName="rolename">
                                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        </Columns>
                                                                        <SettingsBehavior AllowDragDrop="False" AutoExpandAllGroups="True"  AllowFocusedRow="true" 
                                                                            AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True" />
                                                                        <SettingsPager PageSize="10" SEOFriendly="Enabled" >
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
                                                                        <Styles CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                                                                            CssPostfix="Office2010Silver">
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
                                            <dx:TabPage Text="Mail Box">
                                                <ContentCollection>
                                                    <dx:ContentControl runat="server">
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:UpdatePanel ID="MailBoxUpdatePanel" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <dx:ASPxGridView ID="MailBoxGridView" runat="server" AutoGenerateColumns="False" SummaryText="m" 
                                                                        CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                                                                        CssPostfix="Office2010Silver" Width="100%" OnSelectionChanged="MailBoxGridView_SelectionChanged" OnPageSizeChanged="MailBoxGridView_OnPageSizeChanged"                
                                                                        KeyFieldName="ServerName" 
                                                                       Cursor="pointer" EnableCallBacks="False" Theme="Office2003Blue" >                   
                                                                       <SettingsBehavior AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True" 
                                                                            ColumnResizeMode="NextColumn" AllowGroup="False"></SettingsBehavior>
                                                                       <SettingsPager PageSize="20">
                                                                            <PageSizeItemSettings Visible="True"></PageSizeItemSettings>
                                                                       </SettingsPager>
                                                                       <Columns>
                                                                        <dx:GridViewDataTextColumn Caption="Server Name" VisibleIndex="0" 
                                                                               FieldName="ServerName" Width="180px" >                       
                                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader">
                                                                                <Paddings Padding="5px" />
                                                                            </HeaderStyle>
                                                                            <CellStyle CssClass="GridCss"></CellStyle>
                                                                        </dx:GridViewDataTextColumn> 
                                                                        <dx:GridViewDataTextColumn Caption="RedirectTo" FieldName="redirectto" 
                                                                               Visible="False" VisibleIndex="10">
                                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                                            <CellStyle CssClass="GridCss1"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Number of DBs" FieldName="DatabaseCount" 
                                                                               Width='150px' Visible="True" VisibleIndex="1">
                                                                           <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader2" />
                                                                            <CellStyle CssClass="GridCss2"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Total Mail Boxes" VisibleIndex="2" 
                                                                               FieldName="MailBoxCount">
                                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader2" /><CellStyle CssClass="GridCss2"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Total Size" VisibleIndex="3" FieldName="TotalSize">
                                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader2" /><CellStyle CssClass="GridCss2"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>  
                                                                        <dx:GridViewDataTextColumn Caption="Services" VisibleIndex="4" FieldName="services">
                                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        </Columns>

                                                                        <SettingsBehavior AllowDragDrop="False" AutoExpandAllGroups="True"  AllowFocusedRow="true" 
                                                                            AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True" />
                                                                        <SettingsPager PageSize="10" SEOFriendly="Enabled" >
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
                                                                        <Styles CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                                                                            CssPostfix="Office2010Silver">
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
                                        </TabPages>
                                    </dx:ASPxPageControl>
                                </td>
                            </tr>
                            </table>
</asp:Content>
