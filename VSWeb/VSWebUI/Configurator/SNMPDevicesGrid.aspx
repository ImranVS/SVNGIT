<%@ Page Title="VitalSigns Plus-SNMPDevicesGrid" Language="C#" MasterPageFile="~/Site1.Master"AutoEventWireup="true" CodeBehind="SNMPDevicesGrid.aspx.cs" Inherits="VSWebUI.Configurator.SNMPDevicesGrid" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.alert-success').delay(10000).fadeOut("slow", function () {
            });
            $('.alert-danger').delay(10000).fadeOut("slow", function () {
            });
        });
        </script>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <%--<dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" 
        CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
        CssPostfix="Glass" 
        GroupBoxCaptionOffsetY="-24px" 
        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
        Width="100%" HeaderText="SNMP Devices">
        <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
        <HeaderStyle Height="23px">
        <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
        </HeaderStyle>
        <PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">--%>

<table width="100%">
        <tr>
            <td>
                <div class="header" id="servernamelbldisp" runat="server">SNMP Devices</div>
            </td>
        </tr>
        <tr>
            <td>
               <div class="info">An SNMP Device is anything that will respond to a ping. VitalSigns pings it and times the response.
                </div>
                <div id="successDiv" class="alert alert-success" runat="server" style="display: none">Success.
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxGridView ID="SNMPDevicesGridView" runat="server" AutoGenerateColumns="False" 
                    CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                    CssPostfix="Office2010Silver" Width="100%" KeyFieldName="ID" 
                    OnHtmlRowCreated="SNMPDevicesGridView_HtmlRowCreated" 
                    OnRowDeleting="SNMPDevicesGridView_RowDeleting" 
                    OnHtmlDataCellPrepared="SNMPDevicesGridView_HtmlDataCellPrepared" 
                    EnableTheming="True" Theme="Office2003Blue" OnPageSizeChanged="SNMPDevicesGridView_PageSizeChanged">
                    <Columns>
                        <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" 
                            ShowInCustomizationForm="True" FixedStyle="Left"
                                        VisibleIndex="0" Width="60px">
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
                                        <HeaderStyle CssClass="GridCssHeader" />
                                    </dx:GridViewCommandColumn>

                        <dx:GridViewDataCheckColumn Caption="Enabled" VisibleIndex="1" 
                            FixedStyle="Left" FieldName="Enabled" Width="60px">
                            <Settings AllowAutoFilter="False" />
                        <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" >
                            <Paddings Padding="5px" />
                            </HeaderStyle>
                            <CellStyle CssClass="GridCss1"></CellStyle>
                        </dx:GridViewDataCheckColumn>
                        
                        <dx:GridViewDataTextColumn Caption="Name" FixedStyle="Left" 
                            ShowInCustomizationForm="True" VisibleIndex="2" FieldName="Name">
                            <Settings AutoFilterCondition="Contains" />
                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Scan Interval" VisibleIndex="3" 
                            FieldName="Scanning Interval" Width="80px">
                            <Settings AllowAutoFilter="False" />
                        <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                            <CellStyle CssClass="GridCss2"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Off-Hours Scan Interval" VisibleIndex="4" 
                            FieldName="Scanning Interval" Width="90px">
                            <Settings AllowAutoFilter="False" />
                        <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                            <CellStyle CssClass="GridCss2"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Address" VisibleIndex="7" 
                            FieldName="Address">
                        <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Category" VisibleIndex="8" 
                            FieldName="Category">
                        <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Response Threshold" VisibleIndex="6" 
                            FieldName="ResponseThreshold" Width="90px">
                            <Settings AllowAutoFilter="False" />
                        <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                            <CellStyle CssClass="GridCss2"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Location" VisibleIndex="9" visible=false
                            FieldName="Location">
                        <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Password" VisibleIndex="10" 
                            FieldName="Password">
                            <Settings AllowAutoFilter="False" />
                        <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Port" FieldName="Port" 
                            ShowInCustomizationForm="True" VisibleIndex="11" Width="60px">
                            <Settings AllowAutoFilter="False" />
                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader2" /><CellStyle CssClass="GridCss2"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="User Name" FieldName="Username" 
                            ShowInCustomizationForm="True" VisibleIndex="12">
                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Retry Interval" FieldName="RetryInterval" 
                            ShowInCustomizationForm="True" VisibleIndex="5" Width="80px">
                            <Settings AllowAutoFilter="False" />
                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                            <CellStyle CssClass="GridCss2"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="OID" FieldName="OID" 
                            ShowInCustomizationForm="True" VisibleIndex="13">
                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" />
                    <Settings ShowGroupPanel="True" ShowFilterRow="True" />
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
                    <SettingsPager PageSize="50" SEOFriendly="Enabled" >
            <PageSizeItemSettings Visible="true" />
        </SettingsPager>
                </dx:ASPxGridView>&nbsp;</td>
        </tr>
    </table>
<%--</dx:PanelContent>
</PanelCollection>
    </dx:ASPxRoundPanel>--%>
</asp:Content>
