<%@ Page Title="VitalSigns Plus - Server Task Definitions" Language="C#" MasterPageFile="~/Site1.Master"
    AutoEventWireup="true" CodeBehind="ServerTaskDefinitions.aspx.cs" Inherits="VSWebUI.Configurator.ServerTasksDef" %>

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
                ServerTaskDefGridView.GetRowValues(e.visibleIndex, 'TaskName', OnGetRowValues);

            function OnGetRowValues(values) {
                var id = values[0];
                var name = values[1];
                var OK = (confirm('Are you sure you want to delete the server task definition - ' + values + '?'))
                if (OK == true) {
                    ServerTaskDefGridView.DeleteRow(visibleIndex);
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
        CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
        Width="100%" HeaderText="Server Task Definitions">
        <HeaderStyle Height="23px">
            <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
        </HeaderStyle>
        <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
        <PanelCollection>
            <dx:PanelContent runat="server" SupportsDisabledAttribute="True">--%>
    <table width="100%">
        <tr>
            <td>
                <div class="header" id="lblServer" runat="server">
                    IBM Domino Server Task Definitions</div>
            </td>
        </tr>
        <tr>
            <td>
                <div class="info">
                    Server Tasks are programs that run on IBM Domino Servers. The table below lists
                    various server tasks that could be monitored. You may define a server task here
                    and associate it with a server using the Server Tasks tab on the Domino Server page.
                </div>
                <div id="successDiv" class="alert alert-success" runat="server" style="display: none">
                    Success.
                </div>
                <dx:ASPxButton ID="NewButton" runat="server" Text="New" CssClass="sysButton" OnClick="NewButton_Click">
                    <Image Url="~/images/icons/add.png">
                    </Image>
                </dx:ASPxButton>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxGridView ID="ServerTaskDefGridView" runat="server" AutoGenerateColumns="False"
                    ClientInstanceName="ServerTaskDefGridView" KeyFieldName="TaskID" OnHtmlRowCreated="ServerTaskDefGridView_HtmlRowCreated"
                    OnRowDeleting="ServerTaskDefGridView_RowDeleting" Width="100%" Theme="Office2003Blue"
                    OnPageSizeChanged="ServerTaskDefGridView_PageSizeChanged">
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
                            <CellStyle CssClass="GridCss1" Wrap="False">
                            </CellStyle>
                            <ClearFilterButton Visible="True">
                                <Image Url="~/images/clear.png">
                                </Image>
                            </ClearFilterButton>
                            <HeaderStyle CssClass="GridCssHeader">
                                <Paddings Padding="5px" />
                            </HeaderStyle>
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn Caption="Task Name" VisibleIndex="2" FieldName="TaskName">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss" Wrap="True">
                            </CellStyle>
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Load String" VisibleIndex="3" FieldName="LoadString">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Console String" VisibleIndex="4" FieldName="ConsoleString">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Freeze Detection" VisibleIndex="5" FieldName="FreezeDetect">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Idle String" VisibleIndex="6" FieldName="IdleString">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Maximum Busy Time" VisibleIndex="7" FieldName="MaxBusyTime">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Retry Count" VisibleIndex="8" FieldName="RetryCount">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewCommandColumn ButtonType="Image" Caption="Delete" VisibleIndex="1" Width="60px">
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
                    <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                    <SettingsText ConfirmDelete="Are you sure you want to delete this record?" />
                    <Styles>
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
                    <SettingsPager PageSize="50" SEOFriendly="Enabled">
                        <PageSizeItemSettings Visible="true" />
                    </SettingsPager>
                </dx:ASPxGridView>
            </td>
        </tr>
    </table>
    <%-- </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>--%>
</asp:Content>
