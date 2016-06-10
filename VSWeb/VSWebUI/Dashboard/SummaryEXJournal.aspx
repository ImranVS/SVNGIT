<%@ Page Title="VitalSigns Plus - EXJournal Summary" Language="C#" AutoEventWireup="true" MasterPageFile="~/DashboardSite.Master" CodeBehind="SummaryEXJournal.aspx.cs" Inherits="VSWebUI.Dashboard.SummaryEXJournal" %>
<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <meta http-equiv="CACHE-CONTROL" content="NO-CACHE" />
        <meta http-equiv="PRAGMA" content="NO-CACHE" />
        <link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="tableWidth100Percent">
        <tr>
            <td>
                <div class="header" id="servernamelbldisp" runat="server">EXJournal Summary</div>
                </td>
        </tr>
        <tr>
            <td></td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <dx:ASPxGridView ID="EXJournalGridView" runat="server" AutoGenerateColumns="False" 
                    EnableTheming="True" Theme="Office2003Blue" OnPageSizeChanged="EXJournalGridView_PageSizeChanged"
                    onhtmldatacellprepared="EXJournalGridView_HtmlDataCellPrepared" 
                    onhtmlrowcreated="EXJournalGridView_HtmlRowCreated">
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="Name" FieldName="Name" 
                            VisibleIndex="0">
                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss" Wrap="False">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Server Status" FieldName="Status" 
                            VisibleIndex="1">
                            <Settings AllowAutoFilter="False" />
                            <DataItemTemplate>
                                <asp:Label ID="hfNameLabel" Text='<%# Eval("Status") %>' runat="server"></asp:Label>
                            </DataItemTemplate>
                            <HeaderStyle CssClass="GridCssHeader" HorizontalAlign="Center" />
                            <CellStyle CssClass="GridCss1">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="EXJournal Total" FieldName="EXTotal" 
                            VisibleIndex="2">
                            <Settings AllowAutoFilter="False" />
                            <DataItemTemplate>
                                <asp:Label ID="hfExTotalLabel" Text='<%# Eval("EXTotal") %>' runat="server"></asp:Label>
                            </DataItemTemplate>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss2" Font-Bold="True">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="EXJournal3" FieldName="EXJournal" 
                            VisibleIndex="5">
                            <Settings AllowAutoFilter="False" />
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="EXJournal1" FieldName="EXJournal1" 
                            VisibleIndex="3">
                            <Settings AllowAutoFilter="False" />
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="EXJournal2" FieldName="EXJournal2" 
                            VisibleIndex="4">
                            <Settings AllowAutoFilter="False" />
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Pending Mail" FieldName="PendingMail" 
                            VisibleIndex="7" Visible="False">
                            <Settings AllowAutoFilter="False" />
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Held Mail" FieldName="HeldMail" 
                            VisibleIndex="8" Visible="False">
                            <Settings AllowAutoFilter="False" />
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Dead Mail" FieldName="DeadMail" 
                            VisibleIndex="10" Visible="False">
                            <Settings AllowAutoFilter="False" />
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="EXJournal Date" FieldName="ExJournalDate" 
                            VisibleIndex="6">
                            <Settings AllowAutoFilter="False" />
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                    </Columns>                    
                    <SettingsPager PageSize="100" AlwaysShowPager="True" Mode="ShowAllRecords">
                        <AllButton Visible="True">
                        </AllButton>
                        <PageSizeItemSettings Visible="True">
                        </PageSizeItemSettings>
                    </SettingsPager>
                    <Settings ShowFilterRow="True" />
                    <Styles>
                        <AlternatingRow CssClass="GridAltRow" Enabled="True">
                        </AlternatingRow>
                    </Styles>
                    <SettingsText EmptyDataRow="No records found" />
                </dx:ASPxGridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="timer1" />
                    </Triggers>
                </asp:UpdatePanel>
            
            </td>
        </tr>
    </table>

</asp:Content>