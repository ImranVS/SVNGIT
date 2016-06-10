<%@ Page Title="VitalSigns Plus-AlertDefinitions_Grid" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="UserProfilesGrid.aspx.cs" Inherits="VSWebUI.UserProfilesGrid" %>
<%@ MasterType virtualpath="~/Site1.Master" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>




<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Manage Server Profiles" 
                    Font-Bold="True" Font-Size="Large" style="color: #000000; font-family: Verdana"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
               <table>
    <tr><td>
        &nbsp;</td></tr>
    <tr><td>
        </td></tr>
    <tr><td>
        &nbsp;</td></tr>
        <tr>
            <td>
                <dx:ASPxGridView ID="UserProfileView" runat="server" AutoGenerateColumns="False" 
                    CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                    CssPostfix="Office2010Silver" KeyFieldName="ID" 
                    OnHtmlDataCellPrepared="UserProfileView_HtmlDataCellPrepared" 
                    OnHtmlRowCreated="UserProfileView_HtmlRowCreated" 
                    OnRowDeleting="UserProfileView_RowDeleting" Width="100%" 
                    EnableTheming="True" Theme="Office2003Blue" OnPageSizeChanged="UserProfileView_PageSizeChanged">
                    <Columns>
                        <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" 
                                                                            
                            ShowInCustomizationForm="True" VisibleIndex="0" Width="70px">
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
                                                                            <CellStyle  CssClass="GridCss">
                                                                                <Paddings Padding="3px" />
                                                                            </CellStyle>
                                                                            <ClearFilterButton Visible="True">
                                                                                <Image Url="~/images/clear.png">
                                                                                </Image>
                                                                            </ClearFilterButton>
                                                                           <HeaderStyle CssClass="GridCssHeader" >
                                                                            <Paddings Padding="5px" />
                                                                            </HeaderStyle>
                                                                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn Caption="Profile Name" ShowInCustomizationForm="True" 
                            VisibleIndex="1" FieldName="Name">
                              <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                              <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" 
                                  Wrap="True" >
                              <Paddings Padding="5px" />
                              </HeaderStyle>
                              <CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" />
                    <Settings ShowFilterRow="True" />
                    <SettingsText ConfirmDelete="Are you sure you  want to delete this record?" />
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
                        <AlternatingRow CssClass="GridAltRow" Enabled="True">
                        </AlternatingRow>
                        <LoadingPanel ImageSpacing="5px">
                        </LoadingPanel>
                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                        </Header>
                    </Styles>
                    <StylesEditors ButtonEditCellSpacing="0">
                        <ProgressBar Height="21px">
                        </ProgressBar>
                    </StylesEditors>
                      <SettingsPager PageSize="50" SEOFriendly="Enabled" >
            <PageSizeItemSettings Visible="true" />
        </SettingsPager>
                </dx:ASPxGridView>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxPopupControl ID="CreateTestAlertPopupControl" runat="server" ClientInstanceName="CreateTestAlertPopupControl"
                    HeaderText="Create Test Alert" PopupHorizontalAlign="WindowCenter" 
                    PopupVerticalAlign="WindowCenter" Theme="Glass" Height="200px" 
                    AllowDragging="True" Modal="True">
                    <ContentCollection>
<dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">

    
                        </dx:PopupControlContentControl>
</ContentCollection>
                </dx:ASPxPopupControl>
            </td>
        </tr>
    </table> </td>
        </tr>
    </table>
</asp:Content>
