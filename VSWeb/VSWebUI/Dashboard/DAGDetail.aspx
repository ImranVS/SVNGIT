<%@ Page Title="" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="DAGDetail.aspx.cs" Inherits="VSWebUI.Dashboard.DAGDetail" %>
<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
    <%@ Register assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>




<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>



<%@ Register assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc2" %>


<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
        <tr>
            <td>
                <asp:Label ID="lblServerType" runat="server" Text="DAG Name: " Font-Bold="True"
                                                   Font-Size="Large" Style="color: #000000; font-family: Verdana"></asp:Label>
                <asp:Label ID="servernamelbl" runat="server" Text="Label " Font-Bold="True"
                                                   Font-Size="Large" Style="color: #000000; font-family: Verdana"></asp:Label>
                <br />
                <asp:Label ID="lbltext" runat="server" Text="Last scan date: " Font-Size="Small" Style="color: #000000; font-family: Verdana"></asp:Label>
                <asp:Label ID="Lastscanned" runat="server" Text="Not Mentioned" Font-Size="Small" Style="color: #000000; font-family: Verdana"></asp:Label>
            </td>
            <td>
                
            </td>
            <td>
                
                <dx:ASPxButton ID="BackButton" runat="server" onclick="BackButton_Click" 
                    Text="Back" Theme="Office2010Blue">
                </dx:ASPxButton>
                
            </td>
        </tr>
    </table>
    <table width="100%"> 
                                                        <tr>
                                <td>  
                                    
                                </td>
                            </tr>
                            <tr>
                                <td>  
                                    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" 
                                            CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                            GroupBoxCaptionOffsetY="-24px" HeaderText="DAG Status" Height="50px" 
                                            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                                        <ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                        <ContentPaddings Padding="2px" PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>
                                        <HeaderStyle Height="23px">
                                        <Paddings Padding="0px" PaddingBottom="7px" PaddingLeft="2px" 
                                            PaddingRight="2px" PaddingTop="0px" />
                                        <Paddings Padding="0px" PaddingLeft="2px" PaddingTop="0px" PaddingRight="2px" PaddingBottom="7px"></Paddings>
                                        </HeaderStyle>
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                                                <table width="100%" cellspacing="3px">
                                                    <tr>
                                                        <td colspan="5">
                                                       
                                                                <dx:ASPxGridView ID="DAGStatusGridView" runat="server" 
                                                                    AutoGenerateColumns="False" SummaryText="m" 
                                                                        CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                                                                        CssPostfix="Office2010Silver" Width="100%"                 
                                                                        KeyFieldName="ID" ClientInstanceName="cdag"
                                                                        Cursor="pointer" EnableCallBacks="False" Theme="Office2003Blue" 
                                                                    onselectionchanged="DAGStatusGridView_SelectionChanged" 
                                                                    OnCustomCallback="DAGStatusGridView_CustomCallback" 
                                                                    OnHtmlDataCellPrepared="DAGStatusGridView_HtmlDataCellPrepared" >                   
                                                                        <SettingsBehavior AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True" 
                                                                            ColumnResizeMode="NextColumn" AllowSelectSingleRowOnly="True"></SettingsBehavior>
                                                                        <SettingsPager PageSize="20" Mode="ShowAllRecords">
                                                                            <PageSizeItemSettings Visible="True"></PageSizeItemSettings>
                                                                        </SettingsPager>
                                                                        <Columns>
                                                                         
                                                                        <dx:GridViewDataTextColumn Caption="DAG Name" VisibleIndex="1" 
                                                                                FieldName="DAGName" >                       
                                                                            <Settings AllowDragDrop="False" AutoFilterCondition="Contains" />
                                                                            <Settings AllowDragDrop="False" AutoFilterCondition="Contains"></Settings>
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader">
                                                                                <Paddings Padding="5px" />
                                                                            </HeaderStyle>
                                                                            <CellStyle CssClass="GridCss"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                          <dx:GridViewDataTextColumn Caption="File Witness Server Name" 
                                                                                FieldName="FileWitnessSereverName" VisibleIndex="2">
                                                                                <PropertiesTextEdit EnableFocusedStyle="False">
                                                                                </PropertiesTextEdit>
                                                                                <Settings AllowAutoFilter="False" />
                                                                                <EditCellStyle CssClass="GridCss1">
                                                                                </EditCellStyle>
                                                                                <EditFormCaptionStyle CssClass="GridCss1">
                                                                                </EditFormCaptionStyle>
                                                                              
                                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                                <CellStyle CssClass="GridCss">
                                                                                </CellStyle>
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn Caption="Total Mail Boxes" 
                                                                                FieldName="TotalMailBoxes" VisibleIndex="4">
                                                                                <PropertiesTextEdit EnableFocusedStyle="False">
                                                                                </PropertiesTextEdit>
                                                                                <Settings AllowAutoFilter="False" />
                                                                                <EditCellStyle CssClass="GridCss1">
                                                                                </EditCellStyle>
                                                                                <EditFormCaptionStyle CssClass="GridCss1">
                                                                                </EditFormCaptionStyle>
                                                                                
                                                                                <HeaderStyle CssClass="GridCssHeader2" />
                                                                                <CellStyle CssClass="GridCss2">
                                                                                </CellStyle>
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn Caption="Total Databases" FieldName="TotalDatabases" 
                                                                                UnboundType="Boolean" VisibleIndex="5">
                                                                                <PropertiesTextEdit EnableFocusedStyle="False">
                                                                                </PropertiesTextEdit>
                                                                                <Settings AllowAutoFilter="False" />
                                                                                <EditCellStyle CssClass="GridCss1">
                                                                                </EditCellStyle>
                                                                                <EditFormCaptionStyle CssClass="GridCss1">
                                                                                </EditFormCaptionStyle>
                                                                               
                                                                                <HeaderStyle CssClass="GridCssHeader2" />
                                                                                <CellStyle CssClass="GridCss2">
                                                                                </CellStyle>
                                                                            </dx:GridViewDataTextColumn>
                                                                          
                                                                            <dx:GridViewDataTextColumn Caption="Status" FieldName="Status" VisibleIndex="3">
                                                                                <PropertiesTextEdit EnableFocusedStyle="False">
                                                                                    <Style HorizontalAlign="Left" Wrap="True">
                                                                                    </Style>
                                                                                </PropertiesTextEdit>
                                                                                <Settings AllowAutoFilter="False" />
                                                                                <EditCellStyle CssClass="GridCss1">
                                                                                </EditCellStyle>
                                                                                <EditFormCaptionStyle CssClass="GridCss1">
                                                                                </EditFormCaptionStyle>
                                                                             
                                                                                <HeaderStyle CssClass="GridCssHeader1" />
                                                                                <CellStyle CssClass="GridCss1">
                                                                                </CellStyle>
                                                                            </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" Width='150px' Visible="False" VisibleIndex="0">
                                                                            <Settings AllowAutoFilter="True" AllowDragDrop="True" AutoFilterCondition="Contains" />
                                                                            <Settings AllowDragDrop="True" AllowAutoFilter="True" AutoFilterCondition="Contains"></Settings>
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                                            <CellStyle CssClass="GridCss1"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        </Columns>
                                                                       
                                                                        <SettingsBehavior AllowDragDrop="False" AutoExpandAllGroups="True"  AllowFocusedRow="true" 
                                                                            AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True" />
                                                                        <SettingsPager PageSize="10" SEOFriendly="Enabled" >
                                                                            <PageSizeItemSettings Visible="true" />
                                                                        </SettingsPager>
                                                                        <Settings ShowFilterRow="True" />
                                                                       

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
                                                                            <Header ImageSpacing="5px" SortingImageSpacing="5px" CssClass="GridCssHeader">
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
                                </td>
                            </tr>
                            <tr>
                                <td>  
                                    <dx:ASPxRoundPanel ID="DAGRoundPanel" runat="server" 
                                            CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                            GroupBoxCaptionOffsetY="-24px" HeaderText="DAG Members" Height="50px" 
                                            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                                        <ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                        <ContentPaddings Padding="2px" PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>
                                        <HeaderStyle Height="23px">
                                        <Paddings Padding="0px" PaddingBottom="7px" PaddingLeft="2px" 
                                            PaddingRight="2px" PaddingTop="0px" />
                                        <Paddings Padding="0px" PaddingLeft="2px" PaddingTop="0px" PaddingRight="2px" PaddingBottom="7px"></Paddings>
                                        </HeaderStyle>
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                                                <table width="100%" cellspacing="3px">
                                                    <tr>
                                                        <td colspan="5">
                                                    
                                                                <dx:ASPxGridView ID="DAGMembersGridView" runat="server" 
                                                                    AutoGenerateColumns="False" SummaryText="m" 
                                                                        CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                                                                        CssPostfix="Office2010Silver" Width="100%"                 
                                                                        KeyFieldName="ID" 
                                                                        Cursor="pointer" EnableCallBacks="False" Theme="Office2003Blue" 
                                                                    OnHtmlDataCellPrepared="DAGMembersGridView_HtmlDataCellPrepared" >                   
                                                                        <SettingsBehavior ProcessSelectionChangedOnServer="True" 
                                                                            ColumnResizeMode="NextColumn"></SettingsBehavior>
                                                                        <SettingsPager PageSize="20" Mode="ShowAllRecords">
                                                                            <PageSizeItemSettings Visible="True"></PageSizeItemSettings>
                                                                        </SettingsPager>
                                                                        <Columns>
                                                                        <dx:GridViewDataTextColumn Caption="Server Name" VisibleIndex="0" 
                                                                                FieldName="ServerName" Width="150px" >                       
                                                                            <Settings AllowDragDrop="False" AutoFilterCondition="Contains" />
                                                                            <Settings AllowDragDrop="False" AutoFilterCondition="Contains"></Settings>
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
                                                                        <dx:GridViewDataTextColumn Caption="Cluster Service" VisibleIndex="2" 
                                                                                FieldName="ClusterService">
                                                                            <Settings AllowAutoFilter="True" AllowDragDrop="True" 
                                                                                AutoFilterCondition="Contains" />
                                                                            <Settings AllowDragDrop="True" AllowAutoFilter="True" 
                                                                                AutoFilterCondition="Contains"></Settings>
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                                            <CellStyle CssClass="GridCss"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Relay Service" VisibleIndex="3" 
                                                                                FieldName="RelayService">
                                                                            <Settings AllowAutoFilter="True" AllowDragDrop="True" 
                                                                                AutoFilterCondition="Contains" />
                                                                            <Settings AllowDragDrop="True" AllowAutoFilter="True" 
                                                                                AutoFilterCondition="Contains"></Settings>
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                                            <CellStyle CssClass="GridCss"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>  
                                                                            <dx:GridViewDataTextColumn Caption="Active Mgr" FieldName="ActiveMgr" 
                                                                                VisibleIndex="4">
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn Caption="Tasks RPC Listener" 
                                                                                FieldName="TasksRPCListener" VisibleIndex="5">
                                                                            </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="TCP Listner" VisibleIndex="6" 
                                                                                FieldName="TPCListner">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="DAG Members UP" VisibleIndex="7" 
                                                                                FieldName="DAGMembersUP">
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
                                                                            </dx:GridViewDataTextColumn>
                                                                        </Columns>

                                                                        <SettingsBehavior AllowDragDrop="False" AutoExpandAllGroups="True" />
                                                                        <SettingsPager PageSize="10" SEOFriendly="Enabled" >
                                                                            <PageSizeItemSettings Visible="true" />
                                                                        </SettingsPager>
                                                                        <Settings ShowFilterRow="True" />
                                                                        

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
                                </td>
                            </tr> 
                             <tr>
                                <td>  
                                    <dx:ASPxRoundPanel ID="ServersListRoundPanel" runat="server" 
                                        CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                        GroupBoxCaptionOffsetY="-24px" HeaderText="DAG Databases" Height="50px" 
                                        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                                        <ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" 
                                            PaddingTop="10px" />
                                        <HeaderStyle Height="23px">
                                        <Paddings Padding="0px" PaddingBottom="7px" PaddingLeft="2px" 
                                            PaddingRight="2px" PaddingTop="0px" />
                                        </HeaderStyle>
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent21" runat="server" SupportsDisabledAttribute="True">
                                                <table cellspacing="3px" width="100%">
                                                    <tr>
                                                        <td colspan="5">
                                                            
                                                                    <dx:ASPxGridView ID="DAGDBGridView" runat="server" AutoGenerateColumns="False" 
                                                                        Cursor="pointer" EnableCallBacks="False" 
                                                                        KeyFieldName="ID" SummaryText="m" Theme="Office2003Blue" Width="100%" 
                                                                        OnHtmlDataCellPrepared="DAGDBGridView_HtmlDataCellPrepared">
                                                                        <SettingsBehavior ColumnResizeMode="NextColumn" 
                                                                            ProcessSelectionChangedOnServer="True" />
                                                                        <SettingsPager PageSize="20" Mode="ShowAllRecords">
                                                                            <PageSizeItemSettings Visible="True">
                                                                            </PageSizeItemSettings>
                                                                        </SettingsPager>
                                                                        <Columns>
                                                                            <dx:GridViewDataTextColumn Caption="Server Name" FieldName="ServerName" 
                                                                                VisibleIndex="0">
                                                                                <Settings AllowDragDrop="False" AutoFilterCondition="Contains" />
                                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                                <CellStyle CssClass="GridCss">
                                                                                </CellStyle>
                                                                            </dx:GridViewDataTextColumn>
                                                                    
                                                                         <%--   <dx:GridViewDataTextColumn Caption="MemberName" FieldName="MemberName" 
                                                                                VisibleIndex="1" ShowInCustomizationForm="True">
                                                                                <Settings AllowDragDrop="False" AutoFilterCondition="Contains" />
                                                                                <EditCellStyle CssClass="GridCss">
                                                                                </EditCellStyle>
                                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                                </EditFormCaptionStyle>
                                                                                <HeaderStyle CssClass="GridCssHeader" >
                                                                                <Paddings Padding="5px" />
                                                                                </HeaderStyle>
                                                                                <CellStyle CssClass="GridCss">
                                                                                </CellStyle>
                                                                            </dx:GridViewDataTextColumn>--%>
                                                                    
                                                                            <dx:GridViewDataTextColumn Caption="Database Name" FieldName="DatabaseName" 
                                                                                VisibleIndex="1">
                                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                                <CellStyle CssClass="GridCss">
                                                                                </CellStyle>
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn Caption="Activation Preference" 
                                                                                FieldName="Activation Preference" UnboundType="Boolean" VisibleIndex="2">
                                                                                <HeaderStyle CssClass="GridCssHeader2" />
                                                                                <CellStyle CssClass="GridCss2">
                                                                                </CellStyle>
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn Caption="Copy Queue" FieldName="CopyQueue" 
                                                                                VisibleIndex="3">
                                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                                <CellStyle CssClass="GridCss">
                                                                                </CellStyle>
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn Caption="Replay Queue" FieldName="ReplayQueue" 
                                                                                Visible="True" VisibleIndex="4">
                                                                                <PropertiesTextEdit EnableFocusedStyle="False">
                                                                                    <MaskSettings Mask="h:mm tt" />
                                                                                </PropertiesTextEdit>
                                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                                <CellStyle CssClass="GridCss">
                                                                                </CellStyle>
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn Caption="Replay Lagged" 
                                                                                FieldName="ReplayLagged" VisibleIndex="5" Width="150px">
                                                                                <Settings AllowAutoFilter="False" />
                                                                                <EditCellStyle CssClass="GridCss1">
                                                                                </EditCellStyle>
                                                                                <EditFormCaptionStyle CssClass="GridCss1">
                                                                                </EditFormCaptionStyle>
                                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                                <CellStyle CssClass="GridCss">
                                                                                </CellStyle>
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn Caption="Truncation Lagged" FieldName="TruncationLagged" 
                                                                                VisibleIndex="6">
                                                                                <PropertiesTextEdit EnableFocusedStyle="False">
                                                                                </PropertiesTextEdit>
                                                                                <Settings AllowAutoFilter="False" />
                                                                                <EditCellStyle CssClass="GridCss1">
                                                                                </EditCellStyle>
                                                                                <EditFormCaptionStyle CssClass="GridCss1">
                                                                                </EditFormCaptionStyle>
                                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                                <CellStyle CssClass="GridCss">
                                                                                </CellStyle>
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn Caption="Content Index" FieldName="ContendIndex" 
                                                                                VisibleIndex="8">
                                                                                <Settings AllowAutoFilter="True" AllowDragDrop="True" 
                                                                                    AutoFilterCondition="Contains" />
                                                                                <HeaderStyle CssClass="GridCssHeader2" />
                                                                                <CellStyle CssClass="GridCss2">
                                                                                </CellStyle>
                                                                                <PropertiesTextEdit EnableFocusedStyle="False">
                                                                                    <Style HorizontalAlign="Left" Wrap="True">
                                                                                    </Style>
                                                                                </PropertiesTextEdit>
                                                                                <Settings AllowAutoFilter="True" AllowDragDrop="True" 
                                                                                    AutoFilterCondition="Contains" />
                                                                                <Settings AllowAutoFilter="True" AllowDragDrop="True" 
                                                                                    AutoFilterCondition="Contains" />
                                                                                <EditCellStyle CssClass="GridCss1">
                                                                                </EditCellStyle>
                                                                                <EditFormCaptionStyle CssClass="GridCss1">
                                                                                </EditFormCaptionStyle>
                                                                                <HeaderStyle CssClass="GridCssHeader1" />
                                                                                <CellStyle CssClass="GridCss1">
                                                                                </CellStyle>
                                                                                <Settings AllowAutoFilter="True" AllowDragDrop="True" 
                                                                                    AutoFilterCondition="Contains" />
                                                                                <Settings AllowAutoFilter="True" AllowDragDrop="True" 
                                                                                    AutoFilterCondition="Contains" />
                                                                                <EditCellStyle CssClass="GridCss">
                                                                                </EditCellStyle>
                                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                                </EditFormCaptionStyle>
                                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                                <CellStyle CssClass="GridCss1">
                                                                                </CellStyle>
                                                                                <Settings AllowAutoFilter="True" AllowDragDrop="True" 
                                                                                    AutoFilterCondition="Contains" />
                                                                                <EditCellStyle CssClass="GridCss">
                                                                                </EditCellStyle>
                                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                                </EditFormCaptionStyle>
                                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                                <CellStyle CssClass="GridCss">
                                                                                </CellStyle>
                                                                                <Settings AllowAutoFilter="True" AllowDragDrop="True" 
                                                                                    AutoFilterCondition="Contains" />
                                                                                <EditCellStyle CssClass="GridCss">
                                                                                </EditCellStyle>
                                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                                </EditFormCaptionStyle>
                                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                                <CellStyle CssClass="GridCss">
                                                                                </CellStyle>
                                                                            </dx:GridViewDataTextColumn>
                                                                        </Columns>
                                                                        <SettingsBehavior AllowDragDrop="False" />
                                                                        <SettingsPager PageSize="10" SEOFriendly="Enabled">
                                                                            <PageSizeItemSettings Visible="true" />
                                                                        </SettingsPager>
                                                                        <Settings ShowFilterRow="True" />
                                                                        
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
                                                                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                            </Header>
                                                                            <GroupRow Font-Bold="True" Font-Italic="False">
                                                                            </GroupRow>
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
                                                               
                                                        </td>
                                                    </tr>
                                                </table>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dx:ASPxRoundPanel>
                                 </td>
                            </tr> 
                            </table>
 <br />
</asp:Content>
