<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebReport1.aspx.cs" Inherits="VSWebUI.Dashboard.WebReport1" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>



<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx1" %>

<%@ Register Src="~/Controls/StatusBoxHeader.ascx" TagName="StatusBoxHeader" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="shortcut icon" href="../images/favicon.ico" />
    
    <link rel="stylesheet" type="text/css" href="../css/style.css" />
    <link rel="stylesheet" type="text/css" href="../css/control.css" />
    <link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
    <link rel="stylesheet" type="text/css" href="../css/menu_style.css" />
    <script type="text/JavaScript">
<!--
        function timedRefresh(timeoutPeriod) {
            //setTimeout("location.reload(true);", timeoutPeriod);
            setTimeout("window.location.href=window.location.href;", timeoutPeriod);
        }
//   -->
    </script>
    <style type="text/css">
        .menumd
        {
            /*height: 2em;*/
            /*position: absolute;*/
            position: relative;
            /*top: 94px;*/
            width: 100%;
            background-color: #444;
            z-index: 1000;
        }
        .fixedmd
        {
            position: fixed;
            top: 0;
        }
        .dxpnl-expanded
        {
        	position: static!important;
        }
        .dxpnlControl.dxpnl-bar
        {
			padding: 4px!important;
		}
    </style>
    
      <style type="text/css">
   html, body, iframe { margin:0; padding:0; height:98%; }
   iframe { display:block; width:100%; border:none; }
  </style>
</head>
<body id="Body1" runat="server">
    <form id="form1" runat="server">
    <div style="background-color: #444;">
            <table width="100%" style="border-collapse: collapse; background-color: #444;">
                <tr>
                    <td style="background-color: #045FB4; padding-left: 100px" align="left">
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                                                    </asp:ScriptManager>
    <asp:Timer ID="timerupdate" runat="server" Enabled=false>
    </asp:Timer>
                        <table style="background-color: #045FB4;" border="0">
                            <tr>
                                <td rowspan="2" align="right" valign="bottom">
                                    <asp:Label ID="ASPLabel1" runat="server" Text="VitalSigns" style="color: White;" 
                                        Font-Names="Segoe UI" Font-Bold="True" Font-Size="48px">
                                    </asp:Label><br />
                                    <asp:Label ID="ASPLabel2" runat="server" Text="Executive Dashboard"
                                    style="color: White; font-size: large;" Font-Names="Segoe UI" Font-Size="Medium" 
                                        Font-Bold="True"></asp:Label>
                                            </td>
                                <td rowspan="2">
                                    &nbsp;
                                </td>
                                <td rowspan="2" valign="bottom">
                                <asp:UpdatePanel ID="updatepan1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <uc1:StatusBoxHeader ID="StatusBox1" runat="server" Button1CssClass="button1" Button1Link="~/Dashboard/DeviceTypeList.aspx?status=Not Responding"
                                        Button2CssClass="button2" Button2Link="~/Dashboard/DeviceTypeList.aspx?status=OK"
                                        Button3CssClass="button3" Button3Link="~/Dashboard/DeviceTypeList.aspx?status=Issue"
                                        Button4CssClass="button4" Button4Link="~/Dashboard/DeviceTypeList.aspx?status=Maintenance"
                                        ButtonCssClass="button" Height="100%" Label11CssClass="label11" Label11Text="20"
                                        Label12CssClass="label12" Label12Text="Not Responding" Label21CssClass="label11"
                                        Label21Text="10" Label22CssClass="label12" Label22Text="No Issues" Label31CssClass="label41"
                                        Label31Text="3" Label32CssClass="label42" Label32Text="Issues" Label41CssClass="label11"
                                        Label41Text="4" Label42CssClass="label12" Label42Text="In Maintenance" Width="300px" />
                                </ContentTemplate>
                                <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="timerupdate" />
                                </Triggers>
                                </asp:UpdatePanel>
                                    
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                        <td valign="bottom">
                                                <asp:TextBox ID="SearchTextBox" runat="server" Width="250px"></asp:TextBox>
                                            </td>
                                            <td valign="bottom">
                                                <dx:ASPxButton ID="ASPxButton1" runat="server" BackColor="Transparent" 
                                                    onclick="ASPxButton1_Click" Height="40px" Width="40px">
                                                    <BackgroundImage ImageUrl="~/images/zoom.png" Repeat="NoRepeat" />
                                                    <Border BorderWidth="0px" />
                                                </dx:ASPxButton>
                                            </td>
                            </tr>
                            </table>
                    </td>
                </tr>
                
            </table>
        </div>
    <div class="menumd">
        <table width="100%" style="background-color: AliceBlue">
        <tr>
                    <td>
                        <dx:ASPxMenu ID="ASPxMenu2" runat="server" AutoSeparators="RootOnly" 
                            EnableTheming="True" BorderBetweenItemAndSubMenu="HideAll" Theme="Moderno">
                            <Items>
                                <dx:MenuItem Text="Overall Health">
                                    <Items>
                                        <dx:MenuItem Text="Overview" NavigateUrl="~/Dashboard/OverallHealth1.aspx" Target="_self">
                                            <Image Url="images/icons/dash.png">
                                            </Image>
                                        </dx:MenuItem>
                                        <dx:MenuItem Text="Executive Summary" NavigateUrl="~/Dashboard/SummaryLandscape.aspx" Target="_self">
                                             <Image Url="images/icons/dash.png">
                                            </Image>
                                        </dx:MenuItem>
                                        <dx:MenuItem Text="EXJournal Summary" NavigateUrl="~/Dashboard/SummaryEXJournal.aspx" Target="_self">
                                             <Image Url="images/icons/dash.png">
                                            </Image>
                                        </dx:MenuItem>
                                        <dx:MenuItem Text="Mail Delivery Status" NavigateUrl="~/Dashboard/MailDeliveryStatus.aspx" Target="_self">
                                             <Image Url="images/icons/dash.png">
                                            </Image>
                                        </dx:MenuItem>
                                        <dx:MenuItem Text="Status List" NavigateUrl="~/Dashboard/DeviceTypeList.aspx" Target="_self">
                                            <Image Url="images/icons/detail_list.png">
                                            </Image>
                                        </dx:MenuItem>
                                    </Items>
                                </dx:MenuItem>
                                <dx:MenuItem Text="Server Health">
                                    <Items>
                                        <dx:MenuItem Text="Domino Health" NavigateUrl="~/Dashboard/DominoServerHealthPage.aspx"
                                            Target="_self">
                                            <Image Url="images/icons/dominoserver.gif">
                                            </Image>
                                        </dx:MenuItem>
                                        <dx:MenuItem Text="Traveler Health" NavigateUrl="~/Dashboard/TravelerUsersDevicesOS.aspx"
                                            Target="_self">
                                            <Image Url="images/icons/traveler.png">
                                            </Image>
                                        </dx:MenuItem>
                                        <dx:MenuItem Text="Quickr Health" NavigateUrl="~/Dashboard/QuickrHealth.aspx" Target="_self">
                                            <Image Url="images/icons/quickr.gif">
                                            </Image>
                                        </dx:MenuItem>
                                        <dx:MenuItem Text="Sametime Server Health" NavigateUrl="~/Dashboard/SametimeHealth.aspx"
                                            Target="_self">
                                            <Image Url="images/icons/sametime.gif">
                                            </Image>
                                        </dx:MenuItem>
                                    </Items>
                                </dx:MenuItem>
                                <dx:MenuItem Text="Key Metrics">
                                    <Items>
                                        <dx:MenuItem Text="Disk Health" NavigateUrl="~/Dashboard/DiskHealth.aspx" Target="_self">
                                            <Image Url="images/icons/network.gif">
                                            </Image>
                                        </dx:MenuItem>
                                        <dx:MenuItem Text="Mail Health" NavigateUrl="~/Dashboard/MailHealth.aspx" Target="_self">
                                            <Image Url="images/icons/email.png">
                                            </Image>
                                        </dx:MenuItem>
                                        <dx:MenuItem Text="Cluster Health" NavigateUrl="~/Dashboard/ClusterHealth.aspx" Target="_self">
                                            <Image Url="images/icons/cluster.gif">
                                            </Image>
                                        </dx:MenuItem>
                                        <dx:MenuItem Text="URL Health" NavigateUrl="~/Dashboard/URLHealth.aspx" Target="_self">
                                            <Image Url="images/icons/url.gif">
                                            </Image>
                                        </dx:MenuItem>
                                        <dx:MenuItem Text="User Counts" NavigateUrl="~/Dashboard/UserCount.aspx" Target="_self">
                                            <Image Url="images/icons/group.png">
                                            </Image>
                                        </dx:MenuItem>
                                        <dx:MenuItem Text="Response Time" NavigateUrl="~/Dashboard/ResponseTime.aspx" Target="_self">
                                            <Image Url="images/icons/chart_line.png">
                                            </Image>
                                        </dx:MenuItem>
                                        <dx:MenuItem Text="Maintenance Windows" NavigateUrl="~/Dashboard/ServerMaintenanceList.aspx"
                                            Target="_self">
                                            <Image Url="images/icons/information.png">
                                            </Image>
                                        </dx:MenuItem>
                                       
                                    </Items>
                                </dx:MenuItem>
                                <dx:MenuItem Text="Database Health">
                                    <Items>
                                        <dx:MenuItem Text="All Databases" NavigateUrl="~/Dashboard/MonitoredDB.aspx" Target="_self">
                                            <Image Url="images/icons/detail_list.png">
                                            </Image>
                                        </dx:MenuItem>                                       
                                        <dx:MenuItem Text="Replication Disabled" NavigateUrl="~/Dashboard/Replication.aspx"
                                            Target="_self">
                                            <Image Url="images/icons/detail_list.png">
                                            </Image>
                                        </dx:MenuItem>
                                        <dx:MenuItem Text="By Template" NavigateUrl="~/Dashboard/DBsByTemplate.aspx" Target="_self">
                                            <Image Url="images/icons/detail_list.png">
                                            </Image>
                                        </dx:MenuItem>
                                        <dx:MenuItem Text="Problems" NavigateUrl="~/Dashboard/Problems.aspx" Target="_self">
                                            <Image Url="images/icons/exclamation.png">
                                            </Image>
                                        </dx:MenuItem>
                                      <%--  <dx:MenuItem Text="Directory Health" NavigateUrl="~/Dashboard/OverallHealth1.aspx"
                                            Target="_self">
                                            <Image Url="images/icons/nab.gif">
                                            </Image>
                                        </dx:MenuItem>--%>
                                    </Items>
                                </dx:MenuItem>
                                <dx:MenuItem Text="Mail Files">
                                    <Items>
                                        <dx:MenuItem Text="Alphabetical Order" NavigateUrl="~/Dashboard/MailFiles.aspx?MItem=1"
                                            Target="_self">
                                            <Image Url="images/icons/email.png">
                                            </Image>
                                        </dx:MenuItem>
                                            <dx:MenuItem Text="Biggest Mail Files" NavigateUrl="~/Dashboard/BiggestMailFiles.aspx"
                                            Target="_self">
                                            <Image Url="images/icons/email.png">
                                            </Image>
                                        </dx:MenuItem>
                                        <dx:MenuItem Text="By Inbox Count" NavigateUrl="~/Dashboard/MailFiles.aspx?MItem=2"
                                            Target="_self">
                                            <Image Url="images/icons/email.png">
                                            </Image>
                                        </dx:MenuItem>
                                        <dx:MenuItem Text="By Percent of Quota" NavigateUrl="~/Dashboard/MailFiles.aspx?MItem=3"
                                            Target="_self">
                                            <Image Url="images/icons/exclamation.png">
                                            </Image>
                                        </dx:MenuItem>
                                    </Items>
                                </dx:MenuItem>
                                <dx:MenuItem Text="Reports" NavigateUrl="~/Configurator/Reports.aspx?M=d">
                                </dx:MenuItem>
                                <dx:MenuItem Name="dashboard" Text="Configurator" NavigateUrl="~/Configurator/Default.aspx"
                                    Target="_self">
                                </dx:MenuItem>
                                <dx:MenuItem Text="My Account">
                                    <Items>
                                        <dx:MenuItem NavigateUrl="~/Security/MyAccount.aspx?dboard=true" Text="  Account Details"
                                            Target="_self">
                                        </dx:MenuItem>
                                        <dx:MenuItem NavigateUrl="~/Dashboard/MyCustomPages.aspx" Text="  My Custom Pages"
                                            Target="_self">
                                        </dx:MenuItem>
                                        <dx:MenuItem NavigateUrl="~/Login.aspx" Text="  Logout">
                                        </dx:MenuItem>
                                    </Items>
                                </dx:MenuItem>
                            </Items>
                        </dx:ASPxMenu>
                    </td>
                    <td class="menu">
                        <div style="float: right;padding-right:10px">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/menulogo.png" />
                            
                        </div>
                    </td>
                </tr>
        </table>
    </div>
    <iframe id="fraHtml" runat="server" />
    <div id="footer">
            <div id="leftnav">
                <p class="two">
                    Copyright <script language="JavaScript" type="text/javascript">
                                  now = new Date
                                  theYear = now.getYear()
                                  if (theYear < 1900)
                                      theYear = theYear + 1900
                                  document.write(theYear)
                            </script>, RPR Wyatt, Inc. All rights reserved.</p>
            </div>
            <div id="rightnav">
                &nbsp;</div>
        </div>
    </form>
</body>
</html>
