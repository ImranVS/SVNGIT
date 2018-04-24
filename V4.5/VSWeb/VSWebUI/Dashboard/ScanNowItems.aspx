<%@ Page Title="VitalSigns Plus - Scan Preferences Grid" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
    CodeBehind="ScanNowItems.aspx.cs" Inherits="VSWebUI.Configurator.ScanNowItems" %>
	<%@ MasterType virtualpath="~/Site1.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>



	
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
<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.alert-success').delay(10000).fadeOut("slow", function () {
            });
        });
        </script>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>

	<script type="text/javascript">
		$(document).ready(function () {
			$('.alert-success').delay(10000).fadeOut("slow", function () {
			});
			$('.alert-danger').delay(10000).fadeOut("slow", function () {
			});
		});
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
		//10/14/2014 NS modified for VSPLUS-1022
		function GvScanNowItems_ContextMenu(s, e) {
			if (e.objectType == "row") {
				s.GetRowValues(e.index, 'sname', OnGetRowValues);
				s.SetFocusedRowIndex(e.index);
				StatusListPopup.ShowAtPos(ASPxClientUtils.GetEventX(e.htmlEvent), ASPxClientUtils.GetEventY(e.htmlEvent));
			}
		}
		function OnGetRowValues(Value, e) {
		if (Value != 'ScanDominoASAP') {
				StatusListPopup.GetItemByName("SendConsoleCommand").SetEnabled(false);
			}
			else {
				StatusListPopup.GetItemByName("SendConsoleCommand").SetEnabled(true);
			}
		}
		function GvScanNowItems_FocusedRowChanged(s, e) {
			if (e.objectType != "row") return;
			s.SetFocusedRowIndex(e.index);
		}

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <%-- <dx:ASPxRoundPanel ID="LocationsRoundPanel" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
        CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Sametime Servers"
        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%" Height="250px">
        <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
        <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
        <HeaderStyle Height="23px">
            <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
        </HeaderStyle>
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">--%>
                <table width="100%">
                    <tr>
                        <td>
                            <div class="header" id="servernamelbldisp" runat="server">'Scan Now' Items</div>
                        </td>
                    </tr>
					<tr>
					<td valign="top"><%-- <ClientSideEvents GotFocus="function(s, e) { s.SelectAll() }" />--%>
                

                <dx:ASPxPopupMenu ID="StatusListPopupMenu" runat="server" 
                    PopupAction="LeftMouseClick" 
                    PopupHorizontalAlign="RightSides" PopupVerticalAlign="TopSides" 
                    ClientInstanceName="StatusListPopup" 
                    onitemclick="StatusListPopupMenu_ItemClick" Theme="Moderno">
                <ClientSideEvents Init="InitPopupMenuHandler"></ClientSideEvents>
                  
                                      
                   
                </dx:ASPxPopupMenu>
				</td>
				<td>
            <dx:ASPxPopupControl ID="SuspendPopupControl" runat="server" AllowDragging="True" AllowResize="True"
        CloseAction="CloseButton"
        EnableViewState="False" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" ShowFooter="True" ShowOnPageLoad="False" Width="400px"
        Height="70px" FooterText="To resize the control use the resize grip or the control's edges"
        HeaderText="Suspend Monitoring" ClientInstanceName="FeedPopupControl" 
                    EnableHierarchyRecreation="True" Theme="MetropolisBlue">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControlPopup" runat="server">
                <table id="FeedBackTable" class="EditorsTable" style="width: 100%; height: 100%;">
            <tr>
                <td class="Label">
                    <dx:ASPxLabel ID="lblDuration" runat="server" Text="Duration (mins):"></dx:ASPxLabel>
                </td>
                <td>
                    <dx:ASPxTextBox ID="TbDuration" runat="server" Width="100%" EnableViewState="False">
                    <MaskSettings Mask="&lt;1..120&gt;" />
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
                    <RequiredField ErrorText="Enter Time to Suspend Monitoring." IsRequired="True" />
                    <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                         ValidationExpression="^\d+$" />
                    </ValidationSettings>
                       <%-- <ClientSideEvents GotFocus="function(s, e) { s.SelectAll() }" />--%>
                    </dx:ASPxTextBox>
                </td>
            </tr>   
            <tr>
                <td class="Label" colspan="2">
                    <dx:ASPxLabel ID="ASPxLabel9" runat="server" 
                        Text="You may temporarily suspend monitoring for a maximum duration of two hours. If you need to suspend monitoring for more than two hours, please use the Maintenance Windows functionality in the VitalSigns Configurator."></dx:ASPxLabel>
                </td>
            </tr>        
            <tr>
			<td></td>
            </tr>
        </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="ConsoleCmdPopupControl" runat="server" 
                    HeaderText="Send Console Command" Height="70px" 
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
                    Theme="MetropolisBlue" Width="400px" AllowDragging="True" 
                    AllowResize="True" CloseAction="CloseButton" 
                    FooterText="To resize the control use the resize grip or the control's edges" 
                    ShowFooter="True">
                    <ContentCollection>
<dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
    <table class="navbarTbl">
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Enter console command:" 
                    Wrap="False">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxTextBox ID="ConsoleCmdTextBox" runat="server" Width="170px">
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <dx:ASPxLabel ID="ASPxLabel11" runat="server" 
                    Text="Note: the console command will be submitted to the server only if you have proper authorization within VitalSigns. You must have Console Command Access flag enabled in your user profile to be able to perform this operation.">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
                        </dx:PopupControlContentControl>
</ContentCollection>
                </dx:ASPxPopupControl>
            </td>
			<td>
                                    <dx:ASPxPopupControl ID="msgPopupControl" runat="server" 
                                        HeaderText="VitalSigns" Modal="True" PopupHorizontalAlign="WindowCenter" 
                                        PopupVerticalAlign="WindowCenter" Width="300px" Height="120px" 
                                        Theme="Glass">
                                        <ContentCollection>
<dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">
    <table class="tableWidth100Percent">
        <tr>
            <td colspan="3">
                <dx:ASPxLabel ID="msgLabel" runat="server" Text="msglbl">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;</td>
            <td align="left">
                &nbsp;</td>
            <td align="left">
                &nbsp;</td>
        </tr>
        <tr>
            <td align="right">
                <dx:ASPxButton ID="YesButton" runat="server" 
                    Text="Yes" Theme="Office2010Blue" Width="70px">
                </dx:ASPxButton>
            </td>
            <td align="left">
                <dx:ASPxButton ID="NOButton" runat="server" Text="No" 
                    Theme="Office2010Blue" Width="70px">
                </dx:ASPxButton>
            </td>
            <td align="left">
                <dx:ASPxButton ID="CancelButton" runat="server" 
                    Text="Cancel" Theme="Office2010Blue" Visible="False" Width="70px">
                </dx:ASPxButton>
            </td>
        </tr>
    </table>
                                            </dx:PopupControlContentControl>
</ContentCollection>
                                    </dx:ASPxPopupControl>
                                    
                                </td>
								</tr>
								<tr>
								
								<td colspan="5">
                                                            <div id="ErrorMsg" class="alert alert-danger" runat="server" style="display: none">The settings were not updated</div>
                                                            <div id="SuccessMsg" class="alert alert-success" runat="server" style="display: none">Temporarily Suspended monitoring</div>
                                                        </td>
								</tr>



				
				  <tr>
                        <td id="gridCell">
                            <div id="successDiv" class="alert alert-success" runat="server" style="display: none">Success.
                            </div>
							<asp:UpdatePanel ID="updatepan1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                            <dx:ASPxGridView runat="server" CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css"
                                CssPostfix="Office2010Silver" AutoGenerateColumns="False" ID="GvScanNowItems"
                                KeyFieldName="svalue" Width="100%" 
                                Cursor="pointer" Theme="Office2003Blue">
								<ClientSideEvents ContextMenu="GvScanNowItems_ContextMenu">
                        </ClientSideEvents>
                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="Sname" FieldName="sname" 
                                        VisibleIndex="0">
                                        <Settings AutoFilterCondition="Contains" />
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
										 
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Svalue" VisibleIndex="1"
                                        FieldName="svalue">
                                        <Settings AutoFilterCondition="Contains" />
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Stype" VisibleIndex="2"
                                        FieldName="stype">
                                        <Settings AutoFilterCondition="Contains" />
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    
                                    <dx:GridViewDataTextColumn Caption="Enable Report" FieldName="EnableReport" 
                                        VisibleIndex="3">
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss2">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Scan Interval" FieldName="ScanInterval" VisibleIndex="4" 
                                        Visible="False">
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Priority" FieldName="Priority" 
                                        VisibleIndex="5">
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss2">
                                        </CellStyle>
										</dx:GridViewDataTextColumn>
                                </Columns>
                                <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn"  AllowSelectByRowClick="True" 
                                    ProcessSelectionChangedOnServer="True"  AllowFocusedRow="true"/>
                                <Settings ShowGroupPanel="True"
                                    UseFixedTableLayout="True" />
                                <SettingsText ConfirmDelete=" 'Are you sure you want to delete?'" />
                               
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
                                <Styles>
                                    <Header SortingImageSpacing="5px" ImageSpacing="5px">
                                    </Header>
                                    <GroupRow Font-Bold="True">
                                    </GroupRow>
                                    <AlternatingRow CssClass="GridAltRow" Enabled="True">
                        </AlternatingRow>
                                    <LoadingPanel ImageSpacing="5px">
                                    </LoadingPanel>
                                </Styles>
                                <StylesEditors ButtonEditCellSpacing="0">
                                    <ProgressBar Height="21px">
                                    </ProgressBar>
                                </StylesEditors>
                                <SettingsPager PageSize="50" SEOFriendly="Enabled" >
            <PageSizeItemSettings Visible="true" />
        </SettingsPager>
                            </dx:ASPxGridView>
							</ContentTemplate>
							</asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            
		<dx:ASPxGlobalEvents ID="ge" runat="server">
            <ClientSideEvents ControlsInitialized="InitalizejQuery" EndCallback="InitalizejQuery" />
        </dx:ASPxGlobalEvents>
                        </td>
                    </tr>
                </table>
           <%-- </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>--%>
</asp:Content>
