<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
    CodeBehind="GetAssemblyInfo.aspx.cs" Inherits="VSWebUI.Configurator.GetAssemblyInfo" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript">
     function DeviceGridView_ContextMenu(s, e) {
            if (e.objectType == "row") {
                s.GetRowValues(e.index, 'NodeName', OnGetRowValues);
                s.SetFocusedRowIndex(e.index);
                StatusListPopup.ShowAtPos(ASPxClientUtils.GetEventX(e.htmlEvent), ASPxClientUtils.GetEventY(e.htmlEvent));
            }
        }
		</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
        <tr>
            <td>
                <div class="header" id="lblServer" runat="server">
                    VitalSigns Assembly Version Information
                </div>
            </td>
            <td align="right">
                <div class="subheadergreen" id="lblVersion" runat="server">
                    VSx.x_SPRT_xx_RCxx
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <dx:ASPxGridView runat="server" ID="AssemblyGridView" OnPageSizeChanged="AssemblyGridView_PageSizeChanged"
                Width="100%" Theme="Office2003Blue" EnableTheming="true">
               <SettingsBehavior AllowDragDrop="true" AllowFocusedRow="false" AllowSelectByRowClick="false" ColumnResizeMode="NextColumn" ProcessSelectionChangedOnServer="True" AutoExpandAllGroups="True" ></SettingsBehavior>

<Settings ShowGroupPanel="True" ></Settings>
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
                <SettingsPager PageSize="10" SEOFriendly="Enabled" Mode="ShowAllRecords" Visible="False">
                    <PageSizeItemSettings Visible="true" />
                </SettingsPager>
				 <Templates>
     <GroupRowContent>
            <%# Container.GroupText%>
     </GroupRowContent>
</Templates>
            </dx:ASPxGridView>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div class="header" id="Div1" runat="server">
                    VitalSigns Database Version Information
                    </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <dx:ASPxGridView runat="server" ID="DatabaseGridView" Width="100%" Theme="Office2003Blue"
                EnableTheming="True">

                <SettingsBehavior ConfirmDelete="True" AllowDragDrop="true"></SettingsBehavior>
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
                <SettingsPager PageSize="10" SEOFriendly="Enabled" Mode="ShowAllRecords" Visible="False">
                    <PageSizeItemSettings Visible="true" />
                </SettingsPager>
            </dx:ASPxGridView>
            </td>
            
        </tr>
    </table>
</asp:Content>
