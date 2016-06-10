<%@ Page Title="VitalSigns Plus - Mobile Users" Language="C#" MasterPageFile="~/Site1.Master"AutoEventWireup="true" CodeBehind="MobileUsersGrid.aspx.cs" Inherits="VSWebUI.Configurator.MobileUsersGrid" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>




    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
    


<%@ Register assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>

<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<script type="text/javascript">
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
    		NetworkDevicesGridView.SetFocusedRowIndex(e.index);
    		var SortPopupMenu = StatusListPopup;
    		SortPopupMenu.ShowAtPos(evt.clientX + ASPxClientUtils.GetDocumentScrollLeft(), evt.clientY + ASPxClientUtils.GetDocumentScrollTop());
    		return OnPreventContextMenu(evt);
    	}
    	function OnPreventContextMenu(evt) {
    		return ASPxClientUtils.PreventEventAndBubble(evt);
    	}
    	function NetworkDevicesGridView_ContextMenu(s, e) {
    		if (e.objectType == "row") {
    			s.SetFocusedRowIndex(e.index);
    			StatusListPopup.ShowAtPos(ASPxClientUtils.GetEventX(e.htmlEvent), ASPxClientUtils.GetEventY(e.htmlEvent));
    		}
    	}
    	function NetworkDevicesGridView_FocusedRowChanged(s, e) {
    		if (e.objectType != "row") return;
    		s.SetFocusedRowIndex(e.index);
    	}

    	var visibleIndex;
    	function OnCustomButtonClick(s, e) {
    		visibleIndex = e.visibleIndex;

    		if (e.buttonID == "deleteButton")
    			MobUserThGrid.GetRowValues(e.visibleIndex, 'UserName', OnGetRowValues);

    		function OnGetRowValues(values) {
    			var id = values[0];
    			var name = values[1];
    			//5/21/2015 NS modified for VSPLUS-1771
    			var OK = (confirm('Are you sure you want to delete the user - ' + values + '?'))
    			if (OK == true) {

    				MobUserThGrid.DeleteRow(visibleIndex);
    				
    			}

    			else {
    			}

    		}
    	}

    </script>
    <%--<dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" 
        CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
        CssPostfix="Glass" 
        GroupBoxCaptionOffsetY="-24px" 
        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
        Width="100%" HeaderText="Network Devices">
        <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
        <HeaderStyle Height="23px">
        <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
        </HeaderStyle>
        <PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">--%>
<table width="100%">
<tr>
    <td>
        <div class="header" id="servernamelbldisp" runat="server">Mobile Devices</div>
    </td>
</tr>
<tr>
            <td colspan="2">
                <div id="infoDivPersistent" class="info">
                    Right click on the user that you want to monitor in the All Devices list to alert if the sync does not 
                    happen in x number of minutes.</div>
            </td>
        </tr>
<tr>
    <td>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" 
            HeaderText="Critical Devices" Theme="Glass" Width="100%">
            <PanelCollection>
<dx:PanelContent runat="server">
    <table class="navbarTbl">
        <tr>
            <td>
                <dx:ASPxMenu ID="StatusListMenu" runat="server" 
                    OnItemClick="StatusListMenu_ItemClick" Theme="DevEx" Visible="False">
                    <Items>
                        <dx:MenuItem Name="Suspend" Text="Monitor Device">
                        </dx:MenuItem>
                    </Items>
                </dx:ASPxMenu>
                <dx:ASPxPopupMenu ID="StatusListPopupMenu" runat="server" 
                    PopupAction="LeftMouseClick" 
                    PopupHorizontalAlign="RightSides" PopupVerticalAlign="TopSides" 
                    ClientInstanceName="StatusListPopup" 
                    onitemclick="StatusListPopupMenu_ItemClick">
                <ClientSideEvents Init="InitPopupMenuHandler"></ClientSideEvents>
                    <Items>
                        <dx:MenuItem Text="Monitor Device" Name="Suspend">
                        </dx:MenuItem>                       
                    </Items>
                </dx:ASPxPopupMenu>
            </td>
            <td>
                <dx:ASPxPopupControl ID="SuspendPopupControl" runat="server" 
                    AllowDragging="True" AllowResize="True" ClientInstanceName="FeedPopupControl" 
                    CloseAction="CloseButton" EnableHierarchyRecreation="True" 
                    EnableViewState="False" 
                    FooterText="To resize the control use the resize grip or the control's edges" 
                    HeaderText="Sync Duration" Height="70px" PopupHorizontalAlign="WindowCenter" 
                    PopupVerticalAlign="WindowCenter" ShowFooter="True" Width="400px" 
                    Theme="MetropolisBlue">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table ID="FeedBackTable" class="EditorsTable" 
                                style="width: 100%; height: 100%;">
                                <tr>
                                    <td class="Label">
                                        <dx:ASPxLabel ID="lblDuration" runat="server" Text="Duration (mins):" 
                                            Wrap="False">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="TbDuration" runat="server" EnableViewState="False" 
                                            Width="100%">
                                            <MaskSettings Mask="&lt;20..240&gt;" />
                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                    ValidationExpression="^\d+$" />
                                                <RequiredField ErrorText="Enter Sync Duration." IsRequired="True" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Label" colspan="2">
                                        <dx:ASPxLabel ID="ASPxLabel9" runat="server" 
                                            Text="If a device has not synced for more than the amount of duration specified above, an alert will be triggered by the system. Minimum duration value can be 20 minutes and maximum - 240 minutes.">
                                        </dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <dx:ASPxButton ID="test123" runat="server" AutoPostBack="False" 
                                            CausesValidation="False" EnableTheming="True" OnClick="BtnApply_Click" 
                                            Text="Apply" CssClass="sysButton">
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
            <td colspan="2">
                <dx:ASPxGridView ID="MobUserThGrid" runat="server"  ClientInstanceName="MobUserThGrid" AutoGenerateColumns="False" 
                    Cursor="pointer" EnableTheming="True" KeyFieldName="DeviceId" 
                    OnCellEditorInitialize="MobUserThGrid_CellEditorInitialize" 
                    OnPageSizeChanged="MobUserThGrid_PageSizeChanged" 
                    OnRowDeleting="MobUserThGrid_RowDeleting" 
                    OnRowUpdating="MobUserThGrid_RowUpdating" 
                    OnRowValidating="MobUserThGrid_RowValidation" Theme="Office2003Blue" 
                    Width="100%">
					<ClientSideEvents CustomButtonClick="OnCustomButtonClick" />
                    <Columns>
                        <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" 
                            ShowInCustomizationForm="True" VisibleIndex="0" Width="70px">
                            <EditButton Visible="True">
                                <Image Url="../images/edit.png">
                                </Image>
                            </EditButton>
                            <DeleteButton Text="Remove" Visible="false">
                                <Image Url="../images/delete.png">
                                </Image>
                            </DeleteButton>
                            <CancelButton Visible="True">
                                <Image Url="~/images/cancel.gif">
                                </Image>
                            </CancelButton>
                            <UpdateButton Visible="True">
                                <Image Url="~/images/update.gif">
                                </Image>
                            </UpdateButton>
                            <ClearFilterButton Visible="True">
                                <Image Url="~/images/clear.png">
                                </Image>
                            </ClearFilterButton>
                            <HeaderStyle CssClass="GridCssHeader">
                            <Paddings Padding="5px" />
                            </HeaderStyle>
                            <CellStyle>
                                <Paddings Padding="3px" />
                            </CellStyle>
                        </dx:GridViewCommandColumn>
						 <dx:GridViewCommandColumn Caption="Delete" ButtonType="Image" VisibleIndex="1" Width="70px">
                        <CustomButtons>
                            <dx:GridViewCommandColumnCustomButton ID="deleteButton" Text="Delete" 
                                Image-Url="../images/delete.png" >
                             <Image Url="../images/delete.png"></Image>
                            </dx:GridViewCommandColumnCustomButton>
                        </CustomButtons>
						<HeaderStyle CssClass="GridCssHeader" />
                    </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn ShowInCustomizationForm="True" VisibleIndex="2" 
                            Width="30px">
                            <PropertiesTextEdit>
                                <FocusedStyle HorizontalAlign="Center">
                                </FocusedStyle>
                            </PropertiesTextEdit>
                            <EditFormSettings Visible="False" />
                            <DataItemTemplate>
                                <asp:Label ID="lblIcon" runat="server" Text='<%# Eval("OS_Type")%>' 
                                    Visible="false"></asp:Label>
                                <asp:Image ID="IconImage" runat="server" />
                                <dx:ASPxHyperLink ID="ASPxHyperLink2" runat="server" 
                                    NavigateUrl="<%# SetIcon(Container) %>" Visible="false">
                                </dx:ASPxHyperLink>
                            </DataItemTemplate>
                            <HeaderStyle CssClass="GridCssHeader" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="User Name" FieldName="UserName" 
                            ShowInCustomizationForm="True" VisibleIndex="3">
                            <PropertiesTextEdit>
                                <FocusedStyle HorizontalAlign="Center">
                                </FocusedStyle>
                            </PropertiesTextEdit>
                           <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <EditItemTemplate>
                                <dx:ASPxLabel ID="usernameEdit" runat="server" 
                                    value='<%# Bind("UserName") %>' />
                            </EditItemTemplate>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Device Name" FieldName="DeviceName" 
                            ShowInCustomizationForm="True" VisibleIndex="4">
                            <PropertiesTextEdit>
                                <FocusedStyle HorizontalAlign="Center">
                                </FocusedStyle>
                            </PropertiesTextEdit>
                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <EditItemTemplate>
                                <dx:ASPxLabel ID="usernameEdit" runat="server" 
                                    value='<%# Bind("DeviceName") %>' />
                            </EditItemTemplate>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Device ID" FieldName="DeviceId" 
                            ShowInCustomizationForm="True" VisibleIndex="20">
                            <PropertiesTextEdit>
                                <FocusedStyle HorizontalAlign="Center">
                                </FocusedStyle>
                            </PropertiesTextEdit>
                           <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <EditItemTemplate>
                                <dx:ASPxLabel ID="usernameEdit" runat="server" 
                                    value='<%# Bind("DeviceId") %>' />
                            </EditItemTemplate>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Sync Duration (min)" 
                            FieldName="SyncTimeThreshold" ShowInCustomizationForm="True" 
                            VisibleIndex="21">
                            <PropertiesTextEdit>
                                <FocusedStyle HorizontalAlign="Center">
                                </FocusedStyle>
                            </PropertiesTextEdit>
                           <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader2" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" 
                        ColumnResizeMode="NextColumn" />
                    <SettingsPager AlwaysShowPager="True" NumericButtonCount="30" PageSize="50">
                        <PageSizeItemSettings Items="50, 100, 200" Visible="True">
                        </PageSizeItemSettings>
                    </SettingsPager>
                    <Settings ShowFilterRow="True" />
                    <Styles>
                        <Header VerticalAlign="Middle">
                        </Header>
                        <AlternatingRow CssClass="GridAltRow" Enabled="True">
                        </AlternatingRow>
                    </Styles>
                </dx:ASPxGridView>
            </td>
        </tr>
    </table>
                </dx:PanelContent>
</PanelCollection>
        </dx:ASPxRoundPanel>
    </td>
</tr>
<tr>
    <td>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" Width="100%" 
            HeaderText="All Devices" Theme="Glass">
            <PanelCollection>
<dx:PanelContent runat="server">
    <table class="navbarTbl">
        <tr>
            <td>
                <table>
        <tr>
                   
            <td>
                <dx:ASPxButton ID="ExportXlsButton" runat="server" 
                    onclick="ExportXlsButton_Click" Text="Export to XLS" CssClass="sysButton"
                    Wrap="False" Image-Url="~/images/icons/xls.png">
                </dx:ASPxButton>
            </td>
            <td>
                <dx:ASPxButton ID="ExportXlsxButton" runat="server" 
                    onclick="ExportXlsxButton_Click" Text="Export to XLSX" CssClass="sysButton"
                    Wrap="False" Image-Url="~/images/icons/xlsx.png">
                </dx:ASPxButton>
            </td>
            <td>
                <dx:ASPxButton ID="ExportPdfButton" runat="server" 
                    onclick="ExportPdfButton_Click" Text="Export to PDF" CssClass="sysButton"
                    Wrap="False" Image-Url="~/images/icons/AdobePDF.png">
                </dx:ASPxButton>
            </td>
        </tr>
    </table>
                </td>
        </tr>
        <tr>
            <td id="gridCell">
                <dx:ASPxGridView ID="NetworkDevicesGridView" runat="server" AutoGenerateColumns="False" 
	Cursor="pointer" EnableTheming="True" KeyFieldName="DeviceID" 
	OnHtmlDataCellPrepared="UsersGrid_HtmlDataCellPrepared" 
	OnHtmlRowCreated="UsersGrid_HtmlRowCreated" 
	OnPageSizeChanged="UsersGrid_PageSizeChanged" Theme="Office2003Blue" 
	Width="100%" EnableCallBacks="False">
	 <ClientSideEvents ContextMenu="NetworkDevicesGridView_ContextMenu">
                        </ClientSideEvents>
						<SettingsBehavior AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True" 
                        ColumnResizeMode="NextColumn"></SettingsBehavior>
                        <SettingsBehavior AllowFocusedRow="true"/>
	<Columns>

                                                                   
		<dx:GridViewDataTextColumn  ShowInCustomizationForm="True" 
			VisibleIndex="1" Width="30px">
			<PropertiesTextEdit>
				<FocusedStyle HorizontalAlign="Center">
				</FocusedStyle>
			</PropertiesTextEdit>
			<DataItemTemplate>
				<asp:Label ID="lblIcon" runat="server" Text='<%# Eval("OS_Type")%>' 
					Visible="false"></asp:Label>
				<asp:Image ID="IconImage" runat="server" />
				<dx:ASPxHyperLink ID="ASPxHyperLink2" runat="server" 
					NavigateUrl="<%# SetIcon(Container) %>" Visible="false">
				</dx:ASPxHyperLink>
			</DataItemTemplate>
			<HeaderStyle CssClass="GridCssHeader" />
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="User Name" FieldName="UserName" 
				ShowInCustomizationForm="True" VisibleIndex="2">
			<PropertiesTextEdit>
				<FocusedStyle HorizontalAlign="Center">
				</FocusedStyle>
			</PropertiesTextEdit>
			<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
			<EditCellStyle CssClass="GridCss">
			</EditCellStyle>
			<EditFormCaptionStyle CssClass="GridCss">
			</EditFormCaptionStyle>
			<HeaderStyle CssClass="GridCssHeader" />
			<CellStyle CssClass="GridCss" HorizontalAlign="Center">
			</CellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Device Name" FieldName="DeviceName" 
				ShowInCustomizationForm="True" VisibleIndex="3">
			<PropertiesTextEdit>
				<FocusedStyle HorizontalAlign="Center">
				</FocusedStyle>
			</PropertiesTextEdit>
			<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
			<EditCellStyle CssClass="GridCss">
			</EditCellStyle>
			<EditFormCaptionStyle CssClass="GridCss">
			</EditFormCaptionStyle>
			<HeaderStyle CssClass="GridCssHeader" />
			<CellStyle CssClass="GridCss" HorizontalAlign="Center">
			</CellStyle>
		</dx:GridViewDataTextColumn>
                                                                    
                                                                    
		<%--<dx:GridViewDataTextColumn Caption="ID" FieldName="ID" 
			ShowInCustomizationForm="True" Visible="False" VisibleIndex="14">
			<PropertiesTextEdit>
				<FocusedStyle HorizontalAlign="Center">
				</FocusedStyle>
			</PropertiesTextEdit>
			<Settings AutoFilterCondition="Contains" />
			<EditCellStyle CssClass="GridCss">
			</EditCellStyle>
			<EditFormCaptionStyle CssClass="GridCss">
			</EditFormCaptionStyle>
			<HeaderStyle CssClass="GridCssHeader" />
			<CellStyle CssClass="GridCss" HorizontalAlign="Center">
			</CellStyle>
		</dx:GridViewDataTextColumn>--%>
                                                                    
                                                                   
		<dx:GridViewDataTextColumn Caption="OS Type" FieldName="OS_Type" 
			ShowInCustomizationForm="True" VisibleIndex="10" Width="170px">
			<PropertiesTextEdit>
				<FocusedStyle HorizontalAlign="Center">
				</FocusedStyle>
			</PropertiesTextEdit>
			<Settings AutoFilterCondition="Contains" />
			<EditCellStyle CssClass="GridCss">
			</EditCellStyle>
			<EditFormCaptionStyle CssClass="GridCss">
			</EditFormCaptionStyle>
			<HeaderStyle CssClass="GridCssHeader" />
			<CellStyle CssClass="GridCss" HorizontalAlign="Center" Wrap="False">
			</CellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Device ID" FieldName="DeviceID" 
			ShowInCustomizationForm="True" VisibleIndex="20" Width="200px">
			<PropertiesTextEdit>
				<FocusedStyle HorizontalAlign="Center">
				</FocusedStyle>
			</PropertiesTextEdit>
			<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
			<EditCellStyle CssClass="GridCss">
			</EditCellStyle>
			<EditFormCaptionStyle CssClass="GridCss">
			</EditFormCaptionStyle>
			<HeaderStyle CssClass="GridCssHeader" />
			<CellStyle CssClass="GridCss" HorizontalAlign="Center" Wrap="False">
			</CellStyle>
		</dx:GridViewDataTextColumn>
                                                                    
	</Columns>
	<SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" 
		ColumnResizeMode="NextColumn" />
	<SettingsPager AlwaysShowPager="True" NumericButtonCount="50" PageSize="50">
		<PageSizeItemSettings Items="50, 100, 200" Visible="True">
		</PageSizeItemSettings>
	</SettingsPager>
	<Settings ShowFilterRow="True" />
	<Styles>
		<Header VerticalAlign="Middle">
		</Header>
		<AlternatingRow CssClass="GridAltRow" Enabled="True">
		</AlternatingRow>
	</Styles>
</dx:ASPxGridView>
                </td>
        </tr>
    </table>
                </dx:PanelContent>
</PanelCollection>
        </dx:ASPxRoundPanel>
    </td>
</tr>

</table>
<dx:ASPxPageControl ID="ASPxPageControl1" ActiveTabIndex="0" 
    CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" TabSpacing="0px" 
    runat="server" Width="100%" Visible="False" EnableHierarchyRecreation="False">
    <TabPages>
    <dx:TabPage Text="Mobile Devices">
        <TabImage Url="~/images/icons/phone.png">
    </TabImage>
<ContentCollection>
    <dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">
        
        </dx:ContentControl>
    </ContentCollection>
</dx:TabPage>
<dx:TabPage Text="Device Groups" Visible="false">
             <TabImage Url="~/images/icons/database.png">
                                </TabImage>
                <ContentCollection>
                    <dx:ContentControl ID="ContentControl2" runat="server" SupportsDisabledAttribute="True">
                      <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="To Be Implemented."></dx:ASPxLabel>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
<dx:TabPage Text="Carrier Groups" Visible="false">
             <TabImage Url="~/images/icons/cog.png">
                                </TabImage>
                <ContentCollection>
                    <dx:ContentControl ID="ContentControl3" runat="server" SupportsDisabledAttribute="True">
          <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="To Be Implemented."></dx:ASPxLabel>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>

        </TabPages>
    </dx:ASPxPageControl>

	 <dx:ASPxGridViewExporter ID="UsersGridViewExporter" runat="server" 
            GridViewID="NetworkDevicesGridView">
        </dx:ASPxGridViewExporter>
<%--</dx:PanelContent>
</PanelCollection>
    </dx:ASPxRoundPanel>--%>
</asp:Content>
