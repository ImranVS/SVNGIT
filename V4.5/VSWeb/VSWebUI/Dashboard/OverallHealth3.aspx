<%@ Page Title="VitalSigns Plus - Overall Health" Language="C#" MasterPageFile="~/DashboardSite.Master"
	AutoEventWireup="true" CodeBehind="OverallHealth3.aspx.cs" Inherits="VSWebUI.Dashboard.OverallHealth3" %>

<%@ MasterType virtualpath="~/DashboardSite.Master" %>

	<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>
<%@ Register Src="../Controls/StatusBox.ascx" TagName="StatusBox" TagPrefix="uc1" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
	<!--[if !IE]><!-->
	<link rel="stylesheet" type="text/css" href="../css/not-ie.css" />
	<!--<![endif]-->
	<!--[if gt IE 8]><!-->
	<link rel="stylesheet" type="text/css" href="../css/not-ie.css" />
	<!--<![endif]-->
	<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
    <link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
	<style type="text/css">
		a.tooltip2
		{
			text-decoration: none;
			outline: none; /* 4/21/2014 NS commented out */ /* display: inline-block; */
			width: 100%;
			height: 100%; /* 4/21/2014 NS commented out */ /*color: Black;*/
			top: -5px;
		}
		a.tooltip2 strong
		{
			line-height: 30px;
		}
		a.tooltip2:hover
		{
			text-decoration: none; /* 4/21/2014 NS commented out */ /*color: Black;*/
		}
		a.tooltip2 .span2
		{
			text-decoration: none;
			z-index: 10;
			display: none;
			padding: 14px 20px;
			margin-top: -20px;
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
			left: -100px;
			display: inline-block;
			position: absolute;
			color: #111;
			white-space: normal;
			border: 1px solid #DCA;
			background: #fffAF0;
		}
		
		.style2
		{
			width: 100%;
		}
		/* 11/14/2014 NS commented out the code below - it works to tighten up space between Cloud entries, however, completely 
        destroys the On-Premises section in IE8 only. The code below will be moved to non-IE css and referenced accordingly at
        the top of the page. */
		/*
        div.dxdvItem 
        { 
            width: auto !important; 
            height: auto !important;
        }
        */
        #keymetrics2  { display: block; }
        #keymetrics2  td {display: inline-block; }
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<link href="http://maxcdn.bootstrapcdn.com/bootstrap/3.1.1/css/bootstrap.min.css" rel="stylesheet">
	<script language="javascript" type="text/javascript">
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
		function pageLoad() {
			$(document).ready(function () {
				var isUserLogged = '<%=IsUserLogged()%>';
				if (isUserLogged == 'Anonymous') {
					//$('a[class^="statusbutton"]').removeAttr("href");
					$('a[class^="KeyMetrics"]').removeAttr("href");
					$('a[class^="tooltip2"]').removeAttr("href");
				}
			});
		}
		function OnItemClick(s, e) {
			if (e.item.parent == s.GetRootItem())
				e.processOnServer = false;
        }
        function ShowWidgetPanel(widgetPanelUID) {
            var panel = dockManager.GetPanelByUID(widgetPanelUID);

            panel.PerformCallback(widgetPanelUID);

            panel.Show();
        }
        function childLocate(node, className) {
            if (node.classList.contains(className)) {
                return node;
            }
            for (var i = 0; i < node.children.length; i++) {
                var r = childLocate(node.children[i], className);
                if (r != null) {
                    return r;
                }
            }
            return null;
        }
        function panelTransparentInit(s) {
            var cw = childLocate(s.GetMainElement(), 'dxpc-header');
            cw.style.background = '';
            cw.style.backgroundColor = 'transparent';
            cw.style.borderStyle = 'none';
        }
        function UpdateClientLayout(s, e) {
            dockManager.PerformCallback();
        }        
	</script>
	<%--  <script type="text/javascript">
	  
	  	function SetWidgetButtonVisible(widgetName, visible) {
	  		var button = ASPxClientControl.GetControlCollection().GetByName('widgetButton_' + widgetName);
	  		button.GetMainElement().className = visible ? '' : 'disabled';

	  	}
	 
	 

	  
    </script>--%>
    <table align="right">
	<tr>
	    <td align="right">
        	<table id ="menutable" runat="server">
	<tr>
	
	<td align="right">
	
	<dx:ASPxMenu ID="ASPxMenu1" runat="server" EnableTheming="True" 
                                HorizontalAlign="Right"  onitemclick="ASPxMenu1_ItemClick" ShowAsToolbar="True" 
                                Theme="Moderno">
                                <ClientSideEvents ItemClick="OnItemClick" />
                                <Items>
                                    <dx:MenuItem Name="MainItem">
                                        <Items>
                                
                                            <dx:MenuItem Name="Myaccountdetails" Text="Customize this Page">
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
	<table>
		<tr>
			<td>
			<div id="DominoMetrics" runat="server" visible ="false">
			

			
                <dx:ASPxDockPanel ID="DominoDock" runat="server" HeaderText="Domino Metrics" 
                    Theme="Moderno" Width="100%" OwnerZoneUID="RightZone" 
                    ClientSideEvents-AfterDock="function(s, e) {panelTransparentInit(s);}" 
                    AllowedDockState="DockedOnly" BackColor="Transparent" ShowCloseButton="False" ShowCollapseButton="True" 
					 >
<ClientSideEvents AfterDock="function(s, e) {panelTransparentInit(s);}"></ClientSideEvents>
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table id="keymetrics2">
                                <tr>
                                    <td align="right">
                                        <div style="background-color: #F2C7BD; width: 100px; height: 20px; padding-left: 2px; 
                                            border: 1px solid; border-color: #F2C7BD; border-top-left-radius: 5px; border-top-right-radius: 5px; display: none">
                                            <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="User Sessions" CssClass="StatusLabelGray">
											</dx:ASPxLabel>
                                        </div>
                                        <div style="background-image: url('../images/Key 2.jpg'); width: 100px; height: 50px; background-size: 100%; background-repeat: no-repeat;
                                            padding-bottom: 5px; padding-right: 2px; border: 1px solid; border-color: #D24726; border-radius: 5px;">
                                    
							<a id="a3" runat="server" style="border-style: none; float: right" href="~/Dashboard/UserCount.aspx">
                            <dx:ASPxLabel ID="UsersLabel" runat="server" CssClass="StatusLabelWhite" Text="">
												</dx:ASPxLabel>
                                                
									
                                    </a>
								</div>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td align="right">
                                        <div style="background-color: #FAE7D2; width: 100px; height: 20px; padding-left: 2px; padding-right: 2px; display: none">
                                            <dx:ASPxLabel ID="ASPxLabel6" runat="server" CssClass="StatusLabelGray" Text="Down Time (this hr)">
												</dx:ASPxLabel>
                                        </div>
                                        <div style="background-image: url('../images/Key 4.jpg'); width: 100px; height: 50px; background-size: 100%; background-repeat: no-repeat;
                                            padding-bottom: 5px; padding-left: 2px; padding-right: 2px; border: 1px solid; border-color: #FA870A; border-radius: 5px;">
                                            <a id="a4" runat="server" href="~/Dashboard/ServerDownTime.aspx" style="border-style: none; float: right">
                                                <dx:ASPxLabel ID="DwnTimeLabel" runat="server" CssClass="StatusLabelWhite" Text="">
												</dx:ASPxLabel>
                                            </a>
                                        </div>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td align="right">
                                        <div style="background-color: #CFDAE3; width: 100px; height: 20px; padding-left: 2px; display: none;">
                                            <dx:ASPxLabel ID="MailLabel" runat="server" CssClass="StatusLabelGray" Text="Mail">
                                            </dx:ASPxLabel>
                                        </div>
                                        <div style="background-image: url('../images/Key 1.jpg'); width: 100px; height: 50px; background-size: 100%; background-repeat: no-repeat;
                                            padding-bottom: 5px; padding-right: 2px;  border: 1px solid; border-color: #0072C5; border-radius: 5px;">
                                            <a id="a1" runat="server" href="~/Dashboard/MailDeliveryStatus.aspx" style="border-style: none; float: right">
                                                <dx:ASPxLabel ID="PendingLabel" runat="server" CssClass="StatusLabelWhite" Text="">
												</dx:ASPxLabel><br />
                                                <dx:ASPxLabel ID="LblDead" runat="server" CssClass="StatusLabelWhite" Text="">
												</dx:ASPxLabel><br />
                                                <dx:ASPxLabel ID="HeldLabel" runat="server" CssClass="StatusLabelWhite" Text="">
												</dx:ASPxLabel>
                                            </a>
                                        </div>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td align="right">
                                        <div style="background-color: #E1DDF0; width: 100px; height: 20px; padding-left: 2px; display: none;">
                                            <dx:ASPxLabel ID="ASPxLabel5" runat="server" CssClass="StatusLabelGray" Text="Avg Response Time">
									        </dx:ASPxLabel>
                                        </div>
                                        <div style="background-image: url('../images/Key 3.jpg'); width: 100px; height: 50px; background-size: 100%; background-repeat: no-repeat;
                                            padding-bottom: 5px; padding-right: 2px; border: 1px solid; border-color: #5134AB; border-radius: 5px;">
                                                <a id="a2" runat="server" href="~/Dashboard/ResponseTime.aspx" style="border-style: none; float: right">
                                                    <dx:ASPxLabel ID="RespLabel" runat="server" CssClass="StatusLabelWhite" Text="">
												    </dx:ASPxLabel>
                                                </a>
                                        </div>
                                    </td>
                                </tr>
                           </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxDockPanel>
				</div>
			
			</td>
		</tr>
		<tr >
			<td>
                <dx:ASPxDockPanel ID="KeyUserDevicesDock" runat="server" OwnerZoneUID="FarRightZone" AllowedDockState="DockedOnly"
                ClientSideEvents-AfterDock="function(s, e) {panelTransparentInit(s);}" 
                    BackColor="Transparent" Theme="Moderno" HeaderText="Key User Devices" ShowCloseButton="False" ShowCollapseButton="True">
<ClientSideEvents AfterDock="function(s, e) {panelTransparentInit(s);}"></ClientSideEvents>
                    <ContentCollection>
                        <dx:PopupControlContentControl>
                            <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Conditional" runat="server" OnUnload="UpdatePanel_Unload">
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
                            VisibleIndex="1" Visible="False">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Last Sync Time" FieldName="LastSyncTime" 
                            VisibleIndex="4" Visible="False">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Minutes Since Last Sync" VisibleIndex="3" 
                            Width="250px">
                            <DataItemTemplate>
                                <dxchartsui:WebChartControl ID="SyncWebChart" runat="server" 
                    BackColor="Transparent" CrosshairEnabled="True" Height="28px" 
                    onload="SyncWebChart_Load" PaletteName="Oriel" Width="200px" 
                    SideBySideBarDistanceFixed="0">
                    <borderoptions visibility="False" />
                    <diagramserializable>
                        <cc1:XYDiagram Rotated="True">
                            <axisx visibility="False" visibleinpanesserializable="-1"></axisx>
                            <axisy visibility="False" visibleinpanesserializable="-1"><tickmarks minorvisible="False" visible="False" /><Tickmarks Visible="False" MinorVisible="False"></Tickmarks><label textpattern="{V}"></label><gridlines visible="False"></gridlines></axisy>
                            <margins bottom="0" left="0" right="0" top="0" />

<Margins Left="0" Top="0" Right="0" Bottom="0"></Margins>

                            <defaultpane backcolor="Transparent" bordervisible="False"></defaultpane>
                        </cc1:XYDiagram>
                    </diagramserializable>
                    <legend visibility="False"></legend>
                    <seriesserializable>
                        <cc1:Series ArgumentScaleType="Qualitative" LabelsVisibility="True" 
                            Name="MinSinceSync">
                            <viewserializable><cc1:SideBySideBarSeriesView BarWidth="0.6"></cc1:SideBySideBarSeriesView></viewserializable>
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
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxDockPanel>
			</td>
		</tr>

		<tr >
			<td>
                <dx:ASPxDockPanel ID="TravelerDock" runat="server" OwnerZoneUID="FarRightZone" AllowedDockState="DockedOnly"
                ClientSideEvents-AfterDock="function(s, e) {panelTransparentInit(s);}" 
                    BackColor="Transparent" Theme="Moderno" HeaderText="IBM Traveler Server Health"
 ShowCloseButton="False" ShowCollapseButton="True">
<ClientSideEvents AfterDock="function(s, e) {panelTransparentInit(s);}"></ClientSideEvents>
                    <ContentCollection>
                        <dx:PopupControlContentControl>
                <dx:ASPxGridView ID="grid" ClientInstanceName="grid"  runat="server" KeyFieldName="ID"
                                        Width="100%" AutoGenerateColumns="False"  Theme="Office2003Blue" 
                                        OnHtmlDataCellPrepared="grid_HtmlDataCellPrepared"
                            EnableCallBacks="False" OnSelectionChanged="grid_SelectionChanged" 
                                OnFocusedRowChanged="grid_FocusedRowChanged">
                            <ClientSideEvents FocusedRowChanged="function(s, e) { grid.PerformCallback(); }" />
                                        <Columns>
										
                                            <dx:GridViewDataTextColumn Caption="Name" FieldName="Name"  VisibleIndex="0" 
                                                Width="150px">
                                                <HeaderStyle CssClass="GridCssHeader">
                                                    <Paddings Padding="5px" />
                                                    <Paddings Padding="5px"></Paddings>
                                                </HeaderStyle>
                                                <CellStyle CssClass="GridCss">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Status" FieldName="Status" VisibleIndex="1">
											 <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader1">
                                                    <Paddings Padding="5px" />
                                                    <Paddings Padding="5px"></Paddings>
                                                </HeaderStyle>
                                                <CellStyle CssClass="GridCss1">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>                                  
                                            <dx:GridViewDataTextColumn Caption="Traveler Servlet" FieldName="TravelerServlet"
                                                VisibleIndex="2">
                                                <Settings AllowAutoFilter="False" />
                                                <HeaderStyle CssClass="GridCssHeader" 
                                                    Wrap="True" /><CellStyle CssClass="GridCss1">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>     
                                            <dx:GridViewDataTextColumn Caption="ID" 
                                                FieldName="ID"  VisibleIndex="5" 
                                                Visible="False"><HeaderStyle CssClass="GridCssHeader">
                                                    <Paddings Padding="5px" />
                                                </HeaderStyle>
                                                <CellStyle CssClass="GridCss2">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>                                    
                                            <dx:GridViewDataTextColumn Caption="HTTP Status" 
                                                FieldName="HTTP_Status" VisibleIndex="3"
                                                Width="80px"><Settings AllowDragDrop="True" AutoFilterCondition="Contains"></Settings>
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader1" Wrap="True">
                                                    <Paddings Padding="5px" />
                                                    <Paddings Padding="5px"></Paddings>
                                                </HeaderStyle>
                                                <CellStyle CssClass="GridCss1">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>   
                                        <dx:GridViewDataTextColumn Caption="Heart Beat" 
                                                FieldName="HeartBeat" Visible="False" VisibleIndex="4"></dx:GridViewDataTextColumn></Columns>
                                        <SettingsBehavior 
                                            AllowDragDrop="True" AutoExpandAllGroups="True"
                                            AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True" 
                                             ColumnResizeMode="NextColumn" AllowSelectSingleRowOnly="True" 
                                            AllowFocusedRow="True" /><SettingsPager PageSize="10">
                                            <PageSizeItemSettings Visible="True">
                                            </PageSizeItemSettings>
                                        </SettingsPager>
                                        <Styles>
                                            <AlternatingRow CssClass="CssAltRow">
                                            </AlternatingRow>
                                        </Styles>
                                    </dx:ASPxGridView> 
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxDockPanel>
			</td>
		</tr>
		<tr>
			<td>
			<div id ="OnPremises" runat ="server" visible ="false">
			
                <dx:ASPxDockPanel ID="PremisesDock" runat="server" OwnerZoneUID="LeftZone" AllowedDockState="DockedOnly"
                ClientSideEvents-AfterDock="function(s, e) {panelTransparentInit(s);}" 
                    BackColor="Transparent" Theme="Moderno" HeaderText="On-Premises Apps" ShowCloseButton="False" ShowCollapseButton="True">
<ClientSideEvents AfterDock="function(s, e) {panelTransparentInit(s);}"></ClientSideEvents>
                    <ContentCollection>
                        <dx:PopupControlContentControl>
                            
                
    <dx:ASPxPanel ID="ASPxPanel1" runat="server" Width="100%">
    </dx:ASPxPanel>
    <div class="subheaderN" id="dominoDiv" runat="server" style="display: none">Domino Servers</div><div class="progress" id="dominoprogressDiv" runat="server" style="display: none">
      <div class="progress-bar progress-bar-success" id="dominoDiv1" style="display:none; width: 50%" runat="server">
        <span>OK</span> </div><div class="progress-bar progress-bar-warning" id="dominoDiv2" style="display:none; width: 30%" runat="server">
        <span>Issue</span> </div><div class="progress-bar progress-bar-danger" id="dominoDiv3" style="display:none; width: 10%" runat="server">
        <span>Down</span> </div><div class="progress-bar progress-bar-info" id="dominoDiv4" style="display:none; width: 10%" runat="server">
        <span>Maintenance</span> </div></div><div class="subheaderN" id="sametimeDiv" runat="server" style="display: none">Sametime Servers</div><div class="progress" id="sametimeprogressDiv" runat="server" style="display: none">
      <div class="progress-bar progress-bar-success" id="sametimeDiv1" style="display:none; width: 60%" runat="server">
        <span>OK</span> </div><div class="progress-bar progress-bar-warning" id="sametimeDiv2" style="display:none; width: 20%" runat="server">
        <span>Issue</span> </div><div class="progress-bar progress-bar-danger" id="sametimeDiv3" style="display:none; width: 10%" runat="server">
        <span>Down</span> </div><div class="progress-bar progress-bar-info" id="sametimeDiv4" style="display:none; width: 10%" runat="server">
        <span>Maintenance</span> </div></div></dx:PopupControlContentControl></ContentCollection></dx:ASPxDockPanel></div></td></tr><tr>
			<td>
			<div id ="CloudApp" runat ="server"  visible="false">

				<dx:ASPxDockPanel ID="CloudDock" runat="server" HeaderText="Cloud Apps" AllowedDockState="DockedOnly" 
                ClientSideEvents-AfterDock="function(s, e) {panelTransparentInit(s);}"
                OwnerZoneUID="FarRightZone" BackColor="Transparent" Width="100%" Theme="Moderno" 
                    ShowCloseButton="False" ShowCollapseButton="True"  ShowOnPageLoad="false"  >
                    <ContentCollection>
                        <dx:PopupControlContentControl>
                            <dx:ASPxDataView ID="ASPxDataView2" runat="server" AllowPaging="False" Layout="Flow"
					ItemStyle-Wrap="True" Width="100%" >
					<Paddings Padding="0px" />
					<PagerSettings ShowNumericButtons="False">
					</PagerSettings>
					<ItemTemplate>
						<table>
							<tr>
								<td>
									<a href='<%# Eval("url") %>&LastDate=<%# Eval("LastUpdate") %>&Type=Cloud'
										class='tooltip2'>
										<asp:Image ID="Image1" runat="server" Height="90px" Width="90px" ImageUrl='<%# Eval("ImageURL") %>'
											onmousemove="findPos(this,event,'Image1', 'detailsspan');" />
										<asp:Label ID="detailsspan" class="span2" runat="server">Application Name: <font
											style="color: blue; font-size: 16px;"><b>
												<asp:Label ID="NameLabel" Text='<%# Eval("Name") %>' runat="server"></asp:Label></b></font><br />
											Current Status:
											<asp:Label ID="Status" runat="server" Text='<%# Eval("StatusCode") %>'></asp:Label><br />
											Last Scan:
											<asp:Label ID="Scandetails" runat="server" Text='<%# Eval("Lastupdate") %>'></asp:Label>
										</asp:Label></a></td></tr><tr>
								<td style="padding: 5px 5px 5px 5px">
									<div id="statusdiv" class="OKUL" runat="server">
										<asp:Label ID="CStatus" runat="server" Text='<%# Eval("StatusCode") %>' OnDataBinding="CStatus_DataBinding"
											Style="padding: 5px 5px 5px 5px"> </asp:Label></div></td></tr></table></ItemTemplate><Paddings Padding="0px"></Paddings>
					<ContentStyle BackColor="Transparent">
					</ContentStyle>
					<ItemStyle BackColor="Transparent" HorizontalAlign="Center" VerticalAlign="Top">
						<Border BorderWidth="0px"></Border>
						<Paddings Padding="0px"></Paddings>
					</ItemStyle>
					<EmptyDataTemplate>
						No Applications to be monitored.</EmptyDataTemplate><Border BorderWidth="0px"></Border>
				</dx:ASPxDataView>            
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxDockPanel>
			</div>
			</td>
		</tr>
        <tr>
            <td>
		<div id ="NetworkInfra" runat ="server" visible ="false">
                <dx:ASPxDockPanel ID="NetworkDock" runat="server" OwnerZoneUID="FarRightZone" AllowedDockState="DockedOnly"
                ClientSideEvents-AfterDock="function(s, e) {panelTransparentInit(s);}" 
                    BackColor="Transparent" Theme="Moderno" HeaderText="Network Infrastructure" ShowCloseButton="False" ShowCollapseButton="True" >
                    <ContentCollection>
                        <dx:PopupControlContentControl>
                            <dx:ASPxDataView ID="ASPxDataView3" runat="server" AllowPaging="False" Layout="Flow"
					ItemStyle-Wrap="True" Width="100%">
					<Paddings Padding="0px" />
					<PagerSettings ShowNumericButtons="False">
					</PagerSettings>
					<ItemTemplate>
						<table>
							<tr>
								<td>
									<a href='NetworkServerDetails.aspx?Name=<%# Eval("Name") %>&LastDate=<%# Eval("LastUpdate") %>&Type=Network Device'
										class='tooltip2'>
										<asp:Image ID="Image1" runat="server" Height="90px" Width="90px" ImageUrl='<%# Eval("Imageurl") %>'
											onmousemove="findPos(this,event,'Image1', 'detailsspan');" />
										<asp:Label ID="detailsspan" class="span2" runat="server">Application Name: <font
											style="color: blue; font-size: 16px;"><b>
												<asp:Label ID="NameLabel" Text='<%# Eval("Name") %>' runat="server"></asp:Label></b></font><br />
											Current Status:
											<asp:Label ID="Status" runat="server" Text='<%# Eval("StatusCode") %>'></asp:Label><br />
											Last Scan:
											<asp:Label ID="Scandetails" runat="server" Text='<%# Eval("Lastupdate") %>'></asp:Label>
										</asp:Label></a></td></tr><tr>
								<td style="padding: 5px 5px 5px 5px">
									<div id="statusdiv" class="OKUL" runat="server">
										<asp:Label ID="CStatus" runat="server" Text='<%# Eval("StatusCode") %>' OnDataBinding="NDStatus_DataBinding"
											Style="padding: 5px 5px 5px 5px"> </asp:Label></div></td></tr></table></ItemTemplate><Paddings Padding="0px"></Paddings>
					<ContentStyle BackColor="Transparent">
					</ContentStyle>
					<ItemStyle BackColor="Transparent" HorizontalAlign="Center" VerticalAlign="Top">
						<Border BorderWidth="0px"></Border>
						<Paddings Padding="0px"></Paddings>
					</ItemStyle>
					<EmptyDataTemplate>
						No Applications to be monitored.</EmptyDataTemplate><Border BorderWidth="0px"></Border>
				</dx:ASPxDataView>            
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxDockPanel>
		</div>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxDockPanel ID="MailStatusDock" runat="server" OwnerZoneUID="RightZone"
                    AllowedDockState="DockedOnly" Theme="Moderno" 
                    BackColor="Transparent" ShowCloseButton="False" ShowCollapseButton="True" 
                    HeaderText="Mail Delivery Status" ><ClientSideEvents AfterDock="function(s, e) {panelTransparentInit(s);}" >
                    </ClientSideEvents>
<ContentCollection>
<dx:PopupControlContentControl runat="server">
    <div id="dominoheaderDiv" class="subheaderN" runat="server" style="display: block">Domino</div><div><dx:ASPxGridView 
                                                ID="EXJournalGridView" runat="server" AutoGenerateColumns="False" 
                                                EnableTheming="True" 
                                                onhtmldatacellprepared="EXJournalGridView_HtmlDataCellPrepared" 
                                                onhtmlrowcreated="EXJournalGridView_HtmlRowCreated" 
                                                OnPageSizeChanged="EXJournalGridView_PageSizeChanged" Theme="Office2003Blue" 
                                                Width="100%"><Columns><dx:GridViewDataTextColumn Caption="Server Name" 
                                                    FieldName="Name" VisibleIndex="0"><Settings 
                                                    AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" /><HeaderStyle 
                                                    CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle></dx:GridViewDataTextColumn><dx:GridViewDataTextColumn 
                                                    Caption="Overall Status" FieldName="Status" VisibleIndex="1"><Settings 
                                                    AllowAutoFilter="False" /><DataItemTemplate><asp:Label ID="hfNameLabel1" 
                                                        runat="server" Text='<%# Eval("Status") %>'></asp:Label></DataItemTemplate><HeaderStyle 
                                                    CssClass="GridCssHeader1" Wrap="True" /><CellStyle CssClass="GridCss1"></CellStyle></dx:GridViewDataTextColumn><dx:GridViewDataTextColumn 
                                                    Caption="Pending Mail" FieldName="PendingMail" VisibleIndex="4" 
                                                    Width="30px"><Settings 
                                                    AllowAutoFilter="True" AutoFilterCondition="GreaterOrEqual" /><HeaderStyle 
                                                    CssClass="GridCssHeader2" Wrap="True" /><CellStyle CssClass="GridCss2"></CellStyle></dx:GridViewDataTextColumn><dx:GridViewDataTextColumn 
                                                    Caption="Held Mail" FieldName="HeldMail" VisibleIndex="5" Width="30px"><Settings 
                                                    AllowAutoFilter="False" /><HeaderStyle CssClass="GridCssHeader2" 
                                                    Wrap="True" /><CellStyle 
                                                    CssClass="GridCss2"></CellStyle></dx:GridViewDataTextColumn><dx:GridViewDataTextColumn 
                                                    Caption="Dead Mail" FieldName="DeadMail" VisibleIndex="6" Width="30px"><Settings 
                                                        AllowAutoFilter="False" /><HeaderStyle CssClass="GridCssHeader2" 
                                                        Wrap="True" /><CellStyle 
                                                        CssClass="GridCss2"></CellStyle></dx:GridViewDataTextColumn><dx:GridViewDataTextColumn 
                                                    Caption="Location" FieldName="Location" VisibleIndex="8" Visible="False"><Settings 
                                                        AllowAutoFilter="False" /><HeaderStyle CssClass="GridCssHeader" /><CellStyle 
                                                        CssClass="GridCss"></CellStyle></dx:GridViewDataTextColumn><dx:GridViewDataTextColumn 
                                                    Caption="Category" FieldName="Category" VisibleIndex="10" Visible="False"><Settings 
                                                        AllowAutoFilter="False" /><HeaderStyle CssClass="GridCssHeader" /><CellStyle 
                                                        CssClass="GridCss"></CellStyle></dx:GridViewDataTextColumn><dx:GridViewDataTextColumn 
                                                    Caption="Last Updated" FieldName="LastUpdate" VisibleIndex="12" 
                                                        Visible="False"><Settings 
                                                        AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" /><HeaderStyle 
                                                        CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle></dx:GridViewDataTextColumn></Columns><SettingsPager 
                                                AlwaysShowPager="True" PageSize="100"><PageSizeItemSettings Visible="True"></PageSizeItemSettings></SettingsPager><Styles><AlternatingRow 
                                                    CssClass="GridAltRow" Enabled="True"></AlternatingRow></Styles></dx:ASPxGridView></div>
                                                    <div id="spacerDiv" runat="server" style="display: block">&nbsp;</div><div id="exchangeheaderDiv" class="subheaderN" runat="server" style="display: block">Exchange</div><div><dx:ASPxGridView ID="ExchangeMailGridView" 
                                                    runat="server" AutoGenerateColumns="False" 
                                                    OnHtmlDataCellPrepared="ExchangeMailGridView_HtmlDataCellPrepared" 
                                                    
                                OnPageSizeChanged="ExchangeMailGridView_PageSizeChanged" Theme="Office2003Blue" 
                                Width="100%"><Columns><dx:GridViewDataTextColumn 
                                                        Caption="Server Name" FieldName="Name" ShowInCustomizationForm="True" 
                                                        VisibleIndex="1"><Settings AllowAutoFilterTextInputTimer="False" 
                                                        AutoFilterCondition="Contains" /></dx:GridViewDataTextColumn><dx:GridViewDataTextColumn 
                                                        Caption="Submission Queue" FieldName="PendingMail" 
                                                        ShowInCustomizationForm="True" VisibleIndex="3" Width="30px"><Settings 
                                                            AllowAutoFilter="False" /><HeaderStyle CssClass="GridCssHeader2" 
                                                        Wrap="True" /><CellStyle 
                                                            CssClass="GridCss2"></CellStyle></dx:GridViewDataTextColumn><dx:GridViewDataTextColumn 
                                                        Caption="Unreachable Queue" FieldName="DeadMail" ShowInCustomizationForm="True" 
                                                        VisibleIndex="4" Width="30px"><Settings AllowAutoFilter="False" /><HeaderStyle 
                                                            CssClass="GridCssHeader2" Wrap="True" /><CellStyle CssClass="GridCss2"></CellStyle></dx:GridViewDataTextColumn><dx:GridViewDataTextColumn 
                                                        Caption="Poison Queue" FieldName="HeldMail" ShowInCustomizationForm="True" 
                                                        VisibleIndex="5" Width="30px"><Settings AllowAutoFilter="False" /><HeaderStyle 
                                                        CssClass="GridCssHeader2" Wrap="True" /><CellStyle CssClass="GridCss2"></CellStyle></dx:GridViewDataTextColumn><dx:GridViewDataTextColumn 
                                                        Caption="Last Updated" FieldName="LastUpdate" ShowInCustomizationForm="True" 
                                                        VisibleIndex="8" Visible="False"><Settings AllowAutoFilter="False" /></dx:GridViewDataTextColumn><dx:GridViewDataTextColumn 
                                                        Caption="Overall Status" FieldName="Status" ShowInCustomizationForm="True" 
                                                        VisibleIndex="2"><Settings AllowAutoFilter="False" /><DataItemTemplate><asp:Label 
                                                                ID="hfExNameLabel" runat="server" Text='<%# Eval("Status") %>'></asp:Label></DataItemTemplate><HeaderStyle 
                                                            CssClass="GridCssHeader1" Wrap="True" /><CellStyle CssClass="GridCss1"></CellStyle></dx:GridViewDataTextColumn><dx:GridViewDataTextColumn 
                                                        Caption="Location" FieldName="Location" ShowInCustomizationForm="True" 
                                                        VisibleIndex="6" Visible="False"><Settings AllowAutoFilter="False" /></dx:GridViewDataTextColumn><dx:GridViewDataTextColumn 
                                                        Caption="Category" FieldName="Category" ShowInCustomizationForm="True" 
                                                        VisibleIndex="7" Visible="False"><Settings AllowAutoFilter="False" /></dx:GridViewDataTextColumn></Columns><SettingsPager 
                                                    AlwaysShowPager="True" PageSize="100"><PageSizeItemSettings Visible="True"></PageSizeItemSettings></SettingsPager><Styles><Header 
                                                        CssClass="GridCssHeader"></Header><AlternatingRow CssClass="GridAltRow"></AlternatingRow><Cell 
                                                        CssClass="GridCss"></Cell></Styles></dx:ASPxGridView></div>
                                                        </dx:PopupControlContentControl>
</ContentCollection>
</dx:ASPxDockPanel>
            </td>
        </tr>
	</table>
     <div style="float: left;">  
    <dx:ASPxDockZone ID="ASPxDockZone1" runat="server" ZoneUID="LeftZone" 
                    Width="300px" PanelSpacing="12px">
                </dx:ASPxDockZone>
    </div>
    <div style="float: left; margin-left: 10px;">
    <dx:ASPxDockZone ID="ASPxDockZone2" runat="server" ZoneUID="RightZone" 
            Width="490px" PanelSpacing="12px">
                </dx:ASPxDockZone>
    </div>
    <div style="float: left; margin-left: 10px;">
        <dx:ASPxDockZone ID="ASPxDockZone3" runat="server" ZoneUID="FarRightZone"
            Width="400px" PanelSpacing="12px">
        </dx:ASPxDockZone>
    </div>
     <dx:ASPxDockManager ID="ASPxDockManager1" runat="server" 
        ClientInstanceName="dockManager" 
        ClientSideEvents-AfterDock="function(s, e) { s.PerformCallback(); }" 
        onclientlayout="ASPxDockManager1_ClientLayout">
    </dx:ASPxDockManager>
</asp:Content>
