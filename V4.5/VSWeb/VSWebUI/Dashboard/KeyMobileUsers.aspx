<%@ Page Title="VitalSigns Plus - Key User Devices" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="KeyMobileUsers.aspx.cs" Inherits="VSWebUI.Dashboard.KeyMobileUsers" %>
<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>

<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<%@ Register assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%">
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <img alt="" src="../images/icons/group.png" />
                        </td>
                        <td>
                            <div class="header" id="servernamelbldisp" runat="server">Key User Devices</div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div id="infoDiv" class="info">The value of the Minutes Since Last Sync column is set to display a maximum of 240 minutes. Thus, if the value in the column is 240, the actual number of minutes since the last sync is at least that or more.</div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <dx:ASPxGridView ID="KeyMobileDevicesGrid" runat="server" 
                    AutoGenerateColumns="False" EnableTheming="True" Theme="Office2003Blue" 
                    Width="100%" KeyFieldName="DeviceID" 
                    onhtmldatacellprepared="KeyMobileDevicesGrid_HtmlDataCellPrepared">
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="User Name" FieldName="UserName" 
                            VisibleIndex="0">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Device Name" FieldName="DeviceName" 
                            VisibleIndex="1">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Last Sync Time" FieldName="LastSyncTime" 
                            VisibleIndex="4">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Minutes Since Last Sync" VisibleIndex="3">
                            <DataItemTemplate>
                                <dxchartsui:WebChartControl ID="SyncWebChart" runat="server" 
                    BackColor="Transparent" CrosshairEnabled="True" Height="28px" 
                    onload="SyncWebChart_Load" PaletteName="Oriel" Width="500px" 
                    SideBySideBarDistanceFixed="0">
                    <borderoptions visibility="False" />
                    <diagramserializable>
                        <cc1:XYDiagram Rotated="True">
                            <axisx visibility="False" visibleinpanesserializable="-1">
                            </axisx>
                            <axisy visibility="False" visibleinpanesserializable="-1">
                                <tickmarks minorvisible="False" visible="False" />
<Tickmarks Visible="False" MinorVisible="False"></Tickmarks>

                                <label textpattern="{V}">
                                </label>
                                <gridlines visible="False">
                                </gridlines>
                            </axisy>
                            <margins bottom="0" left="0" right="0" top="0" />

<Margins Left="0" Top="0" Right="0" Bottom="0"></Margins>

                            <defaultpane backcolor="Transparent" bordervisible="False">
                            </defaultpane>
                        </cc1:XYDiagram>
                    </diagramserializable>
                    <legend visibility="False"></legend>
                    <seriesserializable>
                        <cc1:Series ArgumentScaleType="Qualitative" LabelsVisibility="True" 
                            Name="MinSinceSync">
                            <viewserializable>
                                <cc1:SideBySideBarSeriesView BarWidth="0.6">
                                </cc1:SideBySideBarSeriesView>
                            </viewserializable>
                        </cc1:Series>
                    </seriesserializable>
                                </dxchartsui:WebChartControl>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Status" FieldName="Status" VisibleIndex="2">
                            <HeaderStyle CssClass="GridCssHeader1" />
                            <CellStyle CssClass="GridCss1">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="ID" FieldName="DeviceID" Visible="False" 
                            VisibleIndex="5">
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
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="timer1" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>