<%@ Page Language="C#" Title="VitalSigns Plus - AD Health" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="ActiveDirectoryHealth.aspx.cs" Inherits="VSWebUI.Dashboard.ActiveDirectoryHealth" %>

<%@ MasterType virtualpath="~/DashboardSite.Master" %>

<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<%@ Register assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts.Web" tagprefix="dxchartsui" %>
<%@ Register assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
        <link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />

     <%--   <style type="text/css">
        
          
        #ContentPlaceHolder1_ResponseWebChartControl



		{
			height:300px !important;
			width:1200px !important;
		}
		#ContentPlaceHolder1_ResponseWebChartControl_IMG
		{
			height:300px !important;
			width:1200px !important;
		}
		
        
        </style>--%>
         
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

 
<input id="chartWidth" type="hidden" runat="server" value="400" />

    <script type="text/javascript">
        function OnGridFocusedRowChanged() {
            // Query the server for the "EmployeeID" field from the focused row  
            // The single value will be returned to the OnGetRowValues() function
            grid1.GetRowValues(grid1.GetFocusedRowIndex(), 'ID', OnGetRowValues);
            //alert(temp);
        }
        // Value contains the "EmployeeID" field value returned from the server, not the list of values 
        function OnGetRowValues(Value) {
            // Right code
            document.getElementById("txtBox").value = Value;
            // This code will cause an error 
            // alert(Value[0]);
        }
    </script>
   <script type="text/javascript">
       function Resized() {
           var callbackState = document.getElementById('ContentPlaceHolder1_callbackState').value;

           if (callbackState == 0)
               DoCallback();
       }

       function DoCallback() {
           document.getElementById('ContentPlaceHolder1_chartWidth').value = document.body.offsetWidth - 55;
           //cSelectServerRoundPanel.style.width = document.getElementById('ContentPlaceHolder1__chartWidth').value + "px";
           //        FileOpensCumulativeWebChart.PerformCallback();
           //        FileOpensWebChart.PerformCallback();
           //        chttpSessionsWebChart.PerformCallback();
           //        DeviceTypeChart.PerformCallback();
           //        OSTypeChart.PerformCallback();

           cResponseWebChartControl.PerformCallback();


       }

       function ResizeChart(s, e) {
           document.getElementById('ContentPlaceHolder1_callbackState').value = 0;
           s.GetMainElement().style.width = document.getElementById('ContentPlaceHolder1__chartWidth').value + "px";
           //cSelectServerRoundPanel.style.width = document.getElementById('ContentPlaceHolder1__chartWidth').value + "px";
       }

       function ResetCallbackState() {
           window.document.form1.elements['ContentPlaceHolder1_callbackState'].value = 1;
       }
       function InitPopupMenuHandler(s, e) {
           var gridCell = document.getElementById('gridCell');
           ASPxClientUtils.AttachEventToElement(gridCell, 'contextmenu', OnGridContextMenu);
           //        var imgButton = document.getElementById('popupButton');
           //        ASPxClientUtils.AttachEventToElement(imgButton, 'contextmenu', OnPreventContextMenu);
       }
       function OnGridContextMenu(evt) {
           var SortPopupMenu = popupmenu;
           SortPopupMenu.ShowAtPos(evt.clientX + ASPxClientUtils.GetDocumentScrollLeft(), evt.clientY + ASPxClientUtils.GetDocumentScrollTop());
           return OnPreventContextMenu(evt);
       }
       function OnPreventContextMenu(evt) {
           return ASPxClientUtils.PreventEventAndBubble(evt);
       }
       //10/30/2013 NS added - fix for when an Enter key is pressed within the text box on the page - redirect the
       //submit function to the actual Go button on the page instead of performing a whole page submit
       function OnKeyDown(s, e) {
           //alert(window.event.keyCode);
           //var keyCode = (window.event) ? e.which : e.keyCode;
           //alert(keyCode);
           var keyCode = window.event.keyCode;
           if (keyCode == 13)
               goButton.DoClick();
       }
</script>
    <table width="100%">
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                        <img alt="" src="../images/ad.png" />
                    </td>
                    <td>
                        <div class="header" id="lblTitle" runat="server">Microsoft Active Directory Server Health</div>
                    </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div style="float: left; width: 20%; display: none">
                <dx:ASPxTextBox ID="txtBox" ClientInstanceName="txtBox" style="display: none" runat="server" Width="170px">
                </dx:ASPxTextBox>       
             </div>
            </td>
            
        </tr>
       
       <%-- <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="DAGGridView" />
                    </Triggers>
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
            <td>
                <dx:ASPxLabel ID="DAGLabel" runat="server" Text="DAGName" Font-Bold="True"
                     Font-Names="Segoe UI" Font-Size="Medium" ForeColor="Black">
                </dx:ASPxLabel>
            </td>
        </tr>--%>
        
        <tr>
            <td>
                <div class="subheader" id="subheader1Div" runat="server">Active Directory Servers</div>
            </td>
        </tr>
        <tr>
            <td>
				<asp:UpdatePanel ID="updatepan1" runat="server" UpdateMode="Conditional">
					<ContentTemplate>
						<dx:ASPxGridView ID="DAGMembersGridView" runat="server" 
                                                                    AutoGenerateColumns="False" SummaryText="m" 
                                                                        CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                                                                        CssPostfix="Office2010Silver" Width="100%"                 
                                                                        KeyFieldName="ID" EnableTheming="True" 
                                                                        Cursor="pointer" EnableCallBacks="False" Theme="Office2003Blue" 
                                                                    OnHtmlDataCellPrepared="DAGMembersGridView_HtmlDataCellPrepared"
																    OnSelectionChanged = "DAGMembersGridView_SelectionChanged" >                   
                                                                    <%--    <SettingsBehavior ProcessSelectionChangedOnServer="True" AllowSelectByRowClick="true"
                                                                            ColumnResizeMode="NextColumn"></SettingsBehavior>--%>
                                                                        <SettingsPager PageSize="20" Mode="ShowAllRecords">
                                                                            <PageSizeItemSettings Visible="True"></PageSizeItemSettings>
                                                                        </SettingsPager>
																		 <Settings ShowFilterRow="True" />
                                                                        <Columns>
                                                                        
                                                                        <dx:GridViewDataTextColumn Caption="Server Name" VisibleIndex="0" 
                                                                                FieldName="ServerName" Width="150px" >                       
                                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilter="True" AllowAutoFilterTextInputTimer="False" />
                                                                             
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader">
                                                                                <Paddings Padding="5px" />
                                                                            </HeaderStyle>
                                                                            <CellStyle CssClass="GridCss"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                         <dx:GridViewDataTextColumn Caption="Status" VisibleIndex="0" 
                                                                                FieldName="Status" Width="150px" >                       
                                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilter="True" AllowAutoFilterTextInputTimer="False" />
                                                                             
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader">
                                                                                <Paddings Padding="5px" />
                                                                            </HeaderStyle>
                                                                            <CellStyle CssClass="GridCss"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                     <%--   <dx:GridViewDataTextColumn Caption="DAGName" FieldName="DAGName" Width='150px'
                                                                                ShowInCustomizationForm="True" Visible="True" VisibleIndex="1">
                                                                            <Settings AllowAutoFilter="True" AllowDragDrop="True" AutoFilterCondition="Contains" />
                                                                            <Settings AllowDragDrop="True" AllowAutoFilter="True" AutoFilterCondition="Contains"></Settings>
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                                            <CellStyle CssClass="GridCss1"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>--%>
                                                                        <dx:GridViewDataTextColumn Caption="LogOn Test" VisibleIndex="2" 
                                                                                FieldName="LogOnTest">
                                                                    <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                           
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                                            <CellStyle CssClass="GridCss"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Query Test" VisibleIndex="3" 
                                                                                FieldName="QueryTest">
                                                                          <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                                            <CellStyle CssClass="GridCss"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>  
                                                                            <dx:GridViewDataTextColumn Caption="LDAPPort Test" FieldName="LDAPPortTest" 
                                                                                VisibleIndex="4">
																				 <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            </dx:GridViewDataTextColumn>

                                                                        <dx:GridViewDataTextColumn Caption="DNS Test" FieldName="DNS" 
                                                                                VisibleIndex="5">
																				 <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Advertising Proper Roles Test" FieldName="Advertising" 
                                                                                VisibleIndex="6">
																				 <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            </dx:GridViewDataTextColumn> 
                                                                        <dx:GridViewDataTextColumn Caption="File Replication System Errors Test" FieldName="FrsSysVol" 
                                                                                VisibleIndex="7">
																				 <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Replication Test" FieldName="Replications" 
                                                                                VisibleIndex="8">
																				 <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            </dx:GridViewDataTextColumn>
																		<dx:GridViewDataTextColumn Caption="Domain Controller Services Test" FieldName="Services" 
                                                                                VisibleIndex="9">
																				 <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            </dx:GridViewDataTextColumn>
																		<dx:GridViewDataTextColumn Caption="Proper Servers can be Contacted Test" FieldName="FsmoCheck" 
                                                                                VisibleIndex="10">
																				 <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Last Scan Date" 
                                                                                FieldName="LastScanDate" VisibleIndex="11">
																				 <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            </dx:GridViewDataTextColumn>
																		<dx:GridViewDataTextColumn Caption="ID" FieldName="ID" 
                                                                                VisibleIndex="0" Visible="false">
																				 <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                            </dx:GridViewDataTextColumn>
                                                                       <%-- <dx:GridViewDataTextColumn Caption="Domain Controller" VisibleIndex="6" 
                                                                                FieldName="DomainController">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="AD Members UP" VisibleIndex="7" 
                                                                                FieldName="ADMembersUP">
                                                                        </dx:GridViewDataTextColumn>  
                                                                            <dx:GridViewDataTextColumn Caption="Cluster Network" FieldName="ClusterNetwork" 
                                                                                VisibleIndex="8">
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn Caption="Quorum Group" FieldName="QuorumGroup" 
                                                                                VisibleIndex="9">
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn Caption="File Share Quorum" 
                                                                                FieldName="FileShareQuorum" VisibleIndex="10">
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn Caption="DB Copy Suspend" FieldName="DBCopySuspend" 
                                                                                VisibleIndex="11">
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn Caption="DB Disconnected" FieldName="DBDisconnected" 
                                                                                VisibleIndex="12">
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn Caption="DB Log Copy Keeping UP" 
                                                                                FieldName="DBLogCopyKeepingUP" VisibleIndex="13">
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn Caption="DB Log Replay Keeping UP" 
                                                                                FieldName="DBLogReplayKeepingUP" VisibleIndex="14">
                                                                            </dx:GridViewDataTextColumn>--%>
                                                                        </Columns>

                                                                        <%--<SettingsBehavior AllowDragDrop="False" AutoExpandAllGroups="True"  />--%>
                                                                       <%-- <SettingsPager PageSize="10" SEOFriendly="Enabled" >
                                                                            <PageSizeItemSettings Visible="true" />
                                                                        </SettingsPager>--%>
                                                                      
                                                                        

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
                                                                        <Styles CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                                                                            CssPostfix="Office2010Silver">
                                                                            <Header ImageSpacing="5px" SortingImageSpacing="5px" CssClass="GridCssHeader" 
                                                                                Wrap="True">
                                                                            </Header>
                                                                            <GroupRow Font-Bold="True" Font-Italic="False">
                                                                            </GroupRow>
                                                                            <Cell CssClass="GridCss">
                                                                            </Cell>
                                                                            <GroupFooter Font-Bold="True">
                                                                            </GroupFooter>
                                                                            <AlternatingRow CssClass="GridAltRow" Enabled="True">
                                                                            </AlternatingRow>
                                                                            <GroupPanel Font-Bold="False">
                                                                            </GroupPanel>
                                                                            <LoadingPanel ImageSpacing="5px">
                                                                            </LoadingPanel>
                                                                        </Styles>
                                                                        <StylesEditors ButtonEditCellSpacing="0">
                                                                            <ProgressBar Height="21px">
                                                                            </ProgressBar>
                                                                        </StylesEditors>
                                                                    </dx:ASPxGridView>
					</ContentTemplate>
				</asp:UpdatePanel>
            </td>
        </tr>
        </table>
      
       <table>
   <tr>
   <td>
        <div class="subheader" id="subheader2Div" runat="server">Active Directory Query Response Time</div>
        <table class="style1">
    <tr>
    <td colspan="2">
        <asp:UpdatePanel ID="update1" runat="server" UpdateMode="Conditional"><ContentTemplate>
       <dxchartsui:WebChartControl ID="ResponseWebChartControl" runat="server" ClientInstanceName="cResponseWebChartControl" 
                    Height="800px" Width="800px" CrosshairEnabled="True" 
                onbounddatachanged="ResponseWebChartControl_BoundDataChanged">
                    <diagramserializable>
                        <cc1:XYDiagram>
                            <axisx gridspacingauto="False" visibleinpanesserializable="-1">
                                <range sidemarginsenabled="True" />
                                <tickmarks minorvisible="False" />
                                <range sidemarginsenabled="True" />
                                <tickmarks minorvisible="False" />
                                <range sidemarginsenabled="True" />
                                <tickmarks minorvisible="False" />
                                <label>
                                <resolveoverlappingoptions allowhide="False" allowrotate="False" />
                                </label>
                                <range sidemarginsenabled="True" />
                            </axisx>
                            <axisy visibleinpanesserializable="-1">
                                <range sidemarginsenabled="True" />
                                <tickmarks minorvisible="False" />
                                <range sidemarginsenabled="True" />
                                <numericoptions format="Number" precision="0" />
                                <tickmarks minorvisible="False" />
                                <tickmarks minorvisible="False" />
                                <label>
                                <resolveoverlappingoptions allowrotate="False" />
                                <resolveoverlappingoptions allowrotate="False"></resolveoverlappingoptions>
                                </label>
                                <range sidemarginsenabled="True" />
                                <numericoptions format="Number" precision="0" />
                                <range sidemarginsenabled="True" />
                                <numericoptions format="Number" precision="0" />
                            </axisy>
                        </cc1:XYDiagram>
                    </diagramserializable>
                    <fillstyle>
                        <optionsserializable>
                            <cc1:SolidFillOptions />
                        </optionsserializable>
                    </fillstyle>
                    <seriesserializable>
                        <cc1:Series Name="Series 1">
                            <viewserializable>
                                <cc1:SideBySideBarSeriesView>
                                </cc1:SideBySideBarSeriesView>
                            </viewserializable>
                            <labelserializable>
                                <cc1:SideBySideBarSeriesLabel LineVisible="True">
                                    <fillstyle>
                                        <optionsserializable>
                                            <cc1:SolidFillOptions />
                                        </optionsserializable>
                                    </fillstyle>
                                    <pointoptionsserializable>
                                        <cc1:PointOptions>
                                        </cc1:PointOptions>
                                    </pointoptionsserializable>
                                </cc1:SideBySideBarSeriesLabel>
                            </labelserializable>
                            <legendpointoptionsserializable>
                                <cc1:PointOptions>
                                </cc1:PointOptions>
                            </legendpointoptionsserializable>
                        </cc1:Series>
                        <cc1:Series Name="Series 2">
                            <viewserializable>
                                <cc1:SideBySideBarSeriesView>
                                </cc1:SideBySideBarSeriesView>
                            </viewserializable>
                            <labelserializable>
                                <cc1:SideBySideBarSeriesLabel LineVisible="True">
                                    <fillstyle>
                                        <optionsserializable>
                                            <cc1:SolidFillOptions />
                                        </optionsserializable>
                                    </fillstyle>
                                    <pointoptionsserializable>
                                        <cc1:PointOptions>
                                        </cc1:PointOptions>
                                    </pointoptionsserializable>
                                </cc1:SideBySideBarSeriesLabel>
                            </labelserializable>
                            <legendpointoptionsserializable>
                                <cc1:PointOptions>
                                </cc1:PointOptions>
                            </legendpointoptionsserializable>
                        </cc1:Series>
                    </seriesserializable>
                    <seriestemplate>
                        <viewserializable>
                            <cc1:SideBySideBarSeriesView>
                            </cc1:SideBySideBarSeriesView>
                        </viewserializable>
                        <labelserializable>
                            <cc1:SideBySideBarSeriesLabel LineVisible="True">
                                <fillstyle>
                                    <optionsserializable>
                                        <cc1:SolidFillOptions />
                                    </optionsserializable>
                                </fillstyle>
                                <pointoptionsserializable>
                                    <cc1:PointOptions>
                                    </cc1:PointOptions>
                                </pointoptionsserializable>
                            </cc1:SideBySideBarSeriesLabel>
                        </labelserializable>
                        <legendpointoptionsserializable>
                            <cc1:PointOptions>
                            </cc1:PointOptions>
                        </legendpointoptionsserializable>
                    </seriestemplate>
                    <crosshairoptions>
                        <commonlabelpositionserializable>
                            <cc1:CrosshairMousePosition />
                        </commonlabelpositionserializable>
                    </crosshairoptions>
                    <tooltipoptions>
                        <tooltippositionserializable>
                            <cc1:ToolTipMousePosition />
                        </tooltippositionserializable>
                    </tooltipoptions>
                </dxchartsui:WebChartControl>
                </td>
      </tr>
           </td></tr>
       
        
                      
                    </ContentTemplate>
                </asp:UpdatePanel></td></tr>
            </table>
            </td></tr>
    </table>
</asp:Content>
