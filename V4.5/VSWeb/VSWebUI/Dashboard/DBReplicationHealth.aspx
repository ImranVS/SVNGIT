<%@ Page Language="C#" Title="VitalSigns Plus - Database Replication Health" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="DBReplicationHealth.aspx.cs" Inherits="VSWebUI.Dashboard.DBReplicationHealth" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc2" %>
<%@ Register Src="~/Controls/DateRange.ascx" TagName="DateRange" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        //window.onresize = Resized;
        function Resized() {
            //alert('hi');
            var callbackState = document.getElementById('ContentPlaceHolder1_callbackState').value;
            if (callbackState == 0)
                DoCallback();
        }

        function DoCallback() {
            //alert('hi');
            document.getElementById('ContentPlaceHolder1_chartWidth').value = document.body.offsetWidth - 95;
            //9/15/2014 NS added for VSPLUS-921
            cClusterRepWebChartControl1.PerformCallback();
            cClusterRepWebChartControl2.PerformCallback();
        }

        function ResizeChart(s, e) {
            alert(hi);
            document.getElementById('ContentPlaceHolder1_callbackState').value = 0;
            s.GetMainElement().style.width = document.getElementById('ContentPlaceHolder1__chartWidth').value + "px";
            //cSelectServerRoundPanel.style.width = document.getElementById('ContentPlaceHolder1__chartWidth').value + "px";
        }

        function ResetCallbackState() {
            window.document.form1.elements['ContentPlaceHolder1_callbackState'].value = 1;
        }
        function OnItemClick(s, e) {
            if (e.item.parent == s.GetRootItem())
                e.processOnServer = false;
        }
    </script>
    
       <%-- 10/2/2016 Sowmya Added for VSPLUS 2455--%>
    <script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
        <script type="text/javascript">
            $(document).ready(function () {
                $('.alert-success').delay(10000).fadeOut("slow", function () {
                });
            });
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input id="chartWidth" type="hidden" runat="server" value="400" />
    <input id="callbackState" type="hidden" runat="server" value="0" />
    <table width="100%">
        <tr>
            <td>
                <div class="header" id="servernamelbldisp" runat="server">Database Replication Health</div>
            </td>
            <td align="right">
                <table>
                    <tr>
                        <td align="right">
                            <dx:ASPxMenu ID="ASPxMenu1" runat="server" ShowAsToolbar="True"
                                                             OnItemClick="ASPxMenu1_ItemClick" Theme="Moderno" HorizontalAlign="Right">
                                                                <ClientSideEvents ItemClick="OnItemClick" />
                                                                <Items>
                                                                    <dx:MenuItem BeginGroup="True" Name="EditConfigItem2" Text="">
                                                                        <Items>
                                                                           
                                                                            <dx:MenuItem Name="ScanItem" Text="Scan Now">
                                                                            </dx:MenuItem>
                                                                  
                                                                        </Items>
                                                                        <Image Url="~/images/icons/Gear.png">
                                                                        </Image>
                                                                    </dx:MenuItem>
                                                                </Items>
                                                            </dx:ASPxMenu>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
                            <td>
                                <div id="clusterInfoDiv" class="info" style="width:96%">The list of databases below may contain duplicates. If you see repeating database entries, it is due to the existence of databases with the same name/path but different replica IDs on the cluster servers.</div>
                            </td>
                        </tr>
        <tr>
        <td valign="top">
            <div id="successDiv" runat="server" class="alert alert-success" style="display: none">
            </div>
            <div id="ErrorDiv" runat ="server" class =" alert alert-danger" style ="display :none">
            </div>
        </td>
    </tr>                        
   </table>
                             <%-- 17/3/2016 sowmya added for VSPLUS 2455--%>
                <table width="100%">
                   
                       <tr>
                        <td>
                            <dx:ASPxGridView ID="ClusterInfoGrid" runat="server" Theme="Office2003Blue" 
                                                    Width="100%" AutoGenerateColumns="False" EnableCallBacks="False" 
                                                    KeyFieldName="ID" 
                                OnPageSizeChanged="ClusterInfoGrid_PageSizeChanged" 
                                onhtmldatacellprepared="ClusterInfoGrid_HtmlDataCellPrepared" >
                                                    <Columns>
                                                        <dx:GridViewDataTextColumn Caption="Cluster Name" FieldName="Name" 
                                                            VisibleIndex="0">
                                                        </dx:GridViewDataTextColumn>
                                                      <dx:GridViewDataTextColumn Caption=" Failure Threshold" FieldName="First_Alert_Threshold" 
                                                           VisibleIndex="2">
                                                          <HeaderStyle CssClass="GridCssHeader2" />
                                                          <CellStyle CssClass="GridCss2">
                                                          </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Last Updated" FieldName="LastUpdate" 
                                                            VisibleIndex="3">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Next Scan" FieldName="NextScan"
                                                            VisibleIndex="4">
                                                        </dx:GridViewDataTextColumn>
                                                           <dx:GridViewDataTextColumn FieldName="ID" VisibleIndex="5"
                                                           Visible ="false">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Status" FieldName="Status" VisibleIndex="1">
                                                        </dx:GridViewDataTextColumn>
                                                    </Columns>
                                                    <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" ProcessSelectionChangedOnServer="true" 
                                                    AllowSelectSingleRowOnly="true" />
                                                    <SettingsPager AlwaysShowPager="True" PageSize="20">
                                                        <PageSizeItemSettings Visible="True">
                                                        </PageSizeItemSettings>
                                                    </SettingsPager>
                                                    <Styles>
                                                        <Header CssClass="GridCssHeader">
                                                        </Header>
                                                        <AlternatingRow CssClass="GridCssAltRow">
                                                        </AlternatingRow>
                                                        <Cell CssClass="GridCss">
                                                        </Cell>
                                                    </Styles>
                                                </dx:ASPxGridView>                    
                        </td>
                      
                      </tr>
                </table>
                        <table width="100%">
        <tr>
            <td>
            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ClusterInfoGrid" />
                                        </Triggers>
                                        <ContentTemplate>
                <dx:ASPxPageControl ID="ASPxPageControl2" runat="server" ActiveTabIndex="0" 
                    Theme="Glass" Width="100%" EnableHierarchyRecreation="False">
                    <TabPages>
                        <dx:TabPage Text="Potential Replication Problems">
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl1" runat="server">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ShowProblemsRadioButtonList" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <table width="100%">
                                            <tr>
                                                <td>
                                                    <dx:ASPxRadioButtonList ID="ShowProblemsRadioButtonList" runat="server" 
                                                        AutoPostBack="True" 
                                                        OnSelectedIndexChanged="ShowProblemsRadioButtonList_SelectedIndexChanged" 
                                                        RepeatDirection="Horizontal" SelectedIndex="0">
                                                        <Items>
                                                            <dx:ListEditItem Selected="True" Text="Show Potential Problems" Value="1" />
                                                            <dx:ListEditItem Text="Show All" Value="0" />
                                                        </Items>
                                                    </dx:ASPxRadioButtonList>
                                                    
                                                </td>
                                            </tr>
                                        <tr>
                                            <td>
                                                <dx:ASPxGridView ID="PotentialIssuesGrid" runat="server" Theme="Office2003Blue" 
                                                    Width="100%" AutoGenerateColumns="False" EnableCallBacks="False" 
                                                    KeyFieldName="DatabaseName" 
                                                    onhtmlrowprepared="PotentialIssuesGrid_HtmlRowPrepared"                                                                                                       
                                                    onpagesizechanged="PotentialIssuesGrid_PageSizeChanged" 
                                                    onhtmlrowcreated="PotentialIssuesGrid_HtmlRowPrepared" 
                                                    onhtmldatacellprepared="PotentialIssuesGrid_HtmlDataCellPrepared">
                                                    <Columns>
                                                        <dx:GridViewDataTextColumn Caption="Database File" FieldName="DatabaseName" 
                                                            VisibleIndex="0">
                                                        </dx:GridViewDataTextColumn>
														 
                                                        <dx:GridViewDataTextColumn Caption="DB Size A" FieldName="DBSizeA" 
                                                            VisibleIndex="5">
                                                            <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                                                            <CellStyle CssClass="GridCss2">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="DB Size B" FieldName="DBSizeB" 
                                                            VisibleIndex="6">
                                                            <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                                                            <CellStyle CssClass="GridCss2">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Document Count A" FieldName="DocCountA" 
                                                            VisibleIndex="2">
                                                            <HeaderStyle CssClass="GridCssHeader2" />
                                                            <CellStyle CssClass="GridCss2">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Document Count B" FieldName="DocCountB" 
                                                            VisibleIndex="3">
                                                            <HeaderStyle CssClass="GridCssHeader2" />
                                                            <CellStyle CssClass="GridCss2">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Document Count C" FieldName="DocCountC" 
                                                            VisibleIndex="4">
                                                            <HeaderStyle CssClass="GridCssHeader2" />
                                                            <CellStyle CssClass="GridCss2">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="DB Size C" FieldName="DBSizeC" 
                                                            VisibleIndex="7">
                                                            <HeaderStyle CssClass="GridCssHeader2" />
                                                            <CellStyle CssClass="GridCss2">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="color" FieldName="color" 
                                                            VisibleIndex="8" Visible="false">
                                                            <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                                                            <CellStyle CssClass="GridCss2">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
														 <dx:GridViewDataTextColumn Caption="ServerNameC" FieldName="ServerNameC" 
                                                            VisibleIndex="9" Visible="false">
                                                            <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                                                            <CellStyle CssClass="GridCss2">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Replica ID" FieldName="ReplicaID" 
                                                            Name="ReplicaID" VisibleIndex="1">
                                                        </dx:GridViewDataTextColumn>
                                                    </Columns>
                                                    <SettingsPager AlwaysShowPager="True" PageSize="20">
                                                        <PageSizeItemSettings Visible="True">
                                                        </PageSizeItemSettings>
                                                    </SettingsPager>
                                                    <Styles>
                                                        <Header CssClass="GridCssHeader">
                                                        </Header>
                                                        <AlternatingRow CssClass="GridCssAltRow">
                                                        </AlternatingRow>
                                                        <Cell CssClass="GridCss">
                                                        </Cell>
                                                    </Styles>
                                                </dx:ASPxGridView>
                                            </td>
                                        </tr>
                                    </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Document Count">
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl2" runat="server">
                                    <table>
                                        <tr>
                                            <td>
                                                <dxchartsui:WebChartControl ID="ClusterRepWebChartControl1" runat="server" 
        ClientInstanceName="cClusterRepWebChartControl1" CrosshairEnabled="True" 
        OnCustomCallback="ClusterRepWebChartControl1_CustomCallback" 
         AppearanceNameSerializable="Chameleon" OnBoundDataChanged="ClusterRepWebChartControl1_BoundDataChanged">
        <diagramserializable>
            <cc1:XYDiagram Rotated="True">
                <axisx visibleinpanesserializable="-1" reverse="True"></axisx>
                <axisy visibleinpanesserializable="-1"></axisy>
            </cc1:XYDiagram>
        </diagramserializable>
        <seriesserializable>
            <cc1:Series ArgumentScaleType="Qualitative" Name="Series 1">
            </cc1:Series>
            <cc1:Series ArgumentScaleType="Qualitative" Name="Series 2">
            </cc1:Series>
        </seriesserializable>
    </dxchartsui:WebChartControl>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Database Size">
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl3" runat="server">
                                    <table>
                                        <tr>
                                            <td>
                                                <dxchartsui:WebChartControl ID="ClusterRepWebChartControl2" runat="server" 
                                                    OnCustomCallback="ClusterRepWebChartControl2_CustomCallback" 
                                                    ClientInstanceName="cClusterRepWebChartControl2" 
                                                    AppearanceNameSerializable="Chameleon" CrosshairEnabled="True" 
                                                    OnBoundDataChanged="ClusterRepWebChartControl2_BoundDataChanged">
                                                    <diagramserializable>
                                                        <cc1:XYDiagram Rotated="True">
                                                            <axisx reverse="True" visibleinpanesserializable="-1"></axisx>
                                                            <axisy visibleinpanesserializable="-1"></axisy>
                                                        </cc1:XYDiagram>
                                                    </diagramserializable>
                                                    <seriesserializable>
                                                        <cc1:Series ArgumentScaleType="Qualitative" Name="Series 1">
                                                        </cc1:Series>
                                                        <cc1:Series ArgumentScaleType="Qualitative" Name="Series 2">
                                                        </cc1:Series>
                                                    </seriesserializable>
                                                </dxchartsui:WebChartControl>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                    </TabPages>
                </dx:ASPxPageControl>
                </ContentTemplate>
                </asp:UpdatePanel>
                                    
            </td>
        </tr>
    </table>
</asp:Content>