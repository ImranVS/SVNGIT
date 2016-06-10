<%@ Page Title="VitalSigns Plus -  Network Latency" Language="C#" MasterPageFile="~/Site1.Master"
    AutoEventWireup="true" CodeBehind="NetworkLatencyServers.aspx.cs" Inherits="VSWebUI.Configurator.NetworkLatencyServers" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
                nlGridView1.GetRowValues(e.visibleIndex, 'TestName', OnGetRowValues);

            function OnGetRowValues(values) {
                var id = values[0];
                var name = values[1];
                var OK = (confirm('Are you sure you want to delete the network latency test - ' + values + '?'))
                if (OK == true) {
                    nlGridView1.DeleteRow(visibleIndex);
                }
            }
        }
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
                <div class="header" id="servernamelbldisp" runat="server">
                    Windows Server Network Latency</div>
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
                <%--<dx:ASPxRoundPanel runat="server" GroupBoxCaptionOffsetY="-24px" Height="50px" HeaderText="Domino Servers"
        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
        ID="ASPxRoundPanel13" Width="100%">
        <ContentPaddings PaddingTop="10px" PaddingBottom="10px" PaddingLeft="4px"></ContentPaddings>
        <HeaderStyle Height="23px">
            <Paddings PaddingLeft="2px" PaddingBottom="0px" PaddingTop="0px"></Paddings>
        </HeaderStyle>
        <PanelCollection>
            <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                <table width="100%">--%>
                <%-- <dx:ASPxButton ID="ImportDominoServers" runat="server" 
                    CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                    CssPostfix="Office2010Blue" OnClick="ImportDominoServers_Click" 
                    SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" 
                    Text="Import Domino Servers" Wrap="False">
                </dx:ASPxButton>--%>
                <dx:ASPxGridView runat="server" KeyFieldName="NetworkLatencyId" AutoGenerateColumns="False"
                    Width="100%" ID="nlGridView1" ClientInstanceName="nlGridView1" OnRowDeleting="nlGridView1_RowDeleting"
                    OnPageSizeChanged="nlGridView1_PageSizeChanged" OnHtmlRowCreated="nlGridView1_HtmlRowCreated"
                    Cursor="pointer" Theme="Office2003Blue" CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css"
                    CssPostfix="Office2010Silver">
                    <ClientSideEvents CustomButtonClick="OnCustomButtonClick" />
                    <Columns>
                        <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" VisibleIndex="0" Width="70px">
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
                            <HeaderStyle CssClass="GridCssHeader1">
                                <Paddings Padding="5px" />
                            </HeaderStyle>
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataCheckColumn Caption="Enabled" FieldName="Enable" VisibleIndex="2"
                            Width="60px">
                            <Settings AllowAutoFilter="False" />
                            <Settings AllowAutoFilter="False"></Settings>
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader">
                                <Paddings Padding="5px" />
                                <Paddings Padding="5px"></Paddings>
                            </HeaderStyle>
                            <CellStyle CssClass="GridCss1">
                                <Paddings Padding="5px" />
                                <Paddings Padding="5px"></Paddings>
                            </CellStyle>
                        </dx:GridViewDataCheckColumn>
                        <%--<dx:gridviewdatacomboboxcolumn FieldName="ServerType" VisibleIndex="3" 
                UnboundType="String">
                <PropertiesComboBox  TextField="ServerType" 
                    ValueField="ServerType">
                    <ValidationSettings>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                      <ClientSideEvents SelectedIndexChanged="OnSelectedIndexChanged" />
                </PropertiesComboBox>
                <Settings AllowAutoFilter="True" AllowHeaderFilter="False" />
<EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
            </dx:gridviewdatacomboboxcolumn>--%>
                        <%--   <dx:GridViewDataTextColumn Caption="Category" FieldName="Category" 
                                        VisibleIndex="3">
                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
<Settings AllowAutoFilter="True" AutoFilterCondition="Contains"></Settings>

                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>--%>
                        <dx:GridViewDataTextColumn Caption="Test Name" FieldName="TestName" VisibleIndex="3">
                            <%-- <PropertiesHyperLinkEdit NavigateUrlFormatString="DominoProperties.aspx?Key={0}" Target="_self"
                                            TextField="Name" TextFormatString="">
                                        </PropertiesHyperLinkEdit>--%>
                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                            <Settings AutoFilterCondition="Contains" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss" HorizontalAlign="Left">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="ScanInterval" Caption="Scan Interval" VisibleIndex="4">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains"></Settings>
                            <HeaderStyle CssClass="GridCssHeader2"></HeaderStyle>
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Test Duration" FieldName="TestDuration" VisibleIndex="5">
                            <HeaderStyle CssClass="GridCssHeader2" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="NetworkLatencyId" FieldName="NetworkLatencyId"
                            Visible="False" VisibleIndex="6">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
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
                    <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" AllowSelectByRowClick="True"
                        ProcessSelectionChangedOnServer="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>
                    <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                    <Settings ShowFilterRow="True" ShowGroupPanel="True"></Settings>
                    <SettingsText ConfirmDelete="Are you sure you want to delete?"></SettingsText>
                    <Styles CssPostfix="Office2010Silver" CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css">
                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
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
                    <SettingsPager PageSize="50" SEOFriendly="Enabled">
                        <PageSizeItemSettings Visible="true" />
                        <PageSizeItemSettings Visible="True">
                        </PageSizeItemSettings>
                    </SettingsPager>
                </dx:ASPxGridView>
                <%-- <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>--%>
                <%-- </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>--%>
            </td>
        </tr>
    </table>
    </table>
</asp:Content>
