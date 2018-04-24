<%@ Page Title="VitalSigns Plus - Network Devices" Language="C#" MasterPageFile="~/Site1.Master"
    AutoEventWireup="true" CodeBehind="NetworkDevicesGrid.aspx.cs" Inherits="VSWebUI.Configurator.NetworkDevicesGrid" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet'
        type='text/css' />
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
                NetworkDevicesGridView.GetRowValues(e.visibleIndex, 'Name', OnGetRowValues);

            function OnGetRowValues(values) {
                var id = values[0];
                var name = values[1];
                var OK = (confirm('Are you sure you want to delete the network device - ' + values + '?'))
                if (OK == true) {
                    NetworkDevicesGridView.DeleteRow(visibleIndex);
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" 
        CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
        CssPostfix="Glass" 
        GroupBoxCaptionOffsetY="-24px" 
        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
        Width="100%" HeaderText="Network Devices">
        <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
        <HeaderStyle Height="23px">
        <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
        </HeaderStyle>
        <PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">--%>
    <table width="100%">
        <tr>
            <td>
                <div class="header" id="servernamelbldisp" runat="server">
                    Network Devices</div>
            </td>
        </tr>
        <tr>
            <td>
                <div class="info">
                    A Network Device is anything that will respond to a ping. VitalSigns pings it and
                    times the response.
                </div>
                <div id="successDiv" class="alert alert-success" runat="server" style="display: none">
                    Success.
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxButton ID="NewButton" runat="server" Text="New" CssClass="sysButton" OnClick="NewButton_Click">
                    <Image Url="~/images/icons/add.png">
                    </Image>
                </dx:ASPxButton>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxGridView ID="NetworkDevicesGridView" runat="server" AutoGenerateColumns="False"
                    Width="100%" KeyFieldName="ID" ClientInstanceName="NetworkDevicesGridView" OnHtmlRowCreated="NetworkDevicesGridView_HtmlRowCreated"
                    OnRowDeleting="NetworkDevicesGridView_RowDeleting" OnHtmlDataCellPrepared="NetworkDevicesGridView_HtmlDataCellPrepared"
                    EnableTheming="True" Theme="Office2003Blue" OnPageSizeChanged="NetworkDevicesGridView_PageSizeChanged">
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
                            <CellStyle CssClass="GridCss">
                                <Paddings Padding="3px" />
                            </CellStyle>
                            <ClearFilterButton Visible="True">
                                <Image Url="~/images/clear.png">
                                </Image>
                            </ClearFilterButton>
                            <HeaderStyle CssClass="GridCssHeader" />
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataCheckColumn Caption="Enabled" VisibleIndex="2" FixedStyle="Left"
                            FieldName="Enabled" Width="60px">
                            <Settings AllowAutoFilter="False" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader">
                                <Paddings Padding="5px" />
                            </HeaderStyle>
                            <CellStyle CssClass="GridCss1">
                            </CellStyle>
                        </dx:GridViewDataCheckColumn>
                        <dx:GridViewDataTextColumn Caption="Name" FixedStyle="Left" ShowInCustomizationForm="True"
                            VisibleIndex="3" FieldName="Name">
                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Scan Interval" VisibleIndex="5" FieldName="Scanning Interval"
                            Width="80px">
                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Off-Hours Scan Interval" VisibleIndex="6" FieldName="OffHoursScanInterval"
                            Width="90px">
                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Address" VisibleIndex="9" FieldName="Address">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Category" VisibleIndex="4" FieldName="Category">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Response Threshold" VisibleIndex="8" FieldName="ResponseThreshold"
                            Width="90px" Visible="false">
                            <Settings AllowAutoFilter="False" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Location" VisibleIndex="10" Visible="false" FieldName="Location">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Password" VisibleIndex="11" FieldName="Password"
                            Visible="false">
                           <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Port" FieldName="Port" ShowInCustomizationForm="True"
                            VisibleIndex="12" Width="60px" Visible="false">
                            <Settings AllowAutoFilter="False" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader2" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="User Name" FieldName="Username" ShowInCustomizationForm="True"
                            VisibleIndex="13" Visible="false">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Retry Interval" FieldName="RetryInterval" ShowInCustomizationForm="True"
                            VisibleIndex="7" Width="80px" Visible="false">
                            <Settings AllowAutoFilter="False" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewCommandColumn ButtonType="Image" Caption="Delete" FixedStyle="Left" VisibleIndex="1"
                            Width="60px">
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
                    <Settings ShowGroupPanel="True" ShowFilterRow="True" />
                    <SettingsText ConfirmDelete="Are you sure you want to delete this record?" />
                    <Styles>
                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                        </Header>
                        <LoadingPanel ImageSpacing="5px">
                        </LoadingPanel>
                        <GroupRow Font-Bold="True">
                        </GroupRow>
                        <AlternatingRow CssClass="GridAltRow" Enabled="True">
                        </AlternatingRow>
                    </Styles>
                    <StylesEditors ButtonEditCellSpacing="0">
                        <ProgressBar Height="21px">
                        </ProgressBar>
                    </StylesEditors>
                    <SettingsPager PageSize="50" SEOFriendly="Enabled">
                        <PageSizeItemSettings Visible="true" />
                    </SettingsPager>
                </dx:ASPxGridView>
                &nbsp;
            </td>
        </tr>
    </table>
    <%--</dx:PanelContent>
</PanelCollection>
    </dx:ASPxRoundPanel>--%>
</asp:Content>
