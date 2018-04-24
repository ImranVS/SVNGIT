<%@ Page Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="CustomPageConfig.aspx.cs" Inherits="VSWebUI.Dashboard.CustomPageConfig" %>

<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
    
        <table class="tableWidth100Percent">
            <tr>
                <td>
                    <div class="header" id="Div1" runat="server">My Custom Pages</div>
                </td>
            </tr>
            <tr>
                <td>
                    <dx:ASPxGridView ID="ConfigureURLsGridView" runat="server" 
                        AutoGenerateColumns="False" EnableTheming="True" 
                        onrowcommand="ConfigureURLsGridView_RowCommand" 
                        onrowdeleting="ConfigureURLsGridView_RowDeleting" 
                        onrowinserting="ConfigureURLsGridView_RowInserting" 
                        onrowupdating="ConfigureURLsGridView_RowUpdating" Theme="Office2003Blue" 
                        Width="70%" OnPageSizeChanged="ConfigureURLsGridView_PageSizeChanged">
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="Title" FieldName="Title" VisibleIndex="1">
                                <HeaderStyle CssClass="GridCssHeader" />
                                <CellStyle CssClass="GridCss">
                                </CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="URL" FieldName="URL" VisibleIndex="2">
                                <HeaderStyle CssClass="GridCssHeader" />
                                <CellStyle CssClass="GridCss">
                                </CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" 
                                FixedStyle="Left" VisibleIndex="0" Width="60px">
                                <ClearFilterButton Visible="True">
                                </ClearFilterButton>
                                <EditButton Visible="True">
                                    <Image Url="../images/edit.png">
                                    </Image>
                                </EditButton>
                                <NewButton Visible="True">
                                    <Image Url="../images/icons/add.png">
                                    </Image>
                                </NewButton>
                                <DeleteButton Visible="True">
                                    <Image Url="../images/delete.png">
                                    </Image>
                                </DeleteButton>
                                <CancelButton Visible="True">
                                    <Image Url="~/images/cancel.gif">
                                    </Image>
                                </CancelButton>
                                <UpdateButton>
                                    <Image Url="~/images/update.gif">
                                    </Image>
                                </UpdateButton>
                                <HeaderStyle CssClass="GridCssHeader" />
                                <CellStyle CssClass="GridCss">
                                </CellStyle>
                            </dx:GridViewCommandColumn>
                            <dx:GridViewDataCheckColumn Caption="Private" FieldName="IsPrivate" 
                                ShowInCustomizationForm="False" Visible="False" VisibleIndex="3">
                                <HeaderStyle CssClass="GridCssHeader" />
                                <CellStyle CssClass="GridCss">
                                </CellStyle>
                            </dx:GridViewDataCheckColumn>
                        </Columns>
                        <SettingsBehavior ConfirmDelete="True" />
                        <SettingsText ConfirmDelete="Are you sure you want to delete this URL?" />

<SettingsBehavior ConfirmDelete="True"></SettingsBehavior>

<SettingsText ConfirmDelete="Are you sure you want to delete this URL?"></SettingsText>

                        <Styles>
                            <EditFormCell CssClass="GridCss">
                            </EditFormCell>
                            <EditFormColumnCaption CssClass="GridCss">
                            </EditFormColumnCaption>
                        </Styles>
                    </dx:ASPxGridView>
                </td>
            </tr>
            </table>
    
    </div>
 </asp:Content>
