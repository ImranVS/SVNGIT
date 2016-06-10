<%@ Page Title="VitalSigns Plus" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="StatusList.aspx.cs" Inherits="VSWebUI.Dashboard.StatusList" %>
<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>




    
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <div>
    <dx:ASPxRoundPanel runat="server" GroupBoxCaptionOffsetY="-24px" Width="95%" HeaderText="Status List"
        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
        ID="ASPxRoundPanel13">
        <ContentPaddings PaddingTop="10px" PaddingBottom="10px" PaddingLeft="4px"></ContentPaddings>
        <HeaderStyle Height="23px">
            <Paddings PaddingLeft="2px" PaddingBottom="0px" PaddingTop="0px"></Paddings>
        </HeaderStyle>
        <PanelCollection>
            <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
            <table style="width: 98%;">
                    <tr>
                        <td colspan="3">
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <img id="imgButton" alt="" src="../images/viewselector.png" style="width: 103px;
                                            height: 28px;" />
                                        <dx:ASPxPopupControl ClientInstanceName="ASPxPopupClientControl" Width="150px" Height="165px"
                                            MaxWidth="800px" MaxHeight="800px" MinHeight="150px" MinWidth="150px" ID="pcMain"
                                            PopupElementID="imgButton" HeaderText="View Selector" runat="server" PopupHorizontalAlign="LeftSides"
                                            PopupVerticalAlign="Below" EnableHierarchyRecreation="True">
                                            <ContentCollection>
                                                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                                                    <asp:Panel ID="Panel1" runat="server">
                                                        <table border="0" cellpadding="4" cellspacing="0">
                                                            <tr>
                                                                <td valign="top" style="color: #666666; width: 110px;">
                                                                    <span><b>View by Location<br />
                                                                        &nbsp;
                                                                        <br />
                                                                        Filter by Type...<br />
                                                                        <br />
                                                                        My Servers<br />
                                                                        <br />
                                                                        All Servers<br />
                                                                    </b></span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </dx:PopupControlContentControl>
                                            </ContentCollection>
                                        </dx:ASPxPopupControl>
                                    </td>
                                    <td>
                                        <dx:ASPxFilterControl ID="filter" runat="server" ClientInstanceName="filter">
                                            <Columns>
                                                <dx:FilterControlTextColumn ColumnType="String" DisplayName="ServerName" 
                                                    PropertyName="ServerName">
                                                </dx:FilterControlTextColumn>
                                                <dx:FilterControlTextColumn ColumnType="String" DisplayName="StatName" 
                                                    PropertyName="StatName">
                                                </dx:FilterControlTextColumn>
                                            </Columns>
                                            <ClientSideEvents Applied="function(s, e) { ASPxGridView1.ApplyFilter(e.filterExpression);}" />

<ClientSideEvents Applied="function(s, e) { ASPxGridView1.ApplyFilter(e.filterExpression);}"></ClientSideEvents>
                                        </dx:ASPxFilterControl>
                                        <dx:ASPxButton runat="server" ID="btnApply" Text="Apply" AutoPostBack="false" UseSubmitBehavior="false">
                                <ClientSideEvents Click="function() { filter.Apply(); }" />
<ClientSideEvents Click="function() { filter.Apply(); }"></ClientSideEvents>
                            </dx:ASPxButton>
                                    </td>
                                    <td align="right">
                                        <img id="imgbutton2" alt="" src="../images/filterselector.png" style="width: 103px;
                                            height: 28px;" />
                                        <dx:ASPxPopupControl ClientInstanceName="ASPxPopupClientControl" Width="150px" Height="165px"
                                            MaxWidth="800px" MaxHeight="800px" MinHeight="150px" MinWidth="150px" ID="ASPxPopupControl1"
                                            PopupElementID="imgbutton2" HeaderText="Filter by Type" runat="server" PopupHorizontalAlign="LeftSides"
                                            PopupVerticalAlign="Below" EnableHierarchyRecreation="True">
                                            <ContentCollection>
                                                <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                                                    <asp:Panel ID="Panel2" runat="server">
                                                        <table border="0" cellpadding="4" cellspacing="0">
                                                            <tr>
                                                                <td valign="top" style="color: #666666; width: 110px;" colspan="3">
                                                                    <span><b>
                                                                        <dx:ASPxCheckBox ID="ASPxCheckBox1" runat="server" CheckState="Unchecked" Text="Domino">
                                                                        </dx:ASPxCheckBox>
                                                                        <br />
                                                                        <dx:ASPxCheckBox ID="ASPxCheckBox2" runat="server" CheckState="Unchecked" Text="BES">
                                                                        </dx:ASPxCheckBox>
                                                                        <br />
                                                                        <dx:ASPxCheckBox ID="ASPxCheckBox3" runat="server" CheckState="Unchecked" Text="Sharepoint">
                                                                        </dx:ASPxCheckBox>
                                                                        <br />
                                                                        <dx:ASPxCheckBox ID="ASPxCheckBox4" runat="server" CheckState="Unchecked" Text="Traveler">
                                                                        </dx:ASPxCheckBox>
                                                                    </b></span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Select All" Width="70px">
                                                                    </dx:ASPxButton>
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxButton ID="ASPxButton2" runat="server" Text="Cancel">
                                                                    </dx:ASPxButton>
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxButton ID="ASPxButton3" runat="server" Text="Save">
                                                                    </dx:ASPxButton>
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
                    </tr>
                    <tr>
                    <td colspan="3">
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:VSS_StatisticsConnectionString %>" 
            SelectCommand="SELECT TOP 100 [ID], [ServerName],[Date],[StatName],[StatValue],[WeekNumber],[MonthNumber],[YearNumber],[DayNumber],[HourNumber] FROM [VSS_Statistics].[dbo].[DominoDailyStats]"></asp:SqlDataSource>
              
         
        <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" 
            DataSourceID="SqlDataSource1" KeyFieldName="ID" Theme="Office2003Blue"
            CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
            CssPostfix="Office2010Silver" Width="95%">
            <Columns>
                <dx:GridViewDataTextColumn FieldName="ID" ReadOnly="True" VisibleIndex="0">
                    <EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataHyperLinkColumn  Width="150px" Caption="ServerName" FieldName="ServerName" ShowInCustomizationForm="True" FixedStyle="Left" VisibleIndex="1">
                    <PropertiesHyperLinkEdit NavigateUrlFormatString="ServerDetails.aspx" Target="_self"
                        TextField="ServerName" TextFormatString="">
                    </PropertiesHyperLinkEdit>
                </dx:GridViewDataHyperLinkColumn>
                <dx:GridViewDataDateColumn FieldName="Date" VisibleIndex="2" FixedStyle="Left">
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataTextColumn FieldName="StatName" VisibleIndex="3">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="StatValue" VisibleIndex="4" Width="200px">
                    <DataItemTemplate>
                      <dx:ASPxTrackBar ID="ASPxTrackBar2" runat="server" Position="0" 
                            PositionStart="0" Step="1" Value='<%# Eval("StatValue") %>' 
                            ClientEnabled="false" DragHandleToolTip="" ScalePosition="LeftOrTop" 
                            ShowChangeButtons="False" style="margin-right: 10px;">
                        </dx:ASPxTrackBar>
                    
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="WeekNumber" VisibleIndex="5">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="MonthNumber" VisibleIndex="6">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="YearNumber" VisibleIndex="7">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="DayNumber" VisibleIndex="8">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="HourNumber" VisibleIndex="9">
                </dx:GridViewDataTextColumn>
            </Columns>
            <Settings ShowHorizontalScrollBar="True" />

<Settings ShowHorizontalScrollBar="True"></Settings>

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
                <LoadingPanel ImageSpacing="5px">
                </LoadingPanel>
            </Styles>
            <StylesEditors ButtonEditCellSpacing="0">
                <ProgressBar Height="21px">
                </ProgressBar>
            </StylesEditors>
        </dx:ASPxGridView>
            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <img id="img1" alt="" src="../images/viewselector.png" style="width: 103px;
                                            height: 28px;" />
                                        <dx:ASPxPopupControl ClientInstanceName="ASPxPopupClientControl" Width="150px" Height="165px"
                                            MaxWidth="800px" MaxHeight="800px" MinHeight="150px" MinWidth="150px" ID="pcMain1"
                                            PopupElementID="img1" HeaderText="View Selector" runat="server" PopupHorizontalAlign="LeftSides"
                                            PopupVerticalAlign="Below" EnableHierarchyRecreation="True">
                                            <ContentCollection>
                                                <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                                                    <asp:Panel ID="Panel3" runat="server">
                                                        <table border="0" cellpadding="4" cellspacing="0">
                                                            <tr>
                                                                <td valign="top" style="color: #666666; width: 110px;">
                                                                    <span><b>View by Location<br />
                                                                        &nbsp;
                                                                        <br />
                                                                        Filter by Type...<br />
                                                                        <br />
                                                                        My Servers<br />
                                                                        <br />
                                                                        All Servers<br />
                                                                    </b></span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </dx:PopupControlContentControl>
                                            </ContentCollection>
                                        </dx:ASPxPopupControl>
                                    </td>
                                    <td>
                                    </td>
                                    <td align="right">
                                        <img id="img2" alt="" src="../images/filterselector.png" style="width: 103px;
                                            height: 28px;" />
                                        <dx:ASPxPopupControl ClientInstanceName="ASPxPopupClientControl" Width="150px" Height="165px"
                                            MaxWidth="800px" MaxHeight="800px" MinHeight="150px" MinWidth="150px" ID="ASPxPopupControl2"
                                            PopupElementID="img2" HeaderText="Filter by Type" runat="server" PopupHorizontalAlign="LeftSides"
                                            PopupVerticalAlign="Below" EnableHierarchyRecreation="True">
                                            <ContentCollection>
                                                <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                                                    <asp:Panel ID="Panel4" runat="server">
                                                        <table border="0" cellpadding="4" cellspacing="0">
                                                            <tr>
                                                                <td valign="top" style="color: #666666; width: 110px;" colspan="3">
                                                                    <span><b>
                                                                        <dx:ASPxCheckBox ID="ASPxCheckBox5" runat="server" CheckState="Unchecked" Text="Domino">
                                                                        </dx:ASPxCheckBox>
                                                                        <br />
                                                                        <dx:ASPxCheckBox ID="ASPxCheckBox6" runat="server" CheckState="Unchecked" Text="BES">
                                                                        </dx:ASPxCheckBox>
                                                                        <br />
                                                                        <dx:ASPxCheckBox ID="ASPxCheckBox7" runat="server" CheckState="Unchecked" Text="Sharepoint">
                                                                        </dx:ASPxCheckBox>
                                                                        <br />
                                                                        <dx:ASPxCheckBox ID="ASPxCheckBox8" runat="server" CheckState="Unchecked" Text="Traveler">
                                                                        </dx:ASPxCheckBox>
                                                                    </b></span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <dx:ASPxButton ID="ASPxButton4" runat="server" Text="Select All" Width="70px">
                                                                    </dx:ASPxButton>
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxButton ID="ASPxButton5" runat="server" Text="Cancel">
                                                                    </dx:ASPxButton>
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxButton ID="ASPxButton6" runat="server" Text="Save">
                                                                    </dx:ASPxButton>
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
                    </tr>
                    </table>
                    </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
        <br />
    
    </div>
</asp:Content>

