<%@ Page Title="VitalSigns Plus-MailServicesGrid" Language="C#" MasterPageFile="~/Site1.Master"
    AutoEventWireup="true" CodeBehind="NetworkLatencyTest.aspx.cs" Inherits=" VSWebUI.Configurator.NetworkLatencyTest" %>

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
    <table>
        <tr>
            <td>
                <div class="info">
                    A Network Latency Tests test a group of servers for network response time.
                </div>
            </td>
            <td>
                <div id="successDiv" class="alert alert-success" runat="server" style="display: none">
                </div>
            </td>
        </tr>
    </table>
    <table>
    <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" Text="Server Type">
                </dx:ASPxLabel>
            </td>
            <td colspan="2">
                <dx:ASPxComboBox ID="ServerTypeComboBox" runat="server" ValueType="System.String"
                    AutoPostBack="True" OnSelectedIndexChanged="ServerTypeComboBox_SelectedIndexChanged">
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Select server type." IsRequired="True" />
                        <RequiredField IsRequired="True" ErrorText="Select server type."></RequiredField>
                    </ValidationSettings>
                    
                </dx:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxLabel ID="enable" runat="server" CssClass="lblsmallFont" Text="Group Name:">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxTextBox ID="groupname" runat="server" CssClass="txtsmall" Width="100px">
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" Text="Scan Interval:">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxTextBox ID="scantext" runat="server" CssClass="txtsmall" Width="100px">
                </dx:ASPxTextBox>
            </td>
            <td align="left">
                <div id="Div1" runat="server" class="lblsmallFont">
                    Minutes
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="lblsmallFont" Text="Test Duration:">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxTextBox ID="testduration" runat="server" CssClass="txtsmall" Width="100px">
                </dx:ASPxTextBox>
            </td>
            <td align="left">
                <div id="Div2" runat="server" class="lblsmallFont">
                    Minutes
                </div>
            </td>
        </tr>
        
    </table>
    <table>
        <tr>
            <td>
                <%--<dx:ASPxGridView ID="networklatencyGridView" runat="server" AutoGenerateColumns="False"
                    KeyFieldName="ID" OnHtmlRowCreated="networklatencyGridView_HtmlRowCreated" OnRowDeleting="networklatencyGridView_RowDeleting"
                    Width="100%" OnHtmlDataCellPrepared="networklatencyGridView_HtmlDataCellPrepared"
                    EnableTheming="True" Theme="Office2003Blue" OnPageSizeChanged="networklatencyGridView_PageSizeChanged">
                    <Columns>
                        <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" VisibleIndex="0" Width="80px"
                            FixedStyle="Left">
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
                            <CellStyle CssClass="GridCss1">
                            </CellStyle>
                            <ClearFilterButton Visible="True">
                                <Image Url="~/images/clear.png">
                                </Image>
                            </ClearFilterButton>
                            <HeaderStyle CssClass="GridCssHeader" />
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataCheckColumn Caption="Enabled" VisibleIndex="1" CellStyle-HorizontalAlign="Center"
                            FieldName="Enabled" FixedStyle="Left" Width="70px">
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
                        <dx:GridViewDataTextColumn Caption="Name" VisibleIndex="2" FieldName="Name" FixedStyle="Left">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Scan Interval" VisibleIndex="4" FieldName="ScanInterval"
                            Width="90px">
                            <Settings AllowAutoFilter="False" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Test Duration" VisibleIndex="5" FieldName="TestDuration"
                            Width="100px">
                            <Settings AllowAutoFilter="False" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" />
                    <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                    <SettingsText ConfirmDelete=" Are you sure you want to delete this record?" />
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
                    <SettingsPager PageSize="50" SEOFriendly="Enabled">
                        <PageSizeItemSettings Visible="true" />
                    </SettingsPager>
                </dx:ASPxGridView>--%>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxGridView runat="server" CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css"
                    CssPostfix="Office2010Silver" KeyFieldName="ID" AutoGenerateColumns="False"
                    Width="50%" ID="NetworkLatencyTestgrd" Cursor="pointer" EnableTheming="True"
                    Theme="Office2003Blue" OnPreRender="NetworkLatencyTestgrd_PreRender">
                    <Columns>
                     <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" VisibleIndex="0" Width="80px"
                            FixedStyle="Left">
                           
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
                            <CellStyle CssClass="GridCss1">
                            </CellStyle>
                            <ClearFilterButton Visible="True">
                                <Image Url="~/images/clear.png">
                                </Image>
                            </ClearFilterButton>
                            <HeaderStyle CssClass="GridCssHeader" />
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn Caption="Enabled" FieldName="Enabled"
                            VisibleIndex="5" Visible="false">
                            <Settings AutoFilterCondition="Contains" />
                            <Settings AutoFilterCondition="Contains"></Settings>
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss" HorizontalAlign="Left">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewCommandColumn Caption="Enable" ShowSelectCheckbox="True" VisibleIndex="0">
                            <ClearFilterButton Visible="True">
                            </ClearFilterButton>
                            <HeaderStyle CssClass="GridCssHeader1" />
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn Caption="Name" FieldName="ServerName" VisibleIndex="1">
                            <Settings AutoFilterCondition="Contains" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss" HorizontalAlign="Left">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Yellow Threshold" FieldName="LatencyYellowThreshold"
                            VisibleIndex="5">
                            <DataItemTemplate>
                                <dx:ASPxTextBox ID="txtyellowthreshValue" runat="server" KeyFieldName="ID"
                                    Width="100px" Value='<%# Eval("LatencyYellowThreshold") %>'>
                                </dx:ASPxTextBox>
                            </DataItemTemplate>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Red Threshold" FieldName="LatencyRedThreshold"
                            VisibleIndex="6">
                            <DataItemTemplate>
                                <dx:ASPxTextBox ID="txtredthreshValue" runat="server" KeyFieldName="ID" Width="100px"
                                    Value='<%# Eval("LatencyRedThreshold") %>'>
                                </dx:ASPxTextBox>
                            </DataItemTemplate>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="Location" Caption="Location" VisibleIndex="2">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                            <HeaderStyle CssClass="GridCssHeader"></HeaderStyle>
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                    </Columns>
                </dx:ASPxGridView>
            </td>
        </tr>
        
        <tr>
			<td>
				<table>
					<tr>
						<td>
							<dx:ASPxButton ID="FormSaveButton" runat="server" Text="Save" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
								CssPostfix="Office2010Blue" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
								Width="75px" OnClick="FormSaveButton_Click">
							</dx:ASPxButton>
						</td>
					</tr>
				</table>
			</td>
		</tr>
    </table>
    <%-- </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>--%>
</asp:Content>
