<%@ Page Title="VitalSigns Plus - CloudHealthPage" Language="C#" MasterPageFile="~/DashboardSite.Master"
    AutoEventWireup="true" CodeBehind="CloudHealthPage.aspx.cs" Inherits="VSWebUI.Dashboard.CloudHealthPage" %>
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
                            <img alt="" src="../images/icons/network-cloud-icon.png" />
                        </td>
                        <td>
                            <div class="header" id="servernamelbldisp" runat="server">
                                Cloud Server Health</div>
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
                <dxchartsui:WebChartControl ID="StatusWebChart" runat="server" AppearanceNameSerializable="Pastel Kit"
                                            CrosshairEnabled="True" Height="200px" OnCustomDrawSeriesPoint="StatusWebChart_CustomDrawSeriesPoint"
                                             BackColor="White" PaletteName="Module" 
                    Width="400px">
                                            <borderoptions visible="False" />
                                            <borderoptions visible="False" visibility="True"></borderoptions>
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
        </tr>
        <tr>
            <td>
                <dx:ASPxPageControl ID="DominoServerHealthGrid" runat="server" ActiveTabIndex="0"  
                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                    TabSpacing="0px" EnableHierarchyRecreation="False" Width="100%">
                    <TabPages>
                        <dx:TabPage Text="Overall Status">
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl1" runat="server">
                                    <asp:UpdatePanel ID="DominoServerListUpdatePanel" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <dx:ASPxGridView ID="cloudHealthGridView" runat="server" 
                                                            AutoGenerateColumns="False" Width="100%"
                                                            CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                                                            Cursor="pointer" KeyFieldName="ServerName" SummaryText="m" Theme="Office2003Blue"
                                                            OnHtmlDataCellPrepared="cloudHealthGridView_HtmlDataCellPrepared" 
                                                            OnPageSizeChanged="cloudHealthGridView_PageSizeChanged">
                                                            <Columns>
                                                                <dx:GridViewDataTextColumn Caption="Server Name" FieldName="ServerName"
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
                                                                <dx:GridViewDataTextColumn Caption="Status" FieldName="Status" VisibleIndex="2">
                                                                    <HeaderStyle CssClass="GridCssHeader1" />
                                                                    <CellStyle CssClass="GridCss1">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="CPU" FieldName="CPU" Width="50px"
                                                                    VisibleIndex="3" Visible="false">
                                                                    <EditCellStyle CssClass="GridCss2">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader2" />
                                                                    <CellStyle CssClass="GridCss2" HorizontalAlign="Right">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Memory" FieldName="Memory" Width="50px"
                                                                    VisibleIndex="4" Visible="false">
                                                                    <EditCellStyle CssClass="GridCss2">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader2" />
                                                                    <CellStyle CssClass="GridCss2" HorizontalAlign="Right">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Response Time" FieldName="ResponseTime"
                                                                    VisibleIndex="5">
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
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>                    
                        <dx:TabPage Text="Issues">
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl2" runat="server">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <dx:ASPxGridView ID="cloudIssuesGrid" runat="server" AutoGenerateColumns="False"
                                                            CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                                                            Cursor="pointer" KeyFieldName="ServerName" 
                                                            OnHtmlDataCellPrepared="cloudIssuesGrid_HtmlDataCellPrepared" OnPageSizeChanged="cloudIssuesGrid_PageSizeChanged"
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
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                    </TabPages>
                </dx:ASPxPageControl>
            </td>
        </tr>
        </table>
</asp:Content>
