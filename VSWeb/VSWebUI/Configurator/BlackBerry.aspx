<%@ Page Title="VitalSigns Plus - BlackBerry Servers" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
    CodeBehind="BlackBerry.aspx.cs" Inherits="VSWebUI.WebForm14" %>

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
    <%--<dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
        CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
        Width="100%" HeaderText="BlackBerry">
        <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
        <HeaderStyle Height="23px">
            <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
        </HeaderStyle>
        <PanelCollection>--%>
         <%-- <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True"> --%>   
                <table width="100%">
                    <tr>
                        <td>
                            <div class="header" id="servernamelbldisp" runat="server">BlackBerry Enterprise Servers</div>
                           <div class="info">BlackBerry Enterprise Servers are monitored using SNMP, which must be enabled on the BES server. VitalSigns sends SNMP queries and reports on the answers.
                           </div>
                           <div id="successDiv" class="alert alert-success" runat="server" style="display: none">Success.
                            </div>
                        </td>
                    </tr>
       
                    <tr>
                        <td>
                            <dx:ASPxGridView ID="BlackBerryGridView" runat="server" AutoGenerateColumns="False"
                                CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                                KeyFieldName="key" OnHtmlRowCreated="BlackBerryGridView_HtmlRowCreated" OnRowDeleting="BlackBerryGridView_RowDeleting"
                                Width="100%"  OnPageSizeChanged="BlackBerryGridView_PageSizeChanged"
                                OnHtmlDataCellPrepared="BlackBerryGridView_HtmlDataCellPrepared" Cursor="pointer" 
                                Theme="Office2003Blue">
                                <Columns>
                                  
                                    <dx:GridViewDataCheckColumn Caption="Enabled" VisibleIndex="1" 
                                        FieldName="Enabled" Width="60px">
                                        <Settings AllowAutoFilter="False" />
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader1" >
                                        <Paddings Padding="5px" />
                                        </HeaderStyle>
                                        <CellStyle CssClass="GridCss1">
                                        </CellStyle>
                                    </dx:GridViewDataCheckColumn>
                                  
                                    <dx:GridViewDataTextColumn Caption="Name" FieldName="Name" 
                                        ShowInCustomizationForm="True" VisibleIndex="2" Width="150px">
                                        <Settings AutoFilterCondition="Contains" AllowAutoFilter="False" />
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Category" VisibleIndex="3" 
                                        FieldName="Category">
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Location" VisibleIndex="4" 
                                        FieldName="Location">
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Description" VisibleIndex="5" 
                                        FieldName="Description">
                                        <Settings AllowAutoFilter="False" />
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Scan Interval" VisibleIndex="6" 
                                        FieldName="ScanInterval" Width="90px">
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader2" />
                                        <CellStyle CssClass="GridCss2">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Off-Hours Scan Interval" VisibleIndex="7" 
                                        FieldName="OffHoursScanInterval" Width="90px">
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                                        <CellStyle CssClass="GridCss2">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Pending Threshold" 
                                        FieldName="PendingThreshold" ShowInCustomizationForm="True" 
                                        VisibleIndex="8">
                                        <Settings AutoFilterCondition="Contains" />
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                                        <CellStyle CssClass="GridCss2">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Address" FieldName="Address" 
                                        ShowInCustomizationForm="True" VisibleIndex="9">
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataCheckColumn Caption="Alert" VisibleIndex="10" FieldName="Alert" 
                                        Width="70px">
                                        <Settings AllowAutoFilter="False" />
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader1" />
                                        <CellStyle CssClass="GridCss1">
                                        </CellStyle>
                                    </dx:GridViewDataCheckColumn>
                                    <dx:GridViewDataTextColumn Caption="Alert IP" VisibleIndex="11" 
                                        FieldName="AlertIP">
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataCheckColumn Caption="Attachments" VisibleIndex="13" 
                                        FieldName="Attachment">
                                        <Settings AllowAutoFilter="False" />
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader1" />
                                        <CellStyle CssClass="GridCss1">
                                        </CellStyle>
                                    </dx:GridViewDataCheckColumn>
                                    <dx:GridViewDataTextColumn Caption="SID" FieldName="SID" 
                                        ShowInCustomizationForm="True" Visible="False" VisibleIndex="12">
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" VisibleIndex="0" 
                                        Width="60px">
                                        <EditButton Visible="True">
                                            <Image Url="~/images/edit.png">
                                            </Image>
                                        </EditButton>
                                        <HeaderStyle CssClass="GridCssHeader1" />
                                        <CellStyle CssClass="GridCss1">
                                        </CellStyle>
                                    </dx:GridViewCommandColumn>
                                </Columns>
                                <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" 
                                    AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True" />
                                <Settings ShowGroupPanel="True" />
                                <SettingsText ConfirmDelete="Are you sure you want to Delete?" />
                                <Styles>
                                    <AlternatingRow CssClass="GridAltRow">
                                    </AlternatingRow>
                                    <GroupRow Font-Bold="True">
                                                        </GroupRow>
                                    <LoadingPanel ImageSpacing="5px">
                                    </LoadingPanel>
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
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
           <%-- </dx:PanelContent> --%> 
       <%-- </PanelCollection>
    </dx:ASPxRoundPanel>--%>
</asp:Content>
