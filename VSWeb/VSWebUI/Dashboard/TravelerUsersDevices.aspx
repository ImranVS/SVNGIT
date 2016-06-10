<%@ Page Title="" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="TravelerUsersDevices.aspx.cs" Inherits="VSWebUI.TravelerUsersDevices" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>




<%@ Register assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts.Web" tagprefix="dxchartsui" %>
<%@ Register assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
           
        }
        .style2
        {
            height: 154px;
        }
    </style>
      <script type="text/javascript">
    // <![CDATA[
          function InitPopupMenuHandler(s, e) {
              var gridCell = document.getElementById('gridCell');
              ASPxClientUtils.AttachEventToElement(gridCell, 'contextmenu', OnGridContextMenu);
              var imgButton = document.getElementById('popupButton');
              ASPxClientUtils.AttachEventToElement(imgButton, 'contextmenu', OnPreventContextMenu);
          }
          function OnGridContextMenu(evt) {
              var SortPopupMenu = popupmenu;
              SortPopupMenu.ShowAtPos(evt.clientX + ASPxClientUtils.GetDocumentScrollLeft(), evt.clientY + ASPxClientUtils.GetDocumentScrollTop());
              return OnPreventContextMenu(evt);
          }
          function OnPreventContextMenu(evt) {
              return ASPxClientUtils.PreventEventAndBubble(evt);
          }
    // ]]>
    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
                                      

    <table class="style1">
    <tr>
            <td>         <dx:ASPxLabel ID="ASPxLabel1" runat="server" 
                    Text="Lotus Traveler Users and Devices" Font-Size="20px" ForeColor="#004040" ></dx:ASPxLabel>    
                        </td>
        </tr>
        <tr>
            <td>            
                              
                <table width="600">
  
                    <tr>
                        <td rowspan="3">
                          <dx:ASPxRadioButtonList ID="travelerButtonList" runat="server" 
                    SelectedIndex="0" AutoPostBack="True" 
                    onselectedindexchanged="travelerButtonList_SelectedIndexChanged">
                    <Items>
                        <dx:ListEditItem Selected="True" Text="Show all devices     " 
                            Value="Show all devices     " />
                        <dx:ListEditItem Text="Show only devices that have synchronized within the last" 
                            Value="Show only devices that have synchronized within the last " />
                        <dx:ListEditItem Text="Show only devices that have synchronized longer than " 
                            Value="Show only devices that have synchronized longer than" />
                    </Items>
                </dx:ASPxRadioButtonList>
                            </td>
                        <td colspan="2">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                         <dx:ASPxTextBox ID="lastminutesTextBox" runat="server" Visible="False" 
                    Width="70px">
                    <MaskSettings Mask="&lt;0..99999&gt;" /> </dx:ASPxTextBox>
                            </td>
                        <td align="left">
                         <dx:ASPxLabel ID="LastminutesLabel" runat="server" Text="minutes" 
                    Visible="False">
                </dx:ASPxLabel>
                           </td>
                        <td align="left">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>    
                            <dx:ASPxTextBox ID="minutesagoTextBox" runat="server" Visible="False" 
                    Width="70px">
                    <MaskSettings Mask="&lt;0..99999&gt;" />  </dx:ASPxTextBox>
                                                </td><td align="left">
                            
                     <dx:ASPxLabel ID="MinutesagoLabel" runat="server" 
                    Text="minutes ago" Visible="False">
                </dx:ASPxLabel>   
                            
                            </td>
                        <td align="left">
                            
                            &nbsp;</td>
                    </tr>
                </table>
               
               
            </td>
        </tr>
        <tr>
            <td align="center">
               
                          
                                         
                <dx:ASPxButton ID="SubButton" runat="server" onclick="SubButton_Click" 
                    Text="Submit" Theme="Office2010Blue" Visible="False">
                </dx:ASPxButton>
            </td>
        </tr>
       
        <tr>
            <td><img alt="" title="" style="border-style: none; border-color: inherit; border-width: 0px; margin-bottom: 2px; width:101px; height:25px; cursor: pointer"
                        src="../images/Action.png" id="popupButton" />
                &nbsp;<dx:ASPxPopupMenu ID="TravelerusersPopupMenu" runat="server" 
                    PopupAction="LeftMouseClick" PopupElementID="popupButton" 
                    PopupHorizontalAlign="RightSides" PopupVerticalAlign="TopSides" 
                    ClientInstanceName="popupmenu" 
                    onitemclick="TravelerusersPopupMenu_ItemClick">
<ClientSideEvents Init="InitPopupMenuHandler"></ClientSideEvents>
                    <Items>
                        <dx:MenuItem Text="Deny Access" Name="DenyAccess">
                        </dx:MenuItem>
                        <dx:MenuItem Text="Wipe Device" Name="WipeDevice">
                        </dx:MenuItem>
                        <dx:MenuItem Text="Clear Wipe Request" Name="ClearWipeRequest">
                        </dx:MenuItem>
                        <dx:MenuItem Text="Change Approval - Deny" Name="ChangeApproval-Deny">
                        </dx:MenuItem>
                        <dx:MenuItem Text="Change Approval - Approve" Name="ChangeApproval-Approve">
                        </dx:MenuItem>
                        <dx:MenuItem Text="Log Level - Enable Finest" Name="LogLevel-EnableFinest">
                        </dx:MenuItem>
                        <dx:MenuItem Text="Log Level - Disable Finest" Name="LogLevel-DisableFinest">
                        </dx:MenuItem>
                        <dx:MenuItem Text="Log Level - Create Dump File" Name="LogLevel-CreateDumpFile">
                        </dx:MenuItem>
                    </Items>
                      <ClientSideEvents Init="InitPopupMenuHandler" />
                </dx:ASPxPopupMenu>
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
            </td>
        </tr>
        <tr>
            <td>
            <dx:ASPxRoundPanel ID="GridASPxRoundPanel" runat="server" 
                            CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                            GroupBoxCaptionOffsetY="-24px" HeaderText="Users Details" 
                            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                            <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                              <ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                                <HeaderStyle Height="23px">
                                    <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                             <Paddings PaddingLeft="2px" PaddingTop="0px" PaddingBottom="0px"></Paddings>
                                </HeaderStyle>
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent5" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%"><tr><td id="gridCell">
                                        <dx:ASPxGridView ID="UsersGrid" runat="server" AutoGenerateColumns="False" 
                                        EnableTheming="True" Theme="Office2010Silver" Width="100%" 
                                            OnHtmlRowCreated="UsersGrid_HtmlRowCreated" Cursor="pointer" 
                                            KeyFieldName="ID">                                            
                              <ClientSideEvents FocusedRowChanged="function(s, e) { edit_panel.PerformCallback(); }" 
                                                ></ClientSideEvents>
                                        <Columns>
                                            <dx:GridViewDataTextColumn Caption="User Name" FieldName="UserName" 
                                            ShowInCustomizationForm="True" VisibleIndex="1"  
                                                PropertiesTextEdit-FocusedStyle-HorizontalAlign="Center" FixedStyle="Left" 
                                                Width="170px">
                                            <PropertiesTextEdit>
                                                <FocusedStyle HorizontalAlign="Center">
                                            </FocusedStyle>
                                        </PropertiesTextEdit>
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss" HorizontalAlign="Center">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Device Name" FieldName="DeviceName" 
                                        ShowInCustomizationForm="True" VisibleIndex="2" FixedStyle="Left">
                                        <PropertiesTextEdit>
                                            <FocusedStyle HorizontalAlign="Center">
                                            </FocusedStyle>
                                        </PropertiesTextEdit>
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss" HorizontalAlign="Center">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Connection State" 
                                        FieldName="ConnectionState" ShowInCustomizationForm="True" VisibleIndex="3" 
                                                FixedStyle="Left" Width="150px">
                                        <PropertiesTextEdit>
                                            <FocusedStyle HorizontalAlign="Center">
                                            </FocusedStyle>
                                        </PropertiesTextEdit>
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss" HorizontalAlign="Center">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Last SyncTime" FieldName="LastSyncTime" 
                                        ShowInCustomizationForm="True" VisibleIndex="4">
                                        <PropertiesTextEdit>
                                            <FocusedStyle HorizontalAlign="Center">
                                            </FocusedStyle>
                                        </PropertiesTextEdit>
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss" HorizontalAlign="Center">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="OS Type" FieldName="OS_Type" 
                                        ShowInCustomizationForm="True" VisibleIndex="5" Width="150px">
                                        <PropertiesTextEdit>
                                            <FocusedStyle HorizontalAlign="Center">
                                            </FocusedStyle>
                                        </PropertiesTextEdit>
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss" HorizontalAlign="Center">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Client_Build" FieldName="Client_Build" 
                                        ShowInCustomizationForm="True" VisibleIndex="6">
                                        <PropertiesTextEdit>
                                            <FocusedStyle HorizontalAlign="Center">
                                            </FocusedStyle>
                                        </PropertiesTextEdit>
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss" HorizontalAlign="Center">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="NotificationType" 
                                        FieldName="NotificationType" ShowInCustomizationForm="True" VisibleIndex="7">
                                        <PropertiesTextEdit>
                                            <FocusedStyle HorizontalAlign="Center">
                                            </FocusedStyle>
                                        </PropertiesTextEdit>
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss" HorizontalAlign="Center">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" 
                                        ShowInCustomizationForm="True" VisibleIndex="8" Visible="False">
                                        <PropertiesTextEdit>
                                            <FocusedStyle HorizontalAlign="Center">
                                            </FocusedStyle>
                                        </PropertiesTextEdit>
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss" HorizontalAlign="Center">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Doc ID" FieldName="DocID" 
                                        ShowInCustomizationForm="True" VisibleIndex="9" Width="250px">
                                        <PropertiesTextEdit>
                                            <FocusedStyle HorizontalAlign="Center">
                                            </FocusedStyle>
                                        </PropertiesTextEdit>
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss" HorizontalAlign="Center">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Device Type" FieldName="device_type" 
                                        ShowInCustomizationForm="True" VisibleIndex="10">
                                        <PropertiesTextEdit>
                                            <FocusedStyle HorizontalAlign="Center">
                                            </FocusedStyle>
                                        </PropertiesTextEdit>
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss" HorizontalAlign="Center">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Access" FieldName="Access" 
                                        ShowInCustomizationForm="True" VisibleIndex="11">
                                        <PropertiesTextEdit>
                                            <FocusedStyle HorizontalAlign="Center">
                                            </FocusedStyle>
                                        </PropertiesTextEdit>
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss" HorizontalAlign="Center">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Security Policy" 
                                        FieldName="Security_Policy" ShowInCustomizationForm="True" VisibleIndex="12">
                                        <PropertiesTextEdit>
                                            <FocusedStyle HorizontalAlign="Center">
                                            </FocusedStyle>
                                        </PropertiesTextEdit>
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss" HorizontalAlign="Center">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="WipeRequested" FieldName="wipeRequested" 
                                        ShowInCustomizationForm="True" VisibleIndex="13">
                                        <PropertiesTextEdit>
                                            <FocusedStyle HorizontalAlign="Center">
                                            </FocusedStyle>
                                        </PropertiesTextEdit>
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss" HorizontalAlign="Center">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="WipeOptions" FieldName="wipeOptions" 
                                        ShowInCustomizationForm="True" VisibleIndex="14">
                                        <PropertiesTextEdit>
                                            <FocusedStyle HorizontalAlign="Center">
                                            </FocusedStyle>
                                        </PropertiesTextEdit>
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss" HorizontalAlign="Center">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="WipeStatus" FieldName="wipeStatus" 
                                        ShowInCustomizationForm="True" VisibleIndex="15">
                                        <PropertiesTextEdit>
                                            <FocusedStyle HorizontalAlign="Center">
                                            </FocusedStyle>
                                        </PropertiesTextEdit>
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss" HorizontalAlign="Center">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="SyncType" FieldName="SyncType" 
                                        ShowInCustomizationForm="True" VisibleIndex="16">
                                        <PropertiesTextEdit>
                                            <FocusedStyle HorizontalAlign="Center">
                                            </FocusedStyle>
                                        </PropertiesTextEdit>
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss" HorizontalAlign="Center">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="WipeSupported" FieldName="wipeSupported" 
                                        ShowInCustomizationForm="True" VisibleIndex="17">
                                        <PropertiesTextEdit>
                                            <FocusedStyle HorizontalAlign="Center">
                                            </FocusedStyle>
                                        </PropertiesTextEdit>
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss" HorizontalAlign="Center">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Server Name" FieldName="ServerName" 
                                        ShowInCustomizationForm="True" VisibleIndex="18" Width="170px">
                                        <PropertiesTextEdit>
                                            <FocusedStyle HorizontalAlign="Center">
                                            </FocusedStyle>
                                        </PropertiesTextEdit>
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss" HorizontalAlign="Center">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Device ID" FieldName="DeviceID" 
                                        ShowInCustomizationForm="True" VisibleIndex="19" Width="200px">
                                        <PropertiesTextEdit>
                                            <FocusedStyle HorizontalAlign="Center">
                                            </FocusedStyle>
                                        </PropertiesTextEdit>
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss" HorizontalAlign="Center">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                                            <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" 
                                                ColumnResizeMode="NextColumn" 
                                                  ProcessSelectionChangedOnServer="True" 
                                               />

       <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" ColumnResizeMode="NextColumn" ></SettingsBehavior>

                                <SettingsPager AlwaysShowPager="True">
                                </SettingsPager>
                                <Settings ShowHorizontalScrollBar="True" ShowGroupPanel="True" ShowFilterRow="True" />

         <Settings ShowFilterRow="True" ShowGroupPanel="True" ShowHorizontalScrollBar="True"></Settings>

                                <Styles>
                                    <Header HorizontalAlign="Center" VerticalAlign="Middle">
                                    </Header>
                                    
       <AlternatingRow CssClass="GridAltrow" Enabled="True">
                        </AlternatingRow>
                                </Styles>
                                    </dx:ASPxGridView>
                                    </td></tr></table>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                </td>
        </tr>
        <tr>
            <td> <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" ClientInstanceName="edit_panel" 
                    runat="server" Width="1040px" oncallback="ASPxCallbackPanel1_Callback">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent6" runat="server">
                            <table>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
              
                <dx:ASPxRoundPanel ID="deviceTypeASPxRoundPanel" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Device Type" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="200px">
                    <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                    <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                    <HeaderStyle Height="23px">
                        <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                    <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                    </HeaderStyle>
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                              
                            <dxchartsui:WebChartControl ID="deviceTypeWebChart" runat="server" 
                                Height="300px" Width="1030px">
<FillStyle><OptionsSerializable>
<cc1:SolidFillOptions></cc1:SolidFillOptions>
</OptionsSerializable>
</FillStyle>

<SeriesTemplate><ViewSerializable>
<cc1:SideBySideBarSeriesView></cc1:SideBySideBarSeriesView>
</ViewSerializable>
<LabelSerializable>
<cc1:SideBySideBarSeriesLabel LineVisible="True">
<FillStyle><OptionsSerializable>
<cc1:SolidFillOptions></cc1:SolidFillOptions>
</OptionsSerializable>
</FillStyle>
<PointOptionsSerializable>
<cc1:PointOptions></cc1:PointOptions>
</PointOptionsSerializable>
</cc1:SideBySideBarSeriesLabel>
</LabelSerializable>
<LegendPointOptionsSerializable>
<cc1:PointOptions></cc1:PointOptions>
</LegendPointOptionsSerializable>
</SeriesTemplate>
<CrosshairOptions><CommonLabelPositionSerializable>
<cc1:CrosshairMousePosition></cc1:CrosshairMousePosition>
</CommonLabelPositionSerializable>
</CrosshairOptions>

<ToolTipOptions><ToolTipPositionSerializable>
<cc1:ToolTipMousePosition></cc1:ToolTipMousePosition>
</ToolTipPositionSerializable>
</ToolTipOptions>
                                    </dxchartsui:WebChartControl>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                            </ContentTemplate>
                </asp:UpdatePanel>            
                                    </td>
                                </tr>
                                <tr>
                                <td>
                                    <dx:ASPxPopupControl ID="msgPopupControl" runat="server" 
                                        HeaderText="MessageBox" Modal="True" PopupHorizontalAlign="WindowCenter" 
                                        PopupVerticalAlign="WindowCenter" Width="300px" Height="120px" 
                                        Theme="Glass">
                                        <ContentCollection>
<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
    <table class="style1">
        <tr>
            <td colspan="3">
                <dx:ASPxLabel ID="msgLabel" runat="server" Text="msglbl">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td align="right">
                <dx:ASPxButton ID="YesButton" runat="server" OnClick="YesButton_Click" 
                    Text="Yes" Theme="Office2010Blue" Width="70px">
                </dx:ASPxButton>
            </td>
            <td align="left">
                <dx:ASPxButton ID="NOButton" runat="server" OnClick="NOButton_Click" Text="NO" 
                    Theme="Office2010Blue" Width="70px">
                </dx:ASPxButton>
            </td>
            <td align="left">
                <dx:ASPxButton ID="CancelButton" runat="server" OnClick="CancelButton_Click" 
                    Text="Cancel" Theme="Office2010Blue" Width="70px">
                </dx:ASPxButton>
            </td>
        </tr>
    </table>
                                            </dx:PopupControlContentControl>
</ContentCollection>
                                    </dx:ASPxPopupControl>
                                    <dx:ASPxPopupControl ID="WipePopupControl" runat="server" 
                                        HeaderText="Wipe Device" Theme="Glass" Width="500px" Modal="True" 
                                        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                                        <ContentCollection>
                                            <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
                                                <table class="style1">
                                                    <tr>
                                                        <td align="center">
                                                            <dx:ASPxLabel ID="DeviceIDLabel" runat="server" Visible="False">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <dx:ASPxLabel ID="UserNameLabel" runat="server" Style="padding-left:150px">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" valign="middle" bgcolor="#FFFFCC">
                                                            <dx:ASPxLabel ID="DeviceNameLabel" runat="server" Style="padding-left:150px">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" valign="middle">
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" class="style2">
                                                            <dx:ASPxRoundPanel ID="wipeoptionsRoundPanel" runat="server" 
                                                                HeaderText="Wipe Options" Theme="Glass" Width="300px">
                                                                <PanelCollection>
                                                                    <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                                                    <table class="style1">
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <dx:ASPxCheckBox ID="HardCheckBox" runat="server" CheckState="Unchecked" 
                                                                                        Text="Hard Device Reset">
                                                                                    </dx:ASPxCheckBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <dx:ASPxCheckBox ID="TravelerAppCheckBox" runat="server" CheckState="Unchecked" 
                                                                                        Text="Traveler Application and Data">
                                                                                    </dx:ASPxCheckBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <dx:ASPxCheckBox ID="StorageCadrCheckBox" runat="server" CheckState="Unchecked" 
                                                                                        Text="Storage Card">
                                                                                    </dx:ASPxCheckBox>
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
                                                            <table class="style1">
                                                                <tr>
                                                                    <td align="right">
                                                                        <dx:ASPxButton ID="WipeButton" runat="server" OnClick="WipeButton_Click" 
                                                                            Text="Wipe" Theme="Office2010Blue" Width="70px">
                                                                        </dx:ASPxButton>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxButton ID="WipeCancelButton" runat="server" 
                                                                            OnClick="WipeCancelButton_Click" Text="Cancel" Theme="Office2010Blue" 
                                                                            Width="70px">
                                                                        </dx:ASPxButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </dx:PopupControlContentControl>
                                        </ContentCollection>
                                    </dx:ASPxPopupControl>
                                </td>
                                </tr>
                                  </table>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxCallbackPanel>
                </td>
        </tr>
    </table>
</asp:Content>
