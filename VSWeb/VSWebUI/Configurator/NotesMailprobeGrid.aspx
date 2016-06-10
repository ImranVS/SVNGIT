<%@ Page Title="VitalSigns Plus - NotesMail Probes" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
    CodeBehind="NotesMailprobeGrid.aspx.cs" Inherits="VSWebUI.Configurator.NotesMailprobeGrid" %>

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
        //5/21/2015 NS added for VSPLUS-1771
        var visibleIndex;
        function OnCustomButtonClick(s, e) {
            visibleIndex = e.visibleIndex;

            if (e.buttonID == "deleteButton")
                NotesMailProbeGridView.GetRowValues(e.visibleIndex, 'Name', OnGetRowValues);

            function OnGetRowValues(values) {
                var id = values[0];
                var name = values[1];
                var OK = (confirm('Are you sure you want to delete the NotesMail probe - ' + values + '?'))
                if (OK == true) {
                    NotesMailProbeGridView.DeleteRow(visibleIndex);
                }
            }
        }
        </script>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%">
        <tr>
                        <td>
                            <div class="header" id="lblServer" runat="server">NotesMail Probes</div>
                        </td>
                    </tr>
        <tr>
            <td valign="top">
                <div class="info">NotesMail Probes send a text message from a specified server to a NotesMail Address. Then the mail file of the recipient is checked to verify that the message has arrived and to see how long it took. NotesMail Probes are useful for spotting trends in delivery times and successful message delivery.
                </div>
                <div id="successDiv" class="alert alert-success" runat="server" style="display: none">Success.
                </div>
                <dx:ASPxButton ID="NewButton" runat="server" Text="New" CssClass="sysButton" 
                    onclick="NewButton_Click">
                    <Image Url="~/images/icons/add.png">
                                            </Image>
                </dx:ASPxButton>
                <dx:ASPxPageControl Font-Bold="True" ID="ASPxPageControl1" runat="server" ActiveTabIndex="0"
                    TabSpacing="0px" Width="100%" Theme="Glass" EnableHierarchyRecreation="False">
                    <TabPages>
                        <dx:TabPage Text="Configure">
                            <TabImage Url="~/images/icons/information.png" />
                            <ContentCollection>
                                <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <dx:ASPxGridView ID="NotesMailProbeGridView" runat="server" AutoGenerateColumns="False" ClientInstanceName="NotesMailProbeGridView"
                                                    KeyFieldName="Name" OnHtmlRowCreated="NotesMailProbeGridView_HtmlRowCreated"
                                                    OnRowDeleting="NotesMailProbeGridView_RowDeleting" Width="100%" 
                                                    EnableTheming="True" Theme="Office2003Blue" OnPageSizeChanged="NotesMailProbeGridView_PageSizeChanged">
                                                    <ClientSideEvents CustomButtonClick="OnCustomButtonClick" />
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
                                                            <DeleteButton Visible="False">
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
                                                            <CellStyle CssClass="GridCss1">
                                                                <Paddings Padding="3px" />
                                                            </CellStyle>
                                                            <ClearFilterButton Visible="True">
                                                                <Image Url="~/images/clear.png">
                                                                </Image>
                                                            </ClearFilterButton>
                                                            <HeaderStyle CssClass="GridCssHeader1" />
                                                        </dx:GridViewCommandColumn>
                                                        <dx:GridViewDataCheckColumn Caption="Enabled" ShowInCustomizationForm="True" VisibleIndex="2"
                                                            FieldName="Enabled" FixedStyle="Left" Width="80px">
                                                            <Settings AllowAutoFilter="False" />
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader" >
                                                            <Paddings Padding="5px" />
                                                            </HeaderStyle>
                                                            <CellStyle CssClass="GridCss1">
                                                            </CellStyle>
                                                        </dx:GridViewDataCheckColumn>
                                                        <dx:GridViewDataTextColumn Caption="Name" ShowInCustomizationForm="True" VisibleIndex="3"
                                                            FieldName="Name" FixedStyle="Left">
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                            <CellStyle CssClass="GridCss">
                                                            </CellStyle>
                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Category" ShowInCustomizationForm="True" VisibleIndex="4"
                                                            FieldName="Category">
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                            <CellStyle CssClass="GridCss">
                                                            </CellStyle>
                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Delivery Threshold" ShowInCustomizationForm="True"
                                                            VisibleIndex="5" FieldName="DeliveryThreshold">
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                                                            <CellStyle CssClass="GridCss2">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Destination Database" ShowInCustomizationForm="True"
                                                            VisibleIndex="6" FieldName="DestinationDatabase">
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                                                            <CellStyle CssClass="GridCss">
                                                            </CellStyle>
                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Destination Server" ShowInCustomizationForm="True"
                                                            VisibleIndex="7" FieldName="DestinationServer">
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                                                            <CellStyle CssClass="GridCss">
                                                            </CellStyle>
                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
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
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader2" />
                                                            <CellStyle CssClass="GridCss2">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Scan Interval" ShowInCustomizationForm="True"
                                                            VisibleIndex="11" FieldName="ScanInterval">
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader2" />
                                                            <CellStyle CssClass="GridCss2">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Source" ShowInCustomizationForm="True" VisibleIndex="12"
                                                            FieldName="SourceServer">
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                            <CellStyle CssClass="GridCss">
                                                            </CellStyle>
                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Echo Service" FieldName="EchoService" ShowInCustomizationForm="True"
                                                            VisibleIndex="13">
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                            <CellStyle CssClass="GridCss">
                                                            </CellStyle>
                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Reply To" FieldName="ReplyTo" ShowInCustomizationForm="True"
                                                            VisibleIndex="14">
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                            <CellStyle CssClass="GridCss">
                                                            </CellStyle>
                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="File Name" FieldName="Filename" ShowInCustomizationForm="True"
                                                            VisibleIndex="15">
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                            <CellStyle CssClass="GridCss">
                                                            </CellStyle>
                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewCommandColumn ButtonType="Image" Caption="Delete" FixedStyle="Left" 
                                                            ShowInCustomizationForm="True" VisibleIndex="1" Width="60px">
                                                            <CustomButtons>
                                                                <dx:GridViewCommandColumnCustomButton ID="deleteButton" Text="Delete">
                                                                    <Image Url="~/images/delete.png">
                                                                    </Image>
                                                                </dx:GridViewCommandColumnCustomButton>
                                                            </CustomButtons>
                                                            <HeaderStyle CssClass="GridCssHeader1" />
                                                            <CellStyle CssClass="GridCss1">
                                                            </CellStyle>
                                                        </dx:GridViewCommandColumn>
                                                    </Columns>
                                                    <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" />
                                                    <Settings ShowGroupPanel="True" ShowHorizontalScrollBar="True" 
                                                        ShowFilterRow="True" />
                                                    <SettingsText ConfirmDelete="Are you sure you want to delete this record?" />
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
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="History">
                            <TabImage Url="~/images/icons/page_white_stack.png" />
                            <ContentCollection>
                                <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                                    <table width="100%">
                                        <tr>
                                            <td width="100%">
                                                <dx:ASPxGridView ID="MailProbeHistoryGridView" runat="server" AutoGenerateColumns="False"
                                                    CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                                                    Width="100%" Theme="Office2003Blue" OnPageSizeChanged="MailProbeHistoryGridView_PageSizeChanged">
                                                    <Columns>
                                                        <dx:GridViewCommandColumn ShowInCustomizationForm="True" Visible="False" 
                                                            VisibleIndex="0">
                                                            <ClearFilterButton Visible="True">
                                                            </ClearFilterButton>
                                                        </dx:GridViewCommandColumn>
                                                        <dx:GridViewDataTextColumn Caption="Device Name" ShowInCustomizationForm="True" VisibleIndex="1"
                                                            FieldName="DeviceName" FixedStyle="Left">
                                                            <Settings AutoFilterCondition="Contains" />
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                            <CellStyle CssClass="GridCss">
                                                            </CellStyle>
                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Status" ShowInCustomizationForm="True" VisibleIndex="2"
                                                            FieldName="Status" FixedStyle="Left">
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                            <CellStyle CssClass="GridCss">
                                                            </CellStyle>
                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Delivery Threshold in Minutes" ShowInCustomizationForm="True"
                                                            VisibleIndex="3" FieldName="DeliveryThresholdInMinutes" FixedStyle="Left">
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                                                            <CellStyle CssClass="GridCss2">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Delivery Time in Minutes" ShowInCustomizationForm="True"
                                                            VisibleIndex="4" FieldName="DeliveryTimeInMinutes">
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                                                            <CellStyle CssClass="GridCss2">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Details" ShowInCustomizationForm="True" VisibleIndex="5"
                                                            FieldName="Details">
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                            <CellStyle CssClass="GridCss">
                                                            </CellStyle>
                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Sent DateTime" ShowInCustomizationForm="True"
                                                            VisibleIndex="6" FieldName="SentDateTime">
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                                                            <CellStyle CssClass="GridCss">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Arrival At MailBox" ShowInCustomizationForm="True"
                                                            VisibleIndex="7" FieldName="ArrivalAtMailBox">
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                                                            <CellStyle CssClass="GridCss">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Sent To" ShowInCustomizationForm="True" VisibleIndex="8"
                                                            FieldName="SentTo">
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                            <CellStyle CssClass="GridCss">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Subject Key" ShowInCustomizationForm="True" VisibleIndex="9"
                                                            FieldName="SubjectKey">
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                            <CellStyle CssClass="GridCss">
                                                            </CellStyle>
                                                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Target Database" ShowInCustomizationForm="True"
                                                            VisibleIndex="10" FieldName="TargetDatabase">
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                                                            <CellStyle CssClass="GridCss">
                                                            </CellStyle><Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Target Server" ShowInCustomizationForm="True"
                                                            VisibleIndex="11" FieldName="TargetServer">
                                                            <EditCellStyle CssClass="GridCss">
                                                            </EditCellStyle>
                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                            </EditFormCaptionStyle>
                                                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                                                            <CellStyle CssClass="GridCss">
                                                            </CellStyle><Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                        </dx:GridViewDataTextColumn>
                                                    </Columns>
                                                    <Settings ShowGroupPanel="True" ShowHorizontalScrollBar="True" 
                                                        ShowFilterRow="True" />
                                                        <SettingsPager>
                        <PageSizeItemSettings Visible="True" 
                            Items="10, 20, 50, 100, 200">
                        </PageSizeItemSettings>
                    </SettingsPager>
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
                                                </dx:ASPxGridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
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
            </td>
        </tr>
    </table>
</asp:Content>
