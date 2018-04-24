<%@ Page Language="C#" Title="VitalSigns Plus - DAG Health" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="DAGHealth.aspx.cs" Inherits="VSWebUI.Dashboard.DAGHealth" %>
<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>






<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
        <link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
           sessionStorage.setItem("Force refresh", "True");
    </script>
    <table width="100%">
        <tr>
            <td>
                <table>
                    <tr>
                    <td>
                        <img alt="" src="../images/icons/small-DAG-icon.png" />
                    </td>
                    <td>
                        <div class="header" id="lblTitle" runat="server">Database Availability Group (DAG) Health</div>
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
        <tr>
            <td>
                
                <dx:ASPxGridView ID="DAGGridView" ClientInstanceName="grid1" runat="server" AutoGenerateColumns="False" 
                    EnableTheming="True" KeyFieldName="DAGName" 
                    onselectionchanged="DAGGridView_SelectionChanged" Theme="Office2003Blue" OnPageSizeChanged="DAGGridView_PageSizeChanged"
                    Width="100%" onhtmldatacellprepared="DAGGridView_HtmlDataCellPrepared" 
                    EnableCallBacks="False">
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="DAG Name" FieldName="DAGName" 
                            VisibleIndex="0">
                        </dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Status" FieldName="Status" VisibleIndex="1">
                            <HeaderStyle CssClass="GridCssHeader1" />
                            <CellStyle CssClass="GridCss1">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="File Witness Server Name" 
                            FieldName="FileWitnessSereverName" VisibleIndex="2">
                        </dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="File Witness Server Status" FieldName="FileWitnessServerStatus" VisibleIndex="3">
                            <HeaderStyle CssClass="GridCssHeader1" />
                            <CellStyle CssClass="GridCss1">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Members" FieldName="Members" 
                            VisibleIndex="4">
                            <HeaderStyle CssClass="GridCssHeader2" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Total Databases" FieldName="TotalDatabases" 
                            VisibleIndex="5">
                            <HeaderStyle CssClass="GridCssHeader2" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Connected MailBoxes" FieldName="ConnectedMailboxes" 
                            VisibleIndex="6">
                            <HeaderStyle CssClass="GridCssHeader2" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Disconnected MailBoxes" FieldName="DisconnectedMailboxes" 
                            VisibleIndex="7">
                            <HeaderStyle CssClass="GridCssHeader2" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" Visible="False" 
                            VisibleIndex="6">
                        </dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Last Scan" FieldName="LastUpdate" 
                            VisibleIndex="8">
                            <HeaderStyle CssClass="GridCssHeader2" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" 
                        ProcessSelectionChangedOnServer="True" />
                    <SettingsPager AlwaysShowPager="True" SEOFriendly="Enabled">
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
                
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="DAGGridView" />
                    </Triggers>
                    <ContentTemplate>
                        <div class="subheader" id="lblSubtitle" runat="server"></div>
                        <dx:ASPxPageControl ID="DAGPageControl" runat="server" ActiveTabIndex="0" 
                            Theme="Glass" Width="100%" EnableHierarchyRecreation="False">
                            <TabPages>
                                <dx:TabPage Name="DAGName" Text="Activation Preferences">
                                    <ContentCollection>
                                        <dx:ContentControl runat="server">
                                            <table class="navbarTbl">
                                                <tr>
                                                    <td>
                                                        <dx:ASPxGridView ID="ActivPrefGridView" runat="server" 
                                                            ClientInstanceName="grid" EnableCallBacks="False" 
                                                            OnHtmlDataCellPrepared="ActivPrefGridView_HtmlDataCellPrepared" OnPageSizeChanged="ActivPrefGridView_PageSizeChanged"
                                                            Theme="Office2003Blue" Width="100%">
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
                                                    </td>
                                                </tr>
                                            </table>
                                        </dx:ContentControl>
                                    </ContentCollection>
                                </dx:TabPage>
                                <dx:TabPage Name="DAGMembers" Text="Members">
                                    <ContentCollection>
                                        <dx:ContentControl runat="server">
                                            <table class="navbarTbl">
                                                <tr>
                                                    <td>
                                                        <dx:ASPxGridView ID="DAGMembersGridView" runat="server" 
                                                            AutoGenerateColumns="False" 
                                                            CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                                                            CssPostfix="Office2010Silver" Cursor="pointer" EnableCallBacks="False" 
                                                            KeyFieldName="ID" 
                                                            OnHtmlDataCellPrepared="DAGMembersGridView_HtmlDataCellPrepared" 
                                                            SummaryText="m" Theme="Office2003Blue" Width="100%">
                                                            <Columns>
                                                                <dx:GridViewDataTextColumn Caption="Server Name" FieldName="ServerName" 
                                                                    ShowInCustomizationForm="True" VisibleIndex="0" Width="150px">
                                                                    <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    <EditCellStyle CssClass="GridCss">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader">
                                                                    <Paddings Padding="5px" />
                                                                    </HeaderStyle>
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Cluster Service" FieldName="ClusterService" 
                                                                    ShowInCustomizationForm="True" VisibleIndex="2">
                                                                   <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    <EditCellStyle CssClass="GridCss">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Relay Service" FieldName="RelayService" 
                                                                    ShowInCustomizationForm="True" VisibleIndex="3">
                                                                   <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    <EditCellStyle CssClass="GridCss">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Active Mgr" FieldName="ActiveMgr" 
                                                                    ShowInCustomizationForm="True" VisibleIndex="4">
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Tasks RPC Listener" 
                                                                    FieldName="TasksRPCListener" ShowInCustomizationForm="True" VisibleIndex="5">
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="TCP Listner" FieldName="TPCListner" 
                                                                    ShowInCustomizationForm="True" VisibleIndex="6">
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="DAG Members UP" FieldName="DAGMembersUP" 
                                                                    ShowInCustomizationForm="True" VisibleIndex="7">
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Cluster Network" FieldName="ClusterNetwork" 
                                                                    ShowInCustomizationForm="True" VisibleIndex="8">
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Quorum Group" FieldName="QuorumGroup" 
                                                                    ShowInCustomizationForm="True" VisibleIndex="9">
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="File Share Quorum" 
                                                                    FieldName="FileShareQuorum" ShowInCustomizationForm="True" VisibleIndex="10">
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="DB Copy Suspend" FieldName="DBCopySuspend" 
                                                                    ShowInCustomizationForm="True" VisibleIndex="11">
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="DB Disconnected" FieldName="DBDisconnected" 
                                                                    ShowInCustomizationForm="True" VisibleIndex="12">
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="DB Log Copy Keeping UP" 
                                                                    FieldName="DBLogCopyKeepingUP" ShowInCustomizationForm="True" VisibleIndex="13">
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="DB Log Replay Keeping UP" 
                                                                    FieldName="DBLogReplayKeepingUP" ShowInCustomizationForm="True" 
                                                                    VisibleIndex="14">
                                                                </dx:GridViewDataTextColumn>
                                                            </Columns>
                                                            <SettingsBehavior AllowDragDrop="False" AutoExpandAllGroups="True" 
                                                                ColumnResizeMode="NextColumn" ProcessSelectionChangedOnServer="True" />
                                                            <SettingsPager Mode="ShowAllRecords" SEOFriendly="Enabled">
                                                                <PageSizeItemSettings Visible="True">
                                                                </PageSizeItemSettings>
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
                                                                <Header CssClass="GridCssHeader" ImageSpacing="5px" SortingImageSpacing="5px" 
                                                                    Wrap="True">
                                                                </Header>
                                                                <GroupRow Font-Bold="True" Font-Italic="False">
                                                                </GroupRow>
                                                                <AlternatingRow CssClass="GridAltRow" Enabled="True">
                                                                </AlternatingRow>
                                                                <Cell CssClass="GridCss">
                                                                </Cell>
                                                                <GroupFooter Font-Bold="True">
                                                                </GroupFooter>
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
                                        </dx:ContentControl>
                                    </ContentCollection>
                                </dx:TabPage>
                                <dx:TabPage Name="DAGDBs" Text="Databases">
                                    <ContentCollection>
                                        <dx:ContentControl runat="server">
                                            <table class="navbarTbl">
                                                <tr>
                                                    <td>
                                                        <dx:ASPxGridView ID="DAGDBGridView" runat="server" AutoGenerateColumns="False" 
                                                            Cursor="pointer" EnableCallBacks="False" KeyFieldName="ID" 
                                                            OnHtmlDataCellPrepared="DAGDBGridView_HtmlDataCellPrepared" SummaryText="m" 
                                                            Theme="Office2003Blue" Width="100%">
                                                            <Columns>
                                                                <dx:GridViewDataTextColumn Caption="Server Name" FieldName="ServerName" 
                                                                    ShowInCustomizationForm="True" VisibleIndex="0">
                                                                   <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Database Name" FieldName="DatabaseName" 
                                                                    ShowInCustomizationForm="True" VisibleIndex="1">
                                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Activation Preference" 
                                                                    FieldName="Activation Preference" ShowInCustomizationForm="True" 
                                                                    UnboundType="Boolean" VisibleIndex="2">
                                                                    <HeaderStyle CssClass="GridCssHeader2" />
                                                                    <CellStyle CssClass="GridCss2">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Copy Queue" FieldName="CopyQueue" 
                                                                    ShowInCustomizationForm="True" VisibleIndex="3">
                                                                    <HeaderStyle CssClass="GridCssHeader2" />
                                                                    <CellStyle CssClass="GridCss2">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Replay Queue" FieldName="ReplayQueue" 
                                                                    ShowInCustomizationForm="True" VisibleIndex="4">
                                                                    <PropertiesTextEdit EnableFocusedStyle="False">
                                                                        <MaskSettings Mask="h:mm tt" />
                                                                    </PropertiesTextEdit>
                                                                    <HeaderStyle CssClass="GridCssHeader2" />
                                                                    <CellStyle CssClass="GridCss2">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Replay Lagged" FieldName="ReplayLagged" 
                                                                    ShowInCustomizationForm="True" VisibleIndex="5" Width="150px">
                                                                    <Settings AllowAutoFilter="False" />
                                                                    <EditCellStyle CssClass="GridCss1">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss1">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Truncation Lagged" 
                                                                    FieldName="TruncationLagged" ShowInCustomizationForm="True" VisibleIndex="6">
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
                                                                    ShowInCustomizationForm="True" VisibleIndex="8">
                                                                    <PropertiesTextEdit EnableFocusedStyle="False">
                                                                        <Style HorizontalAlign="Left" Wrap="True">
                                                                        </Style>
                                                                    </PropertiesTextEdit>
                                                                    <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    <EditCellStyle CssClass="GridCss">
                                                                    </EditCellStyle>
                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                    </EditFormCaptionStyle>
                                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                            </Columns>
                                                            <SettingsBehavior AllowDragDrop="False" ColumnResizeMode="NextColumn" 
                                                                ProcessSelectionChangedOnServer="True" />
                                                            <SettingsPager Mode="ShowAllRecords" SEOFriendly="Enabled">
                                                                <PageSizeItemSettings Visible="True">
                                                                </PageSizeItemSettings>
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
                                                                <AlternatingRow CssClass="GridAltRow" Enabled="True">
                                                                </AlternatingRow>
                                                                <GroupFooter Font-Bold="True">
                                                                </GroupFooter>
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
                                        </dx:ContentControl>
                                    </ContentCollection>
                                </dx:TabPage>
                                <dx:TabPage Name="DAGDBOverview" Text="Database Status">
                                    <ContentCollection>
                                        <dx:ContentControl runat="server">
                                            <table class="navbarTbl">
                                                <tr>
                                                    <td>
                                                        <dx:ASPxGridView ID="DAGDBOverviewGridView" runat="server" 
                                                            AutoGenerateColumns="False" Cursor="pointer" EnableCallBacks="False" 
                                                            KeyFieldName="ID" SummaryText="m" Theme="Office2003Blue" Width="100%">
                                                            <Columns>
                                                                <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" 
                                                                    ShowInCustomizationForm="True" Visible="False" VisibleIndex="1">
                                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Database Name" FieldName="DatabaseName" 
                                                                    ShowInCustomizationForm="True" VisibleIndex="2">
                                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Storage Group" FieldName="StorageGroup" 
                                                                    ShowInCustomizationForm="True" VisibleIndex="3">
                                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Mounted" FieldName="Mounted" 
                                                                    ShowInCustomizationForm="True" VisibleIndex="4">
                                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Backup in Progress" 
                                                                    FieldName="BackupInProgress" ShowInCustomizationForm="True" 
                                                                    VisibleIndex="5">
                                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Online Maintenance in Progress" 
                                                                    FieldName="OnlineMaintInProg" ShowInCustomizationForm="True" 
                                                                    VisibleIndex="6">
                                                                    <HeaderStyle CssClass="GridCssHeader" Wrap="False" />
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Last Backup" FieldName="LastBackup" 
                                                                    ShowInCustomizationForm="True" VisibleIndex="7">
                                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Last Backup Type" FieldName="BackupType" 
                                                                    ShowInCustomizationForm="True" VisibleIndex="8">
                                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                            </Columns>
                                                            <SettingsBehavior AllowDragDrop="False" ColumnResizeMode="NextColumn" 
                                                                ProcessSelectionChangedOnServer="True" />
                                                            <SettingsPager Mode="ShowAllRecords" SEOFriendly="Enabled">
                                                                <PageSizeItemSettings Visible="True">
                                                                </PageSizeItemSettings>
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
                                                                <AlternatingRow CssClass="GridAltRow" Enabled="True">
                                                                </AlternatingRow>
                                                                <GroupFooter Font-Bold="True">
                                                                </GroupFooter>
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
                                        </dx:ContentControl>
                                    </ContentCollection>
                                </dx:TabPage>
                            </TabPages>
                        </dx:ASPxPageControl>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        
    </table>
</asp:Content>
