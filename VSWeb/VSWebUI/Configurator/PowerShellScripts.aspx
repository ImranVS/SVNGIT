<%@ Page Title="VitalSigns Plus-PowerShellScripts" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="PowerShellScripts.aspx.cs" Inherits="VSWebUI.WebForm13" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dx:ASPxRoundPanel ID="PwrShelScriptRoundPanel" runat="server" 
        HeaderText="Power Shell Scripts" Theme="Glass" Width="100%" Font-Size="Small">
        <PanelCollection>
            <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                <dx:ASPxGridView ID="PwrShelGridView" runat="server" AutoGenerateColumns="False" 
                    EnableTheming="True" Theme="Office2003Blue" KeyFieldName="ScriptName" 
                    OnHtmlRowCreated="PwrShelGridView_HtmlRowCreated" OnPageSizeChanged="PwrShelGridView_PageSizeChanged" 
                    OnRowDeleting="PwrShelGridView_RowDeleting" Width="100%">
                    <Columns>
                        <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" ShowInCustomizationForm="True"
                                                    VisibleIndex="0" Width="70px" FixedStyle="Left">
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
                                                    <CellStyle CssClass="GridCss">
                                                        <Paddings Padding="3px" />
<Paddings Padding="3px"></Paddings>
                                                    </CellStyle>
                                                    <ClearFilterButton Visible="True">
                                                        <Image Url="~/images/clear.png">
                                                        </Image>
                                                    </ClearFilterButton>
                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn Caption="Script Name" FieldName="ScriptName" 
                            ShowInCustomizationForm="True" VisibleIndex="1">
                            <Settings AutoFilterCondition="Contains" />
<Settings AutoFilterCondition="Contains"></Settings>

                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Script Details" FieldName="ScriptDetails" 
                            ShowInCustomizationForm="True" VisibleIndex="2">
                            <Settings AutoFilterCondition="Contains" />
<Settings AutoFilterCondition="Contains"></Settings>

                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Category" FieldName="Category" 
                            ShowInCustomizationForm="True" VisibleIndex="2">
                            <Settings AutoFilterCondition="Contains" />
<Settings AutoFilterCondition="Contains"></Settings>

                            <HeaderStyle CssClass="GridCss" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Description" FieldName="Description" 
                            ShowInCustomizationForm="True" VisibleIndex="2">
                            <Settings AutoFilterCondition="Contains" />
<Settings AutoFilterCondition="Contains"></Settings>

                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>





                    </Columns>
                    <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                     <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" 
                        AutoExpandAllGroups="True" />

<SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" AutoExpandAllGroups="True"></SettingsBehavior>

                     <SettingsPager PageSize="50" SEOFriendly="Enabled" >
            <PageSizeItemSettings Visible="true" />
<PageSizeItemSettings Visible="True"></PageSizeItemSettings>
        </SettingsPager>
                    <SettingsText ConfirmDelete="Are you sure you want to delete this record?" />

<Settings ShowFilterRow="True" ShowGroupPanel="True"></Settings>

<SettingsText ConfirmDelete="Are you sure you want to delete this record?"></SettingsText>

                    <Styles>
                        <GroupRow Font-Bold="True">
                        </GroupRow>
                        <AlternatingRow CssClass="GridAltRow" Enabled="True">
                        </AlternatingRow>
                    </Styles>
                      <Templates>
     <GroupRowContent>
            <%# Container.GroupText%>
     </GroupRowContent>
</Templates>
                </dx:ASPxGridView>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
</asp:Content>
