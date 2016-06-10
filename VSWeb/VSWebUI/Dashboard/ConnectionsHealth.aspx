<%@ Page Language="C#" Title="VitalSigns Plus - IBM Connections Health" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="ConnectionsHealth.aspx.cs" Inherits="VSWebUI.Dashboard.ConnectionsHealth" %>

<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>



<%@ Register assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts.Web" tagprefix="dxchartsui" %>
<%@ Register assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>

<%@ Register assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTreeList" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
	<script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
    <link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
    <script type="text/javascript">
        function Resized() {
            var callbackState = document.getElementById('ContentPlaceHolder1_callbackState').value;

            if (callbackState == 0)
                DoCallback();
        }
        function DoCallback() {
        document.getElementById('ContentPlaceHolder1_chartWidth').value = Math.round(document.body.offsetWidth/2) - 20;
//        cNumberVisitorsWebChart.PerformCallback();
//        cNumberVisitsWebChart.PerformCallback();
//        cTop10ContentWebChart.PerformCallback();
//        cMostActiveAppsWebChart.PerformCallback();
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input id="chartWidth" type="hidden" runat="server" value="400" />
<input id="callbackState" type="hidden" runat="server" value="0" />
    <table width="100%">
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                        <img alt="" src="../images/connections.png" height="20px" width="20px" />
                    </td>
                    <td>
                        <div class="header" id="lblTitle" runat="server">IBM Connections Health</div>
                    </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxGridView ID="ConnectionsGridView" runat="server" 
                    AutoGenerateColumns="False" EnableTheming="True" 
                    onhtmldatacellprepared="ConnectionsGridView_HtmlDataCellPrepared" 
                    Theme="Office2003Blue" Width="100%" EnableCallBacks="False" 
                    ondatabound="ConnectionsGridView_DataBound">
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" Visible="False" 
                            VisibleIndex="0">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Server Name" 
                            FieldName="ServerName" VisibleIndex="1" Name="ServerName">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Status" 
                            FieldName="Status" VisibleIndex="2" Name="Status">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataDateColumn Caption="Last Update" FieldName="LastUpdate" 
                            >
                        </dx:GridViewDataDateColumn>
                    </Columns>
                    <SettingsBehavior AllowFocusedRow="True" 
                        ProcessSelectionChangedOnServer="True" AllowSelectByRowClick="True" 
                        AllowSelectSingleRowOnly="True" ColumnResizeMode="NextColumn" />
                    <SettingsPager AlwaysShowPager="True" SEOFriendly="Enabled">
                        <PageSizeItemSettings Visible="True">
                        </PageSizeItemSettings>
                    </SettingsPager>
                    <Styles>
                        <Header CssClass="GridCssHeader" Wrap="False">
                        </Header>
                        <AlternatingRow CssClass="GridAltRow">
                        </AlternatingRow>
                        <Cell CssClass="GridCss">
                        </Cell>
                    </Styles>
                </dx:ASPxGridView>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
                    EnableHierarchyRecreation="False" Theme="Glass" Width="100%">
                    <TabPages>
                        <dx:TabPage Text="Overview">
                            <ContentCollection>
                                <dx:ContentControl runat="server">
                                    <table>
                                        <tr>
                                            <td valign="top">
                                                <dxchartsui:WebChartControl ID="UsersDailyWebChart" runat="server" 
                                        CrosshairEnabled="True" Height="200px" PaletteName="Module" Width="400px">
                                        <diagramserializable>
                                            <cc1:XYDiagram>
                                                <axisx title-text="Date" visibleinpanesserializable="-1">
                                                    <label>
                                                        <resolveoverlappingoptions allowrotate="False" />
<ResolveOverlappingOptions AllowRotate="False"></ResolveOverlappingOptions>
                                                    </label>
                                                </axisx>
                                                <axisy title-text="Number" visibleinpanesserializable="-1">
                                                    <label textpattern="{V}">
                                                        <resolveoverlappingoptions allowrotate="False" />
<ResolveOverlappingOptions AllowRotate="False"></ResolveOverlappingOptions>
                                                    </label>
                                                </axisy>
                                            </cc1:XYDiagram>
                                        </diagramserializable>
                                        <legend alignmenthorizontal="Center" alignmentvertical="BottomOutside" 
                                            maxverticalpercentage="30" visibility="True" MarkerSize="13, 16"></legend>
                                        <seriesserializable>
                                            <cc1:Series ArgumentScaleType="DateTime" Name="Series 1">
                                                <viewserializable>
                                                    <cc1:LineSeriesView MarkerVisibility="True">
                                                        <linemarkeroptions size="7">
                                                        </linemarkeroptions>
                                                    </cc1:LineSeriesView>
                                                </viewserializable>
                                            </cc1:Series>
                                        </seriesserializable>
                                        <seriestemplate argumentscaletype="DateTime">
                                            <viewserializable>
                                                <cc1:LineSeriesView>
                                                </cc1:LineSeriesView>
                                            </viewserializable>
                                        </seriestemplate>
                                        <titles>
                                            <cc1:ChartTitle Font="Tahoma, 12pt" Text="Daily Activities" />
                                        </titles>
                                    </dxchartsui:WebChartControl>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td valign="top">
                                                <dxchartsui:WebChartControl ID="Top5TagsWebChart" runat="server" 
                                                    CrosshairEnabled="True" Height="200px" PaletteName="Module" Width="400px">
                                                    <diagramserializable>
                                                        <cc1:XYDiagram Rotated="True">
                                                            <axisx visibleinpanesserializable="-1" reverse="True">
                                                                <tickmarks minorvisible="False" />
<Tickmarks MinorVisible="False"></Tickmarks>
                                                            </axisx>
                                                            <axisy visibleinpanesserializable="-1">
                                                            </axisy>
                                                        </cc1:XYDiagram>
                                                    </diagramserializable>
                                                    <legend alignmenthorizontal="Center" alignmentvertical="BottomOutside" 
                                                        maxverticalpercentage="30" Visibility="False"></legend>
                                                    <seriesserializable>
                                                        <cc1:Series Name="Series 1">
                                                        </cc1:Series>
                                                    </seriesserializable>
                                                    <titles>
                                                        <cc1:ChartTitle Font="Tahoma, 12pt" Text="Top 5 Tags" />
                                                    </titles>
                                                </dxchartsui:WebChartControl>
                                            </td>
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                            <td valign="top">
                                                <dx:ASPxGridView ID="DailyGridView" runat="server" 
                                                    OnDataBound="DailyGridView_DataBound" Theme="Office2003Blue">
                                                    <Styles>
                                                        <Header CssClass="GridCssHeader">
                                                        </Header>
                                                        <AlternatingRow CssClass="GridAltRow">
                                                        </AlternatingRow>
                                                        <Cell CssClass="GridCss">
                                                        </Cell>
                                                    </Styles>
                                                </dx:ASPxGridView>
                                            </td>
                                        </tr>
                                    </table>
                                    
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Communities">
                            <ContentCollection>
                                <dx:ContentControl runat="server">
                                    <table>
                                        <tr>
                                            <td valign="top">
                                                <dxchartsui:WebChartControl ID="CommunitiesByTypeChart" runat="server" 
                                                    PaletteName="Module" Width="400px">
                                                    <diagramserializable>
                                                        <cc1:SimpleDiagram EqualPieSize="False">
                                                        </cc1:SimpleDiagram>
                                                    </diagramserializable>
                                                    <legend alignmenthorizontal="Center" alignmentvertical="BottomOutside" 
                                                        direction="LeftToRight" equallyspaceditems="False" maxverticalpercentage="30">
                                                        <border visibility="False" />
<Border Visibility="False"></Border>
                                                    </legend>
                                                    <seriesserializable>
                                                        <cc1:Series Name="Series 1">
                                                            <viewserializable>
                                                                <cc1:PieSeriesView>
                                                                </cc1:PieSeriesView>
                                                            </viewserializable>
                                                            <labelserializable>
                                                                <cc1:PieSeriesLabel TextPattern="{A}: {VP}">
                                                                </cc1:PieSeriesLabel>
                                                            </labelserializable>
                                                        </cc1:Series>
                                                    </seriesserializable>
                                                    <seriestemplate>
                                                        <viewserializable>
                                                            <cc1:PieSeriesView>
                                                            </cc1:PieSeriesView>
                                                        </viewserializable>
                                                    </seriestemplate>
                                                    <titles>
                                                        <cc1:ChartTitle Font="Tahoma, 12pt" Text="Communities by Type" 
                                                            WordWrap="True" />
                                                    </titles>
                                                </dxchartsui:WebChartControl>
                                            </td>
                                            <td valign="top">
                                                <dxchartsui:WebChartControl ID="Top5CommunitiesChart" runat="server" 
                                                    PaletteName="Module" Width="400px">
                                                    <diagramserializable>
                                                        <cc1:XYDiagram Rotated="True">
                                                            <axisx reverse="True" visibleinpanesserializable="-1">
                                                            <tickmarks minorvisible="False" /></axisx>
                                                            <axisy visibleinpanesserializable="-1">
                                                            <tickmarks minorvisible="False" /></axisy>
                                                        </cc1:XYDiagram>
                                                    </diagramserializable>
                                                    <seriesserializable>
                                                        <cc1:Series Name="Series 1">
                                                            <viewserializable>
                                                                <cc1:StackedBarSeriesView>
                                                                </cc1:StackedBarSeriesView>
                                                            </viewserializable>
                                                        </cc1:Series>
                                                    </seriesserializable>
                                                    <seriestemplate>
                                                        <viewserializable>
                                                            <cc1:StackedBarSeriesView>
                                                            </cc1:StackedBarSeriesView>
                                                        </viewserializable>
                                                    </seriestemplate>
                                                    <titles>
                                                        <cc1:ChartTitle Font="Tahoma, 12pt" Text="Top 5 Most Active Communities" />
                                                    </titles>
                                                </dxchartsui:WebChartControl>
                                            </td>
                                            <td valign="top">
                                                <dxchartsui:WebChartControl ID="MostActiveCommunityChart" runat="server" 
                                                    CrosshairEnabled="True" Height="200px" PaletteName="Module" 
                                                    Width="400px">
                                                    <diagramserializable>
                                                        <cc1:SimpleDiagram EqualPieSize="False">
                                                        </cc1:SimpleDiagram>
                                                    </diagramserializable>
                                                    <legend alignmenthorizontal="Center" alignmentvertical="BottomOutside" 
                                                        direction="LeftToRight" equallyspaceditems="False"></legend>
                                                    <seriesserializable>
                                                        <cc1:Series Name="Series 1">
                                                            <viewserializable>
                                                                <cc1:DoughnutSeriesView>
                                                                </cc1:DoughnutSeriesView>
                                                            </viewserializable>
                                                        </cc1:Series>
                                                    </seriesserializable>
                                                    <seriestemplate>
                                                        <viewserializable>
                                                            <cc1:DoughnutSeriesView>
                                                            </cc1:DoughnutSeriesView>
                                                        </viewserializable>
                                                    </seriestemplate>
                                                    <titles>
                                                        <cc1:ChartTitle Font="Tahoma, 12pt" Text="Most Active Community" />
                                                    </titles>
                                                </dxchartsui:WebChartControl>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" valign="top">
                                                <table>
                                                    <tr>
                                                        <td colspan="2">
                                                            <div id="infoDiv" class="info">To compare users' community memberships, select two distinct users below and click Compare.</div>
                                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <div id="errorDiv" class="alert alert-danger" style="display:none" runat="server"></div>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="CompareUsersButton" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top">
                                                            <table>
                                                                <tr>
                                                        <td>
                                                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="User 1:" CssClass="lblsmallFont">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                    </tr>
                                                                <tr>
                                                        <td>
                                                            <dx:ASPxComboBox ID="User1ComboBox" ClientInstanceName="User1ComboBox" runat="server" ValueType="System.String">
                                                            </dx:ASPxComboBox>
                                                        </td>
                                                    </tr>
                                                                <tr>
                                                        <td>
                                                            <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="User 2:" CssClass="lblsmallFont">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                    </tr>
                                                                <tr>
                                                        <td>
                                                            <dx:ASPxComboBox ID="User2ComboBox" ClientInstanceName="User2ComboBox" runat="server" ValueType="System.String">
                                                            </dx:ASPxComboBox>
                                                        </td>
                                                    </tr>
                                                                <tr>
                                                        <td>
                                                            <dx:ASPxButton ID="CompareUsersButton" runat="server" Text="Compare" 
                                                                CssClass="sysButton" OnClick="CompareUsersButton_Click">
                                                            </dx:ASPxButton>
                                                        </td>
                                                    </tr>
                                                            </table>
                                                        </td>
                                                        <td valign="top">
                                                           <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <dx:ASPxGridView ID="CompareUsersGrid" runat="server" 
                                                                        AutoGenerateColumns="False" ClientVisible="False" EnableTheming="True" 
                                                                        Theme="Office2003Blue">
                                                                        <Columns>
                                                                            <dx:GridViewDataTextColumn Caption="Category" FieldName="Category" 
                                                                                ShowInCustomizationForm="True" VisibleIndex="0">
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn Caption="Community Name" FieldName="Name" 
                                                                                ShowInCustomizationForm="True" VisibleIndex="1">
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" 
                                                                                ShowInCustomizationForm="True" Visible="False" VisibleIndex="2">
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn Caption="ParentID" FieldName="ParentID" 
                                                                                ShowInCustomizationForm="True" Visible="False" VisibleIndex="3">
                                                                            </dx:GridViewDataTextColumn>
                                                                        </Columns>
                                                                        <SettingsBehavior AutoExpandAllGroups="True" />
                                                                        <Settings GroupFormat="{1}" ShowColumnHeaders="False" />
                                                                        <Styles>
                                                                            <AlternatingRow CssClass="GridAltRow">
                                                                            </AlternatingRow>
                                                                            <Header CssClass="GridCssHeader">
                                                                            </Header>
                                                                            <Cell CssClass="GridCss">
                                                                            </Cell>
                                                                        </Styles>
                                                                    </dx:ASPxGridView>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="CompareUsersButton" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>   
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Profiles">
                            <ContentCollection>
                                <dx:ContentControl runat="server">
                                    <table class="navbarTbl">
                                        <tr>
                                            <td>
                                                <dxchartsui:WebChartControl ID="ManagersWebChart" runat="server" 
                                                    CrosshairEnabled="True" Height="200px" PaletteName="Module" Width="400px">
                                                    <diagramserializable>
                                                        <cc1:SimpleDiagram EqualPieSize="False">
                                                        </cc1:SimpleDiagram>
                                                    </diagramserializable>
                                                    <legend alignmenthorizontal="Center" alignmentvertical="BottomOutside" 
                                                        maxverticalpercentage="30"></legend>
                                                    <seriesserializable>
                                                        <cc1:Series Name="Series 1">
                                                            <viewserializable>
                                                                <cc1:DoughnutSeriesView>
                                                                </cc1:DoughnutSeriesView>
                                                            </viewserializable>
                                                            <labelserializable>
                                                                <cc1:DoughnutSeriesLabel TextPattern="{A}: {VP}">
                                                                </cc1:DoughnutSeriesLabel>
                                                            </labelserializable>
                                                        </cc1:Series>
                                                    </seriesserializable>
                                                    <seriestemplate>
                                                        <viewserializable>
                                                            <cc1:DoughnutSeriesView>
                                                            </cc1:DoughnutSeriesView>
                                                        </viewserializable>
                                                    </seriestemplate>
                                                    <titles>
                                                        <cc1:ChartTitle Font="Tahoma, 12pt" Text="Managers/Non Managers" />
                                                    </titles>
                                                </dxchartsui:WebChartControl>
                                            </td>
                                            <td>
                                                <dxchartsui:WebChartControl ID="PicturesWebChart" runat="server" 
                                                    CrosshairEnabled="True" Height="200px" PaletteName="Module" Width="400px">
                                                    <diagramserializable>
                                                        <cc1:SimpleDiagram EqualPieSize="False">
                                                        </cc1:SimpleDiagram>
                                                    </diagramserializable>
                                                    <legend alignmenthorizontal="Center" alignmentvertical="BottomOutside" 
                                                        maxverticalpercentage="30"></legend>
                                                    <seriesserializable>
                                                        <cc1:Series Name="Series 1">
                                                            <viewserializable>
                                                                <cc1:DoughnutSeriesView>
                                                                </cc1:DoughnutSeriesView>
                                                            </viewserializable>
                                                            <labelserializable>
                                                                <cc1:DoughnutSeriesLabel TextPattern="{A}: {VP}">
                                                                </cc1:DoughnutSeriesLabel>
                                                            </labelserializable>
                                                        </cc1:Series>
                                                    </seriesserializable>
                                                    <seriestemplate>
                                                        <viewserializable>
                                                            <cc1:DoughnutSeriesView>
                                                            </cc1:DoughnutSeriesView>
                                                        </viewserializable>
                                                    </seriestemplate>
                                                    <titles>
                                                        <cc1:ChartTitle Font="Tahoma, 12pt" Text="Picture/No Picture" />
                                                    </titles>
                                                </dxchartsui:WebChartControl>
                                            </td>
                                            <td>
                                                <dxchartsui:WebChartControl ID="JobHierarchyWebChart" runat="server" 
                                                    CrosshairEnabled="True" Height="200px" PaletteName="Module" Width="400px">
                                                    <diagramserializable>
                                                        <cc1:SimpleDiagram EqualPieSize="False">
                                                        </cc1:SimpleDiagram>
                                                    </diagramserializable>
                                                    <legend alignmenthorizontal="Center" alignmentvertical="BottomOutside" 
                                                        maxverticalpercentage="30"></legend>
                                                    <seriesserializable>
                                                        <cc1:Series Name="Series 1">
                                                            <viewserializable>
                                                                <cc1:DoughnutSeriesView>
                                                                </cc1:DoughnutSeriesView>
                                                            </viewserializable>
                                                            <labelserializable>
                                                                <cc1:DoughnutSeriesLabel TextPattern="{A}: {VP}">
                                                                </cc1:DoughnutSeriesLabel>
                                                            </labelserializable>
                                                        </cc1:Series>
                                                    </seriesserializable>
                                                    <seriestemplate>
                                                        <viewserializable>
                                                            <cc1:DoughnutSeriesView>
                                                            </cc1:DoughnutSeriesView>
                                                        </viewserializable>
                                                    </seriestemplate>
                                                    <titles>
                                                        <cc1:ChartTitle Font="Tahoma, 12pt" Text="Job Hierarchy/No Job Hierarchy" />
                                                    </titles>
                                                </dxchartsui:WebChartControl>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    <%--    1/06/2016 sowmya added for VSPLUS-2934--%>
                                        <tr>
                                        <td colspan="3">
                                            <dx:ASPxGridView ID="CommunitiesGrid" runat="server" AutoGenerateColumns="False"
                                                CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                                                CssPostfix="Office2010Silver" Cursor="pointer" KeyFieldName="ID" 
                                                Theme="Office2003Blue" OnPageSizeChanged="CommunitiesGrid_PageSizeChanged">
                                                <Columns>
                                                    <dx:GridViewDataTextColumn Caption="Users" VisibleIndex="1" FieldName="Users">
                                                        <Settings AutoFilterCondition="Contains" />
                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                        <Settings AllowAutoFilter="False" AllowDragDrop="True" />
                                                        <CellStyle CssClass="GridCss1" HorizontalAlign="Center">
                                                        </CellStyle>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Owners" VisibleIndex="2" FieldName="Owners">
                                                        <Settings AutoFilterCondition="Contains" AllowDragDrop="True" />
                                                        <EditCellStyle CssClass="GridCss">
                                                        </EditCellStyle>
                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                        </EditFormCaptionStyle>
                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                        <CellStyle CssClass="GridCss">
                                                        </CellStyle>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Name" FieldName="Name" VisibleIndex="4" Visible="false">
                                                    </dx:GridViewDataTextColumn>
                                                </Columns>
                                              
                                                <Templates>
                                                    <GroupRowContent>
                                                        <%# Container.GroupText %>
                                                    </GroupRowContent>
                                                </Templates>
                                                <Settings ShowFilterRow="True" />
                                                <SettingsBehavior AllowDragDrop="False" AutoExpandAllGroups="True"  />
                                                <SettingsPager>
                                                    <PageSizeItemSettings Visible="true" />
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
                                                    <GroupRow Font-Bold="true" />
                                                </Styles>
                                            </dx:ASPxGridView>
                                        </td>
                                        </tr>
                                    </table>
                                </dx:ContentControl>

                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Activities">
                            <ContentCollection>
                                <dx:ContentControl runat="server">
                                    <table>
                                        <tr>
                                            <td valign="top">
                                                <dxchartsui:WebChartControl ID="ActivitiesWebChart" runat="server" CrosshairEnabled="True" 
                                        Height="200px" PaletteName="Module" Width="400px">
                                        <diagramserializable>
                                            <cc1:XYDiagram>
                                                <axisx visibleinpanesserializable="-1">
                                                    <label textpattern="{A:d}">
                                                        <resolveoverlappingoptions allowrotate="False" />
<ResolveOverlappingOptions AllowRotate="False"></ResolveOverlappingOptions>
                                                    </label>
                                                </axisx>
                                                <axisy visibleinpanesserializable="-1">
                                                    <label textpattern="{V}">
                                                        <resolveoverlappingoptions allowrotate="False" />
<ResolveOverlappingOptions AllowRotate="False"></ResolveOverlappingOptions>
                                                    </label>
                                                </axisy>
                                            </cc1:XYDiagram>
                                        </diagramserializable>
                                        <legend visibility="True" AlignmentHorizontal="Center" 
                                            AlignmentVertical="BottomOutside" MarkerSize="13, 16" 
                                            MaxVerticalPercentage="30"></legend>
                                        <seriesserializable>
                                            <cc1:Series ArgumentScaleType="DateTime" Name="Series 1">
                                                <viewserializable>
                                                    <cc1:LineSeriesView MarkerVisibility="True">
                                                        <linemarkeroptions size="7">
                                                        </linemarkeroptions>
                                                    </cc1:LineSeriesView>
                                                </viewserializable>
                                            </cc1:Series>
                                        </seriesserializable>
                                        <seriestemplate>
                                            <viewserializable>
                                                <cc1:LineSeriesView>
                                                </cc1:LineSeriesView>
                                            </viewserializable>
                                        </seriestemplate>
                                        <titles>
                                            <cc1:ChartTitle Text="Activities" Font="Tahoma, 12pt" />
                                        </titles>
                                    </dxchartsui:WebChartControl>
                                            </td>
                                            <td valign="top">
                                                <dxchartsui:WebChartControl ID="Top5ActivitiesChart" runat="server" 
                                                    CrosshairEnabled="True" Height="200px" PaletteName="Module" Width="400px">
                                                    <diagramserializable>
                                                        <cc1:XYDiagram Rotated="True">
                                                            <axisx visibleinpanesserializable="-1" reverse="True">
                                                                <tickmarks minorvisible="False" />
<Tickmarks MinorVisible="False"></Tickmarks>

                                                                <label>
                                                                    <resolveoverlappingoptions allowrotate="False" allowstagger="False" />
<ResolveOverlappingOptions AllowStagger="False" AllowRotate="False"></ResolveOverlappingOptions>
                                                                </label>
                                                            </axisx>
                                                            <axisy visibleinpanesserializable="-1">
                                                                <tickmarks minorvisible="False" />
<Tickmarks MinorVisible="False"></Tickmarks>

                                                                <label>
                                                                    <resolveoverlappingoptions allowrotate="False" allowstagger="False" />
<ResolveOverlappingOptions AllowStagger="False" AllowRotate="False"></ResolveOverlappingOptions>
                                                                </label>
                                                            </axisy>
                                                        </cc1:XYDiagram>
                                                    </diagramserializable>
                                                    <legend visibility="False"></legend>
                                                    <seriesserializable>
                                                        <cc1:Series Name="Series 1">
                                                        </cc1:Series>
                                                    </seriesserializable>
                                                    <titles>
                                                        <cc1:ChartTitle Font="Tahoma, 12pt" Text="Top 5 Communities for Activities" 
                                                            WordWrap="True" />
                                                    </titles>
                                                </dxchartsui:WebChartControl>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;</td>
                                            <td valign="top">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <div style="background-image: url('../images/icons/users.png'); background-repeat: no-repeat; background-color: white; width: 100px; height: 70px; padding-left: 2px; 
                                                    padding-right: 2px; border: 1px solid; border-color: #D3D3D3; border-radius: 5px; text-align: right; ">
                                            <dx:ASPxLabel ID="UsersActivityLabel" runat="server" Text="0" Font-Size="Large" Font-Bold="true" ForeColor="#666666">
											</dx:ASPxLabel>
                                            <br />
                                            <br />
                                            <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Users Following Activity" Font-Size="Small" Font-Bold="false" ForeColor="#666666">
											</dx:ASPxLabel>
                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="background-image: url('../images/icons/group_key.png'); background-repeat: no-repeat; background-color: white; width: 100px; height: 70px; padding-left: 2px; 
                                                padding-right: 2px; border: 1px solid; border-color: #D3D3D3; border-radius: 5px; text-align: right; ">
                                            <dx:ASPxLabel ID="ActivityOwnersLabel" runat="server" Text="0" Font-Size="Large" Font-Bold="true" ForeColor="#666666">
											</dx:ASPxLabel>
                                            <br />
                                            <br />
                                            <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Activity Owners" Font-Size="Small" Font-Bold="false" ForeColor="#666666">
											</dx:ASPxLabel>
                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <dx:ASPxGridView ID="ActivitiesGridView" runat="server" Theme="Office2003Blue" 
                                                    OnDataBound="ActivitiesGridView_DataBound">
                                                    <Styles>
                                                        <Header CssClass="GridCssHeader">
                                                        </Header>
                                                        <AlternatingRow CssClass="GridAltRow">
                                                        </AlternatingRow>
                                                        <Cell CssClass="GridCss">
                                                        </Cell>
                                                    </Styles>
                                                </dx:ASPxGridView>
                                            </td>
                                        </tr>
                                    </table>
                                    
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Blogs">
                            <ContentCollection>
                                <dx:ContentControl runat="server">
                                    <table>
                                        <tr>
                                            <td valign="top">
                                                <dxchartsui:WebChartControl ID="BlogsWebChart" runat="server" CrosshairEnabled="True" 
                                        Height="200px" PaletteName="Module" Width="400px">
                                        <diagramserializable>
                                            <cc1:XYDiagram>
                                                <axisx visibleinpanesserializable="-1">
                                                </axisx>
                                                <axisy visibleinpanesserializable="-1">
                                                </axisy>
                                            </cc1:XYDiagram>
                                        </diagramserializable>
                                        <legend visibility="True" AlignmentHorizontal="Center" 
                                            AlignmentVertical="BottomOutside" MaxVerticalPercentage="30" 
                                            MarkerSize="13, 16"></legend>
                                        <seriesserializable>
                                            <cc1:Series ArgumentScaleType="DateTime" LegendTextPattern="{A} - {V}" 
                                                Name="Series1">
                                                <viewserializable>
                                                    <cc1:LineSeriesView MarkerVisibility="True">
                                                        <linemarkeroptions size="7">
                                                        </linemarkeroptions>
                                                    </cc1:LineSeriesView>
                                                </viewserializable>
                                            </cc1:Series>
                                        </seriesserializable>
                                        <seriestemplate legendtextpattern="{VP:G4}" argumentscaletype="DateTime">
                                            <viewserializable>
                                                <cc1:LineSeriesView>
                                                </cc1:LineSeriesView>
                                            </viewserializable>
                                        </seriestemplate>
                                        <titles>
                                            <cc1:ChartTitle Font="Tahoma, 12pt" Text="Blogs" />
                                        </titles>
                                    </dxchartsui:WebChartControl>
                                            </td>
                                            <td valign="top">
                                                <dxchartsui:WebChartControl ID="Top5BlogsChart" runat="server" 
                                                    CrosshairEnabled="True" Height="200px" PaletteName="Module" Width="400px">
                                                    <diagramserializable>
                                                        <cc1:XYDiagram Rotated="True">
                                                            <axisx reverse="True" visibleinpanesserializable="-1">
                                                                <tickmarks minorvisible="False" />
<Tickmarks MinorVisible="False"></Tickmarks>
                                                            </axisx>
                                                            <axisy visibleinpanesserializable="-1">
                                                                <tickmarks minorvisible="False" />
<Tickmarks MinorVisible="False"></Tickmarks>
                                                            </axisy>
                                                        </cc1:XYDiagram>
                                                    </diagramserializable>
                                                    <legend visibility="False"></legend>
                                                    <seriesserializable>
                                                        <cc1:Series Name="Series 1">
                                                        </cc1:Series>
                                                    </seriesserializable>
                                                    <titles>
                                                        <cc1:ChartTitle Font="Tahoma, 12pt" Text="Top 5 Communities for Blogs" />
                                                    </titles>
                                                </dxchartsui:WebChartControl>
                                            </td>
                                            <td>
                                                &nbsp;</td>
                                            <td valign="top">
                                                <dx:ASPxGridView ID="BlogsGridView" runat="server" 
                                                    OnDataBound="BlogsGridView_DataBound" Theme="Office2003Blue">
                                                    <Styles>
                                                        <Header CssClass="GridCssHeader">
                                                        </Header>
                                                        <AlternatingRow CssClass="GridAltRow">
                                                        </AlternatingRow>
                                                        <Cell CssClass="GridCss">
                                                        </Cell>
                                                    </Styles>
                                                </dx:ASPxGridView>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Bookmarks">
                            <ContentCollection>
                                <dx:ContentControl runat="server">
                                    <table>
                                        <tr>
                                            <td valign="top">
                                                <dxchartsui:WebChartControl ID="BookmarksWebChart" runat="server" CrosshairEnabled="True" 
                                        Height="200px" PaletteName="Module" Width="400px">
                                        <diagramserializable>
                                            <cc1:XYDiagram>
                                                <axisx visibleinpanesserializable="-1">
                                                </axisx>
                                                <axisy visibleinpanesserializable="-1">
                                                    <tickmarks minorvisible="False" />
<Tickmarks MinorVisible="False"></Tickmarks>

                                                    <label>
                                                        <resolveoverlappingoptions allowrotate="False" allowstagger="False" />
<ResolveOverlappingOptions AllowStagger="False" AllowRotate="False"></ResolveOverlappingOptions>
                                                    </label>
                                                </axisy>
                                            </cc1:XYDiagram>
                                        </diagramserializable>
                                        <legend visibility="True" AlignmentHorizontal="Center" 
                                            AlignmentVertical="BottomOutside" MarkerSize="13, 16" 
                                            MaxVerticalPercentage="30"></legend>
                                        <seriesserializable>
                                            <cc1:Series ArgumentScaleType="DateTime" LegendTextPattern="{A} - {V}" 
                                                Name="Series1">
                                                <viewserializable>
                                                    <cc1:LineSeriesView MarkerVisibility="True">
                                                        <linemarkeroptions size="7">
                                                        </linemarkeroptions>
                                                    </cc1:LineSeriesView>
                                                </viewserializable>
                                            </cc1:Series>
                                        </seriesserializable>
                                        <seriestemplate argumentscaletype="DateTime">
                                            <viewserializable>
                                                <cc1:LineSeriesView>
                                                </cc1:LineSeriesView>
                                            </viewserializable>
                                        </seriestemplate>
                                        <titles>
                                            <cc1:ChartTitle Text="Bookmarks" Font="Tahoma, 12pt" />
                                        </titles>
                                    </dxchartsui:WebChartControl>
                                            </td>
                                            <td valign="top">
                                                <dxchartsui:WebChartControl ID="Top5BookmarksChart" runat="server" 
                                                    CrosshairEnabled="True" Height="200px" PaletteName="Module" Width="400px">
                                                    <diagramserializable>
                                                        <cc1:XYDiagram Rotated="True">
                                                            <axisx visibleinpanesserializable="-1" reverse="True">
                                                                <tickmarks minorvisible="False" />
<Tickmarks MinorVisible="False"></Tickmarks>

                                                                <label textpattern="{A}">
                                                                    <resolveoverlappingoptions allowrotate="False" />
<ResolveOverlappingOptions AllowRotate="False"></ResolveOverlappingOptions>
                                                                </label>
                                                            </axisx>
                                                            <axisy visibleinpanesserializable="-1">
                                                                <tickmarks minorvisible="False" />
<Tickmarks MinorVisible="False"></Tickmarks>

                                                                <label textpattern="{V:#,#}">
                                                                    <resolveoverlappingoptions allowrotate="False" allowstagger="False" />
<ResolveOverlappingOptions AllowStagger="False" AllowRotate="False"></ResolveOverlappingOptions>
                                                                </label>
                                                            </axisy>
                                                        </cc1:XYDiagram>
                                                    </diagramserializable>
                                                    <legend visibility="False"></legend>
                                                    <seriesserializable>
                                                        <cc1:Series Name="Series 1">
                                                        </cc1:Series>
                                                    </seriesserializable>
                                                    <titles>
                                                        <cc1:ChartTitle Font="Tahoma, 12pt" Text="Top 5 Communities for Bookmarks" />
                                                    </titles>
                                                </dxchartsui:WebChartControl>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;</td>
                                            <td valign="top">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <div style="background-image: url('../images/icons/bookmark1.png'); background-repeat: no-repeat; background-color: white; width: 100px; height: 70px; padding-left: 2px; padding-right: 2px; border: 1px solid; border-color: #D3D3D3; border-radius: 5px; text-align: right;">
                                                                <dx:ASPxLabel ID="BookmarksLabel" runat="server" Font-Bold="True" 
                                                                    Font-Size="Large" ForeColor="#666666" Text="0">
                                                                </dx:ASPxLabel>
                                                                <br />
                                                                <br />
                                                                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Bold="False" 
                                                                    Font-Size="Small" ForeColor="#666666" Text="Bookmarks">
                                                                </dx:ASPxLabel>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="background-image: url('../images/icons/link1.png'); background-repeat: no-repeat; background-color: white; width: 100px; height: 70px; padding-left: 2px; padding-right: 2px; border: 1px solid; border-color: #D3D3D3; border-radius: 5px; text-align: right;">
                                                                <dx:ASPxLabel ID="BookmarkURLsLabel" runat="server" Font-Bold="True" 
                                                                    Font-Size="Large" ForeColor="#666666" Text="0">
                                                                </dx:ASPxLabel>
                                                                <br />
                                                                <br />
                                                                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Bold="False" 
                                                                    Font-Size="Small" ForeColor="#666666" Text="Distinct Bookmark URLs">
                                                                </dx:ASPxLabel>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <dx:ASPxGridView ID="BookmarksGridView" runat="server" 
                                                    OnDataBound="BookmarksGridView_DataBound" Theme="Office2003Blue">
                                                    <styles>
                                                        <alternatingrow cssclass="GridAltRow">
                                                        </alternatingrow>
                                                        <header cssclass="GridCssHeader">
                                                        </header>
                                                        <cell cssclass="GridCss">
                                                        </cell>
                                                    </styles>
                                                </dx:ASPxGridView>
                                            </td>
                                        </tr>
                                    </table>
                                    
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Files">
                            <ContentCollection>
                                <dx:ContentControl runat="server">
                                    <table>
                                        <tr>
                                            <td valign="top">
                                                <dxchartsui:WebChartControl ID="FilesWebChart" runat="server" 
                                        CrosshairEnabled="True" Height="200px" PaletteName="Module" Width="400px">
                                        <diagramserializable>
                                            <cc1:XYDiagram>
                                                <axisx visibleinpanesserializable="-1">
                                                    <tickmarks minorvisible="False" />
<Tickmarks MinorVisible="False"></Tickmarks>

                                                    <label>
                                                        <resolveoverlappingoptions allowrotate="False" />
<ResolveOverlappingOptions AllowRotate="False"></ResolveOverlappingOptions>
                                                    </label>
                                                </axisx>
                                                <axisy visibleinpanesserializable="-1">
                                                </axisy>
                                            </cc1:XYDiagram>
                                        </diagramserializable>
                                        <legend alignmenthorizontal="Center" alignmentvertical="BottomOutside" 
                                            markersize="13, 16" maxverticalpercentage="30" visibility="True"></legend>
                                        <seriesserializable>
                                            <cc1:Series ArgumentScaleType="DateTime" Name="Series 1">
                                                <viewserializable>
                                                    <cc1:LineSeriesView MarkerVisibility="True">
                                                        <linemarkeroptions size="7">
                                                        </linemarkeroptions>
                                                    </cc1:LineSeriesView>
                                                </viewserializable>
                                            </cc1:Series>
                                        </seriesserializable>
                                        <seriestemplate argumentscaletype="DateTime">
                                            <viewserializable>
                                                <cc1:LineSeriesView>
                                                </cc1:LineSeriesView>
                                            </viewserializable>
                                        </seriestemplate>
                                        <titles>
                                            <cc1:ChartTitle Font="Tahoma, 12pt" Text="Files" />
                                        </titles>
                                    </dxchartsui:WebChartControl>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td valign="top">
                                                <dx:ASPxGridView ID="FilesGridView" runat="server" 
                                                    OnDataBound="FilesGridView_DataBound" Theme="Office2003Blue">
                                                    <Styles>
                                                        <Header CssClass="GridCssHeader">
                                                        </Header>
                                                        <AlternatingRow CssClass="GridAltRow">
                                                        </AlternatingRow>
                                                        <Cell CssClass="GridCss">
                                                        </Cell>
                                                    </Styles>
                                                </dx:ASPxGridView>
                                            </td>
                                        </tr>
                                    </table>
                                    
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Forums">
                            <ContentCollection>
                                <dx:ContentControl runat="server">
                                    <table>
                                        <tr>
                                            <td valign="top">
                                                <dxchartsui:WebChartControl ID="ForumsWebChart" runat="server" 
                                        CrosshairEnabled="True" Height="200px" PaletteName="Module" Width="400px">
                                        <diagramserializable>
                                            <cc1:XYDiagram>
                                                <axisx visibleinpanesserializable="-1">
                                                </axisx>
                                                <axisy visibleinpanesserializable="-1">
                                                </axisy>
                                            </cc1:XYDiagram>
                                        </diagramserializable>
                                        <legend alignmenthorizontal="Center" alignmentvertical="BottomOutside" 
                                            markersize="13, 16" maxverticalpercentage="30" visibility="True"></legend>
                                        <seriesserializable>
                                            <cc1:Series ArgumentScaleType="DateTime" Name="Series 1">
                                                <viewserializable>
                                                    <cc1:LineSeriesView MarkerVisibility="True">
                                                        <linemarkeroptions size="7">
                                                        </linemarkeroptions>
                                                    </cc1:LineSeriesView>
                                                </viewserializable>
                                            </cc1:Series>
                                        </seriesserializable>
                                        <seriestemplate argumentscaletype="DateTime">
                                            <viewserializable>
                                                <cc1:LineSeriesView>
                                                </cc1:LineSeriesView>
                                            </viewserializable>
                                        </seriestemplate>
                                        <titles>
                                            <cc1:ChartTitle Font="Tahoma, 12pt" Text="Forums" />
                                        </titles>
                                    </dxchartsui:WebChartControl>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td valign="top">
                                                <dx:ASPxGridView ID="ForumsGridView" runat="server" 
                                                    OnDataBound="ForumsGridView_DataBound" Theme="Office2003Blue">
                                                    <Styles>
                                                        <Header CssClass="GridCssHeader">
                                                        </Header>
                                                        <AlternatingRow CssClass="GridAltRow">
                                                        </AlternatingRow>
                                                        <Cell CssClass="GridCss">
                                                        </Cell>
                                                    </Styles>
                                                </dx:ASPxGridView>
                                            </td>
                                        </tr>
                                    </table>
                                    
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Wikis">
                            <ContentCollection>
                                <dx:ContentControl runat="server">
                                    <table>
                                        <tr>
                                            <td valign="top">
                                                <dxchartsui:WebChartControl ID="WikisWebChart" runat="server" 
                                        CrosshairEnabled="True" Height="200px" PaletteName="Module" Width="400px">
                                        <diagramserializable>
                                            <cc1:XYDiagram>
                                                <axisx visibleinpanesserializable="-1">
                                                </axisx>
                                                <axisy visibleinpanesserializable="-1">
                                                </axisy>
                                            </cc1:XYDiagram>
                                        </diagramserializable>
                                        <legend alignmenthorizontal="Center" alignmentvertical="BottomOutside" 
                                            markersize="13, 16" maxverticalpercentage="30" visibility="True"></legend>
                                        <seriesserializable>
                                            <cc1:Series ArgumentScaleType="DateTime" Name="Series 1">
                                                <viewserializable>
                                                    <cc1:LineSeriesView MarkerVisibility="True">
                                                        <linemarkeroptions size="7">
                                                        </linemarkeroptions>
                                                    </cc1:LineSeriesView>
                                                </viewserializable>
                                            </cc1:Series>
                                        </seriesserializable>
                                        <seriestemplate argumentscaletype="DateTime">
                                            <viewserializable>
                                                <cc1:LineSeriesView>
                                                </cc1:LineSeriesView>
                                            </viewserializable>
                                        </seriestemplate>
                                        <titles>
                                            <cc1:ChartTitle Font="Tahoma, 12pt" Text="Wikis" />
                                        </titles>
                                    </dxchartsui:WebChartControl>
                                            </td>
                                            <td valign="top">
                                                <dxchartsui:WebChartControl ID="Top5WikisChart" runat="server" 
                                                    CrosshairEnabled="True" Height="200px" PaletteName="Module" Width="400px">
                                                    <diagramserializable>
                                                        <cc1:XYDiagram Rotated="True">
                                                            <axisx reverse="True" visibleinpanesserializable="-1">
                                                                <tickmarks minorvisible="False" />
<Tickmarks MinorVisible="False"></Tickmarks>
                                                            </axisx>
                                                            <axisy visibleinpanesserializable="-1">
                                                                <tickmarks minorvisible="False" />
<Tickmarks MinorVisible="False"></Tickmarks>
                                                            </axisy>
                                                        </cc1:XYDiagram>
                                                    </diagramserializable>
                                                    <legend visibility="False"></legend>
                                                    <seriesserializable>
                                                        <cc1:Series Name="Series 1">
                                                        </cc1:Series>
                                                    </seriesserializable>
                                                    <titles>
                                                        <cc1:ChartTitle Font="Tahoma, 12pt" Text="Top 5 Communities for Wikis" />
                                                    </titles>
                                                </dxchartsui:WebChartControl>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;</td>
                                            <td valign="top">
                                                <dx:ASPxGridView ID="WikisGridView" runat="server" 
                                                    OnDataBound="WikisGridView_DataBound" Theme="Office2003Blue">
                                                    <Styles>
                                                        <Header CssClass="GridCssHeader">
                                                        </Header>
                                                        <AlternatingRow CssClass="GridAltRow">
                                                        </AlternatingRow>
                                                        <Cell CssClass="GridCss">
                                                        </Cell>
                                                    </Styles>
                                                </dx:ASPxGridView>
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
