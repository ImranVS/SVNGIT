<%@ Page Title="VitalSigns Plus - CustomStatistics" Language="C#" MasterPageFile="~/Site1.Master"AutoEventWireup="true" CodeBehind="CustomStatistics.aspx.cs" Inherits="VSWebUI.Configurator.DominoCustomStat" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
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
        //5/21/2015 NS added for VSPLUS-1771
        var visibleIndex;
        function OnCustomButtonClick(s, e) {
            visibleIndex = e.visibleIndex;

            if (e.buttonID == "deleteButton")
                DominoCustomGridView.GetRowValues(e.visibleIndex, 'StatName', OnGetRowValues);

            function OnGetRowValues(values) {
                var id = values[0];
                var name = values[1];
                var OK = (confirm('Are you sure you want to delete the custom statistic - ' + values + '?'))
                if (OK == true) {
                    DominoCustomGridView.DeleteRow(visibleIndex);
                }
            }
        }
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
<table style="margin-top:0px" width="100%">
<tr><td>   
  <%--  <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" 
        CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
        GroupBoxCaptionOffsetY="-24px" 
        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%" 
        HeaderText="Custom Statistics">
        <HeaderStyle Height="23px" >
        <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="10px" />
        </HeaderStyle>
        <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
        <PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">--%>
<table class="style1">
    <tr>
        <td>
            <div class="header" id="lblServer" runat="server">IBM Domino Custom Statistics</div>
        </td>
    </tr>
        <tr>
            <td>
               <div class="info">You can set an alert threshold on any Server Statistic that is maintained by Domino, such as servers, users or number of transactions per minute.
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
                <dx:ASPxGridView ID="DominoCustomGridView" runat="server" AutoGenerateColumns="False" 
                    OnHtmlRowCreated="DominoCustomGridView_HtmlRowCreated" ClientInstanceName="DominoCustomGridView" 
                    OnRowDeleting="DominoCustomGridView_RowDeleting" KeyFieldName="ID" 
                    Width="100%" OnPageSizeChanged="DominoCustomGridView_PageSizeChanged"
                    OnHtmlDataCellPrepared="DominoCustomGridView_HtmlDataCellPrepared" 
                    EnableTheming="True" Theme="Office2003Blue">
                    <ClientSideEvents CustomButtonClick="OnCustomButtonClick" />
                    <Columns>
                         <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" FixedStyle="Left"
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
                                        </CellStyle>
                                        <ClearFilterButton Visible="True">
                                            <Image Url="~/images/clear.png">
                                            </Image>
                                        </ClearFilterButton>
                                        <HeaderStyle CssClass="GridCssHeader1" >
                                        <Paddings Padding="5px" />
                                        </HeaderStyle>
                                    </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn Caption="Server Name" VisibleIndex="2" 
                            FieldName="ServerName">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
<EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Statistic" VisibleIndex="3" 
                            FieldName="StatName">
                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Threshold Value" VisibleIndex="4" 
                            FieldName="ThresholdValue" Width="100px">
                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss2"></CellStyle><Settings AllowAutoFilter="True" AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Repeat Factor" VisibleIndex="5" 
                            FieldName="TimesInARow" Width="100px">
                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss2"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Comparison" VisibleIndex="6" 
                            FieldName="GreaterThanORLessThan">
                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" /></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Console Command" VisibleIndex="7" 
                            FieldName="ConsoleCommand">
                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" /></dx:GridViewDataTextColumn>
                         <dx:GridViewCommandColumn ButtonType="Image" Caption="Delete" VisibleIndex="1" 
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
                    <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                    <SettingsText ConfirmDelete="Are you sure you want to delete this record?" />
                    <Styles>
                        <LoadingPanel ImageSpacing="5px">
                        </LoadingPanel>
                          <GroupRow Font-Bold="True">
                        </GroupRow>
                          <AlternatingRow CssClass="GridAltRow" Enabled="True">
                        </AlternatingRow>
                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                        </Header>
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

<%--</dx:PanelContent>
</PanelCollection>
    </dx:ASPxRoundPanel>--%>
    </td></tr></table>
</asp:Content>
