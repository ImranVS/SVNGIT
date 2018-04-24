<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ContactSearch.aspx.cs" Inherits="CustomerTracking.ContactSearch" %>

<%@ Register Assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxRoundPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            width: 177px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dx:ASPxRoundPanel ID="SearchContactsRoundPanel" runat="server" Width="100%"
     CssPostfix="Glass" HeaderText="Search Contact" Theme="Office2003Blue">

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
                    </td>
                    </tr>
                    
                    <tr>
                        <td class="style1">
                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" 
                                Text="Contact Name">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxTextBox ID="NSTextbox" runat="server" Width="170px">
                            </dx:ASPxTextBox>
                            <tr>
                                <td class="style1">
                                    <tr>
                                        <td class="style1">
                                            <dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" 
                                                Text="Title">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="TSTextbox" runat="server" Width="170px">
                                            </dx:ASPxTextBox>
                                            <tr>
                                                <td class="style1">
                                                    <tr>
                                                        <td class="style1">
                                                            <dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="lblsmallFont" 
                                                                Text="Phone Number">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox ID="PSTextbox" runat="server" Width="170px">
                                                            <%--<MaskSettings Mask="+9 (999) 000-0000" IncludeLiterals="None"/>--%>
                                                             
                                                            </dx:ASPxTextBox>
                                                            <tr>
                                                                <td class="style1">
                                                                    <tr>
                                                                        <td class="style1">
                                                                            <dx:ASPxLabel ID="ASPxLabel4" runat="server" CssClass="lblsmallFont" 
                                                                                Text="Status">
                                                                            </dx:ASPxLabel>
                                                                        </td>
                                                                        <td>
                                                                            <dx:ASPxTextBox ID="StatusSearchTextbox" runat="server" Width="170px">
                                                                            </dx:ASPxTextBox>
                                                                        </td>
                                                                        <tr>
                                                                            <td class="style1">
                                                                                <tr>
                                                                                    <td class="style1">
                                                                                        <div style="text-align:center; width: 248px;">
                                                                                            <dx:ASPxButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" 
                                                                                                Text="Search" Theme="Office2003Blue" Width="80px">
                                                                                            </dx:ASPxButton>
                                                                                            &nbsp;&nbsp;
                                                                                            </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </td>
                                                                        </tr>
                                                                    </tr>
                                                                </td>
                                                            </tr>
                                                        </td>
                                                    </tr>
                                                </td>
                                            </tr>
                                        </td>
                                    </tr>
                                </td>
                            </tr>
                        </td>
                    </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>

    <dx:ASPxGridView ID="ContactSearchGridView" runat="server" 
                            AutoGenerateColumns="False" Theme="Office2003Blue"
                            CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                            KeyFieldName="ContactID" OnHtmlRowCreated="ContactsGridView_HtmlRowCreated"
                            OnRowDeleting="ContactsGridView_RowDeleting" Width="100%" 
                            EnableTheming="True">
                            <Columns>
                                <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" VisibleIndex="0" 
                                    Width="80px">
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
                                <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" Visible="False" 
                                    VisibleIndex="1">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="ID" FieldName="ContactID" Visible="False" 
                                    VisibleIndex="1">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                
                                <dx:GridViewDataTextColumn Caption="Customer Name" FieldName="Name" VisibleIndex="2" 
                                    Width="170px">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Status Type" FieldName="Status_Type" 
                                    VisibleIndex="7" Visible="False">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Address" FieldName="Address" 
                                    VisibleIndex="8" Visible="False">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Server Count" FieldName="ServerCount" 
                                    VisibleIndex="9" Visible="False">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Comp Replacement" 
                                    FieldName="CompReplacement" VisibleIndex="10" Visible="False">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Overall Status" FieldName="OverallStatus" 
                                    VisibleIndex="11" Visible="False">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Next FollowUp Date" 
                                    FieldName="NextFollowUpDate" VisibleIndex="12" Visible="False">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="License Expiry" FieldName="LicExpDate" 
                                    VisibleIndex="13" Visible="False">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Bud Info" FieldName="BudInfo" 
                                    VisibleIndex="14" Visible="False">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Phone Number" FieldName="PhoneNumber" 
                                    VisibleIndex="5">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Title" FieldName="Title" VisibleIndex="4">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Details" FieldName="Details" 
                                    VisibleIndex="6">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Contact Name" FieldName="ContactName" 
                                    VisibleIndex="3">
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
