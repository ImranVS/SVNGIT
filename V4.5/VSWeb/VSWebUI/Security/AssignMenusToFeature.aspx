<%@ Page Title="VitalSigns Plus - Assign Menus To Feature" Language="C#" MasterPageFile="~/Site1.Master"
    AutoEventWireup="true" CodeBehind="AssignMenusToFeature.aspx.cs" Inherits="VSWebUI.Security.AssignMenusToFeature" %>
	<%@ MasterType virtualpath="~/Site1.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>


<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>



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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Assign Menus To Feature" Font-Bold="True"
                    Font-Size="Large" Style="color: #000000; font-family: Verdana"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <table __designer:mapid="121">
                    <tr __designer:mapid="122">
                        <td __designer:mapid="123" colspan="2">
                            <dx:ASPxLabel ID="ASPxLabel18" runat="server" CssClass="lblsmallFont" EnableDefaultAppearance="False"
                                Text="Feature Name:">
                            </dx:ASPxLabel>
                        </td>
                        <td __designer:mapid="125">
                            <dx:ASPxComboBox ID="FeaturesComboBox" runat="server" AutoPostBack="True" CssClass="lblsmallFont"
                                OnSelectedIndexChanged="FeaturesComboBox_SelectedIndexChanged" TextField="FullName"
                                ValueField="FullName">
                                <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
                                    <RequiredField ErrorText="Please select user name." IsRequired="True" />
                                </ValidationSettings>
                            </dx:ASPxComboBox>
                        </td>
                        <td valign="top">
                            <div id="successDiv" runat="server" class="alert alert-success" style="display: none">
                                Settings were successfully updated.
                                <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                            </div>
                            <div id="errorDiv" class="alert alert-danger" runat="server" style="display: none">
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td valign="top">
                            <dx:ASPxRoundPanel ID="NavigatorRoundPanel" runat="server" HeaderText="Configurator Menus"
                                Theme="Glass" Width="200px">
                                <PanelCollection>
                                    <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                        <table class="tableWidth100Percent">
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table class="tableWidth100Percent">
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td rowspan="2" valign="top">
                                                                        <dx:ASPxTreeList ID="ConfiguratorMenus" runat="server" AutoGenerateColumns="False"
                                                                            CssClass="lblsmallFont" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
                                                                            CssPostfix="Office2010Blue" KeyFieldName="ID" ParentFieldName="ParentID" Theme="Office2003Blue">
                                                                            <Columns>
                                                                                <dx:TreeListTextColumn Caption="Menu Item" FieldName="DisplayText" HeaderStyle-CssClass="lblsmallFont"
                                                                                    ShowInCustomizationForm="True" VisibleIndex="0">
                                                                                    <HeaderStyle CssClass="lblsmallFont" />
                                                                                </dx:TreeListTextColumn>
                                                                                <dx:TreeListTextColumn FieldName="OrderNum" ShowInCustomizationForm="True" Visible="False"
                                                                                    VisibleIndex="1">
                                                                                </dx:TreeListTextColumn>
                                                                                <dx:TreeListTextColumn FieldName="PageLink" ShowInCustomizationForm="True" Visible="False"
                                                                                    VisibleIndex="2">
                                                                                </dx:TreeListTextColumn>
                                                                            </Columns>
                                                                            <Settings GridLines="Both" />
                                                                            <SettingsBehavior AllowDragDrop="False" AutoExpandAllNodes="False" />
                                                                            <SettingsSelection Enabled="True" Recursive="True" />
                                                                            <Images SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css">
                                                                                <LoadingPanel Url="~/App_Themes/Office2010Blue/TreeList/Loading.gif">
                                                                                </LoadingPanel>
                                                                            </Images>
                                                                            <Styles CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" CssPostfix="Office2010Blue">
                                                                                <LoadingPanel ImageSpacing="5px">
                                                                                </LoadingPanel>
                                                                                <AlternatingNode CssClass="GridAltRow" Enabled="True">
                                                                                </AlternatingNode>
                                                                            </Styles>
                                                                            <StylesPager>
                                                                                <PageNumber ForeColor="#3E4846">
                                                                                </PageNumber>
                                                                                <Summary ForeColor="#1E395B">
                                                                                </Summary>
                                                                            </StylesPager>
                                                                            <StylesEditors ButtonEditCellSpacing="0">
                                                                            </StylesEditors>
                                                                        </dx:ASPxTreeList>
                                                                    </td>
                                                                    <td>
                                                                        <br />
                                                                    </td>
                                                                    <td rowspan="2" valign="top">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="FeaturesComboBox" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>
                        </td>
                        <td valign="top">
                            <dx:ASPxRoundPanel ID="NavigatorRoundPanel0" runat="server" HeaderText="Dashboard Menus"
                                Theme="Glass" Width="200px">
                                <PanelCollection>
                                    <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                        <table class="tableWidth100Percent">
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table class="tableWidth100Percent">
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td rowspan="2" valign="top">
                                                                        <dx:ASPxTreeList ID="DashboardMenus" runat="server" AutoGenerateColumns="False" CssClass="lblsmallFont"
                                                                            CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" CssPostfix="Office2010Blue"
                                                                            KeyFieldName="ID" ParentFieldName="ParentID" Theme="Office2003Blue">
                                                                            <Columns>
                                                                                <dx:TreeListTextColumn Caption="Menu Item" FieldName="DisplayText" HeaderStyle-CssClass="lblsmallFont"
                                                                                    ShowInCustomizationForm="True" VisibleIndex="0">
                                                                                    <HeaderStyle CssClass="lblsmallFont" />
                                                                                </dx:TreeListTextColumn>
                                                                                <dx:TreeListTextColumn FieldName="OrderNum" ShowInCustomizationForm="True" Visible="False"
                                                                                    VisibleIndex="1">
                                                                                </dx:TreeListTextColumn>
                                                                                <dx:TreeListTextColumn FieldName="PageLink" ShowInCustomizationForm="True" Visible="False"
                                                                                    VisibleIndex="2">
                                                                                </dx:TreeListTextColumn>
                                                                            </Columns>
                                                                            <Settings GridLines="Both" />
                                                                            <SettingsBehavior AllowDragDrop="False" AutoExpandAllNodes="False" />
                                                                            <SettingsSelection Enabled="True" Recursive="True" />
                                                                            <Images SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css">
                                                                                <LoadingPanel Url="~/App_Themes/Office2010Blue/TreeList/Loading.gif">
                                                                                </LoadingPanel>
                                                                            </Images>
                                                                            <Styles CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" CssPostfix="Office2010Blue">
                                                                                <LoadingPanel ImageSpacing="5px">
                                                                                </LoadingPanel>
                                                                                <AlternatingNode CssClass="GridAltRow" Enabled="True">
                                                                                </AlternatingNode>
                                                                            </Styles>
                                                                            <StylesPager>
                                                                                <PageNumber ForeColor="#3E4846">
                                                                                </PageNumber>
                                                                                <Summary ForeColor="#1E395B">
                                                                                </Summary>
                                                                            </StylesPager>
                                                                            <StylesEditors ButtonEditCellSpacing="0">
                                                                            </StylesEditors>
                                                                        </dx:ASPxTreeList>
                                                                    </td>
                                                                    <td>
                                                                        <br />
                                                                    </td>
                                                                    <td rowspan="2" valign="top">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="FeaturesComboBox" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
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
                            <dx:ASPxPopupControl ID="NavigatorPopupControl" runat="server" HeaderText="Information"
                                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                Theme="Glass">
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                                        <table class="style1">
                                            <tr>
                                                <td>
                                                    <dx:ASPxLabel ID="MsgLabel" runat="server" Text="ASPxLabel">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dx:ASPxButton ID="OKButton" runat="server" Text="OK" Theme="Office2010Blue">
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <table>
                    <tr>
                        <td>
                            <dx:ASPxButton ID="AssignMenuButton" runat="server" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
                                CssPostfix="Office2010Blue" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
                                Text="Assign" Width="80px" OnClick="AssignMenuButton_Click">
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="ResetMenuButton" runat="server" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
                                CssPostfix="Office2010Blue" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
                                Text="Reset" Width="80px">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
