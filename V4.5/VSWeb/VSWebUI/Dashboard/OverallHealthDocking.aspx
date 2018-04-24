<%@ Page Title="VitalSigns Plus - Overall Health" Language="C#" MasterPageFile="~/DashboardSite.Master"
    AutoEventWireup="true" CodeBehind="OverallHealthDocking.aspx.cs" Inherits="VSWebUI.Dashboard.OverallHealthDocking" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ MasterType VirtualPath="~/DashboardSite.Master" %>
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
    <link rel="Stylesheet" type="text/css" href="../css/screen.css" />
    <style type="text/css">
        a.tooltip2
        {
            text-decoration: none;
            outline: none; /* 4/21/2014v NS commented out */ /* display: inline-block; */
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
            position:absolute;
            color: #111;
            white-space: normal;
            border: 1px solid #DCA;
            background: #fffAF0;
        }
        
        .style1
        {
            height: 26px;
            width: 20px;
        }
        .style2
        {
            width: 100%;
        }
        
        
        
        
        .divhover
        {
            border: transparent;
        }
        .divhover:hover
        {
            background: none;
            border: #000 1px solid;
            
        }
        
        
        .over:hover
        {
            border: #ccc 1px solid;
            background: #f3f3f3;
        }
        
        /*div.dxdvItem{float:left; display:inline;}
        
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
    </style>
    <style type="text/css"> #ContentPlaceHolder1_CloudDock_PW-1
        {
            width:1400px !important;
           
            }
            #ContentPlaceHolder1_CloudDock_PW-1 table{width:100%;}
           
            </style>
   
    <style type="text/css"> #ContentPlaceHolder1_PremisesDock_PWC-1
        {
            width:1000px !important;
            height:auto !important;
           
            }
            #ContentPlaceHolder1_PremisesDock_PWC-1 table{width:100%;}
           
            </style>
              <style type="text/css"> #ContentPlaceHolder1_PremisesDock_PW-1
        {
          
            height:auto !important;
           
            }
            #ContentPlaceHolder1_PremisesDock_PW-1 table{width:100%;}
           
            </style>
  
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


//        $(document).ready(function () {
//            $("over").mouseover(function () {
//                $("over").css("background-color", "yellow");
//            });
//            $("over").mouseout(function () {
//                $("over").css("background-color", "black");
//            });
//        });
       
    </script>

    <script type="text/javascript">
        function ShowWidgetPanel(widgetPanelUID) {
            var panel = dockManager.GetPanelByUID(widgetPanelUID);

            panel.PerformCallback(widgetPanelUID);

            panel.Show();


        }



        function SetWidgetButtonVisible(widgetName, visible) {
            var button = ASPxClientControl.GetControlCollection().GetByName('widgetButton_' + widgetName);
            button.GetMainElement().className = visible ? '' : 'disabled';

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




            //  cw.onmousehover.style.bordercolor='black'


            //           cw.mouseover(function () {
            //               cw.css("background-color", "yellow");
            //            });
            //            cw.mouseout(function () {
            //                cw.css("background-color", "black");
            //            });
            //  cw.Styles.header.HoverStyle.BackColor = 'blue';

            // cw.style.Bordercolor = 'black';
            // cw.style.border = 'transparent';

            //            cw.style.Paddings.Padding = 0;
            //            cw.style.Border.BorderWidth = 0;
            //            cw.style.Border.BorderStyle = BorderStyle.None;
            //            cw.style.Border.Bordercolor = 'transparent';
            //   cw.showborder = false;
            //  cw.PerformCallback = true;

            // cw.FloatLocation = new Point(100, 100);

        }




        function UpdateClientLayout(s, e) {
            dockManager.PerformCallback();
        }

//        function ShowProperties() {
//            var panel = GetCurrentDockPanel();
//            var item = cbDockZones.FindItemByValue(panel.IsDocked() ? panel.GetOwnerZone().zoneUID : 'none');
//            cbDockZones.SetSelectedIndex(item.index);
//            cbPanelVisibility.SetValue(panel.GetVisible());
//            seVisibleIndex.SetValue(panel.GetVisibleIndex());
//        }
        //        function ChangeVisibleIndex() {
        //            var panel = GetCurrentDockPanel();
        //            panel.SetVisibleIndex(seVisibleIndex.GetValue());
        //        }

        //        function EndDragging(s)
        //        
        //         {

        //            var zone = s.GetOwnerZone();

        //            if (zone) {

        //                zone.SetWidth(s.GetWidth());

        //                zone.SetHeight(s.GetHeight());
        //               zone.SetVisibleIndex(s.GetVisibleIndex));

        //            
        //        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" align="right">
        <tr>
            <td align="right">
                <table id="menutable" runat="server">
                    <tr>
                        <td align="right">
                            <dx:ASPxMenu ID="ASPxMenu1" runat="server" EnableTheming="True" HorizontalAlign="Right"
                                OnItemClick="ASPxMenu1_ItemClick" ShowAsToolbar="True" Theme="Moderno">
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
                    <div>
                        <dx:ASPxDockPanel ID="DominoDock" runat="server" PanelUID="Domino metrics" HeaderText="Domino Server Metrics" 
                            ClientInstanceName="Domino metrics" OwnerZoneUID="LeftZone" BackColor="Transparent" VisibleIndex="0" 
                             Border-BorderColor="Transparent" ShowCloseButton="false" Styles-Header-BackColor="Transparent" Styles-Header-Border-BorderColor="Transparent"
                             AllowedDockState="DockedOnly" AllowResize="true"  ClientSideEvents-AfterDock="function(s, e) {panelTransparentInit(s);}">
                            <Styles>
                                <Header CssClass="header">
                                </Header>
                            </Styles>
                            
                            <ContentCollection>
                                <dx:PopupControlContentControl>
                                    <table>
                                        <tr>
                                            <td>
                                                <table id="keymetrictable" runat="server">
                                                    <tr>
                                                        <td style="padding-top: 10px">
                                                            <a id="a6" runat="server" class="KeyMetrics" href="~/Dashboard/UserCount.aspx">
                                                                <div style="background-color: Red; width: 144px; float: right; height: 74px; background-image: url('../images/Key 2.jpg');
                                                                    background-position: left bottom;">
                                                                    <table style="float: right;">
                                                                        <tr>
                                                                            <td>
                                                                                <dx:ASPxLabel ID="ASPxLabel4" runat="server" Font-Bold="True" ForeColor="White" Text="User Sessions:">
                                                                                </dx:ASPxLabel>
                                                                            </td>
                                                                            <td>
                                                                                <dx:ASPxLabel ID="UsersLabel" runat="server" Font-Bold="True" ForeColor="White" Text="">
                                                                                </dx:ASPxLabel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </a>
                                                        </td>
                                                        <td style="padding-top: 10px">
                                                            <a id="a7" runat="server" class="KeyMetrics" href="~/Dashboard/ServerDownTime.aspx">
                                                                <div style="width: 144px; float: right; height: 74px; float: right; background-image: url('../images/Key 4.jpg');
                                                                    background-position: left bottom;">
                                                                    <table style="float: right;">
                                                                        <tr>
                                                                            <td style="float: right">
                                                                                <dx:ASPxLabel ID="ASPxLabel6" runat="server" Font-Bold="True" ForeColor="White" Text="Down time this hour">
                                                                                </dx:ASPxLabel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="float: right">
                                                                                <dx:ASPxLabel ID="DwnTimeLabel" runat="server" Font-Bold="True" ForeColor="White"
                                                                                    Text="">
                                                                                </dx:ASPxLabel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </a>
                                                        </td>
                                                        <td style="padding-top: 10px">
                                                            <a id="a8" runat="server" class="KeyMetrics" href="~/Dashboard/MailDeliveryStatus.aspx">
                                                                <div style="background-color: Blue; width: 144px; float: right; height: 74px; background-image: url('../images/Key 1.jpg');
                                                                    background-position: left bottom;">
                                                                    <table style="float: right">
                                                                        <tr>
                                                                            <td style="float: right">
                                                                                <dx:ASPxLabel ID="PendingLabel" runat="server" Font-Bold="True" ForeColor="White"
                                                                                    Text="">
                                                                                </dx:ASPxLabel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="float: right">
                                                                                <dx:ASPxLabel ID="LblDead" runat="server" Font-Bold="True" ForeColor="White" Text="">
                                                                                </dx:ASPxLabel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="float: right">
                                                                                <dx:ASPxLabel ID="HeldLabel" runat="server" Font-Bold="True" ForeColor="White" Text="">
                                                                                </dx:ASPxLabel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </a>
                                                        </td>
                                                        <td style="padding-top: 10px">
                                                            <a id="a9" runat="server" class="KeyMetrics" href="~/Dashboard/ResponseTime.aspx"
                                                                bordertop-bordercolor="Transparent">
                                                                <div style="background-color: Purple; width: 144px; float: right; height: 74px; background-image: url('../images/Key 3.jpg');
                                                                    background-position: left bottom;">
                                                                    <table style="float: right;">
                                                                        <tr>
                                                                            <td style="float: right">
                                                                                <dx:ASPxLabel ID="ASPxLabel12" runat="server" Font-Bold="True" ForeColor="White"
                                                                                    Text="Avg Response Time">
                                                                                </dx:ASPxLabel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="float: right">
                                                                                <dx:ASPxLabel ID="RespLabel" runat="server" Font-Bold="True" ForeColor="White" Text="">
                                                                                </dx:ASPxLabel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </a>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:PopupControlContentControl>
                            </ContentCollection>
                        </dx:ASPxDockPanel>
                    </div>
                </td>
            </tr>
        </table>
    <table style="width:100%"  runat="server">
        <tr>
            <td style="width:100%"  runat="server">
        
            <dx:ASPxDockPanel ID="CloudDock" runat="server" PanelUID="Cloud Applications" ClientInstanceName="Cloud Applications"  
                    OwnerZoneUID="LeftZone" HeaderText="Cloud Applications" BackColor="Transparent" VisibleIndex="1" Styles-Header-BackColor="Transparent" Styles-Header-Border-BorderColor="Transparent"
                    Border-BorderColor="Transparent" AllowedDockState="DockedOnly" AllowResize="true" ShowCloseButton="false" ClientSideEvents-AfterDock="function(s, e) {panelTransparentInit(s);}">
                    <Styles>
                        <Header CssClass="header">
                        </Header>
                        
                    </Styles>
                   
                    <ContentCollection>
                        <dx:PopupControlContentControl>
                            <table>

                                <tr>
                                    <td align="right" rowspan="2" valign="top">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <dx:ASPxDataView ID="ASPxDataView2" runat="server" AllowPaging="False" Layout="Flow"
                                            ItemStyle-Wrap="True" Width="100%">
                                            <Paddings Padding="0px" />
                                            <PagerSettings ShowNumericButtons="False">
                                            </PagerSettings>
                                            <ItemTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <a href='<%# Eval("url") %>&LastDate=<%# Eval("LastUpdate") %>&Type=Cloud' class='tooltip2'>
                                                                <asp:Image ID="Image1" runat="server" Height="90px" Width="90px" ImageUrl='<%# Eval("ImageURL") %>'
                                                                    onmousemove="findPos(this,event,'Image1', 'detailsspan');" />
                                                                <asp:Label ID="detailsspan" class="span2"  runat="server">Application Name: <font
                                                                    style="color: blue; font-size: 16px;"><b>
                                                                        <asp:Label ID="NameLabel" Text='<%# Eval("Name") %>' runat="server"></asp:Label></b></font><br />
                                                                    Current Status:
                                                                    <asp:Label ID="Status" runat="server" Text='<%# Eval("StatusCode") %>'></asp:Label><br />
                                                                    Last Scan:
                                                                    <asp:Label ID="Scandetails" runat="server" Text='<%# Eval("Lastupdate") %>'></asp:Label>
                                                                </asp:Label>
                                                            </a>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding: 5px 5px 5px 5px">
                                                            <div id="statusdiv" class="OKUL" runat="server">
                                                                <asp:Label ID="CStatus" runat="server" Text='<%# Eval("StatusCode") %>' OnDataBinding="CStatus_DataBinding"
                                                                    Style="padding: 5px 5px 5px 5px"> </asp:Label>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <Paddings Padding="0px"></Paddings>
                                            <ContentStyle BackColor="Transparent">
                                            </ContentStyle>
                                            <ItemStyle BackColor="Transparent" HorizontalAlign="Center" VerticalAlign="Top">
                                                <Border BorderWidth="0px"></Border>
                                                <Paddings Padding="0px"></Paddings>
                                            </ItemStyle>
                                            <EmptyDataTemplate>
                                                No Applications to be monitored.</EmptyDataTemplate>
                                            <Border BorderWidth="0px"></Border>
                                        </dx:ASPxDataView>
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxDockPanel>
                
            </td>
        </tr>
    </table>
    <div onmouseover="this.bgColor='black'">
        <table id="third"  onmouseover="this.bgColor='black'">
            <tr>
                <td>
                    <dx:ASPxDockPanel ID="PremisesDock" runat="server" PanelUID="On-Premises Applications"
                        ClientInstanceName="On-Premises Applications" HeaderText="On-Premises Applications" VisibleIndex="0"
                        CssClass="divhover" OwnerZoneUID="RightZone" BackColor="Transparent" ShowCloseButton="false" 
                        Border-BorderColor="Transparent" Styles-ControlStyle-HoverStyle-Border-BorderColor="Black" 
                        Styles-Style-HoverStyle-Border-BorderColor="Black" AllowedDockState="DockedOnly" AllowResize="true" ClientSideEvents-AfterDock="function(s, e) {panelTransparentInit(s);}">
                        <Styles>
                            <Header CssClass="header">
                            </Header>
                        </Styles>
                        <ClientSideEvents Init="function(s, e) {panelTransparentInit(s);}" />
                        <ContentCollection>
                            <dx:PopupControlContentControl>
                                <table>
                                    <%--<tr>
                                    <td align="left" colspan="2">
                                        <div class="header" runat="server" id="divOnPrem">
                                            On-Premises Applications</div>
                                    </td>
                                </tr>--%>
                                    <tr>
                                        <td colspan="2">
                                            <table id="tablebutons" runat="server" style="width: 100%;">
                                                <tr>
                                                    <td>
                                                        <div id="headline">
                                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" OnUnload="UpdatePanel_Unload">
                                                                <ContentTemplate>
                                                                    <table style="width: 100%; vertical-align: middle">
                                                                        <tr>
                                                                            <td width="410px">
                                                                                <table>
                                                                                    <tr>
                                                                                        <td style="padding-left: 5px">
                                                                                            <%--<img id="imgButton1" alt="" src="../images/viewby.png" />--%>
                                                                                            <dx:ASPxButton ID="imgButton1" runat="server" Text="See a different view" AutoPostBack="False"
                                                                                                Width="200px" BackColor="#287070" Font-Bold="False" ForeColor="White" EnableDefaultAppearance="False"
                                                                                                Font-Names="Arial" Font-Size="Medium" Height="30px" Cursor="pointer">
                                                                                                <HoverStyle BackColor="#294545">
                                                                                                </HoverStyle>
                                                                                                <FocusRectPaddings PaddingLeft="3px" />
                                                                                                <BackgroundImage HorizontalPosition="left" ImageUrl="../images/imagesIcons/map.png"
                                                                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                                                                                <Border BorderColor="#294545" BorderWidth="2px" />
                                                                                            </dx:ASPxButton>
                                                                                            <dx:ASPxPopupControl ID="ASPxPopupControl1" HeaderText="ViewBy" runat="server" ClientInstanceName="popcontrolviewby"
                                                                                                PopupElementID="imgButton1" Height="60px" PopupHorizontalAlign="LeftSides" PopupVerticalAlign="Below"
                                                                                                EnableHierarchyRecreation="True">
                                                                                                <ContentCollection>
                                                                                                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" Width="100%">
                                                                                                        <table class="style2">
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <cc1:Accordion ID="Accordion1" FadeTransitions="true" FramesPerSecond="40" TransitionDuration="250"
                                                                                                                        runat="server" Width="120%">
                                                                                                                        <Panes>
                                                                                                                            <cc1:AccordionPane ID="AccordionPane1" runat="server" Enabled="true">
                                                                                                                                <Header>
                                                                                                                                    <asp:LinkButton CssClass="viewby" ID="lnkViewByServerType" OnClick="lnkViewByServerType_Click"
                                                                                                                                        CommandName="ViewBy ServerType" Text="View By Server Type" runat="server" />
                                                                                                                                </Header>
                                                                                                                                <Content>
                                                                                                                                    &nbsp; &nbsp;
                                                                                                                                    <asp:LinkButton CssClass="viewby" ID="lnkbtnFilterByLoc" OnClick="lnkbtnFilterByLoc_Click"
                                                                                                                                        CommandName="ServerType" Text="Filter By Location" runat="server" />
                                                                                                                                    <br />
                                                                                                                                    &nbsp; &nbsp;
                                                                                                                                    <asp:LinkButton CssClass="viewby" ID="lnkMyServers2" OnClick="lnkMyServers2_Click"
                                                                                                                                        Text="My Servers" runat="server" />
                                                                                                                                    <br />
                                                                                                                                    &nbsp; &nbsp;
                                                                                                                                    <asp:LinkButton CssClass="viewby" ID="lnkAllServers2" OnClick="lnkAllServers2_Click"
                                                                                                                                        Text="All Servers" runat="server" />
                                                                                                                                </Content>
                                                                                                                            </cc1:AccordionPane>
                                                                                                                            <cc1:AccordionPane ID="AccordionPane2" runat="server" Width="100%" Enabled="true">
                                                                                                                                <Header>
                                                                                                                                    <asp:LinkButton CssClass="viewby" ID="lnkbtnViewByLoc" Text="View By Server Location"
                                                                                                                                        CommandName="ViewBy ServerLocation" runat="server" OnClick="lnkbtnViewByLoc_Click" />
                                                                                                                                </Header>
                                                                                                                                <Content>
                                                                                                                                    &nbsp; &nbsp;
                                                                                                                                    <asp:LinkButton CssClass="viewby" ID="lnkbtnFilterByType" OnClick="lnkbtnFilterByType_Click"
                                                                                                                                        Text="Filter By Server Type" CommandName="Location" runat="server" />
                                                                                                                                    <br />
                                                                                                                                    &nbsp; &nbsp;
                                                                                                                                    <asp:LinkButton CssClass="viewby" ID="lnkMyServers1" OnClick="lnkMyServers1_Click"
                                                                                                                                        Text="My Servers" runat="server" />
                                                                                                                                    <br />
                                                                                                                                    &nbsp; &nbsp;
                                                                                                                                    <asp:LinkButton CssClass="viewby" ID="lnkAllServers1" OnClick="lnkAllServers1_Click"
                                                                                                                                        Text="All Servers" runat="server" />
                                                                                                                                    <br />
                                                                                                                                </Content>
                                                                                                                            </cc1:AccordionPane>
                                                                                                                            <cc1:AccordionPane ID="AccordionPane3" runat="server" Width="100%" Enabled="true">
                                                                                                                                <Header>
                                                                                                                                    <asp:LinkButton CssClass="viewby" ID="CategoryLinkButton" Text="View By Category"
                                                                                                                                        CommandName="ViewBy Category" runat="server" OnClick="CategoryLinkButton_Click" />
                                                                                                                                </Header>
                                                                                                                                <Content>
                                                                                                                                    &nbsp; &nbsp;
                                                                                                                                    <asp:LinkButton CssClass="viewby" ID="Linkfilterbyloc" OnClick="Linkfilterbyloc_Click"
                                                                                                                                        Text="Filter By Location" CommandName="Location" runat="server" />
                                                                                                                                    <br />
                                                                                                                                    &nbsp; &nbsp;
                                                                                                                                    <asp:LinkButton CssClass="viewby" ID="Linkmyservers" OnClick="Linkmyservers_Click"
                                                                                                                                        Text="My Servers" runat="server" />
                                                                                                                                    <br />
                                                                                                                                    &nbsp; &nbsp;
                                                                                                                                    <asp:LinkButton CssClass="viewby" ID="LinkAllServers" OnClick="LinkAllServers_Click"
                                                                                                                                        Text="All Servers" runat="server" />
                                                                                                                                    <br />
                                                                                                                                </Content>
                                                                                                                            </cc1:AccordionPane>
                                                                                                                        </Panes>
                                                                                                                    </cc1:Accordion>
                                                                                                                    &nbsp;
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    &nbsp;
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </dx:PopupControlContentControl>
                                                                                                </ContentCollection>
                                                                                            </dx:ASPxPopupControl>
                                                                                        </td>
                                                                                        <td style="padding-left: 5px">
                                                                                            <%--<img id="imgbutton2" alt="" src="../images/filterby.png"  />--%>
                                                                                            <%--<dx:ASPxButton ID="imgbutton2" runat="server" Text="Filter the view" 
                                   AutoPostBack="False" Width="200px" >
                               </dx:ASPxButton>--%>
                                                                                            <dx:ASPxButton ID="imgbutton2" runat="server" Text="Filter the view" AutoPostBack="False"
                                                                                                Width="200px" BackColor="#287070" Font-Bold="False" ForeColor="White" EnableDefaultAppearance="False"
                                                                                                Font-Names="Arial" Font-Size="Medium" Height="30px" Cursor="pointer">
                                                                                                <Paddings PaddingLeft="3px" />
                                                                                                <HoverStyle BackColor="#294545">
                                                                                                </HoverStyle>
                                                                                                <BackgroundImage HorizontalPosition="left" ImageUrl="~/images/imagesIcons/contrast.png"
                                                                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                                                                                <Border BorderColor="#294545" BorderWidth="2px" />
                                                                                            </dx:ASPxButton>
                                                                                            <dx:ASPxPopupControl ID="PopupControl2" HeaderText="Filter Selector" runat="server"
                                                                                                ClientInstanceName="PopupControlFilterBy" PopupElementID="imgbutton2" PopupHorizontalAlign="LeftSides"
                                                                                                PopupVerticalAlign="Below">
                                                                                                <ContentCollection>
                                                                                                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                                                                                                        <asp:Panel ID="panel3" runat="server">
                                                                                                            <table width="100%">
                                                                                                                <tr>
                                                                                                                    <td valign="top" style="color: #666666; width: 100%">
                                                                                                                        <dx:ASPxCheckBoxList ID="checkBoxList1" RepeatColumns="2" runat="server" Width="100%"
                                                                                                                            TextWrap="False" EnableClientSideAPI="True">
                                                                                                                        </dx:ASPxCheckBoxList>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td align="left">
                                                                                                                        <table>
                                                                                                                            <tr>
                                                                                                                                <td>
                                                                                                                                    <dx:ASPxButton ID="btnSeleCtAll" runat="server" Text="Select All" OnClick="btnSeleCtAll_Click"
                                                                                                                                        Wrap="False" BorderBottom-BorderColor="Transparent">
                                                                                                                                    </dx:ASPxButton>
                                                                                                                                </td>
                                                                                                                                <td>
                                                                                                                                    <dx:ASPxButton ID="btnCancel" runat="server" Text="Reset Selection" OnClick="Cancel_Click"
                                                                                                                                        Visible="false">
                                                                                                                                    </dx:ASPxButton>
                                                                                                                                </td>
                                                                                                                                <td>
                                                                                                                                    <dx:ASPxButton ID="btnSave" runat="server" Text="Save & Submit" OnClick="btnSave_Click">
                                                                                                                                    </dx:ASPxButton>
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                        </table>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </asp:Panel>
                                                                                                    </dx:PopupControlContentControl>
                                                                                                </ContentCollection>
                                                                                            </dx:ASPxPopupControl>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                            <td align="left">
                                                                            </td>
                                                                            <td align="right">
                                                                                <a id="a5" runat="server" class="LastUpd" href="~/Configurator/ServiceController.aspx">
                                                                                    <div id="lastUpdID" runat="server" class="divColorGreen" style="float: right;">
                                                                                        <dx:ASPxLabel CssClass="divColorGreen" ID="StatusLabel" runat="server">
                                                                                        </dx:ASPxLabel>
                                                                                        :<dx:ASPxLabel CssClass="divColorGreen" ID="DateLabel" runat="server" BorderTop-BorderColor="Transparent">
                                                                                        </dx:ASPxLabel>
                                                                                        <dx:ASPxLabel CssClass="divColorGreen" ID="TimeZoneLabel" runat="server">
                                                                                        </dx:ASPxLabel>
                                                                                    </div>
                                                                                </a>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 100%">
                                                                    <font style="color: Black; font-size: small; font-family: Verdana"><b>
                                                                        <dx:ASPxLabel ID="FilterLabel" runat="server" Text="">
                                                                        </dx:ASPxLabel>
                                                                    </b>.
                                                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="( * ) Servers labeled with an asterisk appear in multiple categories to reflect their secondary role.">
                                                                        </dx:ASPxLabel>
                                                                    </font>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" OnUnload="UpdatePanel_Unload">
                                                            <ContentTemplate>
                                                                <dx:ASPxDataView ID="ASPxDataView1" runat="server" AllowPaging="False" Layout="Flow"
                                                                    OnDataBinding="ASPxDataView1_DataBinding" ItemStyle-Wrap="True">
                                                                    <Paddings Padding="0px" />
                                                                    <ContentStyle BackColor="#f8f0c0" />
                                                                    <PagerSettings ShowNumericButtons="False">
                                                                    </PagerSettings>
                                                                    <ItemTemplate>
                                                                        <uc1:StatusBox ID="StatusBox2" runat="server" Button1CssClass="button1" Button1Link='<%# GetLink("Not Responding",Eval("TypeLoc")) %>'
                                                                            Button2CssClass="button2" Button2Link='<%# GetLink("OK",Eval("TypeLoc")) %>'
                                                                            Button3CssClass="button3" Button3Link='<%# GetLink("Issue",Eval("TypeLoc"))  %>'
                                                                            Button4CssClass="button4" Button4Link='<%# GetLink("Maintenance",Eval("TypeLoc")) %>'
                                                                            ButtonCssClass="button" Label11CssClass="label11" Label11Text='<%# Eval("Not Responding") %>'
                                                                            Label12CssClass="label12" Label12Text="Not Responding" Label21CssClass="label11"
                                                                            Label21Text='<%# Eval("OK") %>' Label22CssClass="label12" Label22Text="No Issues"
                                                                            Label31CssClass="label41" Label31Text='<%# Eval("Issue") %>' Label32CssClass="label42"
                                                                            Label32Text="Issues" Label41CssClass="label11" Label41Text='<%# Eval("Maintenance") %>'
                                                                            Label42CssClass="label12" Label42Text="In Maintenance" Title='<%# Eval("TypeLoc") %>'
                                                                            TitleCssClass="title" TitleLink='<%# GetLink("ALL",Eval("TypeLoc")) %>' TitleTableCssClass="titletbl"
                                                                            Width="300px" Height="100%" />
                                                                    </ItemTemplate>
                                                                    <Paddings Padding="0px"></Paddings>
                                                                    <ContentStyle BackColor="Transparent">
                                                                    </ContentStyle>
                                                                    <ItemStyle Width="300px" Height="105px">
                                                                        <Paddings Padding="0px" />
                                                                    </ItemStyle>
                                                                    <Border BorderWidth="0px"></Border>
                                                                </dx:ASPxDataView>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxDockPanel>
                </td>
            </tr>
        </table>
    </div>
    <table id="forth">
        <tr>
            <td>
                <dx:ASPxDockPanel ID="NetworkDock" runat="server" PanelUID="Network Infrastructure"
                    ClientInstanceName="Network Infrastructure" HeaderText="Network Infrastructure" VisibleIndex="1"
                    OwnerZoneUID="MiddleZone" BackColor="Transparent"  Border-BorderColor="Transparent" AllowedDockState="DockedOnly" AllowResize="true" ShowCloseButton="false" ClientSideEvents-AfterDock="function(s, e) {panelTransparentInit(s);}">
                    <Styles>
                        <Header CssClass="header">
                        </Header>
                    </Styles>
                    <ClientSideEvents Init="function(s, e) {panelTransparentInit(s);}" />
                    <ContentCollection>
                        <dx:PopupControlContentControl>
                            <table>
                                <%-- <tr>
                                    <td align="left" colspan="2">
                                        <div class="header" runat="server" id="divnetwrkInf">
                                            Network Infrastructure</div>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td>
                                        <dx:ASPxDataView ID="ASPxDataView3" runat="server" AllowPaging="False" Layout="Flow" 
                                            ItemStyle-Wrap="True">
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
                                                                </asp:Label>
                                                            </a>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding: 5px 5px 5px 5px">
                                                            <div id="statusdiv" class="OKUL" runat="server">
                                                                <asp:Label ID="CStatus" runat="server" Text='<%# Eval("StatusCode") %>' OnDataBinding="NDStatus_DataBinding"
                                                                    Style="padding: 5px 5px 5px 5px"> </asp:Label>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <Paddings Padding="0px"></Paddings>
                                            <ContentStyle BackColor="Transparent">
                                            </ContentStyle>
                                            <ItemStyle BackColor="Transparent" HorizontalAlign="Center" VerticalAlign="Top">
                                                <Border BorderWidth="0px"></Border>
                                                <Paddings Padding="0px"></Paddings>
                                            </ItemStyle>
                                            <EmptyDataTemplate>
                                                No Applications to be monitored.</EmptyDataTemplate>
                                            <Border BorderWidth="0px"></Border>
                                        </dx:ASPxDataView>
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxDockPanel>
            </td>
        </tr>
    </table>
    <dx:ASPxDockZone ID="ASPxDockZone1" runat="server" ZoneUID="LeftZone" 
        Height="50%" Width="50%" >
    </dx:ASPxDockZone>
    <dx:ASPxDockZone runat="server" ID="ASPxDockZone2" ZoneUID="RightZone"  Width="100%"
       >
    </dx:ASPxDockZone>
    <dx:ASPxDockZone ID="ASPxDockZone3" runat="server" ZoneUID="MiddleZone" 
       Width="100%" >
    </dx:ASPxDockZone>
     
    <div onmouseover="this.bgColor='black'">
        <dx:ASPxDockManager ID="dockManager" runat="server" ClientInstanceName="dockManager"   ClientSideEvents-AfterDock="function(s, e) { s.PerformCallback(); }" 
            FreezeLayout="false" OnClientLayout="dockManager_ClientLayout">
        </dx:ASPxDockManager>
    </div>
</asp:Content>
