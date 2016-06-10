<%@ Page Title="VitalSigns Plus - Scheduled Reports" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="ScheduledReportsGrid.aspx.cs" Inherits="VSWebUI.Configurator.ScheduledReportsGrid" %>

<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table class="tableWidth100Percent">
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Verdana" 
                    Font-Size="Large" ForeColor="Black" Text="Scheduled Reports"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxGridView ID="ScheduledReportsGridView" runat="server" 
                    AutoGenerateColumns="False" EnableTheming="True" KeyFieldName="ID" 
                    onrowdeleting="ScheduledReportsGridView_RowDeleting" 
                    onrowinserting="ScheduledReportsGridView_RowInserting" 
                    onrowupdating="ScheduledReportsGridView_RowUpdating" Theme="Office2003Blue">
                    <Columns>
                        <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" 
                            FixedStyle="Left" VisibleIndex="0" Width="70px">
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
                            <UpdateButton Visible="True">
                                <Image Url="~/images/update.gif">
                                </Image>
                            </UpdateButton>
                            <ClearFilterButton Visible="True">
                                <Image Url="~/images/clear.png">
                                </Image>
                            </ClearFilterButton>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataComboBoxColumn Caption="Report Name" FieldName="ReportName" 
                            VisibleIndex="2">
                            <Settings AutoFilterCondition="Contains" />
                            <EditFormSettings Visible="True" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataComboBoxColumn>
                        <dx:GridViewDataTextColumn Caption="Title" FieldName="Title" VisibleIndex="1">
                            <Settings AutoFilterCondition="Contains" />
                            <EditFormSettings Visible="True" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" Visible="False" 
                            VisibleIndex="5">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataComboBoxColumn Caption="Frequency" FieldName="Frequency" 
                            VisibleIndex="4">
                            <PropertiesComboBox>
                                <Items>
                                    <dx:ListEditItem Text="Daily" Value="Daily" />
                                    <dx:ListEditItem Text="Weekly" Value="Weekly" />
                                    <dx:ListEditItem Text="Monthly" Value="Monthly" />
                                </Items>
                            </PropertiesComboBox>
                            <Settings AllowAutoFilter="False" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataComboBoxColumn>
                        <dx:GridViewDataTextColumn Caption="Send To" FieldName="SendTo" 
                            VisibleIndex="3">
                            <Settings AutoFilterCondition="Contains" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsBehavior ConfirmDelete="True" />
                    <SettingsPager AlwaysShowPager="True" PageSize="20">
                    </SettingsPager>
                    <Settings ShowFilterRow="True" />
                    <SettingsText ConfirmDelete="Are you sure you want to delete this record?" />
                    <Styles>
                        <AlternatingRow CssClass="GridAltrow">
                        </AlternatingRow>
                    </Styles>
                </dx:ASPxGridView>
            </td>
        </tr>
    </table>

</asp:Content>
