<%@ Page Title="VitalSigns Plus - Mail Delivery Status" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="MailDeliveryStatus.aspx.cs" Inherits="VSWebUI.Dashboard.MailDeliveryStatus" %>

<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>








<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
    <table>
        <tr>
            <td>
                <div class="header" id="servernamelbldisp" runat="server">Mail Delivery Status</div>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
                    Theme="Glass" EnableHierarchyRecreation="False">
                    <TabPages>
                        <dx:TabPage Text="Domino">
                            <ContentCollection>
                                <dx:ContentControl runat="server">
                                    <table class="navbarTbl">
                                        <tr>
                                            <td>
                                                <div id="infoDiv" class="info" style="display: block">In order to filter the report by Pending Mail, enter a value into the grid filter area below. The grid will automatically remove the entries below the entered threshold.
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <dx:ASPxGridView ID="EXJournalGridView" runat="server" 
                            AutoGenerateColumns="False" EnableTheming="True" 
                            onhtmldatacellprepared="EXJournalGridView_HtmlDataCellPrepared" 
                            onhtmlrowcreated="EXJournalGridView_HtmlRowCreated" Theme="Office2003Blue" 
                            Width="100%" OnPageSizeChanged="EXJournalGridView_PageSizeChanged">
                            <Columns>
                                <dx:GridViewDataTextColumn Caption="Server Name" FieldName="Name" 
                                    VisibleIndex="0">
                                   <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Overall Status" FieldName="Status" 
                                    VisibleIndex="1">
                                    <Settings AllowAutoFilter="False" />
                                    <DataItemTemplate>
                                        <asp:Label ID="hfNameLabel" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                    </DataItemTemplate>
                                    <HeaderStyle CssClass="GridCssHeader1" />
                                    <CellStyle CssClass="GridCss1">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Pending Mail" FieldName="PendingMail" 
                                    VisibleIndex="4">
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="GreaterOrEqual" />
                                    <HeaderStyle CssClass="GridCssHeader2" />
                                    <CellStyle CssClass="GridCss2">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Held Mail" FieldName="HeldMail" 
                                    VisibleIndex="5">
                                    <Settings AllowAutoFilter="False" />
                                    <HeaderStyle CssClass="GridCssHeader2" />
                                    <CellStyle CssClass="GridCss2">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Dead Mail" FieldName="DeadMail" 
                                    VisibleIndex="6">
                                    <Settings AllowAutoFilter="False" />
                                    <HeaderStyle CssClass="GridCssHeader2" />
                                    <CellStyle CssClass="GridCss2">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Location" FieldName="Location" 
                                    VisibleIndex="8">
                                    <Settings AllowAutoFilter="False" />
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Category" FieldName="Category" 
                                    VisibleIndex="10">
                                    <Settings AllowAutoFilter="False" />
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Last Updated" FieldName="LastUpdate" 
                                    VisibleIndex="12">
                                   <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss"></CellStyle>
                                </dx:GridViewDataTextColumn>
                            </Columns>
                            <SettingsPager AlwaysShowPager="True" PageSize="100">
                                <PageSizeItemSettings Visible="True">
                                </PageSizeItemSettings>
                            </SettingsPager>
                            <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                            <Styles>
                                <AlternatingRow CssClass="GridAltRow" Enabled="True">
                                </AlternatingRow>
                            </Styles>
                        </dx:ASPxGridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Exchange">
                            <ContentCollection>
                                <dx:ContentControl runat="server">
                                    <table class="navbarTbl">
                                        <tr>
                                            <td>
                                                <dx:ASPxGridView ID="ExchangeMailGridView" runat="server" 
                                                    AutoGenerateColumns="False" 
                                                    OnPageSizeChanged="ExchangeMailGridView_PageSizeChanged" 
                                                    Theme="Office2003Blue" 
                                                    OnHtmlDataCellPrepared="ExchangeMailGridView_HtmlDataCellPrepared">
                                                    <Columns>
                                                        <dx:GridViewDataTextColumn Caption="Server Name" FieldName="Name" 
                                                            ShowInCustomizationForm="True" VisibleIndex="1">
															<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Submission Queue" FieldName="PendingMail" 
                                                            ShowInCustomizationForm="True" VisibleIndex="3">
                                                            <Settings AllowAutoFilter="False" />
                                                            <HeaderStyle CssClass="GridCssHeader2" />
                                                            <CellStyle CssClass="GridCss2">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Unreachable Queue" FieldName="DeadMail" 
                                                            ShowInCustomizationForm="True" VisibleIndex="4">
                                                            <Settings AllowAutoFilter="False" />
                                                            <HeaderStyle CssClass="GridCssHeader2" />
                                                            <CellStyle CssClass="GridCss2">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Poison Queue" FieldName="HeldMail" 
                                                            ShowInCustomizationForm="True" VisibleIndex="5">
                                                            <Settings AllowAutoFilter="False" />
                                                            <HeaderStyle CssClass="GridCssHeader2" />
                                                            <CellStyle CssClass="GridCss2">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Last Updated" FieldName="LastUpdate" 
                                                            ShowInCustomizationForm="True" VisibleIndex="8">
                                                            <Settings AllowAutoFilter="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Overall Status" 
                                                            ShowInCustomizationForm="True" VisibleIndex="2" FieldName="Status">
                                                            <Settings AllowAutoFilter="False" />
                                                            <DataItemTemplate>
                                                                <asp:Label ID="hfExNameLabel" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                            </DataItemTemplate>
                                                            <HeaderStyle CssClass="GridCssHeader1" />
                                                            <CellStyle CssClass="GridCss1">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Location" FieldName="Location" 
                                                            ShowInCustomizationForm="True" VisibleIndex="6">
                                                            <Settings AllowAutoFilter="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Category" FieldName="Category" 
                                                            ShowInCustomizationForm="True" VisibleIndex="7">
                                                            <Settings AllowAutoFilter="False" />
                                                        </dx:GridViewDataTextColumn>
                                                    </Columns>
                                                    <SettingsPager AlwaysShowPager="True" PageSize="100">
                                                        <PageSizeItemSettings Visible="True">
                                                        </PageSizeItemSettings>
                                                    </SettingsPager>
                                                    <Settings ShowFilterRow="True" ShowGroupPanel="True" />
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
                    </TabPages>
                </dx:ASPxPageControl>
            </td>
        </tr>
    </table>
</asp:Content>
