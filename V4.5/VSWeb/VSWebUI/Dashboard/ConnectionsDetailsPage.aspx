<%@ Page Language="C#" Title="VitalSigns Plus - IBM Connections" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="ConnectionsDetailsPage.aspx.cs" Inherits="VSWebUI.Dashboard.ConnectionsDetailsPage" %>

<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style id="styles" type="text/css">
		a.tooltip2
		{
			text-decoration: none;
			outline: none; 
			width: 100%;
			height: 100%; 
			top: -5px;
		}
		a.tooltip2 strong
		{
			line-height: 30px;
		}
		a.tooltip2:hover
		{
			text-decoration: none; 
		}
		a.tooltip2 .span2
		{
			text-decoration: none;
			z-index: 10;
			display: none;
			padding: 14px 20px;
			margin-top: -30px;
			float: left;
			width: 240px;
			line-height: 16px;
		}
		a.tooltip2:hover .span2
		{
			text-decoration: none;
			text-align: left;
			float: left;
			margin: 0px;
			left: -240px;
			display: inline-block;
			position: absolute;
			color: #111;
			white-space: normal;
			border: 1px solid #DCA;
			background: #fffAF0;
			word-wrap: break-word;
		}
		a.tooltip2 .span2
		{
			text-decoration: none;
			border-radius: 4px;
			-moz-border-radius: 4px;
			-webkit-border-radius: 4px;
			-moz-box-shadow: 5px 5px 8px #CCC;
			-webkit-box-shadow: 5px 5px 8px #CCC;
			box-shadow: 5px 5px 8px #CCC;
		}
		.callout2
		{
			z-index: 20;
			position: absolute;
			top: 30px;
			border: 0;
			left: -12px;
		}
	</style>
<script type="text/javascript" language="javascript">
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
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%">
        <tr>
            <td valign="top">
                <table>
                    <tr>
                        <td>
                            <div id="StatusHolder">
                            <div class="StatusLabel">Overall Status</div><div class="OK" id="serverstatus" runat="server">OK</div>
                            </div>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <div id="NameHolder">
                            <div class="header" id="servernamelbldisp" runat="server">azphxdom1/RPRWyatt</div>
                            <div class="scan" id="Lastscanned" runat="server">9/16/2014 2:59:00 PM</div>
                            </div>
                            <asp:Label ID="lblServerType" runat="server" Text="IBM Connections: " Font-Bold="True"
                                Font-Size="Large" Style="color: #000000; font-family: Verdana; display: none"></asp:Label>             
 
                            <asp:Label ID="servernamelbl" runat="server" Text="Label" Font-Bold="True"
                                Font-Size="Large" Style="color: #000000; font-family: Verdana; display: none"></asp:Label>

                        </td>
                    </tr>
                </table>
            </td>
            <td>
                &nbsp;
            </td>
            <td valign="top" align="right">
                <dx:ASPxMenu ID="ASPxMenu1" runat="server" HorizontalAlign="Right" 
                    ShowAsToolbar="True" onitemclick="ASPxMenu1_ItemClick" 
                    style="height: 33px" Theme="Moderno">
                    <ClientSideEvents ItemClick="OnItemClick" />
                    <Items>
                        <dx:MenuItem BeginGroup="True" Name="EditConfigItem2" Text="">
                            <Items>
                                <dx:MenuItem Name="BackItem" Text="Back">
                                </dx:MenuItem>
                                <dx:MenuItem Name="ScanItem" Text="Scan Now">
                                </dx:MenuItem>
                                <dx:MenuItem Name="EditConfigItem" Text="Edit in Configurator">
                                </dx:MenuItem>
                            </Items>
                            <Image Url="~/images/icons/Gear.png">
                            </Image>
                        </dx:MenuItem>
                    </Items>
                </dx:ASPxMenu>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
                    Theme="Glass" Width="100%">
                    <TabPages>
                        <dx:TabPage Text="Overall">
                            <TabImage Url="~/images/icons/information.png">
                            </TabImage>
                            <ContentCollection>
                                <dx:ContentControl runat="server">
                                    <table>
                                        <tr>
                                            <td>
                                                <dxchartsui:WebChartControl ID="UsersDailyWebChart" runat="server" 
                                                    PaletteName="Oriel" Width="400px">
                                                    <diagramserializable>
                                                        <cc1:XYDiagram>
                                                            <axisx visibleinpanesserializable="-1">
                                                            </axisx>
                                                            <axisy visibleinpanesserializable="-1">
                                                            </axisy>
                                                        </cc1:XYDiagram>
                                                    </diagramserializable>
                                                    <legend alignmenthorizontal="Center" alignmentvertical="BottomOutside" 
                                                        markersize="13, 16" maxverticalpercentage="30"></legend>
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
                                                <dxchartsui:WebChartControl ID="ActivitiesWebChart" runat="server" 
                                                    PaletteName="Oriel" Width="400px">
                                                    <diagramserializable>
                                                        <cc1:XYDiagram>
                                                            <axisx visibleinpanesserializable="-1">
                                                            </axisx>
                                                            <axisy visibleinpanesserializable="-1">
                                                            </axisy>
                                                        </cc1:XYDiagram>
                                                    </diagramserializable>
                                                    <legend alignmenthorizontal="Center" alignmentvertical="BottomOutside" 
                                                        markersize="13, 16" maxverticalpercentage="30"></legend>
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
                                                        <cc1:ChartTitle Font="Tahoma, 12pt" Text="Activities" />
                                                    </titles>
                                                </dxchartsui:WebChartControl>
                                            </td>
                                            <td>
                                                <dxchartsui:WebChartControl ID="BlogsWebChart" runat="server" 
                                                    PaletteName="Oriel" Width="400px">
                                                    <diagramserializable>
                                                        <cc1:XYDiagram>
                                                            <axisx visibleinpanesserializable="-1">
                                                            </axisx>
                                                            <axisy visibleinpanesserializable="-1">
                                                            </axisy>
                                                        </cc1:XYDiagram>
                                                    </diagramserializable>
                                                    <legend alignmenthorizontal="Center" alignmentvertical="BottomOutside" 
                                                        markersize="13, 16" maxverticalpercentage="30"></legend>
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
                                                        <cc1:ChartTitle Font="Tahoma, 12pt" Text="Blogs" />
                                                    </titles>
                                                </dxchartsui:WebChartControl>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Health Assessment">
                            <TabImage Url="~/images/icons/overallhealth.gif">
                            </TabImage>
                            <ContentCollection>
                                <dx:ContentControl runat="server">
                                    <table class="navbarTbl">
                                        <tr>
                                            <td>
                                                <dx:ASPxGridView ID="HealthAssessmentGrid1" runat="server" AutoGenerateColumns="False"
                                                                                CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                                                                                Cursor="pointer" KeyFieldName="ID" Theme="Office2003Blue" Width="100%" OnHtmlDataCellPrepared="HealthAssessmentGrid1_HtmlDataCellPrepared">
                                                                                <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" ColumnResizeMode="NextColumn" />
                                                                                <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" ColumnResizeMode="NextColumn" />
                                                                                <Columns>
                                                                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" Visible="false" VisibleIndex="0">
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn Caption="Test Name" FieldName="TestName" ShowInCustomizationForm="True"
                                                                                        VisibleIndex="1" Width="200px">
                                                                                        <Settings AllowDragDrop="True" AutoFilterCondition="Contains"></Settings>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn Caption="Details" FieldName="Details" ShowInCustomizationForm="true"
                                                                                        VisibleIndex="3">
												    								<DataItemTemplate>
													<a class='tooltip2'>
														<asp:Label ID="detailsLabel" Text='<%# Eval("Details") %>' runat="server" onmousemove="findPos(this,event,'detailsLabel', 'detailsspan');" />
														<asp:Label ID="detailsspan" class="span2" runat="server">		
															<img class='callout2' src='../images/callout.gif'' />	
																<img src='<%# Eval("imgsource") %>' />											
															<font style="color: blue; font-size: 16px;">&nbsp<b>Details</b></font><br>
															Details Info:
															<asp:Label ID="detailsinfoLabel" Text='<%# Eval("Details") %>' runat="server" />
														</asp:Label></a></DataItemTemplate></dx:GridViewDataTextColumn><dx:GridViewDataTextColumn Caption="Result" FieldName="Result" VisibleIndex="2" Width="80px">
                                                                                        <HeaderStyle CssClass="GridCssHeader1" />
                                                                                        <CellStyle CssClass="GridCss1">
                                                                                        </CellStyle>
                                                                                        <Settings AllowDragDrop="True" AutoFilterCondition="Contains"></Settings>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn Caption="Category" FieldName="Category" ShowInCustomizationForm="True"
                                                                                        VisibleIndex="0" Width="150px" Visible="true">
                                                                                        <Settings AllowDragDrop="True" AutoFilterCondition="Contains"></Settings>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn Caption="Last Update" FieldName="LastUpdate" ShowInCustomizationForm="True"
                                                                                        VisibleIndex="4" Width="200px">
                                                                                    </dx:GridViewDataTextColumn>
                                                                                </Columns>
                                                                                <Settings ShowGroupPanel="True" />
                                                                                <SettingsBehavior AllowDragDrop="False" AllowSelectByRowClick="True" AutoExpandAllGroups="True" />
                                                                                <Settings ShowGroupPanel="True" />
                                                                                <SettingsBehavior AllowDragDrop="False" AllowSelectByRowClick="True" AutoExpandAllGroups="True" />
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
                                            </td>
                                        </tr>
                                    </table>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Maintenance">
                            <TabImage Url="~/images/icons/wrench.png">
                            </TabImage>
                            <ContentCollection>
                                <dx:ContentControl runat="server">
                                    <table class="navbarTbl">
                                        <tr>
                                            <td>
                                                <table>
                                                        <tr>
                                                            <td colspan="8">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" Text="From Date:"
                                                                    Wrap="False">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtfromdate" runat="server" Font-Size="12px"></asp:TextBox><cc1:CalendarExtender ID="calextender" runat="server" Format="MM/dd/yyyy" TargetControlID="txtfromdate">
                                                                </cc1:CalendarExtender>
                                                            </td>
                                                            <td valign="top">
                                                                <asp:RequiredFieldValidator ID="rfvtxtfromdate" runat="server" ControlToValidate="txtfromdate"
                                                                    ErrorMessage="Enter From Date" Font-Size="12px" ForeColor="#FF3300"></asp:RequiredFieldValidator></td><td valign="top">
                                                                &nbsp; </td><td valign="top">
                                                                <dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" Text="From Time:"
                                                                    Wrap="False">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td valign="top">
                                                                <dx:ASPxTimeEdit ID="ASPxTimeEdit1" runat="server">
                                                                </dx:ASPxTimeEdit>
                                                            </td>
                                                            <td valign="top">
                                                                &nbsp; </td><td valign="top">
                                                                <dx:ASPxButton ID="ClearButton" runat="server" OnClick="ClearButton_Click" Text="Clear"
                                                                    CssClass="sysButton" Width="80px">
                                                                </dx:ASPxButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="lblsmallFont" Text="To Date:"
                                                                    Wrap="False">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td valign="top">
                                                                <asp:TextBox ID="txttodate" runat="server" Font-Size="12px"></asp:TextBox><cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="MM/dd/yyyy" TargetControlID="txttodate">
                                                                </cc1:CalendarExtender>
                                                            </td>
                                                            <td valign="top">
                                                                <asp:RequiredFieldValidator ID="RFvtxttodate" runat="server" ControlToValidate="txttodate"
                                                                    ErrorMessage="Enter To Date" Font-Size="12px" ForeColor="Red"></asp:RequiredFieldValidator></td><td valign="top">
                                                                &nbsp; </td><td valign="top">
                                                                <dx:ASPxLabel ID="ASPxLabel4" runat="server" CssClass="lblsmallFont" Text="To Time:"
                                                                    Wrap="False">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td valign="top">
                                                                <dx:ASPxTimeEdit ID="ASPxTimeEdit2" runat="server">
                                                                </dx:ASPxTimeEdit>
                                                            </td>
                                                            <td>
                                                                &nbsp; </td><td>
                                                                <dx:ASPxButton ID="btnsearch" runat="server" OnClick="btnsearch_Click" Text="Search"
                                                                     CssClass="sysButton" Width="80px">
                                                                </dx:ASPxButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dx:ASPxGridView ID="maintenancegrid" runat="server" 
                                                    AutoGenerateColumns="False" EnableTheming="True" KeyFieldName="TypeandName" 
                                                    Theme="Office2003Blue" Width="100%">
                                                    <Columns>
                                                        <dx:GridViewDataTextColumn Caption="Server Name" FieldName="servername" 
                                                            ShowInCustomizationForm="True" VisibleIndex="0">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Server Type" FieldName="ServerType" 
                                                            ShowInCustomizationForm="True" VisibleIndex="1">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Start Date" FieldName="StartDate" 
                                                            ShowInCustomizationForm="True" VisibleIndex="2">
                                                            <Settings AllowAutoFilter="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Start Time" FieldName="StartTime" 
                                                            ShowInCustomizationForm="True" VisibleIndex="3">
                                                            <Settings AllowAutoFilter="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Duration" FieldName="Duration" 
                                                            ShowInCustomizationForm="True" VisibleIndex="4">
                                                            <Settings AllowAutoFilter="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="End Date" FieldName="EndDate" 
                                                            ShowInCustomizationForm="True" VisibleIndex="5">
                                                            <Settings AllowAutoFilter="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Maintenance Type" FieldName="MaintType" 
                                                            ShowInCustomizationForm="True" VisibleIndex="6">
                                                            <Settings AllowAutoFilter="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Maintenance Days List" 
                                                            FieldName="MaintDaysList" ShowInCustomizationForm="True" VisibleIndex="7">
                                                            <Settings AllowAutoFilter="False" />
                                                        </dx:GridViewDataTextColumn>
                                                    </Columns>
                                                    <SettingsPager AlwaysShowPager="True" PageSize="50">
                                                        <PageSizeItemSettings Visible="True">
                                                        </PageSizeItemSettings>
                                                    </SettingsPager>
                                                    <Settings ShowFilterRow="True" ShowGroupPanel="True" />
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
                                        <tr>
                                                <td>
                                                    <dx:ASPxPopupControl ID="MsgPopupControl" runat="server" HeaderText="Information"
                                                        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                                        Theme="MetropolisBlue"><ContentCollection>
                                                            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                                                                <table class="style1">
                                                                    <tr>
                                                                        <td>
                                                                            <dx:ASPxLabel ID="ErrmsgLabel" runat="server">
                                                                            </dx:ASPxLabel>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <dx:ASPxButton ID="OKButton" runat="server" OnClick="OKButton_Click" Text="OK" 
                                                                                CssClass="sysButton"></dx:ASPxButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </dx:PopupControlContentControl>
                                                        </ContentCollection>
                                                    </dx:ASPxPopupControl>
                                                </td>
                                            </tr>
                                    </table>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Alerts History">
                            <TabImage Url="~/images/icons/sounds.gif">
                            </TabImage>
                            <ContentCollection>
                                <dx:ContentControl runat="server">
                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <div id="infoDiv" class="info">
                                                                    The list of configured alerts that apply to the server are listed below. The list
                                                                    includes the last 7 days worth of information. </div></td></tr><tr>
                                                            <td>
                                                                <dx:ASPxGridView ID="AlertGridView" runat="server" AutoGenerateColumns="False" EnableTheming="True"
                                                                    Theme="Office2003Blue" Width="100%">
                                                                    <Columns>
                                                                        <dx:GridViewCommandColumn ShowInCustomizationForm="True" Visible="False" VisibleIndex="0">
                                                                            <ClearFilterButton Visible="True">
                                                                            </ClearFilterButton>
                                                                        </dx:GridViewCommandColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Device Name" FieldName="DeviceName" ShowInCustomizationForm="True"
                                                                            VisibleIndex="1">
                                                                            <Settings AutoFilterCondition="Contains" />
                                                                            <Settings AutoFilterCondition="Contains" />
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
                                                                        <dx:GridViewDataTextColumn Caption="Device Type" FieldName="DeviceType" ShowInCustomizationForm="True"
                                                                            VisibleIndex="2">
                                                                            <EditCellStyle CssClass="GridCss">
                                                                            </EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                                            </EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                                            <CellStyle CssClass="GridCss">
                                                                            </CellStyle>
                                                                            <Settings AutoFilterCondition="Contains" />
                                                                            <Settings AutoFilterCondition="Contains" />
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Alert Type" FieldName="AlertType" ShowInCustomizationForm="True"
                                                                            VisibleIndex="3">
                                                                            <EditCellStyle CssClass="GridCss">
                                                                            </EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                                            </EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                                            <CellStyle CssClass="GridCss">
                                                                            </CellStyle>
                                                                            <Settings AutoFilterCondition="Contains" />
                                                                            <Settings AutoFilterCondition="Contains" />
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Date/Time of Alert" FieldName="DateTimeOfAlert"
                                                                            ShowInCustomizationForm="True" VisibleIndex="4">
                                                                            <EditCellStyle CssClass="GridCss">
                                                                            </EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                                            </EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                                            <CellStyle CssClass="GridCss1">
                                                                            </CellStyle>
                                                                            <Settings AutoFilterCondition="Contains" />
                                                                            <Settings AutoFilterCondition="Contains" />
                                                                        </dx:GridViewDataTextColumn>
                                                                        <%-- <dx:GridViewDataTextColumn Caption="Date/Time Sent" FieldName="DateTimeSent" 
                                                                        ShowInCustomizationForm="True" VisibleIndex="5">
                                                                         <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                    <CellStyle CssClass="GridCss1">
                                                    </CellStyle>
                                                    <Settings AutoFilterCondition="Contains" />
                                                                    </dx:GridViewDataTextColumn>--%>
                                                                        <dx:GridViewDataTextColumn Caption="Date/Time Alert Cleared" FieldName="DateTimeAlertCleared"
                                                                            ShowInCustomizationForm="True" VisibleIndex="6">
                                                                            <EditCellStyle CssClass="GridCss">
                                                                            </EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                                            </EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                                            <CellStyle CssClass="GridCss1">
                                                                            </CellStyle>
                                                                            <Settings AutoFilterCondition="Contains" />
                                                                            <Settings AutoFilterCondition="Contains" />
                                                                        </dx:GridViewDataTextColumn>
                                                                    </Columns>
                                                                    <SettingsBehavior ColumnResizeMode="NextColumn" />
                                                                    <SettingsBehavior ColumnResizeMode="NextColumn"></SettingsBehavior>
                                                                    <SettingsBehavior ColumnResizeMode="NextColumn" />
                                                                    <SettingsPager PageSize="50">
                                                                        <PageSizeItemSettings Visible="True">
                                                                        </PageSizeItemSettings>
                                                                    </SettingsPager>
                                                                    <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                                                                    <Settings ShowFilterRow="True" ShowGroupPanel="True"></Settings>
                                                                    <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                                                                    <Styles>
                                                                        <AlternatingRow CssClass="GridAltRow">
                                                                        </AlternatingRow>
                                                                    </Styles>
                                                                </dx:ASPxGridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Outages">
                            <TabImage Url="~/images/icons/exclamation.png">
                            </TabImage>
                            <ContentCollection>
                                <dx:ContentControl runat="server">
                                    <table class="navbarTbl">
                                        <tr>
                                                <td>
                                                    <dx:ASPxGridView ID="OutageGridView" runat="server" AutoGenerateColumns="False" EnableTheming="True"
                                                        Theme="Office2003Blue" Width="100%">
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn Caption="Server Name" FieldName="ServerName" ShowInCustomizationForm="True"
                                                                VisibleIndex="1" Width="300px">
                                                                <Settings AllowAutoFilter="False" AutoFilterCondition="Contains" />
                                                                <Settings AllowAutoFilter="False" AutoFilterCondition="Contains" />
                                                                <Settings AllowAutoFilter="False" AutoFilterCondition="Contains" />
                                                                <Settings AllowAutoFilter="False" AutoFilterCondition="Contains" />
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader">
                                                                    <Paddings Padding="5px" />
                                                                    <Paddings Padding="5px" />
                                                                    <Paddings Padding="5px" />
                                                                    <Paddings Padding="5px" />
                                                                </HeaderStyle>
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Date/Time Down" FieldName="DateTimeDown" ShowInCustomizationForm="True"
                                                                VisibleIndex="2">
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss1">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Date/Time Up" FieldName="DateTimeUp" ShowInCustomizationForm="True"
                                                                VisibleIndex="3">
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss1">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Duration (mins)" FieldName="Description" ShowInCustomizationForm="True"
                                                                VisibleIndex="4">
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss2">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                            </dx:GridViewDataTextColumn>
                                                        </Columns>
                                                        <SettingsBehavior ColumnResizeMode="NextColumn" />
                                                        <SettingsBehavior ColumnResizeMode="NextColumn" />
                                                        <SettingsPager PageSize="50">
                                                            <PageSizeItemSettings Visible="True">
                                                            </PageSizeItemSettings>
                                                        </SettingsPager>
                                                        <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                                                        <SettingsBehavior ColumnResizeMode="NextColumn" />
                                                        <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                                                        <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                                                        <SettingsBehavior ColumnResizeMode="NextColumn" />
                                                        <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                                                        <Styles>
                                                            <AlternatingRow CssClass="GridAltRow">
                                                            </AlternatingRow>
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