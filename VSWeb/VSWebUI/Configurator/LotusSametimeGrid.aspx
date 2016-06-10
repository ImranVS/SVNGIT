<%@ Page Title="VitalSigns Plus - Sametime Servers" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
    CodeBehind="LotusSametimeGrid.aspx.cs" Inherits="VSWebUI.Configurator.LotusSametimeGrid" %>

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
        </script>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <%-- <dx:ASPxRoundPanel ID="LocationsRoundPanel" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
        CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Sametime Servers"
        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%" Height="250px">
        <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
        <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
        <HeaderStyle Height="23px">
            <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
        </HeaderStyle>
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">--%>
                <table width="100%">
                    <tr>
                        <td>
                            <div class="header" id="servernamelbldisp" runat="server">IBM Sametime Servers</div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="successDiv" class="alert alert-success" runat="server" style="display: none">Success.
                            </div>
                            <dx:ASPxGridView runat="server" AutoGenerateColumns="False" ID="LotusSametimeGridview"
                                OnRowDeleting="LotusSametimeGridview_RowDeleting" OnHtmlRowCreated="LotusSametimeGridview_HtmlRowCreated"
                                KeyFieldName="SID" Width="100%" OnPageSizeChanged="LotusSametimeGridview_PageSizeChanged"
                                Cursor="pointer" Theme="Office2003Blue">
                                <Columns>
                                    <dx:GridViewCommandColumn 
                                        VisibleIndex="1" ButtonType="Image" Caption="Actions" Width="60px">
                                        <EditButton Visible="True">
                                            <Image Url="~/images/edit.png">
                                            </Image>
                                        </EditButton>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss1">
                                        </CellStyle>
                                    </dx:GridViewCommandColumn>
                                    <dx:GridViewDataCheckColumn Caption="Enabled" VisibleIndex="2"
                                        FieldName="Enabled" Width="60px">
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
                                    <dx:GridViewDataTextColumn Caption="Name" FieldName="Name" 
                                        VisibleIndex="3">
                                        <Settings AutoFilterCondition="Contains" />
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Category" VisibleIndex="4"
                                        FieldName="Category">
                                        <Settings AutoFilterCondition="Contains" />
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Description" VisibleIndex="13"
                                        FieldName="Description">
                                        <Settings AutoFilterCondition="Contains" />
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Location" VisibleIndex="8"
                                        FieldName="Location">
                                        <Settings AutoFilterCondition="Contains" />
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Scan Interval" FieldName="ScanInterval" 
                                        VisibleIndex="6">
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss2">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="SID" FieldName="SID" VisibleIndex="45" 
                                        Visible="False">
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                                <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" 
                                    ProcessSelectionChangedOnServer="True" />
                                <Settings ShowGroupPanel="True"
                                    UseFixedTableLayout="True" />
                                <SettingsText ConfirmDelete=" 'Are you sure you want to delete?'" />
                                <SettingsBehavior ConfirmDelete="True"></SettingsBehavior>
                                <Styles>
                                    <Header SortingImageSpacing="5px" ImageSpacing="5px">
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
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
           <%-- </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>--%>
</asp:Content>
