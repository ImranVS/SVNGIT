<%@ Page Title="VitalSigns Plus - RPR Internal Access Pages" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="RPRAccess.aspx.cs" Inherits="VSWebUI.Security.RPRAccess" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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

        .style1
        {
            width: 100%;
        }
    </style>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="Pwd" runat="server">
<table style="width:100%;">
    <tr>
        <td width="5%">
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td>
            &nbsp;</td>
        <td>
            <table>
                <tr>
                    <td>
                        <div id="successDiv" runat="server" class="alert alert-success" style="display: none">
                            Password was changed successully.
                            <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxRoundPanel ID="ChangepwdRoundPanel" runat="server" 
                            CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                            GroupBoxCaptionOffsetY="-24px" HeaderText="Authentication Required!!!" 
                            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="400px">
                            <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                            <HeaderStyle Height="23px">
                            <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                            </HeaderStyle>
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <table class="style1">
                                        <tr>
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <dx:ASPxLabel ID="pwdLabel" runat="server" ForeColor="#FF3300">
                                                </dx:ASPxLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" 
                                                    Text="Enter Password:">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>
                                                <dx:ASPxTextBox ID="PwdTextBox" runat="server" Password="True" Width="170px">
                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                        <RequiredField ErrorText="Enter New Password" IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <dx:ASPxLabel ID="ErrorMsg" runat="server" ForeColor="Red">
                                                </dx:ASPxLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="errorDiv" runat="server" class="alert alert-danger" style="display: none">
                            Passwords don&#39;t match. Please make sure you enter the same password in the New 
                            and Confirm Password fields.
                            <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <dx:ASPxButton ID="SaveButton" runat="server" 
                                        CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                        CssPostfix="Office2010Blue" OnClick="SaveButton_Click" 
                                        SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" 
                                        Text="Submit">
                                    </dx:ASPxButton>
                                </td>
                                <td>
                                    <dx:ASPxButton ID="CancelButton" runat="server" CausesValidation="False" 
                                        CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                        CssPostfix="Office2010Blue" OnClick="CancelButton_Click" 
                                        SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="Cancel">
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
</table>
</div>
<div id="rprAccess" runat="server">

<table style="width:100%;">
    <tr>
        <td >
            &nbsp;</td>
        <td>
        <asp:Label ID="ASPLabel1" runat="server" Text="Welcome RPR Wyatt Admin" 
        Font-Bold="True" Font-Size="Large" style="color: #000000; font-family: Verdana"></asp:Label>
            </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td>
            &nbsp;</td>
        <td>
            <dx:ASPxRoundPanel ID="ReportsRoundPanel" runat="server" 
                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                    GroupBoxCaptionOffsetY="-24px" HeaderText="RPR Wyatt Admin Pages" 
                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                    <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                    <HeaderStyle Height="23px">
                    <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
<Paddings PaddingLeft="2px" PaddingTop="0px" PaddingBottom="0px"></Paddings>
                    </HeaderStyle>
                    <PanelCollection>
<dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
    <table class="style1">
        <tr>
            <td>
                
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxGridView ID="RPRAccessGridView" runat="server" 
                    AutoGenerateColumns="False" 
                    CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                    CssPostfix="Office2010Silver" Cursor="pointer" KeyFieldName="ID" 
                    Width="100%" Theme="Office2003Blue"  OnSelectionChanged="RPRAccessGridView_SelectionChanged" 
                    OnHtmlDataCellPrepared="RPRAccessGridView_HtmlDataCellPrepared" 
                    OnHtmlRowCreated="RPRAccessGridView_HtmlRowCreated" OnPageSizeChanged="RPRAccessGridView_PageSizeChanged">
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="Category" FieldName="Category" 
                            ShowInCustomizationForm="True" VisibleIndex="0">
                            <Settings AllowAutoFilter="False" AllowDragDrop="True" 
                                AutoFilterCondition="Contains" />
<Settings AllowDragDrop="True" AllowAutoFilter="False" AutoFilterCondition="Contains"></Settings>

                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Page" FieldName="Name" 
                            ShowInCustomizationForm="True" VisibleIndex="1">
                            <Settings AllowDragDrop="False" AutoFilterCondition="Contains" />
<Settings AllowDragDrop="False" AutoFilterCondition="Contains"></Settings>
                         
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss" HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Details" FieldName="Description" 
                            ShowInCustomizationForm="True" VisibleIndex="2">
                            <Settings AllowAutoFilter="False" AllowDragDrop="False" 
                                AutoFilterCondition="Contains" />
<Settings AllowDragDrop="False" AllowAutoFilter="False" AutoFilterCondition="Contains"></Settings>

                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="PageURL" FieldName="PageURL" 
                            ShowInCustomizationForm="True" Visible="False" VisibleIndex="4">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" 
                            ShowInCustomizationForm="True" Visible="False" VisibleIndex="3">
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsBehavior AllowDragDrop="False" AllowSelectByRowClick="True" 
                        AutoExpandAllGroups="True" ProcessSelectionChangedOnServer="True" />

<SettingsBehavior AllowDragDrop="False" AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True" AutoExpandAllGroups="True"></SettingsBehavior>

                    <SettingsPager SEOFriendly="Enabled" PageSize="50">
                        <PageSizeItemSettings Visible="True">
                        </PageSizeItemSettings>
                    </SettingsPager>
                    <Settings ShowFilterRow="True" ShowGroupPanel="True" />

<Settings ShowFilterRow="True" ShowGroupPanel="True"></Settings>

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
                    <Styles CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                        CssPostfix="Office2010Silver">
                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                        </Header>
                        <GroupRow Font-Bold="True" Font-Italic="False">
                        </GroupRow>
                        <GroupFooter Font-Bold="True">
                        </GroupFooter>
                        <GroupPanel Font-Bold="False">
                        </GroupPanel>
                        <LoadingPanel ImageSpacing="5px">
                        </LoadingPanel>
                        <AlternatingRow CssClass="GridAltRow" Enabled="True">
                        </AlternatingRow>
                    </Styles>
                    <StylesEditors ButtonEditCellSpacing="0">
                        <ProgressBar Height="21px">
                        </ProgressBar>
                    </StylesEditors>
                </dx:ASPxGridView>
                <br /></td></tr><tr><td> 
               <%-- <table><tr><td valign="top" align="left"> --%>
                  <dx:ASPxLabel ID="MsgLabel" runat="server" Text="Categories" Visible="false">
                    </dx:ASPxLabel> 
                     <br />           
                <dx:ASPxListBox ID="CategoryListBox" runat="server" AutoPostBack="True" 
                    Visible="False">
                </dx:ASPxListBox>
               <%-- </td>--%>
               <%-- <td>--%>
                    
                <%--<dx:ASPxDataView ID="ReportsDataView" runat="server" Visible="false" Width="100%" 
                    Layout="Flow" OnDataBound="ReportsDataView_DataBound" 
                        OnItemCommand="ReportsDataView_ItemCommand">
                <ItemTemplate>
                <a href='<%# Eval("PageURL") %>' id="A" runat="server">
                <table>
                <tr><td><img src='<%# Eval("ImageURL") %>' height="102px" width="102px"/></td></tr>
                <tr><td><asp:Label ID="RepName" Text='<%# Eval("Name") %>' runat ="server"/></td></tr>
                </table></a>
                </ItemTemplate>
                <ItemStyle Width="110px" Height="140px">
                                    <Paddings Padding="0px" />
                                    <Paddings Padding="0px"></Paddings>
                                </ItemStyle>
                </dx:ASPxDataView>--%>
            <%--</td>--%>
       <%-- </tr></table>--%></td></tr>
    </table>
                        </dx:PanelContent>
</PanelCollection>
                </dx:ASPxRoundPanel>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
</table>
</div>
</asp:Content>
