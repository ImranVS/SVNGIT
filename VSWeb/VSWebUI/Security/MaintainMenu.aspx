<%@ Page Title="VitalSigns Plus - Maintain Menus" Language="C#" MasterPageFile="~/Site1.Master"
    AutoEventWireup="true" CodeBehind="MaintainMenu.aspx.cs" Inherits="VSWebUI.Security.MaintainMenu" %>
	<%@ MasterType virtualpath="~/Site1.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx1" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function hidepopup() {

            var popup = document.getElementById('ContentPlaceHolder1_pnlAreaDtls');
            popup.style.visibility = 'hidden';

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="pnlAreaDtls" style="height: 100%; width: 100%; visibility: hidden" runat="server"
        class="pnlDetails12">
        <table align="center" width="30%" style="height: 100%">
            <tr>
                <td align="center" valign="middle" style="height: auto;">
                    <table border="1" cellspacing="0" class="csline" cellpadding="2px" id="table_txt_edit"
                        style="border-width: 1px; border-style: solid; border-collapse: collapse; border-color: silver;
                        background-color: #F8F8FF" width="100%">
                        <tr style="background-color: White">
                            <td align="left">
                                <div class="subheading">
                                    Delete Server</div>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table>
                                    <tr>
                                        <td valign="top">
                                        </td>
                                        <td align="center">
                                            <div style="overflow: auto; height: 60px; font-size: 12px; font-weight: normal; font-family: Arial, Helvetica, sans-serif;
                                                text-align: left; color: black; width: 350px;" id="divmsg" runat="server">
                                            </div>
                                            <asp:Button ID="btnok1" runat="server" OnClick="btn_OkClick" OnClientClick="hidepopup()"
                                                Text="OK" Width="50px" Font-Names="Arial" Font-Size="Small" />
                                            <asp:Button ID="btncancel1" runat="server" OnClick="btn_CancelClick" OnClientClick="hidepopup()"
                                                Text="Cancel" Width="70px" Font-Names="Arial" Font-Size="Small" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <table>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Maintain Menus" Font-Bold="True" Font-Size="Large"
                    Style="color: #000000; font-family: Verdana"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="color: Black" id="tdmsg" runat="server" align="center">
            </td>
        </tr>
    </table>
    <dx:ASPxGridView runat="server" KeyFieldName="ID" AutoGenerateColumns="False" ID="MenusGridView"
        OnRowDeleting="MenusGridView_RowDeleting" OnRowInserting="MenusGridView_RowInserting"
        OnRowUpdating="MenusGridView_RowUpdating" OnPageSizeChanged="MenusGridView_PageSizeChanged"
        OnCellEditorInitialize="MenusGridView_CellEditorInitialize" Width="100%" OnAutoFilterCellEditorInitialize="MenusGridView_AutoFilterCellEditorInitialize"
        Theme="Office2003Blue" EnableTheming="True">
        <Columns>
            <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" FixedStyle="Left"
                VisibleIndex="0" Width="40px">
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
                <HeaderStyle CssClass="GridCssHeader" />
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn Caption="Delete" VisibleIndex="1" CellStyle-HorizontalAlign="Center"
                Width="30px">
                <DataItemTemplate>
                    <asp:Label ID="lblmenu" runat="server" Text='<%#Eval("DisplayText") %>' Visible="false"></asp:Label>
                    <asp:ImageButton ID="btndele" runat="server" ImageUrl="../images/delete.png" Width="15px"
                        Height="15px" CommandName="Delete" CommandArgument='<%#Eval("ID") %>' AlternateText='<%#Eval("DisplayText") %>'
                        ToolTip="Delete" OnClick="btn_Click" />
                </DataItemTemplate>
                <EditFormSettings Visible="False" />
                <EditCellStyle CssClass="GridCss">
                </EditCellStyle>
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss1">
                </CellStyle>
                <CellStyle HorizontalAlign="Center">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ID" ReadOnly="True" Visible="False" VisibleIndex="2">
                <EditFormSettings Visible="False"></EditFormSettings>
                <EditCellStyle CssClass="GridCss">
                </EditCellStyle>
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="DisplayText" VisibleIndex="3">
                <PropertiesTextEdit>
                    <ValidationSettings>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
                <Settings AutoFilterCondition="Contains" />
                <EditFormSettings VisibleIndex="0"></EditFormSettings>
                <EditCellStyle CssClass="GridCss">
                </EditCellStyle>
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ImageURL" VisibleIndex="5">
                <EditCellStyle CssClass="GridCss">
                </EditCellStyle>
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataSpinEditColumn FieldName="OrderNum" UnboundType="String" VisibleIndex="8">
                <PropertiesSpinEdit DisplayFormatString="g">
                    <ValidationSettings>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </PropertiesSpinEdit>
                <Settings AllowAutoFilter="False" AllowHeaderFilter="False" />
                <EditCellStyle CssClass="GridCss">
                </EditCellStyle>
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataSpinEditColumn>
            <%-- <dx:gridviewdatatextcolumn FieldName="Parentmenu" VisibleIndex="3" 
                Caption="ParentMenu">
                <Settings AllowAutoFilter="True" />
            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>

            </dx:gridviewdatatextcolumn>--%>
            <dx:GridViewDataComboBoxColumn FieldName="Parentmenu" VisibleIndex="6">
                <PropertiesComboBox TextField="Parentmenu" ValueField="ID" IncrementalFilteringMode="Contains">
                </PropertiesComboBox>
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                <EditCellStyle CssClass="GridCss">
                </EditCellStyle>
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataComboBoxColumn FieldName="MenuArea" VisibleIndex="9">
                <PropertiesComboBox TextField="MenuArea" ValueField="MenuArea">
                    <Items>
                        <dx:ListEditItem Text="Configurator" Value="Configurator" />
                        <dx:ListEditItem Text="Dashboard" Value="Dashboard" />
                    </Items>
                    <ValidationSettings>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </PropertiesComboBox>
                <Settings AllowAutoFilter="True" />
                <EditCellStyle CssClass="GridCss">
                </EditCellStyle>
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataTextColumn FieldName="PageLink" VisibleIndex="4">
                <Settings AllowAutoFilter="True"></Settings>
                <EditCellStyle CssClass="GridCss">
                </EditCellStyle>
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <HeaderStyle CssClass="GridCssHeader"></HeaderStyle>
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataSpinEditColumn FieldName="Level" VisibleIndex="7">
                <PropertiesSpinEdit DisplayFormatString="g">
                </PropertiesSpinEdit>
                <Settings AllowAutoFilter="False" />
                <EditCellStyle CssClass="GridCss">
                </EditCellStyle>
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataSpinEditColumn>
        </Columns>
        <SettingsBehavior ConfirmDelete="True"></SettingsBehavior>
        <Settings ShowFilterRow="True" ShowGroupPanel="True" />
        <SettingsText ConfirmDelete="Are you sure you want to delete this server?"></SettingsText>
        <Images SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css">
            <LoadingPanelOnStatusBar Url="~/App_Themes/Office2010Blue/GridView/Loading.gif">
            </LoadingPanelOnStatusBar>
            <LoadingPanel Url="~/App_Themes/Office2010Blue/GridView/Loading.gif">
            </LoadingPanel>
        </Images>
        <ImagesFilterControl>
            <LoadingPanel Url="~/App_Themes/Office2010Blue/GridView/Loading.gif">
            </LoadingPanel>
        </ImagesFilterControl>
        <Styles>
            <Header SortingImageSpacing="5px" ImageSpacing="5px">
            </Header>
            <LoadingPanel ImageSpacing="5px">
            </LoadingPanel>
            <AlternatingRow CssClass="GridAltRow" Enabled="True">
            </AlternatingRow>
        </Styles>
        <StylesPager>
            <PageNumber ForeColor="#3E4846">
            </PageNumber>
            <Summary ForeColor="#1E395B">
            </Summary>
        </StylesPager>
        <StylesEditors ButtonEditCellSpacing="0">
            <ProgressBar Height="21px">
            </ProgressBar>
        </StylesEditors>
        <SettingsPager PageSize="10" SEOFriendly="Enabled">
            <PageSizeItemSettings Visible="true" />
        </SettingsPager>
    </dx:ASPxGridView>
    <dx:ASPxPopupControl ID="NavigatorPopupControl" runat="server" ClientInstanceName="popup1"
        HeaderText="Information" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        Theme="Glass">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                <table class="style1">
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="MsgLabel" runat="server" ClientInstanceName="poplbl">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxButton ID="OKButton" runat="server" OnClick="OKButton_Click" Text="OK" Theme="Office2010Blue">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
