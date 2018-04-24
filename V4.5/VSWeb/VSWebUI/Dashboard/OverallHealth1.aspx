 <%@ Page Title="VitalSigns Plus - Overall Health" Language="C#" MasterPageFile="~/DashboardSite.Master"
	AutoEventWireup="true" CodeBehind="OverallHealth1.aspx.cs" Inherits="VSWebUI.Dashboard.OverallHealth1" %>

<%@ MasterType virtualpath="~/DashboardSite.Master" %>

	<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>



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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
	</script>
	 
	<table width="100px" align="right">
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
                                            <dx:MenuItem Name="ViewbyType" Text="View by Server Type">
                                            </dx:MenuItem>
                                            <dx:MenuItem Name="ViewbyLocation" Text="View by Server Location">
                                            </dx:MenuItem>
                                            <dx:MenuItem Name="ViewbyCat" Text="View by Category">
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
				<div class="header" runat="server" id="divOnPrem">
					Status Overview</div>
			</td>
		</tr>
		<tr>
			<td valign="top">
				<table id="tablebutons" runat="server" style="width: 100%;">
					<tr>
						<td>
							<asp:UpdatePanel ID="UpdatePanel2" runat="server">
								<ContentTemplate>
									<dx:ASPxDataView ID="ASPxDataView1" runat="server" AllowPaging="False"
										OnDataBinding="ASPxDataView1_DataBinding" ItemStyle-Wrap="True" Layout="Flow">
										<Paddings Padding="0px" />
										<ContentStyle BackColor="White" />
										<PagerSettings ShowNumericButtons="False">
										</PagerSettings>
										<ItemTemplate>
											<uc1:StatusBox ID="StatusBox2" runat="server" Button1CssClass="button1" Button1Link='<%# GetLink("Not Responding",Eval("TypeLoc"),"false") %>'
												Button2CssClass="button2" Button2Link='<%# GetLink("OK",Eval("TypeLoc"),"false") %>'
												Button3CssClass="button3" Button3Link='<%# GetLink("Issue",Eval("TypeLoc"),"false")  %>'
												Button4CssClass="button4" Button4Link='<%# GetLink("Maintenance",Eval("TypeLoc"),"false") %>'
												ButtonCssClass="button" Label11CssClass="label11" Label11Text='<%# Eval("Not Responding") %>'
												Label12CssClass="label12" Label12Text="Not Responding" Label21CssClass="label11"
												Label21Text='<%# Eval("OK") %>' Label22CssClass="label12" Label22Text="No Issues"
												Label31CssClass="label41" Label31Text='<%# Eval("Issue") %>' Label32CssClass="label42"
												Label32Text="Issues" Label41CssClass="label11" Label41Text='<%# Eval("Maintenance") %>'
												Label42CssClass="label12" Label42Text="In Maintenance" Title='<%# Eval("TypeLoc") %>'
												TitleCssClass="title" TitleLink='<%# GetLink("ALL",Eval("TypeLoc"),"false") %>' TitleTableCssClass="titletbl"
												Width="300px" Height="100%" Visible="false" />
                                            <table style="border: 1px; border-spacing: 2px; background-color: #F8F8C0">
                                                <tr>
                                                    <td colspan="4">
                                                        <a class="overall" href='<%# GetLink("ALL",Eval("TypeLoc"),"false") %>' onclick='<%# GetLink("ALL",Eval("TypeLoc"),"false") %>'><span><%# Eval("TypeLoc") %></span></a>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <a href='<%# GetLink("Not Responding",Eval("TypeLoc"),"false") %>' class="tooltipH">
                                                            <input type="button" id="NRBtn" class="redheaderButton" value='<%# Eval("Not Responding") %>' runat="server" onclick='<%# GetLink("Not Responding",Eval("TypeLoc"),"true") %>' />  
                                                            <span id="NRLabel" runat="server"><%# Eval("Not Responding") + " server(s) are not responding" %>                   
                                                            </span>
                                                        </a>
                                                    </td>
                                                    <td>
                                                        <a href='<%# GetLink("OK",Eval("TypeLoc"),"false") %>' class="tooltipH">
                                                            <input type="button" id="OKBtn" class="greenheaderButton" value='<%# Eval("OK") %>' runat="server" onclick='<%# GetLink("OK",Eval("TypeLoc"),"true") %>' />
                                                            <span id="OKLabel" runat="server"><%# Eval("OK") + " server(s) have no issues" %>
                                                            </span>
                                                        </a>
                                                    </td>
                                                    <td>
                                                        <a href='<%# GetLink("Issue",Eval("TypeLoc"),"false")  %>' class="tooltipH">
                                                            <input type="button" id="IBtn" class="yellowheaderButton" value='<%# Eval("Issue") %>' runat="server" onclick='<%# GetLink("Issue",Eval("TypeLoc"),"true")  %>' />
                                                            <span id="ILabel" runat="server"><%# Eval("Issue") + " server(s) have issues"%>
                                                            </span>
                                                        </a>
                                                    </td>
                                                    <td>
                                                        <a href='<%# GetLink("Maintenance",Eval("TypeLoc"),"false") %>' class="tooltipH">
                                                            <input type="button" id="MBtn" class="grayheaderButton" value='<%# Eval("Maintenance") %>' runat="server" onclick='<%# GetLink("Maintenance",Eval("TypeLoc"),"true") %>' />
                                                            <span id="MLabel" runat="server"><%# Eval("Maintenance") + " server(s) are in maintenance" %>
                                                            </span>
                                                        </a>
                                                    </td>
                                                </tr>
                                            </table>
                                                
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
                    <tr>
						<td>
							<div id="headline" style="display:none">
								<asp:UpdatePanel ID="UpdatePanel1" runat="server" >
									<ContentTemplate>
										<table style="width: 100%; vertical-align: middle;">
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
																<dx:ASPxButton ID="imgbutton2" runat="server" Text="Filter the view" AutoPostBack="true"
																	Width="200px" BackColor="#287070" Font-Bold="False" ForeColor="White" EnableDefaultAppearance="False"
																	Font-Names="Arial" Font-Size="Medium" Height="30px" Cursor="pointer" >
																	<Paddings PaddingLeft="3px" />
																	<HoverStyle BackColor="#294545">
																	</HoverStyle>
																	<BackgroundImage HorizontalPosition="left" ImageUrl="~/images/imagesIcons/contrast.png"
																		Repeat="NoRepeat" VerticalPosition="center" />
																	<Border BorderColor="#294545" BorderWidth="2px" />
																</dx:ASPxButton>
																<dx:ASPxPopupControl ID="PopupControl2" HeaderText="Filter Selector" runat="server"
																	ClientInstanceName="PopupControlFilterBy" PopupElementID="imgbutton2" PopupHorizontalAlign="LeftSides"
																	PopupVerticalAlign="Below"    >
																	<ContentCollection>
																		<dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server"    >
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
																											Wrap="False">
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
														<div id="lastUpdID" runat="server" class="divColorGreen" style="float: right; display: none;">
															<dx:ASPxLabel   ID="StatusLabel" runat="server">
															</dx:ASPxLabel>
															:<dx:ASPxLabel   ID="DateLabel" runat="server">
															</dx:ASPxLabel>
															<dx:ASPxLabel  ID="TimeZoneLabel" runat="server">
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
                            <table width="100%" style=" display: none">
								<tr>
									<td>
										<font style="color: Black; font-size: small; font-family: Verdana;"><b>
											<dx:ASPxLabel ID="FilterLabel" runat="server" Text="">
											</dx:ASPxLabel>
										</b>
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
                            <table>
                                <tr>
                                    <td>
                                    <div class="info">(*) Servers labeled with an asterisk appear in multiple categories to reflect their secondary role.</div>
                                    </td>
                                </tr>
                            </table>
                            
                        </td>
                    </tr>
				</table>
			</td>
		</tr>
        <tr>
            <td>
				<div class="header" id="servernamelbldisp" runat="server">
					<%--<img alt="" src="../images/icons/dominoserver.gif" />--%>
					Domino Server Metrics</div>
			</td>

        </tr>
        <tr>
            <td valign="top">
				<table id="keymetrictable" runat="server">
                                <tr>
                                    <td align="right">
                                        <div style="background-color: #F2C7BD; width: 100px; height: 20px; padding-left: 2px; 
                                            border: 1px solid; border-color: #F2C7BD; border-top-left-radius: 5px; border-top-right-radius: 5px; display: none">
                                            <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="User Sessions" CssClass="StatusLabelGray">
											</dx:ASPxLabel>
                                        </div>
                                        <div style="background-image: url('../images/Key 2.jpg'); width: 100px; height: 48px; background-size: 100%; background-repeat: no-repeat;
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
                                        <div style="background-image: url('../images/Key 4.jpg'); width: 100px; height: 48px; background-size: 100%; background-repeat: no-repeat;
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
                                        <div style="background-image: url('../images/Key 1.jpg'); width: 100px; height: 48px; background-size: 100%; background-repeat: no-repeat;
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
                                        <div style="background-image: url('../images/Key 3.jpg'); width: 100px; height: 48px; background-size: 100%; background-repeat: no-repeat;
                                            padding-bottom: 5px; padding-right: 2px; border: 1px solid; border-color: #5134AB; border-radius: 5px;">
                                                <a id="a2" runat="server" href="~/Dashboard/ResponseTime.aspx" style="border-style: none; float: right">
                                                    <dx:ASPxLabel ID="RespLabel" runat="server" CssClass="StatusLabelWhite" Text="">
												    </dx:ASPxLabel>
                                                </a>
                                        </div>
                                    </td>
                                </tr>
                           </table>
			</td>
        </tr>
		<tr>
			<td align="left" valign="top">
				<div class="header" runat="server" id="divCloud">
					Cloud Applications</div>
			</td>
			<td align="right" rowspan="2" valign="top">
			</td>
		</tr>
		<tr >
			<td align="left" style="width:auto;" >
				<dx:ASPxDataView ID="ASPxDataView2" runat="server" AllowPaging="False" Layout="Flow"
					ItemStyle-Wrap="True" Width="100%">
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
		<tr>
			<td align="left" colspan="2">
				<div class="header" runat="server" id="divnetwrkInf">
					Network Infrastructure</div>
			</td>
		</tr>
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
</asp:Content>
