<%@ Page Title="VitalSigns Plus - Notes Databases" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
    CodeBehind="NotesDatabase.aspx.cs" Inherits="VSWebUI.Configurator.NotesDatabase" %>

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
                NotesDatabaseGridView.GetRowValues(e.visibleIndex, 'Name', OnGetRowValues);

            function OnGetRowValues(values) {
                var id = values[0];
                var name = values[1];
                var OK = (confirm('Are you sure you want to delete the Notes database - ' + values + '?'))
                if (OK == true) {
                    NotesDatabaseGridView.DeleteRow(visibleIndex);
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
    <%--<dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
        CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Notes Databases"
        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="95%">
        <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
        <HeaderStyle Height="23px">
            <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
        </HeaderStyle>
        <PanelCollection>
            <dx:PanelContent runat="server" SupportsDisabledAttribute="True">--%>
                <table class="style1">
                    <tr>
                        <td>
                            <div class="header" id="lblServer" runat="server">IBM Notes Databases</div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="info">IBM Notes Databases can be monitored for replication, response time, size, document count and existence.
                            </div>
                            <div id="successDiv" class="alert alert-success" runat="server" style="display: none">Success.
                            </div>
                            <dx:ASPxButton ID="NewButton" runat="server" Text="New" CssClass="sysButton" 
                                onclick="NewButton_Click">
                                <Image Url="~/images/icons/add.png">
                                            </Image>
                            </dx:ASPxButton>
                        </td>
                        <%--<td>
                Lotus Notes Databases can be monitored for replication, response 
                time,size,Document<br />
                count, and existence.</td>--%>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxGridView ID="NotesDatabaseGridView" runat="server" KeyFieldName="ID" AutoGenerateColumns="False"
                                OnHtmlRowCreated="NotesDatabaseGridView_HtmlRowCreated" Width="100%" ClientInstanceName="NotesDatabaseGridView"
                                OnRowDeleting="NotesDatabaseGridView_RowDeleting" 
                                OnHtmlDataCellPrepared="NotesDatabaseGridView_HtmlDataCellPrepared" 
                                Theme="Office2003Blue" OnPageSizeChanged="NotesDatabaseGridView_PageSizeChanged">
                                <Settings ShowGroupPanel="True" ShowHorizontalScrollBar="True" 
                                    ShowFilterRow="True"></Settings>
                                <ClientSideEvents CustomButtonClick="OnCustomButtonClick" />
                                <Columns>
                                    <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" 
                                        ShowInCustomizationForm="True" FixedStyle="Left"
                                        VisibleIndex="0" Width="70px">
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
                                            <Paddings PaddingLeft="3px" />
                                        </CellStyle>
                                        <ClearFilterButton Visible="True">
                                            <Image Url="~/images/clear.png">
                                            </Image>
                                        </ClearFilterButton>
                                        <HeaderStyle CssClass="GridCssHeader1" />
                                    </dx:GridViewCommandColumn>
                                    <dx:GridViewDataCheckColumn Caption="Enabled" VisibleIndex="2" FieldName="Enabled"
                                        FixedStyle="Left" Width="70px">
                                        <Settings AllowAutoFilter="False" />
                                        <EditFormSettings ColumnSpan="2" Caption="Enabled for Task Scanning"></EditFormSettings>
                                        <HeaderStyle CssClass="lblsmallFont" >
                                        <Paddings Padding="5px" />
                                        </HeaderStyle>
                                        <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss1"  Wrap="True"></CellStyle>
                                    </dx:GridViewDataCheckColumn>
                                    <dx:GridViewDataTextColumn Caption="Name" VisibleIndex="3" FieldName="Name" 
                                        FixedStyle="Left" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                                     <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Scan Interval" VisibleIndex="4" 
                                        FieldName="ScanInterval" Width="80px">
                                    <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                                        <CellStyle CssClass="GridCss2"></CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Off-Hours Scan Interval" VisibleIndex="5" 
                                        FieldName="OffHoursScanInterval" Width="90px">
                                    <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                                        <CellStyle CssClass="GridCss2"></CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Threshold" VisibleIndex="6" 
                                        FieldName="ResponseThreshold" Width="80px">
                                    <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                                        <CellStyle CssClass="GridCss2"></CellStyle>
                                    </dx:GridViewDataTextColumn>

                                    <dx:GridViewDataTextColumn Caption="Server Name" VisibleIndex="7" 
                                        FieldName="servernames" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                                     <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="File Name" VisibleIndex="8" 
                                        FieldName="FileName" Width="150px">
                                    <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                                     <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" /></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Category" VisibleIndex="9" 
                                        FieldName="Category">
                                    <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss" Wrap="False"></CellStyle>
                                     <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" /></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Retry Interval" VisibleIndex="10" 
                                        FieldName="RetryInterval" Width="60px">
                                    <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                                        <CellStyle CssClass="GridCss2"></CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Trigger Type" VisibleIndex="11" 
                                        FieldName="TriggerType">
                                    <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="ID" ShowInCustomizationForm="True" Visible="False"
                                        VisibleIndex="12">
                                        <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewCommandColumn ButtonType="Image" Caption="Delete" FixedStyle="Left" 
                                        VisibleIndex="1" Width="60px">
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
                                <SettingsPager AlwaysShowPager="True" PageSize="50">
                                    <AllButton Text="All">
                                    </AllButton>
                                    <NextPageButton Text="Next &gt;">
                                    </NextPageButton>
                                    <PrevPageButton Text="&lt; Prev">
                                    </PrevPageButton>
                                    <PageSizeItemSettings Visible="True">
                                    </PageSizeItemSettings>
                                </SettingsPager>
                                <Settings ShowGroupPanel="True" ShowHorizontalScrollBar="True" />
                                <SettingsText ConfirmDelete=" Are you sure you want to delete this record?" />
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
                            </dx:ASPxGridView>
                        </td>
                    </tr>
                    </table>
          <%--  </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>--%>
</asp:Content>
