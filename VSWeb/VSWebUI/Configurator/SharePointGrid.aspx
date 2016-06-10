<%@ Page Title="VitalSigns Plus - SharePoint Servers" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind=" .aspx.cs" Inherits="VSWebUI.Configurator.SharePointGrid" %>

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
            $('.alert-danger').delay(10000).fadeOut("slow", function () {
            });
        });
        </script>
<style type="text/css">
        .dxrpControl_Office2003Blue .dxrpTE, .dxrpControl_Office2003Blue .dxrpNHTE, .dxrpControlGB_Office2003Blue .dxrpNHTE
        {
            border-top: 1px solid #002D96;
        }
        .dxrpControl_Office2003Blue .dxrpTE
        {
            background-image: none;
            background-color: #bbd4f5;
        }
        .dxrpControl_Office2003Blue .dxrpHeader_Office2003Blue, .dxrpControl_Office2003Blue .dxrpHLE, .dxrpControl_Office2003Blue .dxrpHeader_Office2003Blue, .dxrpControl_Office2003Blue .dxrpBE, .dxrpControl_Office2003Blue .dxrpHLE, .dxrpControl_Office2003Blue .dxrpHRE, .dxrpControlGB_Office2003Blue .dxrpBE
        {
            border-bottom: 1px solid #002D96;
        }
        .dxrpControl_Office2003Blue .dxrpLE, .dxrpControl_Office2003Blue .dxrpHLE, .dxrpControlGB_Office2003Blue .dxrpLE
        {
            border-left: 1px solid #002D96;
        }
        .dxrpControl_Office2003Blue .dxrpHeader_Office2003Blue, .dxrpControl_Office2003Blue .dxrpHLE, .dxrpControl_Office2003Blue .dxrpHRE
        {
            background-image: none;
            background-color: #7BA4E0;
        }
        .dxrpControl_Office2003Blue .dxrpRE, .dxrpControl_Office2003Blue .dxrpHRE, .dxrpControlGB_Office2003Blue .dxrpRE
        {
            border-right: 1px solid #002D96;
        }
        .dxrpControl_Office2003Blue .dxrpcontent, .dxrpControl_Office2003Blue .dxrpLE, .dxrpControl_Office2003Blue .dxrpRE, .dxrpControl_Office2003Blue .dxrpBE, .dxrpControl_Office2003Blue .dxrpNHTE, .dxrpControlGB_Office2003Blue .dxrpcontent, .dxrpControlGB_Office2003Blue .dxrpLE, .dxrpControlGB_Office2003Blue .dxrpRE, .dxrpControlGB_Office2003Blue .dxrpBE, .dxrpControlGB_Office2003Blue .dxrpNHTE, .dxrpControlGB_Office2003Blue span.dxrpHeader_Office2003Blue
        {
            background-image: none;
            background-color: #DDECFE;
        }
        .dxrpControl_Office2003Blue .dxrpcontent, .dxrpControlGB_Office2003Blue .dxrpcontent
        {
            background-image: none;
            vertical-align: top;
        }
        .style1
        {
            height: 18px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <table width="100%">
    <tr>
        <td>
            <div class="header" id="servernamelbldisp" runat="server">Microsoft SharePoint Servers</div>
        </td>
    </tr>
    <tr><td>
        <%--<dx:ASPxRoundPanel runat="server" GroupBoxCaptionOffsetY="-24px" Height="50px" HeaderText="Exchange Servers"
        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
        ID="ASPxRoundPanel13" Width="100%">
        <ContentPaddings PaddingTop="10px" PaddingBottom="10px" PaddingLeft="4px"></ContentPaddings>
        <HeaderStyle Height="23px">
            <Paddings PaddingLeft="2px" PaddingBottom="0px" PaddingTop="0px"></Paddings>
        </HeaderStyle>
        <PanelCollection>
            <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                <table width="100%">--%>
                   <div id="successDiv" class="alert alert-success" runat="server" style="display: none">Success.
                            </div>
                            <dx:ASPxGridView runat="server" KeyFieldName="ID" AutoGenerateColumns="False"
                                Width="100%" ID="sharepointGridView" OnPageSizeChanged="sharepointGridView_PageSizeChanged" 
                                
            OnHtmlRowCreated="sharepointGridView_HtmlRowCreated" Cursor="pointer" 
            EnableTheming="True" Theme="Office2003Blue" >
                                <Columns>
                                        <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions"
                                        VisibleIndex="0" Width="60px">
                                        <EditButton Visible="True" >
                                            <Image Url="../images/edit.png">
                                            </Image>
                                        </EditButton>
                                        <CellStyle CssClass="GridCss1">
                                            <Paddings Padding="3px" />
<Paddings Padding="3px"></Paddings>
                                        </CellStyle>
                                        <ClearFilterButton Visible="True">
                                            <Image Url="~/images/clear.png">
                                            </Image>
                                        </ClearFilterButton>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                    </dx:GridViewCommandColumn>
                                    <dx:GridViewDataCheckColumn Caption="Enabled"
                                        VisibleIndex="1" Width="60px" FieldName="Enabled">
                                        <Settings AllowAutoFilter="False" />
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" >
                                        <Paddings Padding="5px" />
                                        </HeaderStyle>
                                        <CellStyle CssClass="GridCss1">
                                            <Paddings Padding="5px" />
                                        </CellStyle>
                                    </dx:GridViewDataCheckColumn>
                                    <dx:GridViewDataTextColumn Caption="Category" FieldName="category" 
                                        VisibleIndex="3">
                                       <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Name" FieldName="ServerName"
                                        VisibleIndex="2">
                                       <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss"  HorizontalAlign="Left">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                       <%-- <PropertiesHyperLinkEdit NavigateUrlFormatString="ExchangeProperties.aspx?Key={0}" Target="_self"
                                            TextField="Name" TextFormatString="">
                                        </PropertiesHyperLinkEdit>--%>
                                    </dx:GridViewDataTextColumn>
<dx:GridViewDataTextColumn FieldName="Location" Caption="Location" VisibleIndex="4">
    <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />

<HeaderStyle CssClass="GridCssHeader"></HeaderStyle>

<CellStyle CssClass="GridCss"></CellStyle>
</dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Scan Interval" FieldName="ScanInterval"
                                        VisibleIndex="5">
                                        <HeaderStyle CssClass="GridCssHeader2" />
                                        <CellStyle CssClass="GridCss2">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="ID"
                                        Visible="False" VisibleIndex="7">
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="IP Address" FieldName="ipaddress" 
                                        VisibleIndex="6"><HeaderStyle CssClass="GridCssHeader" />
                                    </dx:GridViewDataTextColumn>
									
                                </Columns>
                     <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" 
                                    AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True" 
                                    AllowSelectSingleRowOnly="True"></SettingsBehavior>
                                <Settings ShowFilterRow="True" ShowGroupPanel="True" />

                            <Settings ShowFilterRow="True" ShowGroupPanel="True"></Settings>

                                <SettingsText ConfirmDelete="Are you sure you want to delete?"></SettingsText>
                                <Styles CssPostfix="Office2010Silver" CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css">
                                    <Header ImageSpacing="5px" SortingImageSpacing="5px" CssClass="GridCssHeader">
                                    </Header>
                                      <GroupRow Font-Bold="True">
                                    </GroupRow>
                                      <AlternatingRow CssClass="GridAltRow" Enabled="True">
                        </AlternatingRow>
                                    <Cell CssClass="GridCss">
                                    </Cell>
                                    <LoadingPanel ImageSpacing="5px">
                                    </LoadingPanel>
                                </Styles>
                                <StylesEditors ButtonEditCellSpacing="0">
                                    <ProgressBar Height="21px">
                                    </ProgressBar>
                                </StylesEditors>
                                <SettingsPager PageSize="50" SEOFriendly="Enabled" >
            <PageSizeItemSettings Visible="true" />
<PageSizeItemSettings Visible="True"></PageSizeItemSettings>
        </SettingsPager>
                                
                            </dx:ASPxGridView>
                       
                   
                       <%-- <PropertiesHyperLinkEdit NavigateUrlFormatString="ExchangeProperties.aspx?Key={0}" Target="_self"
                                            TextField="Name" TextFormatString="">
                                        </PropertiesHyperLinkEdit>--%>           <%-- <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>--%>
    </td></tr></table>
</asp:Content>
