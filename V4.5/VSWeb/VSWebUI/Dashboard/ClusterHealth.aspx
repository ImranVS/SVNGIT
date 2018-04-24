<%@ Page Title="VitalSigns Plus - Cluster Replicator Health" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="ClusterHealth.aspx.cs" Inherits="VSWebUI.Dashboard.ClusterHealth" %>
<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>


    <%@ Register assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>




<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>



<%@ Register assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc2" %>



<%@ Register Src="~/Controls/DateRange.ascx" TagName="DateRange" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script><script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
	<style type="text/css">
	    /* 6/3/2015 NS modified for VSPLUS-1828 */
	    /*
		#ContentPlaceHolder1_ASPxPageControl1_ASPxPageControl2_ClusterRepWebChartControl1
		{
			height:400px !important;
			width:1200px !important;
		}
	    #ContentPlaceHolder1_ASPxPageControl1_ASPxPageControl2_ClusterRepWebChartControl1_IMG
		{
			height:400px !important;
			width:1200px !important;
			
		}
		#ContentPlaceHolder1_ASPxPageControl1_ASPxPageControl2_ClusterRepWebChartControl2
		{
			height:400px !important;
			width:1200px !important;
		}
		#ContentPlaceHolder1_ASPxPageControl1_ASPxPageControl2_ClusterRepWebChartControl2_IMG
		{
			height:400px !important;
			width:1200px !important;
		}
  
       #ContentPlaceHolder1_ASPxPageControl1_WebChartControl1
	    {
		height:400px !important;
			width:1200px !important;
		}
		*/
        .style1
        {
            width: 100%;
        }
    </style>
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
        	//document.getElementById('ContentPlaceHolder1_chartWidth').value = document.body.offsetWidth - 95;
            //cSelectServerRoundPanel.style.width = document.getElementById('ContentPlaceHolder1__chartWidth').value + "px";
        	CClusterchart.PerformCallback();
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<input id="chartWidth" type="hidden" runat="server" value="400" />
        <input id="callbackState" type="hidden" runat="server" value="0" />
        <table width="100%">
    <tr>
        <td>
            <div class="header" id="servernamelbldisp" runat="server">Cluster Replicator Health</div>
        </td>
        <td>
            &nbsp;
        </td>    
        <td align="right">
            <table>
                <tr>
                    <td>
                    </td>
                </tr>
            </table>
        </td>        
    </tr>
</table>
<table width="100%" >
        <tr>
        <td>
            <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Popup Legend" CssClass="sysButton"
                AutoPostBack="False" Wrap="False">
            </dx:ASPxButton>
            <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" AllowDragging="True" 
                CloseAction="CloseButton" HeaderText="Legend" PopupElementID="ASPxButton1" 
                Theme="Moderno" Width="350px" Height="200px" ScrollBars="Vertical">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                        <table class="style1">
                            <tr>
                                <td colspan="2">
                                    <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Seconds On Queue Current" 
                                        Font-Bold="True">
                                    </dx:ASPxLabel>
                                </td>
                            </tr>
                            <tr>
                                <td width="30px">
                                    &nbsp;</td>
                                <td>
                                    <table class="style1">
                                        <tr>
                                              <td style="background-color:Red" width="10px"></td><td width="50px">
                                                &gt;30</td>
                                            <td width="10px" style="background-color:Yellow"> </td>
                                               <td> &gt; 15 and &lt;=30</td>
                                             <td width="10px" style="background-color:#90EE90"> </td>
                                            <td>
                                                &lt;=15</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Seconds On Queue Average" Font-Bold="True">
                                    </dx:ASPxLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                <table class="style1">
                                        <tr>
                                              <td style="background-color:Red" width="10px"></td><td width="50px">
                                                &gt;30</td>
                                            <td width="10px" style="background-color:Yellow"> </td>
                                               <td> &gt; 15 and &lt;=30</td>
                                             <td width="10px" style="background-color:#90EE90"> </td>
                                            <td>
                                                &lt;=15</td>
                                        </tr>
                                    </table>
                                   </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <dx:ASPxLabel ID="ASPxLabel9" runat="server" Font-Bold="True" Text="Seconds On Queue Max">
                                    </dx:ASPxLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                <table class="style1">
                                        <tr>
                                              <td style="background-color:Red" width="10px"></td><td width="50px">
                                                &gt;30</td>
                                            <td width="10px" style="background-color:Yellow"> </td>
                                               <td> &gt;15 and &lt;=30</td>
                                             <td width="10px" style="background-color:#90EE90"> </td>
                                            <td>
                                                &lt;=15</td>
                                        </tr>
                                    </table>
                                   </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <dx:ASPxLabel ID="ASPxLabel10" runat="server" Font-Bold="true" Text="Work Queue Depth Current">
                                    </dx:ASPxLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                   <table class="style1">
                                        <tr>
                                              <td style="background-color:Red" width="10px"></td><td width="50px">
                                                &gt;30</td>
                                            <td width="10px" style="background-color:Yellow"> </td>
                                               <td> &gt; 15 and &lt;=30</td>
                                             <td width="10px" style="background-color:#90EE90"> </td>
                                            <td>
                                                &lt;=15</td>
                                        </tr>
                                    </table></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <dx:ASPxLabel ID="ASPxLabel11" runat="server"  Font-Bold="true" Text="Work Queue Depth Average">
                                    </dx:ASPxLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                   <table class="style1">
                                        <tr>
                                              <td style="background-color:Red" width="10px"></td><td width="50px">
                                                &gt;=30</td>
                                            <td width="10px" style="background-color:Yellow"> </td>
                                               <td width="130px">  &gt;=15 and &lt;30</td>
                                             <td width="10px" style="background-color:#90EE90"> </td>
                                            <td>
                                                &lt;15</td>
                                        </tr>
                                    </table></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <dx:ASPxLabel ID="ASPxLabel12" runat="server"  Font-Bold="true" Text="Work Queue Depth Max">
                                    </dx:ASPxLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                   <table class="style1">
                                        <tr>
                                              <td style="background-color:Red" width="10px"></td><td width="50px">
                                                &gt;=30 </td>
                                            <td width="10px" style="background-color:Yellow"> </td>
                                               <td width="130px">  &gt;=15 and &lt;30</td>
                                             <td width="10px" style="background-color:#90EE90"> </td>
                                            <td>
                                                &lt;15</td>
                                        </tr>
                                    </table></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <dx:ASPxLabel ID="ASPxLabel13" runat="server"  Font-Bold="true" Text="Availability Percentage">
                                    </dx:ASPxLabel>
                                </td>
                                </tr>
                               <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                   <table class="style1">
                                        <tr>
                                              <td style="background-color:Red" width="10px"></td><td width="50px">
                                                &lt;25</td>
                                            <td width="10px" style="background-color:Yellow"> </td>
                                               <td width="130px">  &gt;50 </td>
                                             <td width="10px" style="background-color:#90EE90"> </td>
                                            <td>
                                                &lt;50 </td>
                                        </tr>
                                    </table></td>
                            </tr>
                           <tr>
                                <td colspan="2">
                                    <dx:ASPxLabel ID="ASPxLabel14" runat="server" Font-Bold="true" Text="Availability Threshold">
                                    </dx:ASPxLabel>
                                </td>
                                </tr>
                                <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                   <table class="style1">
                                        <tr>
                                              <td style="background-color:Red" width="10px"></td><td width="80px" 
                                                  nowrap="nowrap">
                                                &lt;Availability %</td>
                                          
                                             <td width="10px" style="background-color:#90EE90"> </td>
                                             <td width="80px" nowrap="nowrap">&gt;Availability % </td>
                                             <td width="10px" style="background-color:#D3D3D3"></td>
                                            <td>
                                                =0</td>
                                        </tr>
                                    </table></td>
                            </tr>
                            
                            
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            </td>
        </tr>
        
        </table>
    <table width="100%">
            
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                <dx:ASPxGridView ID="ClusterHealthGrid" runat="server" AutoGenerateColumns="False" 
                    CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                    CssPostfix="Office2010Silver" Width="100%" OnPageSizeChanged="ClusterHealthGrid_PageSizeChanged"
                   
                    KeyFieldName="ID" 
                   Cursor="pointer" 
                    OnHtmlDataCellPrepared="ClusterHealthGrid_HtmlDataCellPrepared" 
                    OnHtmlRowCreated="ClusterHealthGrid_HtmlRowCreated" EnableCallBacks="False" Theme="Office2003Blue"  
                    >
                   
<SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" ColumnResizeMode="NextColumn" 
                                                ProcessSelectionChangedOnServer="True"></SettingsBehavior>

                    <SettingsPager PageSize="20">
<PageSizeItemSettings Visible="True"></PageSizeItemSettings>
                    </SettingsPager>
                    <Columns>
                    <dx:GridViewDataTextColumn Caption="" VisibleIndex="0"  
                            FieldName="ClusterName" >
                       <CellStyle ><%--CssClass="GridCss" BorderBottom-BorderColor="ActiveBorder" BorderBottom-BorderWidth="" BorderLeft-BorderWidth="" BorderLeft-BorderColor="ActiveBorder" BorderRight-BorderColor="ActiveBorder" BorderRight-BorderWidth="" BorderTop-BorderColor="ActiveBorder" BorderTop-BorderWidth="">--%>
                         
                            </CellStyle>

                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
<Settings AllowDragDrop="True" AutoFilterCondition="Contains"></Settings>

                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                        <HeaderStyle CssClass="GridCssHeader" >
                        <Paddings Padding="5px" />
<Paddings Padding="5px"></Paddings>
                        </HeaderStyle>
                        <CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Server Name" VisibleIndex="1"  
                            FieldName="ServerName" Width="200px" >
                            <CellStyle>
                            </CellStyle>
                            <Settings AllowDragDrop="False" AutoFilterCondition="Contains" />
<Settings AllowDragDrop="False" AutoFilterCondition="Contains"></Settings>

                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" >
                            <Paddings Padding="5px" />
<Paddings Padding="5px"></Paddings>
                            </HeaderStyle>
                            <CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Seconds On Queue Current" 
                            FieldName="SecondsOnQueue" Visible="True" VisibleIndex="2">
                            
                             <PropertiesTextEdit><Style HorizontalAlign="Center"></Style></PropertiesTextEdit>
                            <Settings AllowAutoFilter="False" AllowDragDrop="True" 
                                AutoFilterCondition="Contains" />

<Settings AllowDragDrop="True" AllowAutoFilter="False" AutoFilterCondition="Contains"></Settings>

                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" Wrap="True" >
                             <Paddings Padding="5px" />
<Paddings Padding="5px"></Paddings>
                             </HeaderStyle>
                             <CellStyle CssClass="GridCss1"></CellStyle>
                        </dx:GridViewDataTextColumn>
                          <dx:GridViewDataTextColumn Caption="Seconds On Queue Avg" VisibleIndex="3" 
                            FieldName="SecondsOnQueueAvg">
                             <CellStyle CssClass="GridCss1">
                              </CellStyle>
                            <Settings AllowAutoFilter="False" AllowDragDrop="True" 
                                AutoFilterCondition="Contains" />
<Settings AllowDragDrop="True" AllowAutoFilter="False" AutoFilterCondition="Contains"></Settings>

                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                              <HeaderStyle CssClass="GridCssHeader" Wrap="True" >
                              <Paddings Padding="5px" />
<Paddings Padding="5px"></Paddings>
                              </HeaderStyle>
                              <CellStyle CssClass="GridCss1"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Seconds On Queue Max" VisibleIndex="4" 
                            FieldName="SecondsOnQueueMax" Visible ="false">
                             <CellStyle CssClass="GridCss1">
                             </CellStyle>
                            <Settings AllowAutoFilter="False" AllowDragDrop="True" 
                                AutoFilterCondition="Contains" />
<Settings AllowDragDrop="True" AllowAutoFilter="False" AutoFilterCondition="Contains"></Settings>

                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" Wrap="True" >
                             <Paddings Padding="5px" />
<Paddings Padding="5px"></Paddings>
                             </HeaderStyle>
                             <CellStyle CssClass="GridCss1"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Work Queue Depth Current" VisibleIndex="5" 
                            FieldName="WorkQueueDepth">
                             <CellStyle CssClass="GridCss1">
                             </CellStyle>
                            <Settings AllowAutoFilter="False" AllowDragDrop="True" />
<Settings AllowDragDrop="True" AllowAutoFilter="False"></Settings>

                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" Wrap="True" >
                             <Paddings Padding="5px" />
<Paddings Padding="5px"></Paddings>
                             </HeaderStyle>
                             <CellStyle CssClass="GridCss1"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        
                        
                        <dx:GridViewDataTextColumn Caption="Work Queue Depth Avg" VisibleIndex="6" 
                            FieldName="WorkQueueDepthAvg">
                             <CellStyle CssClass="GridCss1">
                             </CellStyle>
                            <Settings AllowAutoFilter="False" AllowDragDrop="True" />
<Settings AllowDragDrop="True" AllowAutoFilter="False"></Settings>

                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" Wrap="True" >
                             <Paddings Padding="5px" />
<Paddings Padding="5px"></Paddings>
                             </HeaderStyle>
                             <CellStyle CssClass="GridCss1"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        
                        <dx:GridViewDataTextColumn Caption="Work Queue Depth Max" VisibleIndex="7" 
                            FieldName="WorkQueueDepthMax" Visible="false">
                             <CellStyle CssClass="GridCss1">
                             </CellStyle>
                            <Settings AllowAutoFilter="False" AllowDragDrop="True" />
<Settings AllowDragDrop="True" AllowAutoFilter="False"></Settings>

                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" Wrap="True" >
                             <Paddings Padding="5px" />
<Paddings Padding="5px"></Paddings>
                             </HeaderStyle>
                             <CellStyle CssClass="GridCss1"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Availability Percentage" VisibleIndex="8" 
                            FieldName="Availability">
                             <CellStyle CssClass="GridCss1">
                             </CellStyle>
                            <Settings AllowAutoFilter="False" AllowDragDrop="True" />
<Settings AllowDragDrop="True" AllowAutoFilter="False"></Settings>

                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" Wrap="True" >
                             <Paddings Padding="5px" />
<Paddings Padding="5px"></Paddings>
                             </HeaderStyle>
                             <CellStyle CssClass="GridCss1"></CellStyle>
                        </dx:GridViewDataTextColumn>
                         <dx:GridViewDataTextColumn Caption="Availability Threshold" VisibleIndex="9" 
                            FieldName="AvailabilityThreshold">
                             <CellStyle CssClass="GridCss1">
                             </CellStyle>
                            <Settings AllowAutoFilter="False" AllowDragDrop="True" />
<Settings AllowDragDrop="True" AllowAutoFilter="False"></Settings>

                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" Wrap="True" >
                             <Paddings Padding="5px" />
<Paddings Padding="5px"></Paddings>
                             </HeaderStyle>
                             <CellStyle CssClass="GridCss1"></CellStyle>
                        </dx:GridViewDataTextColumn>
                         <dx:GridViewDataTextColumn Caption="Last Update" VisibleIndex="10" 
                            FieldName="LastUpdate">
                             <CellStyle CssClass="GridCss1">
                             </CellStyle>
                            <Settings AllowAutoFilter="False" AllowDragDrop="True" />
<Settings AllowDragDrop="True" AllowAutoFilter="False"></Settings>

                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" >
                             <Paddings Padding="5px" />
<Paddings Padding="5px"></Paddings>
                             </HeaderStyle>
                             <CellStyle CssClass="GridCss1"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Analysis" VisibleIndex="11" 
                            FieldName="Analysis" Width="200px">
                             <CellStyle CssClass="GridCss1">
                             </CellStyle>
                            <Settings AllowAutoFilter="False" AllowDragDrop="True" />
<Settings AllowDragDrop="True" AllowAutoFilter="False"></Settings>

                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" >
                             <Paddings Padding="5px" />
<Paddings Padding="5px"></Paddings>
                             </HeaderStyle>
                             <CellStyle CssClass="GridCss1"></CellStyle>
                        </dx:GridViewDataTextColumn>                      
                                             
                      

                    </Columns>

                    <SettingsBehavior AllowDragDrop="False" AutoExpandAllGroups="True"  AllowFocusedRow="true" 
                        AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True" />
                     <SettingsPager PageSize="10" SEOFriendly="Enabled" >
            <PageSizeItemSettings Visible="true" />
        </SettingsPager>
                    <Settings ShowGroupPanel="True" ShowFilterRow="True" />

<Settings ShowFilterRow="True" ShowGroupPanel="True"></Settings>

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
                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                        </Header>
                        <GroupRow Font-Bold="True" Font-Italic="False">
                        </GroupRow>
                        <GroupFooter Font-Bold="True">
                        </GroupFooter>
                        <GroupPanel Font-Bold="False">
                        </GroupPanel>
                        <LoadingPanel ImageSpacing="5px">
                        </LoadingPanel>
                         <AlternatingRow CssClass="GridAltRow" Enabled="True">
                        </AlternatingRow>
                    </Styles>
                    <StylesEditors ButtonEditCellSpacing="0">
                        <ProgressBar Height="21px">
                        </ProgressBar>
                    </StylesEditors>
                       <Templates>
                                    <GroupRowContent>
                                        <%# Container.GroupText%>
                                    </GroupRowContent>
                                </Templates>
                </dx:ASPxGridView>
                </ContentTemplate>
                </asp:UpdatePanel>
                
                <br />
            </td>
           
        </tr>
        <tr>
            <td align="left">
                <table>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Date range:" CssClass="lblsmallFont">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <uc1:DateRange ID="dtPick" runat="server" Width="100px" Height="100%"></uc1:DateRange>    
                        </td>
                        <td>
                            &nbsp;
                        </td>
            <td>
                <dx:ASPxButton ID="GoButton" runat="server" OnClick="GoButton_Click" Text="Submit" CssClass="sysButton" 
                   Width="50px">
                </dx:ASPxButton>
            </td>
                    </tr>
                </table>
            </td>
            
        </tr>
        <tr>
            <td width="100%">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ClusterHealthGrid" />
                    <asp:AsyncPostBackTrigger ControlID="GoButton" />
                </Triggers>
                <ContentTemplate>
                    <dxchartsui:WebChartControl ID="WebChartControl1" runat="server" 
                                 ClientInstanceName="CClusterchart" 
                        oncustomcallback="WebChartControl1_CustomCallback"   Width="800px" 
                        Height="400px" CrosshairEnabled="True">
                                  
								
                        <diagramserializable>
						
                            <cc2:XYDiagram>
                                <axisx datetimemeasureunit="Hour" 
                                    visibleinpanesserializable="-1">
                                    <range sidemarginsenabled="True" />
                                    <range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><label>
                                    <resolveoverlappingoptions allowrotate="False" />
                                    <resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /></label>
                                    <range sidemarginsenabled="True" />
                                    <datetimeoptions format="General" />
                                <range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /><range sidemarginsenabled="True" /><datetimeoptions format="General" /></axisx>
                                <axisy visibleinpanesserializable="-1">
                                    <range sidemarginsenabled="True" />
								
                                    <range sidemarginsenabled="True" />
                                <range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /></axisy>
                            </cc2:XYDiagram>
                        </diagramserializable>

<FillStyle><OptionsSerializable>
<cc2:SolidFillOptions></cc2:SolidFillOptions>
</OptionsSerializable>
</FillStyle>

                        <seriesserializable>
                            <cc2:Series ArgumentScaleType="DateTime" Name="Series 1">
                                <viewserializable>
                                    <cc2:LineSeriesView>
                                    </cc2:LineSeriesView>
                                </viewserializable>
                                <labelserializable>
                                    <cc2:PointSeriesLabel LineVisible="True">
                                        <fillstyle>
                                            <optionsserializable>
                                                <cc2:SolidFillOptions />
                                            </optionsserializable>
                                        </fillstyle>
                                        <pointoptionsserializable>
                                            <cc2:PointOptions>
                                            </cc2:PointOptions>
                                        </pointoptionsserializable>
                                    </cc2:PointSeriesLabel>
                                </labelserializable>
                                <legendpointoptionsserializable>
                                    <cc2:PointOptions>
                                    </cc2:PointOptions>
                                </legendpointoptionsserializable>
                            </cc2:Series>
                            <cc2:Series ArgumentScaleType="DateTime" Name="Series 2">
                                <viewserializable>
                                    <cc2:LineSeriesView>
                                    </cc2:LineSeriesView>
                                </viewserializable>
                                <labelserializable>
                                    <cc2:PointSeriesLabel LineVisible="True">
                                        <fillstyle>
                                            <optionsserializable>
                                                <cc2:SolidFillOptions />
                                            </optionsserializable>
                                        </fillstyle>
                                        <pointoptionsserializable>
                                            <cc2:PointOptions>
                                            </cc2:PointOptions>
                                        </pointoptionsserializable>
                                    </cc2:PointSeriesLabel>
                                </labelserializable>
                                <legendpointoptionsserializable>
                                    <cc2:PointOptions>
                                    </cc2:PointOptions>
                                </legendpointoptionsserializable>
                            </cc2:Series>
                        </seriesserializable>

<SeriesTemplate><ViewSerializable>
    <cc2:LineSeriesView>
    </cc2:LineSeriesView>
</ViewSerializable>
<LabelSerializable>
    <cc2:PointSeriesLabel LineVisible="True">
        <fillstyle>
            <optionsserializable>
                <cc2:SolidFillOptions />
            </optionsserializable>
        </fillstyle>
        <pointoptionsserializable>
            <cc2:PointOptions>
            </cc2:PointOptions>
        </pointoptionsserializable>
    </cc2:PointSeriesLabel>
</LabelSerializable>
<LegendPointOptionsSerializable>
<cc2:PointOptions></cc2:PointOptions>
</LegendPointOptionsSerializable>
</SeriesTemplate>

<CrosshairOptions><CommonLabelPositionSerializable>
<cc2:CrosshairMousePosition></cc2:CrosshairMousePosition>
</CommonLabelPositionSerializable>
</CrosshairOptions>

<ToolTipOptions><ToolTipPositionSerializable>
<cc2:ToolTipMousePosition></cc2:ToolTipMousePosition>
</ToolTipPositionSerializable>
</ToolTipOptions>
                </dxchartsui:WebChartControl>
                </ContentTemplate>
            </asp:UpdatePanel>
                
            </td>
            </tr>
        <tr>
        <td>
     
            <dx:ASPxPopupControl ID="MsgPopupControl" runat="server" 
                    HeaderText="Information" Modal="True" PopupHorizontalAlign="WindowCenter" 
                    PopupVerticalAlign="WindowCenter" Theme="Glass">
                    <ContentCollection>
<dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">
    <table class="style1">
        <tr>
            <td>
                <dx:ASPxLabel ID="ErrmsgLabel" runat="server">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxButton ID="OKButton" runat="server"  Text="OK" 
                    Theme="Office2010Blue" OnClick="OKButton_Click">
                </dx:ASPxButton>
            </td>
        </tr>
    </table>
                        </dx:PopupControlContentControl>
</ContentCollection>
                </dx:ASPxPopupControl>
            
        </td>
        </tr>
    </table>
    <br />
</asp:Content>
