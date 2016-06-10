<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Customer.aspx.cs" Inherits="CustomerTracking.Customer" %>
<%@ Register assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTabControl" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxClasses" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0"
    CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                            width="100%" Theme="Office2003Blue" OnPageSizeChanged="ContactsGridView_PageSizeChanged">
        <TabPages>
            <dx:TabPage Text="Customer">
                <ContentCollection>
                    <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                        <table width="100%">
                        <dx:ASPxGridView ID="CustomerGridView" runat="server" 
                            AutoGenerateColumns="False" Theme="Office2003Blue"
                            CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                            KeyFieldName="ID" OnHtmlRowCreated="CustomerGridView_HtmlRowCreated"
                            OnRowDeleting="CustomerGridView_RowDeleting" Width="100%" 
                            EnableTheming="True" OnPageSizeChanged="CustomerGridView_PageSizeChanged">
                            <Columns>
                                <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" 
                                    ShowInCustomizationForm="True" VisibleIndex="0" Width="80px">
                                    <EditButton Visible="True">
                                        <Image Url="~/images/edit.png">
                                        </Image>
                                    </EditButton>
                                    <NewButton Visible="True">
                                        <Image Url="~/images/add.gif">
                                        </Image>
                                    </NewButton>
                                    <DeleteButton Visible="True">
                                        <Image Url="~/images/delete.png">
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
                                    <ClearFilterButton Visible="True">
                                        <Image Url="~/images/clear.png">
                                        </Image>
                                    </ClearFilterButton>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewCommandColumn>
                                <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" 
                                    ShowInCustomizationForm="True" Visible="False" VisibleIndex="1">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Name" FieldName="Name" 
                                    ShowInCustomizationForm="True" VisibleIndex="2" Width="170px">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Status Type" FieldName="Status_Type" 
                                    ShowInCustomizationForm="True" VisibleIndex="3">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Address" FieldName="Address" 
                                    ShowInCustomizationForm="True" VisibleIndex="4">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Server Count" FieldName="ServerCount" 
                                    ShowInCustomizationForm="True" VisibleIndex="5">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Comp Replacement" 
                                    FieldName="CompReplacement" ShowInCustomizationForm="True" VisibleIndex="6">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Overall Status" FieldName="OverallStatus" 
                                    ShowInCustomizationForm="True" VisibleIndex="7">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Next FollowUp Date" 
                                    FieldName="NextFollowUpDate" ShowInCustomizationForm="True" VisibleIndex="8">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="License Expiry" FieldName="LicExpDate" 
                                    ShowInCustomizationForm="True" VisibleIndex="9">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Bud Info" FieldName="BudInfo" 
                                    ShowInCustomizationForm="True" VisibleIndex="10">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                            </Columns>
                            <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" />
                            <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                            <SettingsText ConfirmDelete="Are you sure you want to delete?" />
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
                                                    <Styles CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver">
                                                        <LoadingPanel ImageSpacing="5px">
                                                        </LoadingPanel>
                                                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                        </Header>

                                                        <GroupRow Font-Bold="True">
                                                        </GroupRow>

                                                        <AlternatingRow CssClass="GridAltRow" Enabled="True">
                        </AlternatingRow>
                                                    </Styles>
                                                    <StylesEditors ButtonEditCellSpacing="0">
                                                        <ProgressBar Height="21px">
                                                        </ProgressBar>
                                                    </StylesEditors>
                                                    <SettingsPager PageSize="50" SEOFriendly="Enabled" >
            <PageSizeItemSettings Visible="true" />
        </SettingsPager>
                        </dx:ASPxGridView>
                        </table>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
        </TabPages>
        <LoadingPanelImage Url="~/App_Themes/Glass/Web/Loading.gif">
                    </LoadingPanelImage>
                    <Paddings PaddingLeft="0px" />
                    <ContentStyle>
                        <Border BorderColor="#4986A2" />
                    </ContentStyle>
    </dx:ASPxPageControl>
 </asp:Content>
