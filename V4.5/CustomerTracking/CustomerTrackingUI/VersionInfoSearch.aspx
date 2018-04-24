<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VersionInfoSearch.aspx.cs" Inherits="CustomerTracking.VersionInfoSearch" %>

<%@ Register Assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxRoundPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPanel" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style3
        {
            width: 250px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dx:ASPxRoundPanel ID="VersionInfoRoundPanel" runat="server" Width="100%"
    CssPostfix="Glass" HeaderText="Version Information" Theme="Office2003Blue">
     <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
    <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px"></ContentPaddings>
<HeaderStyle Height="23px">
            <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
<Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px"></Paddings>
        </HeaderStyle>
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                <table width="100%">
               <tr>
                <td class="style1">
               
                <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Customer Name"
                 CssClass="lblsmallFont">
                </dx:ASPxLabel>
                </td>
                <td>
                    <dx:ASPxComboBox ID="CustomerComboBox" runat="server">
                      <ValidationSettings CausesValidation="True" Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField IsRequired="True" ErrorText="Select Customer"></RequiredField>
                        <ErrorImage ToolTip="Select Customer"></ErrorImage>
                        <RequiredField ErrorText="Select Customer" IsRequired="True" />
                      </ValidationSettings>
                    </dx:ASPxComboBox>
                    </td></tr>
                    
                <tr>
                <td class="style3">
               
                    <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Version Number"
                     CssClass="lblsmallFont">
                    </dx:ASPxLabel>
                </td>
                <td>
                    <dx:ASPxTextBox ID="VersionNumberTextbox" runat="server" Width="170px">
                    </dx:ASPxTextBox>
                </td></tr>
                    
                <tr>
                                
                    <td class="style3">
                        <div style="text-align:center; width: 248px;">
                            <dx:ASPxButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" 
                                Text="Search" Theme="Office2003Blue" Width="80px">
                            </dx:ASPxButton>
                            &nbsp;&nbsp;
                            </div>
                    </td>
                    </tr>
                    </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
    
    <dx:ASPxGridView ID="VersionInfoSearchGridView" runat="server" 
                            AutoGenerateColumns="False" Theme="Office2003Blue"
                            CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                            KeyFieldName="VersionInfoID" OnHtmlRowCreated="VersionInfoGridView_HtmlRowCreated"
                            OnRowDeleting="VersionInfoGridView_RowDeleting" Width="100%" 
                            EnableTheming="True" OnPageSizeChanged="VersionInfoGridView_PageSizeChanged">
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
                                <dx:GridViewDataTextColumn Caption="ID" FieldName="VersionInfoID" 
                                    ShowInCustomizationForm="True" Visible="False" VisibleIndex="1">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Customer Name" FieldName="Name" 
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
                                    ShowInCustomizationForm="True" VisibleIndex="3" Visible="False">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Address" FieldName="Address" 
                                    ShowInCustomizationForm="True" VisibleIndex="4" Visible="False">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Server Count" FieldName="ServerCount" 
                                    ShowInCustomizationForm="True" VisibleIndex="5" Visible="False">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Comp Replacement" 
                                    FieldName="CompReplacement" ShowInCustomizationForm="True" 
                                    VisibleIndex="6" Visible="False">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Overall Status" FieldName="OverallStatus" 
                                    ShowInCustomizationForm="True" VisibleIndex="7" Visible="False">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Next FollowUp Date" 
                                    FieldName="NextFollowUpDate" ShowInCustomizationForm="True" 
                                    VisibleIndex="8" Visible="False">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="License Expiry" FieldName="LicExpDate" 
                                    ShowInCustomizationForm="True" VisibleIndex="9" Visible="False">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Bud Info" FieldName="BudInfo" 
                                    ShowInCustomizationForm="True" VisibleIndex="10" Visible="False">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Version Number" FieldName="VersionNumber" 
                                    ShowInCustomizationForm="True" VisibleIndex="11">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Install Date" FieldName="InstallDate" 
                                    ShowInCustomizationForm="True" VisibleIndex="12">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Version Info Details" FieldName="Details" 
                                    ShowInCustomizationForm="True" VisibleIndex="13">
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
</asp:Content>
