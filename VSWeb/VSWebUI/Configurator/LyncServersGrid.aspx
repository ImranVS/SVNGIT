<%@ Page Language="C#" Title="VitalSigns Plus - Skype for Business Servers" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="LyncServersGrid.aspx.cs" Inherits="VSWebUI.Configurator.LyncServersGrid" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.alert-success').delay(10000).fadeOut("slow", function () {
            });
        });
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%">
        <tr>
            <td>
                <div class="header" id="servernamelbldisp" runat="server">Microsoft Skype for Business Servers</div>
            </td>
        </tr>
        <tr>
            <td>
                <div id="successDiv" class="alert alert-success" runat="server" style="display: none">Success.
                </div>
                <dx:ASPxGridView ID="MSLyncGridView" runat="server" AutoGenerateColumns="False" 
                    EnableTheming="True" Theme="Office2003Blue" Width="100%" 
                    onpagesizechanged="MSLyncGridView_PageSizeChanged" Cursor="pointer" 
                    KeyFieldName="ID" onhtmlrowcreated="MSLyncGridView_HtmlRowCreated">
                    <Columns>
                        <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" VisibleIndex="0" 
                            Width="60px">
                            <EditButton Visible="True">
                                            <Image Url="~/images/edit.png">
                                            </Image>
                                        </EditButton>
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataCheckColumn Caption="Enabled" FieldName="Enabled" 
                            VisibleIndex="1" Width="60px">
                            <HeaderStyle CssClass="GridCssHeader1" />
                            <CellStyle CssClass="GridCss1">
                            </CellStyle>
                        </dx:GridViewDataCheckColumn>
                        <dx:GridViewDataTextColumn Caption="Name" FieldName="ServerName" 
                            VisibleIndex="2">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Category" FieldName="Category" 
                            VisibleIndex="3">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Scan Interval" FieldName="ScanInterval" 
                            VisibleIndex="4">
                            <HeaderStyle CssClass="GridCssHeader2" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Group Chats" 
                            VisibleIndex="5">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="One-on-one Chats" 
                            VisibleIndex="6">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Desktop Client" 
                            VisibleIndex="7">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Mobile Client" 
                            VisibleIndex="8">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Number of Users Connected" VisibleIndex="9">
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsBehavior AllowSelectByRowClick="True" AllowSelectSingleRowOnly="True" 
                        ProcessSelectionChangedOnServer="True" />
                    <SettingsPager AlwaysShowPager="True">
                        <PageSizeItemSettings Visible="True">
                        </PageSizeItemSettings>
                    </SettingsPager>
                    <Settings ShowGroupPanel="True" />
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
</asp:Content>