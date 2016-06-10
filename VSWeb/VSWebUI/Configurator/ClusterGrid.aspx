<%@ Page Title="VitalSigns Plus - Domino Clusters" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
    CodeBehind="ClusterGrid.aspx.cs" Inherits="VSWebUI.Configurator.ClusterGrid" %>

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
                DominoClusterGridView.GetRowValues(e.visibleIndex, 'Name', OnGetRowValues);

            function OnGetRowValues(values) {
                var id = values[0];
                var name = values[1];
                var OK = (confirm('Are you sure you want to delete the Notes database replica - ' + values + '?'))
                if (OK == true) {
                    DominoClusterGridView.DeleteRow(visibleIndex);
                }
            }
        }
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
              <%--  <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                    CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Domino Clusters"
                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                    <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                    <HeaderStyle Height="23px">
                        <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                    </HeaderStyle>
                    <PanelCollection>
                        <dx:PanelContent runat="server" SupportsDisabledAttribute="True">--%>
                            <table width="100%">
                                <tr>
                                    <td>
                                        <div class="header" id="lblServer" runat="server">Notes Database Replicas</div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="info">Notes Database Replicas are monitored to ensure that replicas of each database exist on all servers, and that document counts between replicas are consistent.
                                        </div>     
                                        <div id="successDiv" class="alert alert-success" runat="server" style="display: none">Success.
                                        </div>
                                        <dx:ASPxButton ID="NewButton" runat="server" Text="New" CssClass="sysButton" 
                                            onclick="NewButton_Click">
                                            <Image Url="~/images/icons/add.png">
                                            </Image>
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxGridView ID="DominoClusterGridView" runat="server"
                                             AutoGenerateColumns="False" ClientInstanceName="DominoClusterGridView" 
                                            KeyFieldName="ID" OnHtmlRowCreated="DominoClusterGridView_HtmlRowCreated"
                                            OnRowDeleting="DominoClusterGridView_RowDeleting" Width="100%" 
                                            OnHtmlDataCellPrepared="DominoClusterGridView_HtmlDataCellPrepared" 
                                            EnableTheming="True" Theme="Office2003Blue" OnPageSizeChanged="DominoClusterGridView_PageSizeChanged">
                                            <ClientSideEvents CustomButtonClick="OnCustomButtonClick" />
                                            <Columns>
                                                <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions"
                                                    VisibleIndex="0" Width="70px" FixedStyle="Left">
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
                                                       
                                                    </CellStyle>
                                                    <ClearFilterButton Visible="True">
                                                        <Image Url="~/images/clear.png">
                                                        </Image>
                                                    </ClearFilterButton>
                                                    <HeaderStyle CssClass="GridCssHeader1" />
                                                </dx:GridViewCommandColumn>
                                                <dx:GridViewDataCheckColumn Caption="Enabled" VisibleIndex="2" CellStyle-HorizontalAlign="Center"
                                                    FieldName="Enabled" FixedStyle="Left" Width="70px">
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
                                                <dx:GridViewDataTextColumn Caption="Name" VisibleIndex="3"
                                                    FieldName="Name" FixedStyle="Left">
													<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                    <CellStyle CssClass="GridCss">
                                                    </CellStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Scan Interval"
                                                    VisibleIndex="4" FieldName="ScanInterval" Width="90px">
                                                    <Settings AllowAutoFilter="False" />
                                                    <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                    <CellStyle CssClass="GridCss2">
                                                    </CellStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Off-Hours Scan Interval"
                                                    VisibleIndex="5" FieldName="OffHoursScanInterval" Width="100px">
                                                    <Settings AllowAutoFilter="False" />
                                                    <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                                                    <CellStyle CssClass="GridCss2">
                                                    </CellStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Server A" VisibleIndex="6"
                                                    FieldName="ServerA" Width="150px">
													<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                                                    <CellStyle CssClass="GridCss">
                                                    </CellStyle>
                                                     <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Server B" VisibleIndex="7"
                                                    FieldName="ServerB" Width="150px">
													<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                    <CellStyle CssClass="GridCss">
                                                    </CellStyle>
                                                     
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Server C" VisibleIndex="8"
                                                    FieldName="ServerC" Width="150px">
													<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                    <CellStyle CssClass="GridCss">
                                                    </CellStyle>
                                                     
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Category" VisibleIndex="9"
                                                    FieldName="Category">
                                                    <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                    <CellStyle CssClass="GridCss">
                                                    </CellStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewCommandColumn ButtonType="Image" Caption="Delete" VisibleIndex="1" 
                                                    Width="60px" FixedStyle="Left">
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
                                            <SettingsText ConfirmDelete=" Are you sure you want to delete this record?" />
                                            <Styles>
                                                <LoadingPanel ImageSpacing="5px">
                                                </LoadingPanel>
                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                </Header>
                                                <GroupRow Font-Bold="True" Wrap="False">
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
                       <%-- </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>--%>
</asp:Content>
