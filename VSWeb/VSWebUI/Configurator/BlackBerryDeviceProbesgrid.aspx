<%@ Page Title="VitalSigns Plus - BlackBerry Device Probes" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
    CodeBehind="BlackBerryDeviceProbesgrid.aspx.cs" Inherits="VSWebUI.Configurator.BlackBerryDeviceProbesgrid" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>





<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
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
            <td valign="top">
                <div class="info">BlackBerry Device Probes send a test message to a NotesMail account associated with a BlackBerry device. The message requests a delivery confirmation from the RIM network. VitalSigns checks the target NotesMail file to verify that the message and the confirmation are both there within the specified time limit.
                </div>
                <div id="successDiv" class="alert alert-success" runat="server" style="display: none">Success.
                </div>
                <dx:ASPxButton ID="NewButton" runat="server" Text="New" CssClass="sysButton" 
                    onclick="NewButton_Click">
                    <Image Url="~/images/icons/add.png">
                    </Image>
                </dx:ASPxButton>
                <dx:ASPxPageControl Font-Bold="True" ID="ASPxPageControl1" runat="server" ActiveTabIndex="0"
                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                    TabSpacing="0px" Width="100%" EnableHierarchyRecreation="False">
                    <TabPages>
                        <dx:TabPage Text="Configure">
                            <TabImage Url="~/images/information.png" />
                            <ContentCollection>
                                <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                                    <table class="style1" width="100%">
                                        <tr>
                                            <td>
                                                <dx:ASPxGridView ID="BlackBerryDeviceProbegrid" runat="server" AutoGenerateColumns="False"
                                                    CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                                                    Width="100%" KeyFieldName="NotesMailAddress" OnHtmlRowCreated="BlackBerryDeviceProdegridGridView_HtmlRowCreated"
                                                    OnHtmlDataCellPrepared="BlackBerryDeviceProbegrid_HtmlDataCellPrepared" 
                                                    OnRowDeleting="BlackBerryDeviceProbegrid_RowDeleting" EnableTheming="True" 
                                                    Theme="Office2003Blue" OnPageSizeChanged="BlackBerryDeviceProbegrid_PageSizeChanged">
                                                    <Columns>
                                                        <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" ShowInCustomizationForm="True"
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
                                                            <CellStyle CssClass="GridCss">
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
                                                        <dx:GridViewDataCheckColumn Caption="Enabled" ShowInCustomizationForm="True" VisibleIndex="1"
                                                            FieldName="Enabled" Width="80px">
                                                            <Settings AllowAutoFilter="False" />
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                            <CellStyle CssClass="GridCss1">
                                                            </CellStyle>
                                                        </dx:GridViewDataCheckColumn>
                                                        <dx:GridViewDataTextColumn Caption="Name" ShowInCustomizationForm="True" VisibleIndex="2"
                                                            FieldName="Name">
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                            <CellStyle CssClass="GridCss">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Category" ShowInCustomizationForm="True" VisibleIndex="3"
                                                            FieldName="Category">
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                            <CellStyle CssClass="GridCss">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Delivery Threshold" ShowInCustomizationForm="True"
                                                            VisibleIndex="4" FieldName="DeliveryThreshold">
                                                            <Settings AllowAutoFilter="False" />
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                                                            <CellStyle CssClass="GridCss2">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Destination Database" ShowInCustomizationForm="True"
                                                            VisibleIndex="5" FieldName="DestinationDatabase">
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                                                            <CellStyle CssClass="GridCss">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Destination Server" ShowInCustomizationForm="True"
                                                            VisibleIndex="6" FieldName="DestinationServer">
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                                                            <CellStyle CssClass="GridCss">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Internet Address" ShowInCustomizationForm="True"
                                                            VisibleIndex="7" FieldName="InternetMailAddress">
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                                                            <CellStyle CssClass="GridCss">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="NotesMail Address" ShowInCustomizationForm="True"
                                                            VisibleIndex="8" FieldName="NotesMailAddress">
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                                                            <CellStyle CssClass="GridCss">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Off-Hours Scan Interval" ShowInCustomizationForm="True"
                                                            VisibleIndex="9" FieldName="OffHoursScanInterval">
                                                            <Settings AllowAutoFilter="False" />
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                                                            <CellStyle CssClass="GridCss2">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Retry Interval" ShowInCustomizationForm="True"
                                                            VisibleIndex="10" FieldName="RetryInterval">
                                                            <Settings AllowAutoFilter="False" />
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                                                            <CellStyle CssClass="GridCss2">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Scan Interval" ShowInCustomizationForm="True"
                                                            VisibleIndex="11" FieldName="ScanInterval">
                                                            <Settings AllowAutoFilter="False" />
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                                                            <CellStyle CssClass="GridCss2">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Source Server" ShowInCustomizationForm="True"
                                                            VisibleIndex="12" FieldName="SourceServer">
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                                                            <CellStyle CssClass="GridCss">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Confirmation Server" ShowInCustomizationForm="True"
                                                            VisibleIndex="13" FieldName="ConfirmationServer">
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                                                            <CellStyle CssClass="GridCss">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                    </Columns>
                                                    <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" />
                                                    <Settings ShowHorizontalScrollBar="True" ShowFilterRow="True" 
                                                        ShowGroupPanel="True" />
                                                    <SettingsText ConfirmDelete="Are you sure you want to delete this record?" />
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
                                                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                        </Header>
                                                          <GroupRow Font-Bold="True">
                                                        </GroupRow>
                                                          <AlternatingRow CssClass="GridAltRow" Enabled="True">
                                    </AlternatingRow>
                                                        <LoadingPanel ImageSpacing="5px">
                                                        </LoadingPanel>
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
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="History">
                            <TabImage Url="~/images/icons/page_white_stack.png" />
                            <ContentCollection>
                                <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                                    <dx:ASPxGridView ID="ASPxGridView2" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css"
                                        CssPostfix="Office2010Silver" Width="100%" Theme="Office2003Blue" OnPageSizeChanged="ASPxGridView2_PageSizeChanged">
                                        <Columns>
                                            <dx:GridViewDataTextColumn Caption="Name" FixedStyle="Left" ShowInCustomizationForm="True"
                                                VisibleIndex="1">
                                                <Settings AllowAutoFilter="True" />
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" />
                                                <CellStyle CssClass="GridCss">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Status" FixedStyle="Left" ShowInCustomizationForm="True"
                                                VisibleIndex="2">
                                                <Settings AllowAutoFilter="True" />
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" />
                                                <CellStyle CssClass="GridCss">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Details" FixedStyle="Left" ShowInCustomizationForm="True"
                                                VisibleIndex="3">
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" />
                                                <CellStyle CssClass="GridCss">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Delivery Threshold" ShowInCustomizationForm="True"
                                                VisibleIndex="4">
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                                                <CellStyle CssClass="GridCss">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Delivery Time" ShowInCustomizationForm="True"
                                                VisibleIndex="5">
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                                                <CellStyle CssClass="GridCss">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Arrival At MailBox" ShowInCustomizationForm="True"
                                                VisibleIndex="6">
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                                                <CellStyle CssClass="GridCss">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="RIM Receipt" ShowInCustomizationForm="True" 
                                                VisibleIndex="7">
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" />
                                                <CellStyle CssClass="GridCss">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Sent DateTime" ShowInCustomizationForm="True"
                                                VisibleIndex="8">
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                                                <CellStyle CssClass="GridCss">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Sent To" ShowInCustomizationForm="True" 
                                                VisibleIndex="9">
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" />
                                                <CellStyle CssClass="GridCss">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Subject Key" ShowInCustomizationForm="True" 
                                                VisibleIndex="10">
                                                <EditCellStyle CssClass="GridCss">
                                                </EditCellStyle>
                                                <EditFormCaptionStyle CssClass="GridCss">
                                                </EditFormCaptionStyle>
                                                <HeaderStyle CssClass="GridCssHeader" />
                                                <CellStyle CssClass="GridCss">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                        <SettingsBehavior ColumnResizeMode="NextColumn" />
                                        <Settings ShowGroupPanel="True" ShowHorizontalScrollBar="True" 
                                            ShowFilterRow="True" />
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
                                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                            </Header>
                                            <GroupRow Font-Bold="True">
                                            </GroupRow>
                                            <AlternatingRow CssClass="GridAltRow">
                                            </AlternatingRow>
                                            <LoadingPanel ImageSpacing="5px">
                                            </LoadingPanel>
                                        </Styles>
                                        <StylesEditors ButtonEditCellSpacing="0">
                                            <ProgressBar Height="21px">
                                            </ProgressBar>
                                        </StylesEditors>
                                    </dx:ASPxGridView>
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
            </td>
        </tr>
    </table>
</asp:Content>
