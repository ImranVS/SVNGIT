<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerInfo.aspx.cs" Inherits="CustomerTracking.CustomerInfo" %>

<%@ Register Assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTabControl" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxClasses" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxRoundPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxMenu" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <dx:ASPxRoundPanel ID="CustomerTrackingRoundPanel" runat="server" 
                                        CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                        CssPostfix="Glass" 
                                        GroupBoxCaptionOffsetY="-24px" HeaderText="Customer Tracking Information" 
                                        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
                                        Width="100%" Height="250px">
                                        <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                                        <HeaderStyle Height="23px">
                                        <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
<Paddings PaddingLeft="2px" PaddingTop="0px" PaddingBottom="0px"></Paddings>
                                        </HeaderStyle>
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent0" runat="server" SupportsDisabledAttribute="True">
        <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0"
            Width="100%" CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass"
            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" TabSpacing="0px" 
            Font-Bold="True">
            <TabPages>
                <dx:TabPage Text="Customer Details">
                    <ContentCollection>
                        <dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">
                            <table>
                                <tr>
                                    <td valign="top">
                                    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px"
                                HeaderText="Customer Information" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                                Width="100%" ClientInstanceName="RoundPanel">
                                <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
                                <ContentPaddings Padding="2px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>
                                <HeaderStyle Height="23px">
                                    <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                    <Paddings Padding="0px" PaddingLeft="2px" PaddingRight="2px" PaddingBottom="7px">
                                    </Paddings>
                                </HeaderStyle>
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                        <table>
                                            <tr>
                                                <td>
                                               
                                                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Customer Name:" CssClass="lblsmallFont">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="NameTextbox" runat="server" Width="170px">
                                                        <ValidationSettings>
                                                            <RequiredField IsRequired="True" />
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>
                                                <td>
                                               
                                                    <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Status Type:" CssClass="lblsmallFont">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="Status_TypeTextbox" runat="server" Width="170px">
                                                        <ValidationSettings>
                                                            <RequiredField IsRequired="True" />
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                               
                                                    <dx:ASPxLabel ID="ASPxLabel16" runat="server" Text="Address:" CssClass="lblsmallFont">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="AddressTextBox" runat="server" Width="170px">
                                                        <ValidationSettings>
                                                            <RequiredField IsRequired="True" />
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>
                                                 <td>
                                               
                                                    <dx:ASPxLabel ID="ASPxLabel15" runat="server" Text="Server Count:" CssClass="lblsmallFont">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="ServerCountTextbox" runat="server" Width="170px">
                                                        <ValidationSettings>
                                                            <RequiredField IsRequired="True" />
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>
                                               </tr>
                                                <tr>
                                                 <td>
                                               
                                                    <dx:ASPxLabel ID="ASPxLabel17" runat="server" Text="Comp Replacement:" CssClass="lblsmallFont">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="CompReplacementTextbox" runat="server" Width="170px">
                                                        <ValidationSettings>
                                                            <RequiredField IsRequired="True" />
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>
                                                
                                          
                                                       
                                                         <td>
                                                                <dx:ASPxLabel ID="ASPxLabel18" runat="server" CssClass="lblsmallFont" 
                                                                    Text="Overall Status:">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxTextBox ID="OverallStatusTextbox" runat="server" Width="170px">
                                                                    <ValidationSettings>
                                                                        <RequiredField IsRequired="True" />
<RequiredField IsRequired="True"></RequiredField>
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
                                                            </td>
                                                            </tr>
                                                                <tr>
                                                                <td>
                                                                    <dx:ASPxLabel ID="ASPxLabel19" runat="server" CssClass="lblsmallFont" 
                                                                        Text="Next FollowUp Date:">
                                                                    </dx:ASPxLabel>
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxDateEdit ID="NextFollowUpDate" runat="server" Width="150px">
                                                                           <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                            SetFocusOnError="True">
                                                                            <RequiredField ErrorText="Follow Up Date is a required field" IsRequired="True" />
                                                                        </ValidationSettings>
                                                                        </dx:ASPxDateEdit>
                                                                </td>
                                                            
                                                           
                                                                <td>
                                                                    <dx:ASPxLabel ID="ASPxLabel20" runat="server" CssClass="lblsmallFont" 
                                                                        Text="License Expiry:">
                                                                    </dx:ASPxLabel>
                                                                </td>
                                                                <td>
                                                                <dx:ASPxDateEdit ID="LicExpDate" runat="server" Width="150px">
                                                                           <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                            SetFocusOnError="True">
                                                                            <RequiredField ErrorText="License Exiry Date is a required field" IsRequired="True" />
                                                                        </ValidationSettings>
                                                                        </dx:ASPxDateEdit>
                                                                </td>
                                                            </tr>
                                                            
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel21" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Bud Info:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="BudInfoTextbox" runat="server" Width="170px">
                                                                            <ValidationSettings>
                                                                                <RequiredField IsRequired="True" />
<RequiredField IsRequired="True"></RequiredField>
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                 
                                                                 
                                                            </tr>
                                                       
                                                       <tr>
                                                        <td colspan = "4">
                                                        <div id="successDiv" runat="server" class="success" style="display: none">Contact Details were updated successfully.
                                                        </div>
                                                        <div id="errorDiv1" class="error" runat="server" style="display: none">Contact Details were not updated.
                                                        </div>
                                                        </td>
                                                        </tr>
                                        </table>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>
                                    </td>
                                    <td valign="top">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
                <dx:TabPage Text="Contacts">
                    <ContentCollection>
                        <dx:ContentControl ID="ContentControl2" runat="server" SupportsDisabledAttribute="True">
                            
                            <table class="style16">
                                <tr>
                                    <td>
                                        <dx:ASPxRoundPanel ID="ASPxRoundPanel4" runat="server" 
                                            CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                            GroupBoxCaptionOffsetY="-24px" HeaderText="Contacts" Height="104px" 
                                            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                                            <ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" 
                                                PaddingTop="10px" />
<ContentPaddings Padding="2px" PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                                            <HeaderStyle Height="23px">
                                            <Paddings Padding="0px" PaddingBottom="7px" PaddingLeft="2px" 
                                                PaddingRight="2px" PaddingTop="0px" />
<Paddings Padding="0px" PaddingLeft="2px" PaddingTop="0px" PaddingRight="2px" PaddingBottom="7px"></Paddings>
                                            </HeaderStyle>
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
                                                    <table>
                                                                                                                
                                                <tr>
                                                <td>
                                               
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
                                                                        </td>
                                                                    </tr>
                                                        
                                                          
                                                    </table>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxRoundPanel>
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
                <dx:TabPage Text="Notes">
                
                    <ContentCollection>
                        <dx:ContentControl ID="ContentControl5" runat="server" SupportsDisabledAttribute="True">
                            
                            <table class="style16">
                                <tr>
                                    <td>
                                        <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" 
                                            CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                            GroupBoxCaptionOffsetY="-24px" HeaderText="Notes" Height="104px" 
                                            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                                            <ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" 
                                                PaddingTop="10px" />
<ContentPaddings Padding="2px" PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                                            <HeaderStyle Height="23px">
                                            <Paddings Padding="0px" PaddingBottom="7px" PaddingLeft="2px" 
                                                PaddingRight="2px" PaddingTop="0px" />
<Paddings Padding="0px" PaddingLeft="2px" PaddingTop="0px" PaddingRight="2px" PaddingBottom="7px"></Paddings>
                                            </HeaderStyle>
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                                                    <table>
                                                        <tr>
                                               
                                               <td>
                                                    <%--<dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Date:" CssClass="lblsmallFont">
                                                    </dx:ASPxLabel>
                                               
                                                <td>
                                                    <dx:ASPxTextBox ID="NCDateTextbox" runat="server" Width="170px">
                                                        <ValidationSettings>
                                                            <RequiredField IsRequired="True" />
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>
                                                
                                                <tr>
                                                <td>
                                               
                                                    <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Details:" CssClass="lblsmallFont">
                                                    </dx:ASPxLabel>
                                               
                                                <td>
                                                    <dx:ASPxTextBox ID="NCDetailTextbox" runat="server" Width="170px">
                                                        <ValidationSettings>
                                                            <RequiredField IsRequired="True" />
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>--%>
                                                    <dx:ASPxGridView ID="NotesSearchGridView" runat="server" 
                            AutoGenerateColumns="False" Theme="Office2003Blue"
                            CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                            KeyFieldName="NotesID" OnHtmlRowCreated="NotesGridView_HtmlRowCreated"
                            OnRowDeleting="NotesGridView_RowDeleting" Width="100%" 
                            EnableTheming="True" >
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
                                 <dx:GridViewDataTextColumn Caption="ID" FieldName="NotesID" 
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
                                <dx:GridViewDataTextColumn Caption="Date" FieldName="Date" 
                                    ShowInCustomizationForm="True" VisibleIndex="11">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Details" FieldName="Details" 
                                    ShowInCustomizationForm="True" VisibleIndex="12">
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
                                                </td>
                                                        </tr>
                                               </table>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxRoundPanel>
                                    </td>
                                    
                                </tr>
                            </table>
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
                <dx:TabPage Text="Tickets">
                
                    <ContentCollection>
                        <dx:ContentControl ID="ContentControl6" runat="server" SupportsDisabledAttribute="True">
                            
                            <table class="style16">
                                <tr>
                                    <td>
                                        <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" 
                                            CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                            GroupBoxCaptionOffsetY="-24px" HeaderText="Tickets" Height="104px" 
                                            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                                            <ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" 
                                                PaddingTop="10px" />
<ContentPaddings Padding="2px" PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                                            <HeaderStyle Height="23px">
                                            <Paddings Padding="0px" PaddingBottom="7px" PaddingLeft="2px" 
                                                PaddingRight="2px" PaddingTop="0px" />
<Paddings Padding="0px" PaddingLeft="2px" PaddingTop="0px" PaddingRight="2px" PaddingBottom="7px"></Paddings>
                                            </HeaderStyle>
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                                                    <table>
                                                   <tr>     
                                               <td>
                                                   
                                                    <dx:ASPxGridView ID="TicketSearchGridView" runat="server" 
                            AutoGenerateColumns="False" Theme="Office2003Blue"
                            CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                            KeyFieldName="TicketID" OnHtmlRowCreated="TicketsGridView_HtmlRowCreated"
                            OnRowDeleting="TicketsGridView_RowDeleting" Width="100%" 
                            EnableTheming="True" >
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
                                <dx:GridViewDataTextColumn Caption="ID" FieldName="TicketID" 
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
                                <dx:GridViewDataTextColumn Caption="Date" FieldName="Date" 
                                    ShowInCustomizationForm="True" VisibleIndex="11">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Ticket Number" FieldName="TicketNumber" 
                                    ShowInCustomizationForm="True" VisibleIndex="12">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Ticket Details" FieldName="Details" 
                                    ShowInCustomizationForm="True" VisibleIndex="13">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Ticket Status" FieldName="Status" 
                                    ShowInCustomizationForm="True" VisibleIndex="14">
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
                                                </td>
                                                  </tr>      
                                                          
                                                    </table>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxRoundPanel>
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
                <dx:TabPage Text="Version Info">
                
                    <ContentCollection>
                        <dx:ContentControl ID="ContentControl4" runat="server" SupportsDisabledAttribute="True">
                            
                            <table class="style16">
                                <tr>
                                    <td>
                                        <dx:ASPxRoundPanel ID="ASPxRoundPanel5" runat="server" 
                                            CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                            GroupBoxCaptionOffsetY="-24px" HeaderText="Version Info" Height="104px" 
                                            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                                            <ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" 
                                                PaddingTop="10px" />
<ContentPaddings Padding="2px" PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                                            <HeaderStyle Height="23px">
                                            <Paddings Padding="0px" PaddingBottom="7px" PaddingLeft="2px" 
                                                PaddingRight="2px" PaddingTop="0px" />
<Paddings Padding="0px" PaddingLeft="2px" PaddingTop="0px" PaddingRight="2px" PaddingBottom="7px"></Paddings>
                                            </HeaderStyle>
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent5" runat="server" SupportsDisabledAttribute="True">
                                                    <table>
                                                       
                                                       <tr>
                                               <td>
                                                    <%--<dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="Version Number:" CssClass="lblsmallFont">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="VersionNumberTextbox" runat="server" Width="170px">
                                                        <ValidationSettings>
                                                            <RequiredField IsRequired="True" />
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>
                                                
                                                <td>
                                               
                                                    <dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="Install Date:" CssClass="lblsmallFont">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="InstallDateTextbox" runat="server" Width="170px">
                                                        <ValidationSettings>
                                                            <RequiredField IsRequired="True" />
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>
                                                        
                                                        <td>
                                               
                                                    <dx:ASPxLabel ID="ASPxLabel22" runat="server" Text="Details:" CssClass="lblsmallFont">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="VersionDetailsTextbox" runat="server" Width="170px">
                                                        <ValidationSettings>
                                                            <RequiredField IsRequired="True" />
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>--%>
                                                    <dx:ASPxGridView ID="VersionInfoSearchGridView" runat="server" 
                            AutoGenerateColumns="False" Theme="Office2003Blue"
                            CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                            KeyFieldName="VersionInfoID" OnHtmlRowCreated="VersionInfoGridView_HtmlRowCreated"
                            OnRowDeleting="VersionInfoGridView_RowDeleting" Width="100%" 
                            EnableTheming="True">
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
                                                </td>
                                                </tr>
                                                    </table>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxRoundPanel>
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
            </TabPages>
            <LoadingPanelImage Url="~/App_Themes/Glass/Web/Loading.gif">
            </LoadingPanelImage>
            <Paddings PaddingLeft="0px" />

<Paddings PaddingLeft="0px"></Paddings>

            <ContentStyle>
                <Border BorderColor="#4986A2" />
                <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px"></Border>
            </ContentStyle>
        </dx:ASPxPageControl>
        <div id="errorDiv" class="error" runat="server" style="display: none">Error.
        </div>
        <table>
            <tr>
                <td>
                    &nbsp;
                </td>
                <caption>
                    <dx:ASPxPopupControl ID="ErrorMessagePopupControl" runat="server"
                    AllowDragging="true" ClientInstanceName="pcErrorMessage"
                    CloseAction="CloseButton" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                    CssPostfix="Glass" EnableAnimation="False" EnableViewState="False" 
                    HeaderText="Validation Failure" Height="150px" Modal="True" 
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="300px">
                    <LoadingPanelImage Url="~/App_Themes/Glass/Web/Loading.gif">
                        </LoadingPanelImage>

                        <ContentCollection>
                    <dx:PopupControlContentControl ID = "PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                    <dx:ASPxPanel ID="Panel1" runat="server" DefaultButton="btOK">
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent6" runat="server">
                                            <div style="min-height: 70px;">
                                                <dx:ASPxLabel ID="ErrorMessageLabel" runat="server">
                                                </dx:ASPxLabel>
                                            </div>
                                            <div>
                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td align="center">
                                                            <dx:ASPxButton ID="ValidationOkButton" runat="server" AutoPostBack="False" 
                                                                CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                                                CssPostfix="Office2010Blue" 
                                                                SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="OK" 
                                                                Width="80px">
                                                                <ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }" />
<ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }"></ClientSideEvents>
                                                            </dx:ASPxButton>
                                                            <dx:ASPxButton ID="ValidationUpdatedButton" runat="server" AutoPostBack="False" 
                                                                CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                                                CssPostfix="Office2010Blue"  
                                                                SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="OK" 
                                                                Visible="False" Width="80px" OnClick="ValidationUpdatedButton_Click">
                                                                <ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }" />
<ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }"></ClientSideEvents>
                                                            </dx:ASPxButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxPanel>
                    </dx:PopupControlContentControl>
                    </ContentCollection>
                    </dx:ASPxPopupControl>
                    <tr>
                        <td>
                            <dx:ASPxButton ID="formOKButton" runat="server" 
                                CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                CssPostfix="Office2010Blue" Height="29px" OnClick="formOKButton_Click" 
                                SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="OK" 
                                Width="76px">
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="FormCancelButton" runat="server" CausesValidation="False" 
                                CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                CssPostfix="Office2010Blue" Height="29px" OnClick="FormCancelButton_Click" 
                                SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="Cancel" 
                                Width="76px">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </caption>
            </tr>
        </table>
      </dx:PanelContent>
                                        </PanelCollection>
                                    </dx:ASPxRoundPanel>
                                    
</asp:Content>
