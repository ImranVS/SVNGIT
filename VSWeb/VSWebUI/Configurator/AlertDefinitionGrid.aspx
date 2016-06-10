<%@ Page Title="VitalSigns Plus-AlertDefinitionGrid" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="AlertDefinitionGrid.aspx.cs" Inherits="VSWebUI.AlertDefinitionGrid" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>






<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">

.dxeEditAreaSys
{
	width: 100%;
	background-position: 0 0; /*iOS Safari*/
}

.dxeEditAreaSys, .dxeEditAreaNotStrechSys
{
	border: 0px!important;
	padding: 0px;
}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table><tr><td valign="top">

    <dx:ASPxPageControl Font-Bold="True"  ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
        CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
        CssPostfix="Glass" Height="308px" 
        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
        Width="513px" TabSpacing="0px" EnableHierarchyRecreation="False">
        <TabPages>
            <dx:TabPage Text="Alert Definition">
                <ContentCollection>
                    <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                        <table style="width:100%;">
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" 
                                        CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                        CssPostfix="Glass" 
                                        GroupBoxCaptionOffsetY="-24px" HeaderText="Alert Options" Height="16px" 
                                        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="350px">
                                        <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
                                        <HeaderStyle Height="23px">
                                        <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                        </HeaderStyle>
                                        <PanelCollection>
                                            <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                                <table style="width:100%;">
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxButton ID="ASPxButton2" runat="server" Text="Alerts ON" 
                                                                CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                                                CssPostfix="Office2010Blue" 
                                                                SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css">
                                                            </dx:ASPxButton>
                                                        </td>
                                                        <td>
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxCheckBox ID="ASPxCheckBox1" runat="server" CheckState="Unchecked" 
                                                                Text="Repeat Down Server alerts every " CssClass="lblsmallFont">
                                                            </dx:ASPxCheckBox>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox ID="ASPxTextBox1" runat="server" CssClass="txtsmall" 
                                                                Height="16px" Width="38px">
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="seconds" CssClass="lblsmallFont">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
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
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <dx:ASPxGridView ID="MntWinDominoMaintGridView" runat="server" AutoGenerateColumns="False"
                                                                    KeyFieldName="MaintWindow" 
                                        CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver" Theme="Office2003Blue"
                                                                    Width="850px" >
                                                                    <Columns>
                                                                       <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" 
                                                                            ShowInCustomizationForm="True" VisibleIndex="0">
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
                                                                            <CellStyle  CssClass="GridCss">
                                                                                <Paddings Padding="3px" />
                                                                            </CellStyle>
                                                                           <HeaderStyle CssClass="GridCssHeader" />
                                                                        </dx:GridViewCommandColumn>
                                                                        <dx:GridViewDataCheckColumn Caption="Enabled" VisibleIndex="1">
                                                                        <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                                                                        </dx:GridViewDataCheckColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Name" ShowInCustomizationForm="True" 
                                                                            VisibleIndex="2">
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="eMail Address" 
                                                                            ShowInCustomizationForm="True" VisibleIndex="3">
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Applies to Business Hours" 
                                                                            ShowInCustomizationForm="True" VisibleIndex="4">
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" /><CellStyle CssClass="GridCss"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Applies to Off Hours" 
                                                                            ShowInCustomizationForm="True" VisibleIndex="5">
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" /><CellStyle CssClass="GridCss"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Applies to All" 
                                                                            ShowInCustomizationForm="True" VisibleIndex="6">
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" /><CellStyle CssClass="GridCss"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Applies to Domino" 
                                                                            ShowInCustomizationForm="True" VisibleIndex="7">
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" /><CellStyle CssClass="GridCss"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Applies to BlackBerry" 
                                                                            ShowInCustomizationForm="True" VisibleIndex="8">
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" /><CellStyle CssClass="GridCss"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Applies to Mail Services" 
                                                                            ShowInCustomizationForm="True" VisibleIndex="9">
                                                                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" /><CellStyle CssClass="GridCss"></CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                    </Columns>
                                                                    <SettingsBehavior ConfirmDelete="True" />
                                                                    <Settings ShowHorizontalScrollBar="True" />
                                                                    <SettingsText ConfirmDelete="Are you sure you want to delete?" />
                                                                    <SettingsLoadingPanel ImagePosition="Top" />
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
                                                                        <LoadingPanel ImageSpacing="5px">
                                                                        </LoadingPanel>
                                                                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                        </Header>
                                                                    </Styles>
                                                                    <StylesEditors ButtonEditCellSpacing="0">
                                                                        <ProgressBar Height="21px">
                                                                        </ProgressBar>
                                                                    </StylesEditors>
                                                                </dx:ASPxGridView>
                                    
                                    </td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                        </table>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
            <dx:TabPage Text="SMTP Server">
                <ContentCollection>
                    <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                        <table style="width:100%;">
                            <tr>
                                <td valign="top">
                                    <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" 
                                        CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                        CssPostfix="Glass" 
                                        GroupBoxCaptionOffsetY="-24px" HeaderText="SMTP Server" Height="16px" 
                                        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="323px">
                                        <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
                                        <HeaderStyle Height="23px">
                                        <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                        </HeaderStyle>
                                        <PanelCollection>
                                            <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                                <table style="width:100%;">
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="SMTP Server Address" CssClass="lblsmallFont">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox ID="ASPxTextBox4" runat="server" CssClass="txtmed">
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" colspan="3">
                                                            <dx:ASPxLabel ID="ASPxLabel5" runat="server" CssClass="lblsmallFont" 
                                                                style="font-style: italic" Text="format: smtp.comcast.net">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <dx:ASPxCheckBox ID="ASPxCheckBox4" runat="server" CheckState="Unchecked" 
                                                                Text="Use SMTP alerts only when Domino servers are down" CssClass="lblsmallFont">
                                                            </dx:ASPxCheckBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <dx:ASPxCheckBox ID="ASPxCheckBox2" runat="server" CheckState="Unchecked" 
                                                                Text="Server Requires Authentication" CssClass="lblsmallFont">
                                                            </dx:ASPxCheckBox>
                                                        </td>
                                                        <td>
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Username : " CssClass="lblsmallFont">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox ID="ASPxTextBox5" runat="server" CssClass="txtmed">
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Password : " CssClass="lblsmallFont">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox ID="ASPxTextBox6" runat="server" CssClass="txtmed" >
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" colspan="2">
                                                            <dx:ASPxLabel ID="ASPxLabel7" runat="server" 
                                                                Text="(password is triple-DES encrypted)" CssClass="lblsmallFont">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                        <td>
                                                            &nbsp;</td>
                                                    </tr>
                                                </table>
                                                <br />
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dx:ASPxRoundPanel>
                                </td>
                                <td valign="top">
                                    <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" 
                                        CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                        CssPostfix="Glass" 
                                        GroupBoxCaptionOffsetY="-24px" HeaderText="Message Settings(Required)" Height="16px" 
                                        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="323px">
                                        <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
                                        <HeaderStyle Height="23px">
                                        <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                        </HeaderStyle>
                                        <PanelCollection>
                                            <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                                <table style="width:100%;">
                                                    <tr>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td align="right">
                                                            <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="From : " CssClass="lblsmallFont">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox ID="ASPxTextBox7" runat="server" CssClass="txtmed">
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td align="right">
                                                            <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Reply-To : " CssClass="lblsmallFont">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox ID="ASPxTextBox8" runat="server" CssClass="txtmed">
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dx:ASPxRoundPanel>
                                </td>
                            </tr>
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
                                    &nbsp;</td>
                            </tr>
                        </table>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
        </TabPages>
        <LoadingPanelImage Url="~/App_Themes/Glass/Web/Loading.gif">
        </LoadingPanelImage>
        <Paddings PaddingLeft="0px" />
        <ContentStyle>
            <Border BorderColor="#4986A2" />
        </ContentStyle>
    </dx:ASPxPageControl>

    </td></tr></table>



</asp:Content>
