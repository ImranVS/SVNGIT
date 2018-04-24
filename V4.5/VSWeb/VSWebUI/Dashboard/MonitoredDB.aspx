<%@ Page Title="VitalSigns Plus - Database Health" Language="C#" MasterPageFile="~/DashboardSite.Master"
    AutoEventWireup="true" CodeBehind="MonitoredDB.aspx.cs" Inherits="VSWebUI.WebForm4" %>
	<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>






<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>

    <script language="javascript" type="text/javascript">
        function PopupCenter(pageURL) {
           //alert(pageURL);
            var w = 500;
            var h = 550;
            var left = (screen.width / 2) - (w / 2);
            var top = (screen.height / 2) - (h / 2);
            //1/11/2013 NS modified the parameters below - second param blows up in IE, must be empty
            var targetWin = window.open(pageURL, '', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
        }
        function OnItemClick(s, e) {
            if (e.item.parent == s.GetRootItem())
                e.processOnServer = false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%">
        <tr>
            <td>
                <div class="header" id="Div1" runat="server">Notes Database Health</div>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <div class="subheader" id="Div2" runat="server">Monitored Databases</div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <dx:ASPxGridView ID="MonitoredDBGridView" runat="server" AutoGenerateColumns="False"
                    EnableTheming="True" Theme="Office2003Blue" Width="100%" Cursor="pointer" OnHtmlDataCellPrepared="MonitoredDBGridView_HtmlDataCellPrepared"
                    KeyFieldName="TypeandName" OnPageSizeChanged="MonitoredDBGridView_PageSizeChanged"
                        OnSelectionChanged="MonitoredDBGridView_SelectionChanged" 
                        >
                    <SettingsPager PageSize="50" >
                        <PageSizeItemSettings Visible="True">
                        </PageSizeItemSettings>
                    </SettingsPager>
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="Status" FieldName="Status" VisibleIndex="1" PropertiesTextEdit-FocusedStyle-HorizontalAlign="Center">
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
                            <CellStyle CssClass="GridCss1">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="File Name" FieldName="DominoServerTasks" 
                            VisibleIndex="0">
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
                        <dx:GridViewDataTextColumn Caption="Title" FieldName="Name" VisibleIndex="2">
                           <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Monitor" VisibleIndex="3" FieldName="Category">
                           <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="TypeandName" FieldName="TypeandName" Visible="False"
                            VisibleIndex="5">
                        </dx:GridViewDataTextColumn>
                           <dx:GridViewDataTextColumn Caption="LastUpdate" FieldName="LastUpdate" Visible="False"
                            VisibleIndex="7"></dx:GridViewDataTextColumn>
                       
                         <dx:GridViewDataTextColumn Caption="Type" FieldName="Type" Visible="False"
                            VisibleIndex="6"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Server" FieldName="Server" VisibleIndex="4">
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
                    <SettingsBehavior AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True"
                        ColumnResizeMode="NextColumn" />
                    <Settings ShowGroupPanel="True" ShowFilterRow="True" />
                    <Styles>
                        <GroupFooter Font-Bold="True">
                        </GroupFooter>
                        <AlternatingRow CssClass="GridAltRow" Enabled="True">
                        </AlternatingRow>
                    </Styles>
                </dx:ASPxGridView>
                </ContentTemplate>
                </asp:UpdatePanel>
                
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%">
                    <tr>
                        <td>
                            <div class="subheader" id="Div3" runat="server">All Databases</div>
                        </td>
                        <td>
                           &nbsp;
                        </td>
                        <td align="right">
                            <table>
                                <tr>
                                    <td>
                                        <dx:ASPxMenu ID="ASPxMenu1" runat="server" ShowAsToolbar="True" Theme="Moderno" 
                                            onitemclick="ASPxMenu1_ItemClick">
                                        <ClientSideEvents ItemClick="OnItemClick" />
                                        <Items>
                                            <dx:MenuItem Name="MainItem">
                                            <Items>
                                                <dx:MenuItem Name="OpenDBItem" Text="Open Database">
                                                </dx:MenuItem>
                                                <dx:MenuItem Name="CompactItem" Text="Compact">
                                                </dx:MenuItem>
                                                <dx:MenuItem Name="FixupItem" Text="Fixup">
                                                </dx:MenuItem>
                                                <dx:MenuItem Name="UpdallItem" Text="Updall">
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
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <dx:ASPxGridView ID="AllDBGridView" runat="server" AutoGenerateColumns="False" EnableTheming="True"
                                Theme="Office2003Blue" Width="100%" KeyFieldName="ID" OnPageSizeChanged="AllDBGridView_PageSizeChanged" 
                                    OnHtmlDataCellPrepared="AllDBGridView_HtmlDataCellPrepared" 
                                 >
                                <SettingsPager PageSize="50">
                                    <PageSizeItemSettings Visible="True" 
                                        Items="10, 20, 50, 100, 200, 500, 1000, 1500">
                                    </PageSizeItemSettings>
                                </SettingsPager>
                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="Database Name" VisibleIndex="0" FieldName="Title"
                                        FixedStyle="Left" Width="200px">
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
                                    <dx:GridViewDataTextColumn Caption="Server" VisibleIndex="2" FieldName="Server" 
                                        FixedStyle="Left" Width="200px">
                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Folder Count" FieldName="FolderCount" 
                                        VisibleIndex="4">
										<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss2">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Folder" VisibleIndex="3" FieldName="Folder">
                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Status" VisibleIndex="1" FieldName="Status" FixedStyle="Left">
                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss1">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Details" VisibleIndex="5" 
                                        FieldName="Details" Width="250px">
                                       <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="File Name" VisibleIndex="6" FieldName="FileName"
                                        Width="150px">
                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss" Wrap="True">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="FT Indexed" VisibleIndex="7" 
                                        FieldName="FTIndexed" Visible="False">
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="FT Indexed Frequency" VisibleIndex="8" 
                                        FieldName="FTIndexFrequency" Visible="False">
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Last FT Indexed" VisibleIndex="9" 
                                        FieldName="LastFTIndexed" Visible="False">
                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="In Service" VisibleIndex="10" 
                                        FieldName="IsInService" Visible="False">
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Enabled for Cluster Replication" VisibleIndex="11"
                                        FieldName="EnabledForClusterReplication" Visible="False">
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Enabled for Replication" VisibleIndex="12" 
                                        FieldName="EnabledForReplication" Visible="False">
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Public NAB" VisibleIndex="13" 
                                        FieldName="IsPublicAddressBook" Visible="False">
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Design Template" VisibleIndex="14" 
                                        FieldName="DesignTemplateName" Width="120px">
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Private NAB" VisibleIndex="15" 
                                        FieldName="IsPrivateAddressBook" Visible="False">
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Last Modified" VisibleIndex="16" 
                                        FieldName="LastModified" Visible="False">
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Size(MB)" VisibleIndex="17" 
                                        FieldName="FileSize">
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss2">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Quota(MB)" VisibleIndex="18" 
                                        FieldName="Quota">
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss2">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Inbox Documents" VisibleIndex="19" 
                                        FieldName="InboxDocCount">
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                                        <CellStyle CssClass="GridCss2">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataDateColumn Caption="Scan Date" FieldName="ScanDateOnly" 
                                        VisibleIndex="20">
                                        <PropertiesDateEdit AllowUserInput="False" DisplayFormatInEditMode="True" 
                                            EditFormatString="d">
                                        </PropertiesDateEdit>
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss1">
                                        </CellStyle>
                                    </dx:GridViewDataDateColumn>
                                    <dx:GridViewDataTextColumn Caption="ReplicaID" VisibleIndex="21" 
                                        FieldName="ReplicaID" Width="130px">
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Document Count" VisibleIndex="22" 
                                        FieldName="DocumentCount">
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                                        <CellStyle CssClass="GridCss2">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Categories" VisibleIndex="23" 
                                        FieldName="Categories">
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Created" VisibleIndex="24" 
                                        FieldName="Created" Visible="False">
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Current Access" VisibleIndex="25" 
                                        FieldName="CurrentAccessLevel" Visible="False">
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Percent Used" VisibleIndex="26" 
                                        FieldName="PercentUsed" Visible="False">
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="ODS" VisibleIndex="27" FieldName="ODS" 
                                        Visible="False">
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" Visible="False" VisibleIndex="28">
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                                <Styles>
                                    <AlternatingRow CssClass="GridAltRow" Enabled="True">
                                    </AlternatingRow>
                                </Styles>
                                <SettingsBehavior AllowSelectByRowClick="True" ColumnResizeMode="NextColumn" AllowFocusedRow="True" />
                                <Settings ShowGroupPanel="True" ShowHorizontalScrollBar="True" ShowFilterRow="True" />
                            </dx:ASPxGridView>
                            </ContentTemplate>
                            </asp:UpdatePanel>
                            
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxPopupControl ID="DBPopupControl" runat="server" HeaderText="Msgbox" Modal="True"
                                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Theme="MetropolisBlue"
                                Width="400px">
                                <ContentCollection>
                                    <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
                                        <table class="style1">
                                            <tr>
                                                <td>
                                                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="ASPxLabel">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <%-- <tr>
            <td colspan="2">
                &nbsp;</td>
        </tr>--%>
                                            <tr>
                                                <td>
                                                    <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="ASPxLabel">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="ASPxLabel" Visible="False">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="ASPxLabel">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                            <%-- <tr>
            <td colspan="2">
                &nbsp;</td>
        </tr>--%>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dx:ASPxTextBox ID="popuptextBox" runat="server" Width="300px">
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <%-- <tr>
            <td colspan="2">
                &nbsp;</td>
        </tr>--%>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                   <table>
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxButton ID="OKButton" runat="server" OnClick="OKButton_Click" Text="OK" 
                                                                Theme="Office2010Blue" Width="70px">
                                                            </dx:ASPxButton>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxButton ID="CancelButton" runat="server" OnClick="CancelButton_Click" 
                                                                Text="Cancel" Theme="Office2010Blue" Width="70px">
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
                    <tr>
                        <td>
                            <dx:ASPxPopupControl ID="msgPopupControl" runat="server" HeaderText="Information"
                                Theme="MetropolisBlue" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                Width="300px">
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                                        <table class="style1">
                                            <tr>
                                                <td>
                                                    <dx:ASPxLabel ID="msglbl" runat="server" Text="ASPxLabel">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxButton ID="msgbtn" runat="server" OnClick="msgbtn_Click" Text="OK" 
                                                        Theme="Office2010Blue">
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            <dx:ASPxButton ID="OpenButton" runat="server" Text="Open Database" Theme="Office2010Blue"
                                OnClick="OpenButton_Click" style="height: 21px" Visible="False">
                            </dx:ASPxButton>
                            <dx:ASPxButton ID="CompactButton" runat="server" Text="Compact" Theme="Office2010Blue"
                                OnClick="CompactButton_Click" Visible="False">
                            </dx:ASPxButton>
                            <dx:ASPxButton ID="FixupButton" runat="server" Text="Fixup" Theme="Office2010Blue"
                                OnClick="FixupButton_Click" Visible="False">
                            </dx:ASPxButton>
                            <dx:ASPxButton ID="UpdallButton" runat="server" Text="Updall" Theme="Office2010Blue"
                                OnClick="UpdallButton_Click" Visible="False">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
